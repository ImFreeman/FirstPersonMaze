## Управление

| Действие | Клавиша / Устройство |
|---|---|
| Перемещение | `W` `A` `S` `D` |
| Обзор (вращение камеры) | Мышь |
| Бег (спринт) | `Left Shift` (удерживать) |
| Перезапуск (на экранах победы/поражения) | Кнопка на экране |

---

## Архитектура проекта

Проект выполнен в Unity с использованием **Zenject** (DI-контейнер) и следует feature-based структуре. Вся логика разбита по папкам в `Assets/Features/`.

### 1. Классы и их ответственность

| Папка / Класс | Назначение |
|---|---|
| **`Core/`** | |
| `ApplicationLauncher` | Точка входа: активирует системы ввода и запускает игровой цикл через `GameFlow`. |
| `ApplicationInstaller` | Zenject-инсталлер: связывает все зависимости, настраивает пулы объектов, передаёт spawn-точки и маршруты врагов. |
| **`Player/`** | |
| `PlayerView` (MonoBehaviour) | Визуальное представление игрока: хранит ссылки на `Camera`, `CharacterController`, `Transform` тела. |
| `PlayerMovement` | Логика перемещения: обычная скорость / спринт, направление по вводу, движение через `CharacterController.Move()`. |
| `PlayerCameraRotation` | Логика вращения камеры: считывает дельту мыши, ограничивает вертикальный угол (±90°), вращает тело и камеру. |
| `PlayerConfig` (ScriptableObject) | Настройки игрока: чувствительность, скорости, привязки клавиш. |
| **`Input/`** | |
| `InputElement` (abstract) | Базовый класс для обработчиков ввода; управляет подпиской на `TickableManager` (обновление каждый кадр). |
| `MovementInput` | Считывает нажатия `WASD` и `Left Shift`, формирует `Vector2` направления и флаг спринта. |
| `MousePositionInput` | Считывает `Mouse X` / `Mouse Y` для вращения камеры. |
| `IMovementInput`, `ICameraRotationInput` | Интерфейсы ввода, через которые игровые системы получают данные, не завися от конкретного устройства. |
| **`GameState/`** | |
| `IGameState` | Интерфейс состояния игры: `OnEntry()` / `OnExit()`. |
| `GameStateMachine` | Машина состояний: при смене вызывает `OnExit()` у текущего и `OnEntry()` у нового состояния. |
| `MainGameState` | Инициализация игры: спавнит алмазы и врагов, телепортирует игрока, включает управление и камеру. |
| `GameOverState` | Состояние поражения: показывает экран Game Over. |
| `VictoryGameState` | Состояние победы: показывает экран Victory. |
| `GameFlow` | Оркестратор игры: подписывается на события выхода и контакта с врагом, переключает состояния, обрабатывает рестарт. |
| **`Collectables/`** | |
| `CollectableView` (MonoBehaviour) | Визуал алмаза: триггер-коллайдер, пул объектов (MonoMemoryPool). |
| `Collectables` | Менеджер всех алмазов: спавн, удаление, подсчёт оставшихся, событие `OnCountChanged`. |
| **`Enemy/`** | |
| `EnemyView` (MonoBehaviour) | Визуал врага: `NavMeshAgent`, триггер-коллайдер, настройки радиусов обнаружения/потери, пул объектов. |
| `Enemy` | ИИ врага: патрулирование по точкам, преследование игрока, потеря при выходе из радиуса. |
| `Enemies` | Менеджер всех врагов: спавн, удаление, пробрасывает событие `OnPlayerContact`. |
| **`Exit/`** | |
| `ExitView` (MonoBehaviour) | Визуал выхода: меняет материал на «открытый»/«закрытый», триггер-коллайдер. |
| `ExitController` | Логика выхода: открывает выход, когда все алмазы собраны; при входе игрока в открытый выход вызывает событие победы. |
| **`UI/`** | |
| `UIWindow` (abstract) | Базовый класс UI-окна с жизненным циклом `Show()` / `Hide()`. |
| `UIService` (`IUIService`) | Сервис UI: загружает окна из `Resources`, показывает/скрывает, управляет пулом. |
| `UIHudWindow` | HUD: отображает счётчик собранных алмазов. |
| `UIGameOverWindow` | Экран поражения с кнопкой рестарта. |
| `UIVictoryWindow` | Экран победы с кнопкой рестарта. |
| `Hud` | Связывает событие `OnCountChanged` от `Collectables` с текстом счёта в `UIHudWindow`. |

