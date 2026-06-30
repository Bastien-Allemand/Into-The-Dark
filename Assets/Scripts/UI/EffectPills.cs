using UnityEngine;

public class EffectPills : GameEffect
{
    [SerializeField] private InsaneMeterScript insaneMeterScript;
    public override void ApplyEffect()
    {
        insaneMeterScript.insaneMeter -= insaneMeterScript.maxInsaneMeter * 0.15f;
    }
}
