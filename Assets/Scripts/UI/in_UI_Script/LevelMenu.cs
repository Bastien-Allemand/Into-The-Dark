using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] GameObject GO_option_panel;
    [SerializeField] GameObject GO_content;
    [SerializeField] Button GO_prefab_bouton;
    [SerializeField] SceneAsset[] Scenes_Level;

    GameSave gameSave => GameSaveManager.Instance.gameSave;
    List<Button> m_button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        m_button = new List<Button>();
        CallUpdate();
    }
    public void CallUpdate()
    {
        foreach (var button in m_button)
        {
            Destroy(button.gameObject);
        }
        m_button.Clear();
        for (int i = 0; i < Scenes_Level.Length; i++)
        {
            m_button.Add(Instantiate(GO_prefab_bouton, GO_content.transform));
            m_button[i].GetComponent<Bouton>().sc_target = Scenes_Level[i];
            m_button[i].GetComponentInChildren<TextMeshProUGUI>().text = $"Level {i + 1}";
            if (gameSave.level < i)
            {
                m_button[i].interactable = false;
            }
            else
            {
                m_button[i].interactable = true;
            }
        }
        Debug.Log($"Player level : {gameSave.level}");
    }
}
