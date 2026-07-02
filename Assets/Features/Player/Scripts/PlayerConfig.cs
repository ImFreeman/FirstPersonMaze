using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig", order = 1)]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float sensivity = 1.0f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backwardKey = KeyCode.S;


    public float Sensivity => sensivity;
    public float Speed => speed;
    public float SprintSpeed => sprintSpeed;

    public KeyCode SprintKey => sprintKey;
    public KeyCode ForwardKey => forwardKey;
    public KeyCode BackwardKey => backwardKey;
    public KeyCode LeftKey => leftKey;
    public KeyCode RightKey => rightKey;

}