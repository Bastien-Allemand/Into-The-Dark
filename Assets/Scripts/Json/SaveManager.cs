using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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
    public void Save(fileType type, path jsonpath, string fileName, string text)
    {
        string extention = $".{type.ToString()}";
        if (!fileName.EndsWith(extention, System.StringComparison.OrdinalIgnoreCase))
        {
            fileName = Path.GetFileNameWithoutExtension(fileName) + extention;
        }
        string path = GetPath(jsonpath, fileName);
        if(File.Exists(path))
        {
            Debug.Log("path already existing : overwriting");
        }
        File.WriteAllText(path, text);
    }
    public void SaveInputInJson(string fileName)
    {
        InputActionMap map = controls.GamePlay;
        string json = map.SaveBindingOverridesAsJson();
        Save(fileType.json, path.Input, fileName, json);
    }

    public string[] getFileNames(path path, fileType type)
    {
        string[] files = Directory.GetFiles(GetPath(path), $"*.{type.ToString()}");

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
    public string nameToFileType(string name, fileType type)
    {
        string extention = $".{type.DisplayName()}";
        if (name.EndsWith(extention, System.StringComparison.OrdinalIgnoreCase))
        {
            return name;
        }
        return Path.GetFileNameWithoutExtension(name) + extention;
    }
}
