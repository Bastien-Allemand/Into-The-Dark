using UnityEngine;

public class MouseState : MonoBehaviour
{
    void Start()
    {
        lockMouse();
    }

    void lockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void unlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
