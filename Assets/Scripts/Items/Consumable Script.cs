using UnityEngine;

public enum ConsumableType
{
    NONE,
    PILL,
    BATTERY,
    VENTOLINE
}

public class ConsumableTypeScript : MonoBehaviour
{
    [SerializeField] private ConsumableType ConsumableType;

    public ConsumableType GetConsumambleType()
    {
        return ConsumableType;
    }
}
