using TMPro;
using UnityEngine;

public class VerticalSyncController : MonoBehaviour
{
    public TextMeshProUGUI label; void Start() { UpdateLabel(); }
    public void SwapVSync()
    {
        QualitySettings.vSyncCount = 1 - QualitySettings.vSyncCount;
        Debug.Log("VSync: " + QualitySettings.vSyncCount);
        UpdateLabel();
    }
    public void UpdateLabel() { label.text = QualitySettings.vSyncCount == 0 ? "OFF" : "ON"; }
}
