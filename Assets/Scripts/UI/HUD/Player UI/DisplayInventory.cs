using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

[Serializable]
public class Consumable
{
    public GameObject Image;
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
    public void ShowAndHideUi(ConsumableType _type,bool _state)
    {
        if (_type == ConsumableType.NONE)
        {
            return;
        }
        for (int i = 0; i < consumables.Count; ++i)
        {
            if(consumables[i].Type == _type)
            {
                consumables[i].Image.SetActive(_state);

            }
        }
    }
}
