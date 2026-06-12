using UnityEngine;
using UnityEngine.InputSystem;

public static class JsonManager
{
    private static PlayerAction controls => InputManager.controls;
    public static void SaveInputInJson()
    {
        InputActionMap map = controls.GamePlay;
        string json = map.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("Bindings", json);
        PlayerPrefs.Save();
    }
    public static void LoadInputFromJson(string jsonName)
    {
        if (PlayerPrefs.HasKey(jsonName))
        {
            Debug.Log($"Input preference : {jsonName}");
            Debug.Log("loading ...");

            string json = PlayerPrefs.GetString("Bindings");
            controls.asset.LoadBindingOverridesFromJson(json);

            Debug.Log("Input preference Loaded");

        }
        else
        {
            Debug.Log($"No Json name {jsonName}");
        }
    }
}
