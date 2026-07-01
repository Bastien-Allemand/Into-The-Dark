using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.LookDev;

public class JsonManager : MonoBehaviour
{
    private static JsonManager instance;
    public static JsonManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("JsonManager");
                instance = go.AddComponent<JsonManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    public static string defaultName;
    private static string savePath;


    public static UsePathSave pathUsed;
    public static PlayerAction controls;
    public static GameSave gameSave;
    public enum path
    {
        Input,
        GameSave,
        Use // keep Use as Last (function a bit like Size but not only for Size)
    }
    private void OnEnable()
    {
        Debug.Log("JsonManager : OnEnable");
        if (controls != null)
            controls.Enable();

    }
    private void OnDisable()
    {
        Debug.Log("JsonManager : OnDisable");
        if (controls != null)
            controls.Disable();
    }
    private void Awake()
    {
        Debug.Log("JsonManager : Awake Start");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("JsonManager : Awake Destroy (is copie)");
            Destroy(gameObject);
        }


        defaultName = "Default";
        savePath = Application.persistentDataPath;

        pathUsed = getUsedPath();
        controls = new PlayerAction();
        //  update the pathUsed
        for (int i = 0; i < (int)path.Use; i++)
        {
            string currentJsonPathUsed;
            object obj = Create((path)i);
            if (!File.Exists(GetPath((path)i, defaultName)))    //  if default don't exist, create it
            {
                if (obj is PlayerAction action)
                    SaveInputInJson(action.asset,defaultName);
                else
                    Save(GetPath((path)i, defaultName), JsonUtility.ToJson(obj));
            }
            currentJsonPathUsed = pathUsed.paths[i];
            if (!File.Exists(currentJsonPathUsed))              //  if current path don't exist, use default one
            {
                currentJsonPathUsed = Instance.GetPath(path.Input, defaultName);
                pathUsed.paths[i] = currentJsonPathUsed;
            }
            else        //  merge the default and json then save the change (it change if the version change)
            {
                if (obj is PlayerAction action)
                {
                    action.asset.LoadBindingOverridesFromJson(File.ReadAllText(pathUsed.paths[(int)path.Input]));
                    Save(currentJsonPathUsed, action.asset.SaveBindingOverridesAsJson());
                }
                else
                {
                    JsonUtility.FromJsonOverwrite(File.ReadAllText(currentJsonPathUsed), obj);
                    Save(currentJsonPathUsed, JsonUtility.ToJson(obj));
                }
            }
            break;
        }

        PathChangeUpdate();
        Debug.Log("JsonManager : Awake End");
    }
    private object Create(path i)
    {
        switch (i)
        {
            case path.Use:
                {
                    var o = new UsePathSave();
                    o.Default();
                    return o;
                }

            case path.Input:
                {
                    var o = new PlayerAction();
                    o.Disable();
                    return o;
                }

            case path.GameSave:
                return new GameSave();

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PathChangeUpdate()
    {
        controls.Disable();

        controls.asset.RemoveAllBindingOverrides();
        controls.asset.LoadBindingOverridesFromJson(File.ReadAllText(pathUsed.paths[(int)path.Input]));

        gameSave = JsonUtility.FromJson<GameSave>(File.ReadAllText(pathUsed.paths[(int)path.GameSave]));
        //InputSaveManager.Instance.updateBoutonText();

        if (gameObject.activeSelf)
            controls.Enable();
    }
    public UsePathSave getUsedPath()
    {
        UsePathSave result;
        string usePath = GetPath(path.Use, defaultName);

        result = new UsePathSave();
        result.Default();
        if (File.Exists(usePath))
        {
            Debug.Log(usePath);
            JsonUtility.FromJsonOverwrite(File.ReadAllText(usePath), result);
        }

        string json = JsonUtility.ToJson(result);
        Save(path.Use, defaultName, json);
        Debug.Log("Save Default Json");
        return result;
    }
    public string GetPath(path path, string fileName = null)
    {
        string state = "bug";
#if UNITY_EDITOR
        state = "Editor";
#else
        state = "Build";
#endif
        string resultPath;
        if (string.IsNullOrEmpty(fileName))
        {
            resultPath = Path.Combine(
                savePath,
                state, path.ToString()
            );
        }
        else
        {
            resultPath = Path.Combine(
                savePath,
                state, path.ToString(), nameToJsonFile(fileName)
            );
        }
        return resultPath;
    }
    public void Save(path jsonpath, string fileName, string text)
    {

        string extention = ".json";
        if (!fileName.EndsWith(extention, System.StringComparison.OrdinalIgnoreCase))
        {
            fileName = Path.GetFileNameWithoutExtension(fileName) + extention;
        }
        string folderPath = GetPath(jsonpath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string path = GetPath(jsonpath, fileName);
        if (File.Exists(path))
        {
            Debug.Log("path already existing : overwriting");
        }
        File.WriteAllText(path, text);
    }
    public void Save(string fullPath, string json) // no as secure as the other one
    {
        if (File.Exists(fullPath))
        {
            Debug.Log("path already existing : overwriting");
        }
        File.WriteAllText(fullPath, json);
    }
    public void SaveInputInJson(InputActionAsset asset, string fileName)
    {
        string json = asset.SaveBindingOverridesAsJson();
        Save(path.Input, fileName, json);
    }
    
    public string[] getJsonNames(path path)
    {
        string[] files = Directory.GetFiles(GetPath(path), "*.json");

        //  ! pas de foreach avec modification pour les []      pas touche ! :3
        for (int i = 0; i < files.Length; i++)  //  enlaive tout le path et extention pour n'avoir que le name
        {
            files[i] = System.IO.Path.GetFileNameWithoutExtension(files[i]);
        }

        return files;
    }
    public bool Rename_MoveFile(string path, string newPath)
    {
        if (File.Exists(newPath))
        {
            Debug.Log($"Can't rename {path} to {newPath} because new name already existing");
            return false;
        }
        if (!File.Exists(path))
        {
            Debug.Log($"{path} don't exist");
            return false;
        }
        File.Move(path, newPath);
        return true;
    }
    public void supp_File(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    public void LoadInputFromJson(string targetPath)
    {
        //  use getJsonFileName and use a select system to get the fileName
        if (targetPath.EndsWith(".json", System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log($"BUG : LoadInputFromJson() > fileName : {targetPath} is not a json");
            return;
        }

        string path = GetPath(JsonManager.path.Input, targetPath);
        if (File.Exists(path))
        {
            Debug.Log("loading ...");

            string json = File.ReadAllText(path);
            controls.asset.RemoveAllBindingOverrides();
            controls.asset.LoadBindingOverridesFromJson(json);

            Debug.Log($"Input prefab {targetPath} Loaded");
        }
        else
        {
            Debug.Log($"No Json name {targetPath} in path {path}");
        }
    }
    public string nameToJsonFile(string name)
    {
        string extention = ".json";
        return Path.GetFileNameWithoutExtension(name) + extention;
    }
}
