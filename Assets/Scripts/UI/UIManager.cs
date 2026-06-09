using System;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    PlayerAction controls;
    [Header("Pause Menu Setting")]
    [SerializeField] Transform GO_Controls_Content;
    [SerializeField] GameObject GO_Prefab_Keybind;
    [SerializeField] Transform pauseMenu;
    private void Awake()
    {
        controls = InputManager.controls;
        Init();
    }
    private void Update()
    {
        if (controls.Menu.Pause.WasPressedThisFrame())
        {
            Pause();
        }
    }
    public void Pause()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        if (pauseMenu.gameObject.activeSelf)
            controls.GamePlay.Disable();
        else
            controls.GamePlay.Enable();
    }
    public void Pause(bool pause)
    {
        pauseMenu.gameObject.SetActive(pause);
        if (pauseMenu.gameObject.activeSelf)
            controls.GamePlay.Disable();
        else
            controls.GamePlay.Enable();
    }
    private void Init()
    {
        Pause(false);
        foreach (InputAction action in controls.GamePlay.Get())
        {
            if (action.name == "Look")
                continue;
            CreateBoutonFromAction(action, GO_Controls_Content);
        }
    }
    private void CreateBoutonFromAction(InputAction action, Transform parent)
    {
        int bindingIndex = 0;
        foreach (InputBinding binding in action.bindings)
        {
            if (binding.isComposite)
            {
                bindingIndex++;
                continue;
            }
            Debug.Log($"name={action.name + " " + binding.name} | path={binding.path} | composite={binding.isComposite} | part={binding.isPartOfComposite}");

            GameObject buffer = Instantiate(GO_Prefab_Keybind, parent, false);
            var tmp = buffer.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = $"{action.name} {binding.name}";

            Rebind script = buffer.GetComponentInChildren<Rebind>();
            if (script != null)
                script.Init(action, bindingIndex);
            else
                Debug.Log("script Rebind not found");

            bindingIndex++;
        }
    }
}
