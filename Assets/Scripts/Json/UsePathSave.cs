using System;
using UnityEngine;

[Serializable]
public class UsePathSave
{
    public string path_map_PlayerActionMap_GamePlay;
    public UsePathSave()
    {
        path_map_PlayerActionMap_GamePlay = JsonManager.Instance.GetPath(JsonManager.path.Input, JsonManager.defaultName);
    }
}
