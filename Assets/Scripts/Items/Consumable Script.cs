using UnityEngine;

public enum ItemType
{
    Pill,
    Battery,
    Ventolin
}

public class CollectibleScript : MonoBehaviour
{
    [SerializeField] private ItemType myType;

    public ItemType GetItemType()
    {
        return myType;
    }
}
