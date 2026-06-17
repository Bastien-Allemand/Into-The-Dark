using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveBouton : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI m_TextMeshPro;
    UsePathSave json;

    public void Use()
    {
        //  get
        json = JsonManager.Instance.pathUsed;
        //  change
        json.path_map_PlayerActionMap_GamePlay = JsonManager.Instance.GetPath(JsonManager.path.Input, m_TextMeshPro.text);
        //  rewrite
        JsonManager.Instance.Save(JsonManager.path.Use, JsonManager.defaultName, JsonUtility.ToJson(json));
    }
    public void Supp()
    {
        InputSaveManager.Instance.SuppSave(m_TextMeshPro.text);
    }
    public void NewSave(string name, InputActionMap map)
    {
        string path = JsonManager.Instance.GetPath(JsonManager.path.Input, name);
        if (File.Exists(path))
        {
            Debug.Log($"{path} already existing, function not for overwrinting");
            return;
        }
        JsonManager.Instance.Save(JsonManager.path.Input, name, map.ToJson());
    }
    public void Rename(string newName) //  inputfield  | input fiel is actif after a bouton
    {
        InputSaveManager.Instance.RenameSave(newName);
    }
}
