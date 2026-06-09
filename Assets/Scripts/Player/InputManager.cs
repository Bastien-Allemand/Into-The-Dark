using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static PlayerAction _controls;
    public static PlayerAction controls
    {
        get
        {
            if (_controls == null)
            {
                _controls = new PlayerAction();
                _controls.Enable();
            }
            return _controls;
        }
    }
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}
