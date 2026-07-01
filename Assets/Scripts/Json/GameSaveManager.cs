using System.IO;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    private static GameSaveManager _instance;
    public static GameSaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameSaveManager");
                _instance = go.AddComponent<GameSaveManager>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }
    public GameSave gameSave => JsonManager.gameSave;

    string currentJsonPathUsed; // full path
                                //  save somewhere the current used json
    private void Awake()
    {
        Debug.Log("GameSaveManager : Awake Start");
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.Log("GameSaveManager : Awake Destroy GameObject (is copie)");
            return;
        }

        _instance = this;

        transform.SetParent(null);

        DontDestroyOnLoad(gameObject);
        Debug.Log("GameSaveManager : Awake End");
    }

    //  récupère les var du json dedans persistant path puis vérifié que le path exist
    private void Start()
    {
        Debug.Log("GameSaveManager : Start Start");
        Debug.Log(JsonManager.Instance);
        currentJsonPathUsed = JsonManager.pathUsed.paths[(int)JsonManager.path.GameSave];

        GameSave gameSave = new GameSave();
        JsonUtility.FromJsonOverwrite(File.ReadAllText(currentJsonPathUsed), gameSave);
        Debug.Log("GameSaveManager : Start End");
    }

    public void Save()
    {
        JsonManager.Instance.Save(currentJsonPathUsed, JsonUtility.ToJson(gameSave));
    }
    public void NewSave(string name, bool current = true)
    {
        if (current)
        {
            JsonManager.Instance.Save(JsonManager.path.GameSave, name, JsonUtility.ToJson(gameSave));
        }
        else
        {
            JsonManager.Instance.Save(JsonManager.path.GameSave, name, JsonUtility.ToJson(new GameSave()));
        }

    }
    public void SuppSave()
    {
        JsonManager.Instance.supp_File(
            JsonManager.Instance.GetPath(
            JsonManager.path.Input,
            name
            )
        );
    }
}
