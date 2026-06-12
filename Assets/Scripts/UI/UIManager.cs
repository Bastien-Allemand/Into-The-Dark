using System;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get;
        private set;
    }
    private void Awake() => Instance = this;

    PlayerAction controls;
    [Header("Manager Setting")]
    [Space(5)]
    [SerializeField] public Transform[] UIs;
    [SerializeField] public List<ListUI> actifUI;



    public enum ListUI
    {
        MainMenu,
        PauseMenu,
        PlayerUI
    }

    private enum conditionList
    {
        enter,
        exit
    }
    private enum actionList
    {
        init,
        enter,
        update,
        fixedUpdate,
        exit
    }
    Dictionary<ListUI, List<Func<bool>>> condition;
    Dictionary<ListUI, System.Action[]> action;
    void FoncInit()
    {
        condition = new Dictionary<ListUI, List<Func<bool>>>();
        action = new Dictionary<ListUI, Action[]>();

        Action<UI, ListUI> init = (UI ui, ListUI index) =>
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
                ui.M_Update,
                ui.M_FixedUpdate,
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
            init(ui, (ListUI)i);
        }
    }
    private void Init()
    {
        Pause(false);
        //  init all UI
        foreach (var pair in action)
        {
            pair.Value[(int)actionList.init]();
        }
        //  Hide all
        foreach (var ui in UIs)
        {
            ui.gameObject.SetActive(false);
        }
        //  Show All Active UI
        foreach (ListUI ui in actifUI)
        {
            //  same code as the ShowUI but without the actifUI.Add()
            UIs[(int)ui].gameObject.SetActive(true);
            action[ui][(int)actionList.enter]();
            Debug.Log($"{UIs[(int)ui].gameObject.name} : {UIs[(int)ui].gameObject.activeSelf}");
        }
    }
    private void Start()
    {
        controls = InputManager.controls;
        FoncInit();
        Init();
    }
    private void Update()
    {
        for (int i = 0; i < UIs.Length; i++)
            if (!UIs[i].gameObject.activeSelf)
            {

                if (condition[(ListUI)i][(int)conditionList.enter]())
                {
                    ShowUI((ListUI)i);
                }
            }
            else
            {
                if (condition[(ListUI)i][(int)conditionList.exit]())
                {
                    Debug.Log(UIs[i].gameObject.name + " exit.");
                    HideUI((ListUI)i);
                }
                else
                    action[(ListUI)i][(int)actionList.update]();
            }

    }

    private void FixedUpdate()
    {
        foreach (var i in actifUI)
            action[i][(int)actionList.fixedUpdate]();
    }
    public Transform getUI(ListUI index) => UIs[(int)index];
    public void SwapActive(ListUI it)
    {
        getUI(it).gameObject.SetActive(!getUI(it).gameObject.activeSelf);
    }
    public void ShowUI(ListUI it)
    {
        UIs[(int)it].gameObject.SetActive(true);
        action[it][(int)actionList.enter]();
        Debug.Log($"{UIs[(int)it].gameObject.name} : {UIs[(int)it].gameObject.activeSelf}");
        actifUI.Add(it);
    }
    public void HideUI(ListUI it)
    {
        action[it][(int)actionList.exit]();
        UIs[(int)it].gameObject.SetActive(false);
        Debug.Log($"{UIs[(int)it].gameObject.name} : {UIs[(int)it].gameObject.activeSelf}");
        actifUI.Remove(it);
    }
    public void HideAllUI()
    {
        Debug.Log("Hide All");
        for (int i = actifUI.Count - 1; i >= 0; i--)
        {
            HideUI(actifUI[i]);
        }
    }
    public void ShowOnly(ListUI it)
    {
        Debug.Log($" Show Only : {UIs[(int)it].gameObject.name}");
        HideAllUI();
        ShowUI(it);
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
