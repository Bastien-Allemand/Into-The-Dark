//using UnityEngine;

//public class MainMenu : UI
//{
//    [SerializeField] private Bouton b_option;
//    public override bool EnterCondition()
//    {
//        return false;
//    }
//    public override bool ExitCondition()
//    {
//        return false;
//    }
//    private void Awake()
//    {
//        cursorWantedState = CursorLockMode.None;
//        Transform target = manager.GetUIs<PauseMenu>();
//        if (target)
//        {
//            Debug.Log("Found PauseMenu");
//            b_option.show_target.Add(target);
//            Debug.Log(target.transform);
//        }
//        else
//            Debug.Log("No Pause Menu");
//    }
//}
