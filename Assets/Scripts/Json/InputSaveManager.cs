using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSaveManager : MonoBehaviour
{


    private static InputSaveManager _instance;
    public static InputSaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputSaveManager();
            }

            return _instance;
        }
    }
    private InputSaveManager() { }

    [SerializeField] public TMP_InputField inputField;
    [SerializeField] public Transform boutonSlider_content;
    [SerializeField] private GameObject boutonPrefab;

    string currentJsonPathUsed; // full path
    //  save somewhere the current used json
    private void Start()
    {
        currentJsonPathUsed = JsonManager.pathUsed.path_map_PlayerActionMap_GamePlay;
        //  look if the default json of input is existing
        if (!File.Exists(JsonManager.Instance.GetPath(JsonManager.path.Input, JsonManager.defaultName)))
        {
            PlayerAction actionMap = new PlayerAction();
            JsonManager.Instance.SaveInputInJson(actionMap.GamePlay, JsonManager.defaultName);
        }
        //  look if the current used inputjson is existing
        if (!File.Exists(currentJsonPathUsed))
        {
            currentJsonPathUsed = JsonManager.Instance.GetPath(JsonManager.path.Input, JsonManager.defaultName);
            UsePathSave overide = JsonManager.pathUsed;
            overide.path_map_PlayerActionMap_GamePlay = currentJsonPathUsed;
            JsonManager.Instance.Save(JsonManager.path.Use, JsonManager.defaultName, JsonUtility.ToJson(overide, true));
        }
        CreateBouton();
    }
    private void updateBouton()
    {
        //  supp all ui bouton json and recreate all
        //  (an update for when there is a rename, create or supp)
        Transform[] childs = boutonSlider_content.GetComponentsInChildren<Transform>();
        for (int i = childs.Length - 1; i >= 0; i--)
        {
            Destroy(childs[i].gameObject);
        }
        CreateBouton();
    }
    public void SaveInput(string fileName = null)
    {
        if (fileName == null)
            fileName = currentJsonPathUsed;
        //  get automaticly the current used json
        JsonManager.Instance.Save(JsonManager.path.Input, fileName, InputManager.controls.SaveBindingOverridesAsJson());
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
        JsonManager.Instance.supp_File(
            JsonManager.Instance.GetPath(
                JsonManager.path.Input,
                name
            )
        );
        updateBouton();
    }
    public void RenameSave(string newName)
    {
        JsonManager.Instance.Rename_MoveFile(
            currentJsonPathUsed,
            JsonManager.Instance.GetPath(
                JsonManager.path.Input,
                newName
            )
        );
        updateBouton();
    }
    private void CreateBouton()
    {
        string[] jsonNames = JsonManager.Instance.getJsonNames(JsonManager.path.Input);
        foreach (string jsonName in jsonNames)
        {
            GameObject go = Instantiate(boutonPrefab, boutonSlider_content);
            go.name = jsonName;
            TMPro.TMP_Text text = go.GetComponentInChildren<TMPro.TMP_Text>();
            text.text = jsonName;
        }
    }


    public void ValidateSaveName()
    {
        string texte = inputField.text;
    }
}
