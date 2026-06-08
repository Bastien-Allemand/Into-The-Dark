using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerAction controls;
    private void Awake()
    {
        if (controls == null)
        {
            controls = new PlayerAction();
        }
    }
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}
