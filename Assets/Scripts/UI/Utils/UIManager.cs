using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<UIManager>(); //  code needed in a prefab so use FindFirst
            }

            return _instance;
        }
    }

    public List<Transform> UIs;

    Dictionary<Transform, List<Func<bool>>> condition;
    private enum conditionList
    {
        enter,
        exit
    }
    private UIManager() { }

    private void Awake()
    {
        Debug.Log("UIManager : Awake Start");
        Init();
        Debug.Log("UIManager : Awake End");
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
    private void Init()
    {
        Debug.Log("Init UIs in UIManager");
        UIs = new List<Transform>();
        foreach (Transform enfant in transform.GetComponentsInChildren<Transform>(true))
        {
            if (enfant.GetComponent<UI>())
            {
                UIs.Add(enfant);
                Debug.Log($"Add in UIs : {enfant.name}");
            }
        }
        FoncInit();

        UpdateCursorState();
    }
    private void FoncInit()
    {
        condition = new Dictionary<Transform, List<Func<bool>>>();

        Action<Transform, UI> init = (Transform index, UI ui) =>
        {
            //      Condition
            List<Func<bool>> tmpCondition = new List<Func<bool>>()
            {
                ui.EnterCondition,
                ui.ExitCondition
            };

            condition.Add(index, tmpCondition);
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
    public Transform GetUIs<T>() where T : Component
    {
        Debug.Log($"Seach in UIs the componant {typeof(T).Name}");
        Debug.Log($"debug test UIs : {UIs}");
        foreach (Transform ui in UIs)
        {
            T result = ui.transform.GetComponentInChildren<T>(true);
            Debug.Log($"found : {result}");
            if (result != null)
            {
                return ui;
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
            Debug.Log($"{it.gameObject.name} : {it.gameObject.activeSelf}");
            UpdateCursorState();
        }
    }
    public void HideUI(Transform it)
    {
        if (it.gameObject.activeSelf)
        {
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
                it.gameObject.SetActive(false);
                Debug.Log($"{it.gameObject.name} : {it.gameObject.activeSelf}");
            }
        }
        UpdateCursorState();
    }
    private void UpdateCursorState()
    {
        CursorLockMode cursorWantedState = CursorLockMode.Locked;
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
}
