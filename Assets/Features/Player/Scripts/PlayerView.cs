using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private CharacterController characterController;
    public Camera MainCamera => mainCamera;
    public Transform MainCameraTransform => mainCameraTransform;
    public Transform BodyTransform => bodyTransform;
    public CharacterController CharacterController => characterController;

    public void SetPositionTo(Vector3 position)
    {
        characterController.enabled = false;
        bodyTransform.position = position;
        characterController.enabled = true;
    }
}