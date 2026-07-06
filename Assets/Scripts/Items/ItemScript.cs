using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public bool deployed = false;
    public bool canBeRepickUp = false; 
    public bool needsToBePlaced = false;
    public bool usingTimer = false;
    
    public float placingDelay = 0f;
    public float timer = 0f;

    public bool usingEnergy = false;
    public float energy = 100f;
    public float maxEnergy = 100f;
    public bool rechargable = false; 

    [SerializeField] private BatteryScript batteryScript;

    public enum ItemType
    {
        NONE,
        ITEM_CAMERA
    }

   [SerializeField] public ItemType type = ItemType.NONE;

    void Start()
    {
        batteryScript = GetComponentInChildren<BatteryScript>();
        if (batteryScript != null)
        {
            batteryScript.SetMaxBattery(maxEnergy);
        }

    }

    public void Update()
    {
        if (deployed && usingTimer)
        {
            if (timer < 0f)
            {
                Destroy(gameObject);
            }
            timer -= Time.deltaTime;
            placingDelay -= Time.deltaTime; 
        }
        if (usingEnergy)
        {
            energy -= Time.deltaTime;
            if(batteryScript != null)
            {
                batteryScript.SetBattery(energy);
            }
            if(energy <= 0f)
            { 
                energy = 0f; 
            }
        }

    }
}
