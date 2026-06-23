using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get;
        private set;
    }

    CursorLockMode cursorWantedState;

    private void Awake() => Instance = this;

    PlayerAction controls;
    public List<Transform> UIs;

    private enum conditionList
    {
        enter,
        exit
    }
    private enum actionList
    {
        init,
        enter,
        exit
    }
    Dictionary<Transform, List<Func<bool>>> condition;
    Dictionary<Transform, System.Action[]> action;
    void FoncInit()
    {
        condition = new Dictionary<Transform, List<Func<bool>>>();
        action = new Dictionary<Transform, Action[]>();

        Action<Transform, UI> init = (Transform index, UI ui) =>
        {
            //      Condition
            List<Func<bool>> tmpCondition = new List<Func<bool>>()
            {
                ui.EnterCondition,
                ui.ExitCondition
            };

            //      Action
            System.Action[] tmpAction = new System.Action[]
            {
                ui.Init,
                ui.Enter,
                ui.Exit
            };

            condition.Add(index, tmpCondition);
            action.Add(index, tmpAction);
        };

        for (int i = 0; i < UIs.Count(); i++)
        {
            UI ui = UIs[i].GetComponent<UI>();
            Debug.Log("UI Code : " + ui);
            if (!ui)
            {
                Debug.Log("Creating DebugCode");
                UIs[i].AddComponent<UI>();
                ui = UIs[i].GetComponent<UI>();
                Debug.Log("UI Add Code : " + ui);
            }
            init(UIs[i], ui);
        }
    }
    private void Init()
    {
        UIs = new List<Transform>();
        foreach (Transform enfant in transform.GetComponentsInChildren<Transform>(true))
        {
            if (enfant.GetComponent<UI>())
            {
                UIs.Add(enfant);
            }
        }
        FoncInit();
        Pause(false);
        //  init all UI
        foreach (var pair in action)
        {
            pair.Value[(int)actionList.init]();
        }

        UpdateCursorState();
        HideAllUI();
    }
    private void UpdateCursorState()
    {
        cursorWantedState = CursorLockMode.Locked;
        foreach (Transform t in UIs)
        {
            if (t.gameObject.activeSelf)
            {
                cursorWantedState = t.GetComponent<UI>().cursorWantedState;
                if (cursorWantedState == CursorLockMode.None)
                {
                    break;
                }
            }
        }
        Cursor.lockState = cursorWantedState;
    }
    private void Start()
    {
        controls = InputManager.controls;
        Init();
    }
    private void Update()
    {
        for (int i = 0; i < UIs.Count; i++)
        {
            if (!UIs[i].gameObject.activeSelf)  //  check if enter condition is true    because it's inactif
            {
                if (condition[UIs[i]][(int)conditionList.enter]())
                {
                    ShowUI(UIs[i]);
                }
            }
            else
            {
                if (condition[UIs[i]][(int)conditionList.exit]())   //  check if exit condition is true because it's actif
                {
                    Debug.Log(UIs[i].gameObject.name + " exit.");
                    HideUI(UIs[i]);
                }
            }
        }

    }
    public Transform GetUIs<T>()
    {
        foreach (Transform t in UIs)
        {
            T tmp = t.transform.GetComponentInChildren<T>();
            if (tmp != null)
            {
                return t;
            }
        }
        Debug.Log($"not found {typeof(T).Name}");
        return null;
    }
    public void SwapActive(Transform it)
    {
        it.gameObject.SetActive(!it.gameObject.activeSelf);
    }
    public void ShowUI(Transform it)
    {
        if (!it.gameObject.activeSelf)
        {
            it.gameObject.SetActive(true);
            action[it][(int)actionList.enter]();
            Debug.Log($"{it.gameObject.name} : {it.gameObject.activeSelf}");
            UpdateCursorState();
        }
    }
    public void HideUI(Transform it)
    {
        if (it.gameObject.activeSelf)
        {
            action[it][(int)actionList.exit]();
            it.gameObject.SetActive(false);
            Debug.Log($"{it.gameObject.name} : {it.gameObject.activeSelf}");
            UpdateCursorState();
        }
    }
    public void HideAllUI()
    {
        Debug.Log("Hide All");
        foreach (Transform it in UIs)
        {
            if (it.gameObject.activeSelf)
            {
                action[it][(int)actionList.exit]();
                it.gameObject.SetActive(false);
                Debug.Log($"{it.gameObject.name} : {it.gameObject.activeSelf}");
            }
        }
        UpdateCursorState();
    }
    public void Pause(bool pause)
    {
        //  il faut rajouter la pause pour les entité
        if (pause)
        {
            Debug.Log("Time : Pause");
            controls.GamePlay.Disable();
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.Log("Time : Continue");
            controls.GamePlay.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
