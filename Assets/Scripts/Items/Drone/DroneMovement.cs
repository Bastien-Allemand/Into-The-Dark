using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 120f;

    private PlayerAction controls;

    private void Awake()
    {
        controls = InputManager.controls;
    }

    private void Update()
    {
        Vector2 input = controls.Global.MoveForward.ReadValue<Vector2>();

        transform.position += transform.forward * input.y * moveSpeed * Time.deltaTime;

        transform.Rotate(0f, input.x * rotationSpeed * Time.deltaTime, 0f);

        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }
}