using System;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Bouton : MonoBehaviour
{
    UIManager uiManager => UIManager.Instance;
    [SerializeField] public List<Transform> show_target;
    [SerializeField] public List<Transform> hide_target;
    [SerializeField] public SceneAsset sc_target;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sc_target.name);
    }
    public void ActionUI()
    {
        foreach (Transform t in show_target)
        {
            uiManager.ShowUI(t);
        }
        foreach (Transform t in hide_target)
        {
            uiManager.HideUI(t);
        }
    }
    public void Action()
    {
        foreach (Transform t in show_target)
        {
            t.gameObject.SetActive(true);
        }
        foreach (Transform t in hide_target)
        {
            t.gameObject.SetActive(false);
        }
    }
    public void ActiveSwap()
    {
        foreach (Transform t in show_target)
        {
            t.gameObject.SetActive(!t.gameObject.activeSelf);
        }
    }
    public void ActiveSwapUI()
    {
        foreach (Transform t in show_target)
        {
            uiManager.SwapActive(t);
        }
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
