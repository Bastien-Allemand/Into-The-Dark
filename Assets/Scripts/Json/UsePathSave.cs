using System;
using UnityEngine;

[Serializable]
public class UsePathSave
{
    public string path_map_PlayerActionMap_GamePlay;
    public string path_GameSave;
    public UsePathSave() { }
    public void Default()
    {
        path_map_PlayerActionMap_GamePlay = JsonManager.Instance.GetPath(JsonManager.path.Input, JsonManager.defaultName);
        path_GameSave = JsonManager.Instance.GetPath(JsonManager.path.GameSave, JsonManager.defaultName);
    }
}
