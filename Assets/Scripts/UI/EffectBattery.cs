using UnityEngine;

public class EffectBattery : GameEffect
{
    public void ApplyBatteryEffect(GameObject hitObject, float val)
    {
        if (hitObject.TryGetComponent(out ItemScript item))
        {
            if (item.rechargable)
            {
                item.energy = val;
                //Debug.Log($"{hitObject.name} a été rechargé à 100% !");
            }
        }
    }
    public override void ApplyEffect()
    {
        
    }
}
