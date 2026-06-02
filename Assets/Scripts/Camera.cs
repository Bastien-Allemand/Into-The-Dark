using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Transform mTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mTarget.position;
        transform.rotation = mTarget.rotation;
        //  récupaire pas le scale

    }
}
