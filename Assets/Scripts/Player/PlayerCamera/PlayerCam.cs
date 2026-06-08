using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    [SerializeField] Transform yRotationTarget;
    [SerializeField] float sensitivity = 100f;
    [SerializeField] float XMaxAngle = 60;

    //  target
    [SerializeField] Transform mTarget;
    float xRotation = 0f;
    float yRotation = 0f;

    //  current
    float currentX = 0f;
    float currentY = 0f;


    private void LateUpdate()
    {
        Rotate();
        Move();
    }
    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

        transform.Rotate(0, mouseX, 0);

        float bufferX = xRotation;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        float test = xRotation * sensitivity / 10 + transform.rotation.x;

        if (test > XMaxAngle && test < -XMaxAngle)
            xRotation = bufferX;

        currentX = Mathf.Lerp(currentX, xRotation, 10f * Time.deltaTime);
        currentY = Mathf.Lerp(currentY, yRotation, 10f * Time.deltaTime);

        transform.localRotation =
            Quaternion.Euler(currentX * sensitivity / 10, 0, 0);

        yRotationTarget.localRotation =
            Quaternion.Euler(0, currentY * sensitivity / 10, 0);
    }
    void Move()
    {

    }
}
