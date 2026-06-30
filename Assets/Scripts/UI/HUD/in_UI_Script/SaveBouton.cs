using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class SaveBouton : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI m_TextMeshPro;

    public void Use()
    {
        //  change  path
        JsonManager.pathUsed.paths[(int)JsonManager.path.Input] = JsonManager.Instance.GetPath(JsonManager.path.Input, m_TextMeshPro.text);
        //  rewrite usePath (default because there isn't any other because it's UsePath)
        JsonManager.Instance.Save(JsonManager.path.Use, JsonManager.defaultName, JsonUtility.ToJson(JsonManager.pathUsed));
        //  update jsonManager
        JsonManager.Instance.PathChangeUpdate();
    }
    public void Supp()
    {
        JsonManager.Instance.supp_File(
            JsonManager.Instance.GetPath(
                JsonManager.path.Input,
                m_TextMeshPro.text
            )
        );
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
