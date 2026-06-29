using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UsePathSave
{
    public List<string> paths = new List<string>();
    public UsePathSave() { }
    public void Default()
    {
        paths.Clear();
        for (int i = 0; i < (int)JsonManager.path.Use; i++)
        {
            if ((JsonManager.path)i != JsonManager.path.Use)
            {
                paths.Add(JsonManager.Instance.GetPath((JsonManager.path)i, JsonManager.defaultName));
            }
        }
    }
}
