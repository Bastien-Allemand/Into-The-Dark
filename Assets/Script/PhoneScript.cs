using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneScript : MonoBehaviour
{
    public GameObject phone;

    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            phone.SetActive(!phone.activeSelf);
        }
    }
}