using System.Collections.Generic;
using UnityEngine;

public class UIStack : MonoBehaviour
{
    [Header("Panneau de départ (optionnel)")]
    [SerializeField] private GameObject initialPanel;
    private Stack<GameObject> history = new Stack<GameObject>();
    private GameObject currentPanel;
    public bool HasHistory => history.Count > 0;

    private void Start()
    {
        if (initialPanel != null)
        {
            OpenPanelDirectly(initialPanel);
        }
    }

    public void OpenPanel(GameObject newPanel)
    {
        if (newPanel == null) return;
        
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
            history.Push(currentPanel);
        }

        currentPanel = newPanel;
        currentPanel.SetActive(true);
    }

    public void GoBack()
    {
        if (history.Count > 0)
        {
            if (currentPanel != null)
            {
                currentPanel.SetActive(false);
            }

            currentPanel = history.Pop();
            currentPanel.SetActive(true);
        }
        else
        {
            CloseAll();
        }
    }

    public void OpenPanelDirectly(GameObject panel)
    {
        if (panel == null) return;

        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        history.Clear();

        currentPanel = panel;
        currentPanel.SetActive(true);
    }

    public void CloseAll()
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
            currentPanel = null;
        }

        history.Clear();
    }
}