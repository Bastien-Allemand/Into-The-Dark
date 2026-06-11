using UnityEngine;

public class MainMenu : UI
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
    public override void M_Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public override void M_FixedUpdate()
    {
    }
    public override void Exit()
    {
    }
}
