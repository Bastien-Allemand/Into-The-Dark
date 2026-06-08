using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rebind : MonoBehaviour
{
    [Header("ActionName doit être écrit a la main avec le meme nom qu'il a dedans le ActionMap")]
    [SerializeField] private string actionName;
    [SerializeField] private int bindingIndex = 0;
    [SerializeField] private TMP_Text bindingText;
    private PlayerAction controls;
    private InputAction action;
    private void Start()
    {
        controls = InputManager.controls;
        action = controls.asset.FindAction(actionName);
        UpdateBindingText();
    }

    public void UpdateBindingText()
    {
        bindingText.text = action.GetBindingDisplayString(bindingIndex);
    }
    public void KeyRebind()
    {
        action.PerformInteractiveRebinding(bindingIndex)
                    .OnComplete(op =>
                    {
                        op.Dispose();

                        UpdateBindingText();
                    })
        .Start();
    }
}
