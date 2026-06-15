using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveManager
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveManager();
            }

            return _instance;
        }
    }
    private SaveManager() { }
    private PlayerAction controls => InputManager.controls;
    public enum path
    {
        Input
    }
    public enum fileType
    {
        json
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
        if (!string.IsNullOrEmpty(fileName))
        {
            resultPath = Path.Combine(
                UnityEngine.Application.persistentDataPath,
                state, path.ToString(), fileName
                );
        }
        else
        {
            resultPath = Path.Combine(
                UnityEngine.Application.persistentDataPath,
                state, path.ToString()
                );
        }
        return resultPath;
    }
    public void Save(fileType type, path jsonpath, string fileName, string json)
    {
        string check = $".{type.ToString()}";
        if (!fileName.EndsWith(check, System.StringComparison.OrdinalIgnoreCase))
        {
            fileName = Path.GetFileNameWithoutExtension(fileName) + check;
        }
        string path = GetPath(jsonpath, fileName);
        File.WriteAllText(path, json);
    }

    public void SaveInputInJson(string fileName)
    {
        InputActionMap map = controls.GamePlay;
        string json = map.SaveBindingOverridesAsJson();
        Save(fileType.json,path.Input, fileName, json);
    }

    public string[] getJsonFileName(path path, fileType type)
    {
        string[] files = Directory.GetFiles(GetPath(path), $"*.{type.ToString()}");
        return files;
    }

    public void LoadInputFromJson(string fileName)
    {
        //  use getJsonFileName and use a select system to get the fileName
        if (fileName.EndsWith(".json", System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log($"BUG : LoadInputFromJson() > fileName : {fileName} is not a json");
            return;
        }

        string path = GetPath(SaveManager.path.Input, fileName);
        if (File.Exists(path))
        {
            Debug.Log("loading ...");

            string json = File.ReadAllText(path);
            controls.asset.LoadBindingOverridesFromJson(json);

            Debug.Log($"Input prefab {fileName} Loaded");
        }
        else
        {
            Debug.Log($"No Json name {fileName} in path {path}");
        }
    }
}
