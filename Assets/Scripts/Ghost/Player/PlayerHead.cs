using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float sensitivity = 100f;
    float xRotation;
    float yRotation;

    void Start()
    {
    }

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

        transform.Rotate(0, mouseX, 0);

        float bufferX = xRotation;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        float test = xRotation * sensitivity / 10 + transform.rotation.x;

        if (test < 60 && test > -60)
            transform.localRotation = Quaternion.Euler(xRotation * sensitivity / 10, 0, 0);
        else
            xRotation = bufferX;

        playerTransform.localRotation = Quaternion.Euler(0, yRotation * sensitivity / 10, 0);
    }
}
