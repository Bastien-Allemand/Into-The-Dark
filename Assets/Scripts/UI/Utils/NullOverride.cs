using UnityEngine;

public class NullOverride : UI
{
    public override bool EnterCondition()
    {
        return false;
    }
    public override bool ExitCondition()
    {
        return false;
    }
    public override void Init()
    {
    }
    public override void Enter()
    {
    }
    public override void Exit()
    {
    }
}
