using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Rebind : MonoBehaviour
{
    private PlayerAction controls => JsonManager.controls;

    public InputAction m_action;
    public int m_bindingIndex = 0;
    private void Start()
    {
        UpdateBindingText();
    }
    public void Init(InputAction action, int bindingIndex)
    {
        m_action = action;
        m_bindingIndex = bindingIndex;
    }
    public void UpdateBindingText()
    {
        Transform bindingText = transform.Find("Text (TMP)");
        if (bindingText != null)
        {
            bindingText.GetComponent<TMPro.TextMeshProUGUI>().text = m_action.GetBindingDisplayString(m_bindingIndex);
        }
        else
            Debug.Log("Text (TMP) not found");
    }
    public void KeyRebind()
    {
        bool previousState = controls.GamePlay.enabled;

        if (previousState)
            controls.GamePlay.Disable();

        m_action.PerformInteractiveRebinding(m_bindingIndex)
                    .OnComplete(op =>
                    {
                        op.Dispose();

                        UpdateBindingText();
                    })
        .Start();

        if (previousState)
            controls.GamePlay.Enable();
    }
}
