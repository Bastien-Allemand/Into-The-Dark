using UnityEngine;

public class Bouton : MonoBehaviour
{
    UIManager uiManager;
    [SerializeField] Transform target;
    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }
    public void ExitPauseMenu()
    {
        target.GetComponent<PauseMenu>().exit = true;
        Debug.Log(target.GetComponent<PauseMenu>().exit);
    }
    public void ExitPause()
    {
        uiManager.Pause(false);
    }
    public void Hide()
    {
        target.gameObject.SetActive(false);
    }
    public void SettingPanelShowOnly()
    {
        Transform parent = GameObject.Find("SettingPanel").transform;
        foreach(Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }
        target.gameObject.SetActive(true);
    }
}
