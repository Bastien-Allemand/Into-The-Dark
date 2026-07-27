using UnityEngine;

public class MouseState : MonoBehaviour
{
    void Awake()
    {
        lockMouse();
    }

    public void lockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("locking");
    }
    public void unlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("unlocking");
    }
}
