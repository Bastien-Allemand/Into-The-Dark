using UnityEngine;

public class EffectBattery : GameEffect
{
    public void ApplyBatteryEffect(GameObject hitObject)
    {
        if (hitObject.TryGetComponent(out ItemScript item))
        {
            if (item.rechargable)
            {
                item.energy = item.maxEnergy;
                Debug.Log($"{hitObject.name} a été rechargé à 100% !");
            }
        }
    }
    public override void ApplyEffect()
    {
        
    }
}