---

### 2. Логика победы / поражения

Вся логика сосредоточена в классе **`GameFlow`** (`Assets/Features/GameState/Scripts/GameFlow.cs`):

* **Победа:**
  1. Игрок входит в триггер выхода → `ExitView.OnPlayerEntered`.
  2. `ExitController` проверяет, что выход открыт (все алмазы собраны), и вызывает событие `OnPlayerExited`.
  3. `GameFlow.OnPlayerExited()` переключает `GameStateMachine` в состояние `VictoryGameState`.
  4. Отображается `UIVictoryWindow`.

* **Поражение:**
  1. Игрок касается триггера врага → `EnemyView.OnPlayerContact`.
  2. `Enemies` пробрасывает событие `OnPlayerContact`.
  3. `GameFlow.EnemyOnPlayerContact()` переключает `GameStateMachine` в состояние `GameOverState`.
  4. Отображается `UIGameOverWindow`.

* **Рестарт:** Нажатие кнопки на любом из экранов вызывает `GameFlow.RestartGame()`, который переводит машину состояний обратно в `MainGameState`.

---

### 3. Логика сбора бриллиантов (алмазов)

Логика находится в классе **`Collectables`** (`Assets/Features/Collectables/Scripts/Collectables.cs`):

1. При старте игры `MainGameState.OnEntry()` вызывает `Collectables.SpawnCollectables()`, создавая объекты `CollectableView` в заданных точках через пул.
2. Каждый `CollectableView` имеет триггер-коллайдер. При входе игрока срабатывает `OnTriggerEnter` → событие `OnPlayerEntered`.
3. `Collectables.HandlePlayerEntered()` проверяет, что вошёл именно игрок (по `InstanceID`), удаляет алмаз из списка, возвращает его в пул и генерирует событие `OnCountChanged`.
4. На это событие подписаны:
   * **`Hud`** — обновляет текст счёта на экране.
   * **`ExitController`** — проверяет, что `count == 0`, и открывает выход (`ExitView.SetOpen(true)`).

---

### 4. Логика врагов

Логика находится в классе **`Enemy`** (`Assets/Features/Enemy/Scripts/Enemy.cs`):

Враг работает в двух режимах (конечный автомат):

| Режим | Поведение |
|---|---|
| **Патрулирование** (`UpdatePatrol`) | Двигается по массиву точек `_patrolPoints` через `NavMeshAgent.SetDestination()`. При достижении точки переходит к следующей. Параллельно проверяет, не обнаружен ли игрок. |
| **Преследование** (`UpdateChase`) | Если игрок обнаружен, `NavMeshAgent` ведёт врага к позиции игрока. Продолжает преследование, пока расстояние до игрока не превысит `_lossRadius` — тогда враг возвращается к патрулированию. |

**Обнаружение игрока** (`IsPlayerDetected`):
1. Расстояние до игрока ≤ `DetectionRadius` (настраивается в `EnemyView`).
2. Прямая видимость: `Physics.Linecast` между врагом и игроком не пересекает объекты из слоя `_obstacleMask`.

**Контакт с игроком:**
* `EnemyView.OnTriggerEnter` → событие `OnPlayerContact` → `Enemies.HandlePlayerContact` → `Enemies.OnPlayerContact` → `GameFlow.EnemyOnPlayerContact` → переход в `GameOverState`.