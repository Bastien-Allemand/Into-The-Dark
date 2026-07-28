using UnityEngine;

public class SettingMenu : MonoBehaviour
{

    [SerializeField] public GameObject OptionSetting;
    [SerializeField] public GameObject soundSettingUI;
    [SerializeField] public GameObject keyboardSettingUI;
    [SerializeField] public GameObject graphicsSettingUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        OptionSetting.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
