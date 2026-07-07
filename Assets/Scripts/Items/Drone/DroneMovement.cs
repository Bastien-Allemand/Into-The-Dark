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
        Vector2 input = controls.GamePlay.Movement.ReadValue<Vector2>();

        transform.position += transform.forward * input.y * moveSpeed * Time.deltaTime;

        transform.Rotate(0f, input.x * rotationSpeed * Time.deltaTime, 0f);
    }
}