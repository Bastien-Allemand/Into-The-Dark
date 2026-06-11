using System;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bouton : MonoBehaviour
{
    UIManager uiManager;
    [SerializeField] private Transform t_target;
    [SerializeField] private SceneAsset sc_target;
    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }
    public void ExitPauseMenu()
    {
        t_target.GetComponent<PauseMenu>().exit = true;
        Debug.Log(t_target.GetComponent<PauseMenu>().exit);
    }
    public void ExitPause()
    {
        uiManager.Pause(false);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(sc_target.name);
    }
    public void Hide()
    {
        t_target.gameObject.SetActive(false);
    }
    public void UIShowOnly()
    {
        int a = Array.FindIndex(uiManager.UIs, x => x.name == t_target.name);
        uiManager.ShowOnly((UIManager.ListUI)a);
    }
    public void UIShow()
    {
        int a = Array.FindIndex(uiManager.UIs, x => x.name == t_target.name);
        uiManager.ShowUI((UIManager.ListUI)a);
    }
    public void UIHide()
    {
        int a = Array.FindIndex(uiManager.UIs, x => x.name == t_target.name);
        uiManager.HideUI((UIManager.ListUI)a);
    }
    public void SwapActiveState()
    {
        if (t_target.gameObject.activeSelf)
            UIHide();
        else
            UIShow();
    }
    public void StopProgram()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
