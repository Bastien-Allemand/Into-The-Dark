using System.IO;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static JsonManager;

public class PauseMenu : UI
{
    PlayerAction controls_buffer;
    //  first letter of var, then what it does
    [SerializeField] public GameObject GO_Controls_Content;
    [SerializeField] public GameObject GO_Prefab_Keybind;
    [SerializeField] private Bouton b_continue;
    [SerializeField] private Bouton b_main_menu;

    [Header("Saves var")]

    [SerializeField] public TMP_InputField inputField;
    [SerializeField] public Transform boutonSlider_content;
    [SerializeField] private GameObject boutonPrefab;
    [SerializeField] private TextMeshProUGUI current_json_name;

    string currentJsonPathUsed; // full path
    //  save somewhere the current used json
    public override bool EnterCondition()
    {
        if (JsonManager.controls.Menu.Pause.WasPressedThisFrame())
        {
            enter = true;
            Debug.Log("Pause Menu Input Pressed");
        }
        bool result = enter;
        enter = false;
        return result;
    }
    public override bool ExitCondition()
    {
        if (JsonManager.controls.Menu.Pause.WasPressedThisFrame())
        {
            exit = true;
            Debug.Log("Pause Menu Input Pressed");
        }
        bool result = exit;
        exit = false;
        return result;
    }
    private void Awake()
    {
        Debug.Log("Pause Menu : Awake Start");

        cursorWantedState = CursorLockMode.None;

        controls_buffer = new PlayerAction();
        controls_buffer.Disable();
        ResetBuffer();

        Transform target = manager.GetUIs<MainMenu>();
        if (target)
        {
            b_continue.show_target.Add(target);
            Debug.Log(target.transform);
            b_main_menu.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("No MainMenu");
            b_main_menu.gameObject.SetActive(true);
        }

        foreach (InputAction action in controls_buffer.GamePlay.Get())
        {
            if (action.name == "Look")
                continue;
            CreateBoutonFromAction(action, GO_Controls_Content.transform);
        }
        Debug.Log("Pause Menu : Awake End");
    }
    private void Start()
    {
        currentJsonPathUsed = JsonManager.pathUsed.paths[(int)JsonManager.path.Input];
        CreateBouton();
    }
    private void OnEnable()
    {
        Debug.Log("Pause Menu : OnEnable Start");

        ResetBuffer();

        exit = false;
        //  can be opti, but a bit weird to t
        foreach (var ui in UIManager.Instance.UIs)
        {
            if (ui.GetComponent<MainMenu>() != null)
            {
                ui.gameObject.SetActive(false);
            }
        }


        updateBoutonText();
        Debug.Log("Pause Menu : OnEnable End");

    }
    private void OnDisable()
    {
        Debug.Log("Pause Menu : OnDisable");
        foreach (var ui in UIManager.Instance.UIs)
        {
            if (ui.GetComponent<MainMenu>() != null)
            {
                ui.gameObject.SetActive(true);
            }
        }
    }
    void ResetBuffer()
    {
        JsonManager.Instance.GetInputFromJson(controls_buffer);
    }
    private void CreateBoutonFromAction(InputAction action, Transform parent)
    {
        int bindingIndex = 0;
        foreach (InputBinding binding in action.bindings)
        {
            if (binding.isComposite)
            {
                bindingIndex++;
                continue;
            }
            Debug.Log($"name={action.name + " " + binding.name} | path={binding.path} | composite={binding.isComposite} | part={binding.isPartOfComposite}");

            GameObject buffer = Instantiate(GO_Prefab_Keybind, parent, false);
            var tmp = buffer.GetComponent<TMPro.TextMeshProUGUI>();
            tmp.text = $"{action.name} {binding.name}";

            Rebind script = buffer.GetComponentInChildren<Rebind>();
            if (script != null)
                script.Init(action, bindingIndex);
            else
                Debug.Log("script Rebind not found");

            bindingIndex++;
        }
    }



    public void updateBoutonText()
    {
        current_json_name.text = System.IO.Path.GetFileNameWithoutExtension(JsonManager.pathUsed.paths[(int)JsonManager.path.Input]);
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
    public void SaveInput(string fileName)
    {
        //  get automaticly the current used json
        JsonManager.Instance.SaveInputInJson(controls_buffer, fileName);
        JsonManager.Instance.PathChangeUpdate();
    }
    public void SaveInput()
    {
        //  get automaticly the current used json
        JsonManager.Instance.SaveInputInJson(controls_buffer);
        JsonManager.Instance.PathChangeUpdate();
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
