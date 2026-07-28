using System.Collections.Generic;
using UnityEngine;

public class UIStack : MonoBehaviour
{
    [Header("Panneau de départ (optionnel)")]
    [SerializeField] private GameObject initialPanel;
    private Stack<GameObject> history = new Stack<GameObject>();
    private GameObject currentPanel;

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
            Debug.LogWarning("Aucun panneau dans l'historique !");
        }
    }
    public void OpenPanelDirectly(GameObject panel)
    {
        if (panel == null) return;

        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        currentPanel = panel;
        currentPanel.SetActive(true);
    }
}