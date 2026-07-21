using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance { get; private set; }

    public static string defaultName;
    private string savePath;
    public static UsePathSave pathUsed;
    PlayerAction controls => InputManager.controls;
    public enum path
    {
        Use,
        Input,
        GameSave
    }
    private void Awake()
    {
        Debug.Log("JsonManager Awake");

        Instance = this;
        DontDestroyOnLoad(gameObject);


        defaultName = "Default";
        savePath = Application.persistentDataPath;

        pathUsed = getUsedPath();
        Debug.Log("JsonManager Awake End");

    }

    public UsePathSave getUsedPath()
    {
        UsePathSave result;
        string usePath = GetPath(path.Use, defaultName);
        if (!File.Exists(usePath))
        {
            result = new UsePathSave();
            result.Default();
        }
        else
        {
            result = new UsePathSave();
            result.Default();
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
        Debug.Log("Get Path Called");
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
