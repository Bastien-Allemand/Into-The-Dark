using UnityEngine;

public class SettingMenu : MonoBehaviour
{

    [SerializeField] public GameObject BackGroundUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        BackGroundUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateSettingUI()
    {
        BackGroundUI.SetActive(true);
    }

    public void EscapeButton()
    {
        BackGroundUI.SetActive(false);
        GetComponent<PauseMenu>().EscapeFunction();
    }
}
