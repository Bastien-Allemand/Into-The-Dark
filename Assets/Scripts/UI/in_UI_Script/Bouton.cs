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
    [SerializeField] public Action @delegate;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sc_target.name);
    }
    public void ActionUI()
    {
        foreach (Transform t in hide_target)
        {
            uiManager.HideUI(t);
        }
        foreach (Transform t in show_target)
        {
            uiManager.ShowUI(t);
        }
    }
    public void Action()
    {
        Debug.Log("Bouton Action");
        foreach (Transform t in hide_target)
        {
            t.gameObject.SetActive(false);
            Debug.Log($"Hide {t.name}");
        }
        foreach (Transform t in show_target)
        {
            t.gameObject.SetActive(true);
            Debug.Log($"Show {t.name}");
        }
    }
    public void Delegate()
    {
        @delegate?.Invoke();
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
    public void Save()
    {
        GameSaveManager.Instance.Save();
    }
    public void TMP_ADD()
    {
        GameSaveManager.Instance.gameSave.level++;
        FindFirstObjectByType<LevelMenu>().CallUpdate();
    }
    public void TMP_SUPP()
    {
        GameSaveManager.Instance.gameSave.level--;
        FindFirstObjectByType<LevelMenu>().CallUpdate();
    }
}
