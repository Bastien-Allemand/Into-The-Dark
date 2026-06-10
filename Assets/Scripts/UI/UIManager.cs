using System;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    PlayerAction controls;



    [Header("Manager Setting")]

    [Space(5)]

    [SerializeField] Transform[] UIs;
    [SerializeField] List<ListUI> actifUI;
    [Header("Remenber to modify the enum ListUI when adding UI")]
    [SerializeField] List<UI> uiScripts;



    public enum ListUI
    {
        PauseMenu
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

        for (int i = 0; i < uiScripts.Count; i++)
        {
            init(uiScripts[i], (ListUI)i);
        }
    }
    private void Init()
    {
        foreach (var pair in action)
        {
            pair.Value[(int)actionList.init]();
        }
        HideAllUI();
        foreach (ListUI ui in actifUI)
        {
            action[ui][(int)actionList.enter]();
        }
        Pause(false);
    }

    private void Awake()
    {
        controls = InputManager.controls;
        FoncInit();
        Init();
    }
    private void Update()
    {
        for (int i = 0; i < UIs.Length; i++)
            if (!UIs[i].gameObject.activeSelf)
                if (condition[(ListUI)i][(int)conditionList.enter]())
                {
                    ShowUI((ListUI)i);
                }
                else
                {
                    if (condition[(ListUI)i][(int)conditionList.exit]())
                    {
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
        actifUI.Add(it);
    }
    public void HideUI(ListUI it)
    {
        action[it][(int)actionList.exit]();
        UIs[(int)it].gameObject.SetActive(false);
        actifUI.Remove(it);
    }
    public void HideAllUI()
    {
        foreach (var t in actifUI)
            HideUI(t);
    }
    public void ShowOnly(ListUI it)
    {
        HideAllUI();
        ShowUI(it);
    }
    public void Pause(bool pause)
    {
        //  il faut rajouter la pause pour les entité
        if (pause)
        {
            controls.GamePlay.Disable();
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            controls.GamePlay.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
