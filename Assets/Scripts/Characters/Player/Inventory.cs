using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int medicineCount;
    [SerializeField] private int batteryCount;
    [SerializeField] private int ventolineCount;
    [SerializeField] public DisplayInventory inventoryUI;

    public void AddConsumable(GameObject _Consumable)
    {
        ConsumableType type = _Consumable.GetComponent<ConsumableTypeScript>().GetConsumambleType();

        if (type == ConsumableType.NONE) return;
        switch (type)
        {
            case ConsumableType.PILL:
                medicineCount++;
                break;
            case ConsumableType.BATTERY:
                batteryCount++;
                break;
            case ConsumableType.VENTOLINE:
                ventolineCount++;
                break;
            default:
                break;
        }
        inventoryUI.UpdateCount(this);
    }

    public void Remove(int _count,ConsumableType _type)
    {
        switch (_type)
        {
            case ConsumableType.PILL:
                medicineCount -= _count;
                if (medicineCount <= 0)
                {
                    medicineCount = 0;
                }
                break;
            case ConsumableType.BATTERY:
                batteryCount -= _count;
                if (batteryCount <= 0)
                {
                    batteryCount = 0;
                }
                break;
            case ConsumableType.VENTOLINE:
                ventolineCount -= _count;
                if (ventolineCount <= 0)
                {
                    ventolineCount = 0;
                }
                break;
            default:
                break;
        }
        inventoryUI.UpdateCount(this);
    }

    public int GetConsumableCount(ConsumableType _type)
    {
        if(_type == ConsumableType.NONE) return -1;

        switch (_type)
        {
            case ConsumableType.PILL:
                return medicineCount;
            case ConsumableType.BATTERY:
                return batteryCount;
            case ConsumableType.VENTOLINE:
                return ventolineCount;
            default:
                break;
        }

        return -1;
    }

}