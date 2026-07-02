using UnityEngine;

public class EffectVentolin : GameEffect
{
    [SerializeField] private PlayerStateMachine playerStateMachine;
    public override void ApplyEffect()
    {
        playerStateMachine.staminaConfigs.staminaLeft += playerStateMachine.staminaConfigs.maxStamina ;
    }
}
