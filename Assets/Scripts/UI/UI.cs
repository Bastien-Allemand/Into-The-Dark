using UnityEngine;

public class UI : MonoBehaviour
{
    protected UIManager manager;
    private void Awake()
    {
        Debug.Log($"Awake UI before = {manager}");
        manager = UIManager.Instance;
        Debug.Log($"Aveke UI after = {manager}");
    }
    public virtual bool EnterCondition()
    {
        debug("EnterCondition");
        return false;
    }
    public virtual bool ExitCondition()
    {
        debug("ExitCondition");
        return false;
    }
    public virtual void Init()
    {
        debug("Init");
    }
    public virtual void Enter()
    {
        debug("Enter");
    }
    public virtual void M_Update()
    {
        Cursor.lockState = CursorLockMode.None;
        debug("Update");
    }
    public virtual void M_FixedUpdate()
    {
        debug("FixedUpdate");
    }
    public virtual void Exit()
    {
        debug("Exit");
    }

    private void debug(string func)
    {
        Debug.Log($"Null : {GetType().FullName} : {func}");
    }
}
