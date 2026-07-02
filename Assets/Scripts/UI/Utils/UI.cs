using UnityEngine;

public class UI : MonoBehaviour
{
    static protected UIManager manager => UIManager.Instance;
    public CursorLockMode cursorWantedState = CursorLockMode.Locked;
    public bool enter = false;
    public bool exit = false;
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

    private void debug(string func)
    {
        //Debug.Log($"Null : {GetType().FullName} : {func}");
    }
}
