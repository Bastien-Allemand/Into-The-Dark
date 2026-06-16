using UnityEngine;
using UnityEngine.InputSystem;

public class InputSaveManager : MonoBehaviour
{
    [SerializeField] public Transform boutonSlider_content;
    [SerializeField] private GameObject boutonPrefab;

    string currentFileJsonUsed; // full path
    //  save somewhere the current used json
    private void Start()
    {
        CreateBouton();
    }
    private void updateBouton()
    {
        Transform[] childs = boutonSlider_content.GetComponentsInChildren<Transform>();
        foreach (Transform child in childs)
        {
            Destroy(child.gameObject);
        }
        CreateBouton();
    }
    public void SaveInput(string fileName = null)
    {
        if (fileName == null)
            fileName = currentFileJsonUsed;
        //  get automaticly the current used json
        SaveManager.Instance.Save(SaveManager.fileType.json, SaveManager.path.Input, fileName, InputManager.controls.SaveBindingOverridesAsJson());
    }
    public void CreateSave(string fileName)
    {
        //ask filename inputfield
        SaveInput(fileName);
        updateBouton();
    }
    public void SuppSave(string name)
    {
        //  name without path
        SaveManager.Instance.supp_File(
            SaveManager.Instance.GetPath(
                SaveManager.path.Input,
                SaveManager.Instance.nameToFileType(
                name,
                SaveManager.fileType.json)
            )
        );
    }
    public void RenameSave(string newName)
    {
        SaveManager.Instance.Rename_MoveFile(
            currentFileJsonUsed,
            SaveManager.Instance.GetPath(SaveManager.path.Input,
            SaveManager.Instance.nameToFileType(
                newName,
                SaveManager.fileType.json)
            )
        );
    }
    private void CreateBouton()
    {
        string[] jsonNames = SaveManager.Instance.getFileNames(SaveManager.path.Input, SaveManager.fileType.json);
        foreach (string jsonName in jsonNames)
        {
            GameObject go = Instantiate(boutonPrefab, boutonSlider_content);
            go.name = jsonName;
            TMPro.TMP_Text text = go.GetComponentInChildren<TMPro.TMP_Text>();
            text.text = jsonName;
        }
    }
}
