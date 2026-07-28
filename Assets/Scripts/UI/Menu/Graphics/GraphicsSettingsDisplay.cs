using UnityEngine;
using TMPro;

public class GraphicsSettingsDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text settingsText;

    private void Start()
    {
        DisplayCurrentSettings();
    }

    public void DisplayCurrentSettings()
    {
        // 1. Get current Quality Preset name
        string currentQuality = QualitySettings.names[QualitySettings.GetQualityLevel()];

        // 2. Get current Screen Resolution & Refresh Rate
        Resolution res = Screen.currentResolution;
        string resolutionStr = $"{Screen.width} x {Screen.height} @ {res.refreshRateRatio.value:F0}Hz";

        // 3. Get Display Mode & VSync
        string displayMode = Screen.fullScreenMode.ToString();
        string vsyncStatus = QualitySettings.vSyncCount > 0 ? "On" : "Off";

        // 4. Update UI Text
        settingsText.text = $"<b>Current Graphics Settings:</b>\n" +
                            $"• Quality Level: {currentQuality}\n" +
                            $"• Resolution: {resolutionStr}\n" +
                            $"• Display Mode: {displayMode}\n" +
                            $"• VSync: {vsyncStatus}";
    }
}