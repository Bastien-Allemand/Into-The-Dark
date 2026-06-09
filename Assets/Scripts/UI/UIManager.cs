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
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIManager();
            return _instance;
        }
    }

    PlayerAction controls;
    [Header("Pause Menu Setting")]
    [SerializeField] Transform GO_Controls_Content;
    [SerializeField] GameObject GO_Prefab_Keybind;
    [SerializeField] Transform[] UIs;
    [SerializeField] ListUI[] actifUI;
    public enum ListUI
    {
        PauseMenu,
        MainMenu,
        PlayerUI
    }

    System.Action[] enterCondition;
    System.Action[] enter;
    System.Action[] update;
    System.Action[] fixedUpdated;
    System.Action[] exit;









    private bool PauseMenuEnterCondition()
    {
        return controls.Menu.Pause.WasPressedThisFrame();
    }
    private void PauseMenuEnter()
    {
        getUI(ListUI.PauseMenu).gameObject.SetActive(true);

        controls.GamePlay.Disable();
        Cursor.lockState = CursorLockMode.None;
        Pause(true);
    }
    private void PauseMenuUpdate()
    {
        if (controls.Menu.Pause.WasPressedThisFrame())
        {
            PauseMenuExit();
        }
    }
    private void PauseMenuFixedUpdate()
    {

    }
    private void PauseMenuExit()
    {
        getUI(ListUI.PauseMenu).gameObject.SetActive(false);
        controls.GamePlay.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Pause(false);

        actifUI = System.Array.FindAll(actifUI, n => n != ListUI.PauseMenu);
    }







    private void Awake()
    {
        controls = InputManager.controls;
        Init();
    }
    private void Update()
    {

    }
    private void Init()
    {
        HideAllUI();
        foreach (ListUI ui in actifUI)
        {
            switch (ui)
            {
                case ListUI.PauseMenu:
                    Pause(true);
                    break;
            }
            ShowUI(ui);
        }
        Pause(false);
    }
    private Transform getUI(ListUI index) => UIs[(int)index];
    public void SwapActive(ListUI it) => getUI(it).gameObject.SetActive(!getUI(it).gameObject.activeSelf);
    public void ShowUI(ListUI it) => getUI(it).gameObject.SetActive(true);
    public void HideUI(ListUI it) => getUI(it).gameObject.SetActive(false);
    public void HideAllUI()
    {
        Pause(false);
        foreach (Transform ui in UIs)
            ui.gameObject.SetActive(false);
    }
    public void ShowOnly(ListUI it)
    {
        
        foreach (Transform t in UIs)
            t.gameObject.SetActive(false);
        if (it == ListUI.PauseMenu)
            Pause(true);
        else
            Pause(false);
        ShowUI(it);
    }
    public void Pause(bool pause)
    {
        //  il faut rajouter la pause pour les entité
        if (pause)
        {
            controls.GamePlay.Disable();
        }
        else
        {
            controls.GamePlay.Enable();
        }
    }
}
