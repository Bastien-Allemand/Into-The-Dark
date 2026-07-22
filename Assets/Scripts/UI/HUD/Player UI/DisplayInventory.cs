using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

[Serializable]
public class Consumable
{
    public GameObject image;
    public TextMeshProUGUI Keybind;
    public TextMeshProUGUI Count;
    public ConsumableType Type = ConsumableType.NONE;
}

public class DisplayInventory : MonoBehaviour
{
    [SerializeField] public List<Consumable> consumables;

    public void UpdateCount(Inventory _inv)
    {
        for (int i = 0; i < consumables.Count; ++i)
        {
            consumables[i].Count.text = _inv.GetConsumableCount(consumables[i].Type).ToString();
        }
    }
    public void ShowAndHideUi(ConsumableType _type, bool _state)
    {
        if (_type == ConsumableType.NONE)
        {
            return;
        }
        for (int i = 0; i < consumables.Count; ++i)
        {
            if (consumables[i].Type == _type)
            {
                consumables[i].image.SetActive(_state);

            }
        }
    }

    public void DeviceChangeUpdate(InputDevice device)
    {
        string group = device switch
        {
            Gamepad => "Gamepad",
            Keyboard => "Keyboard&Mouse",
            Mouse => "Keyboard&Mouse",
            _ => "Keyboard&Mouse"
        };

        Action<TextMeshProUGUI, InputAction> updateTxt = (TMP_UGUI, action) =>
        {
            TMP_UGUI.text = action.GetBindingDisplayString(InputBinding.MaskByGroup(group));
        };

        updateTxt(consumables[0].Keybind, InputManager.controls.PlayerInteraction.ConsumeItem1);
        updateTxt(consumables[1].Keybind, InputManager.controls.PlayerInteraction.ConsumeItem2);
        updateTxt(consumables[2].Keybind, InputManager.controls.PlayerInteraction.ConsumeItem3);

    }

}
