using UnityEngine;

public class MainMenu : UI
{
    [SerializeField] private Bouton option;
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
        cursorWantedState = CursorLockMode.None;
        Transform target = manager.GetUIs<PauseMenu>();
        if (target)
        {
            option.show_target.Add(target);
            Debug.Log(target.transform);
        }
        else
            Debug.Log("No Pause Menu");
    }
    public override void Enter()
    {
    }
    public void Update()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public override void Exit()
    {
    }
}
