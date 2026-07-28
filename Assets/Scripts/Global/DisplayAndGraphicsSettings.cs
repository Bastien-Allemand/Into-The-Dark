using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DisplayAndGraphicsSettings : MonoBehaviour
{
    [Header("UI - Affichage")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private TMP_Dropdown fpsDropdown;

    [Header("UI - Graphismes")]
    [SerializeField] private TMP_Dropdown aaDropdown;
    [SerializeField] private Slider renderScaleSlider;
    [SerializeField] private TMP_Dropdown textureQualityDropdown;

    private Resolution[] availableResolutions;

    private void Start()
    {
        InitResolutions();
        LoadSettings();
    }

    #region --- AFFICHAGE ---

    private void InitResolutions()
    {
        if (resolutionDropdown == null) return;

        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + " x " + availableResolutions[i].height;
            options.Add(option);

            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedResIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResIndex);
        resolutionDropdown.value = savedResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        if (availableResolutions == null || index < 0 || index >= availableResolutions.Length) return;

        Resolution res = availableResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);

        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetVSync(bool isVSync)
    {
        QualitySettings.vSyncCount = isVSync ? 1 : 0;

        PlayerPrefs.SetInt("VSync", isVSync ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetFPSLimit(int index)
    {
        int[] fpsLimits = { 30, 60, 120, 144, -1 };
        int selectedFPS = fpsLimits[Mathf.Clamp(index, 0, fpsLimits.Length - 1)];

        Application.targetFrameRate = selectedFPS;

        PlayerPrefs.SetInt("FPSLimitIndex", index);
        PlayerPrefs.Save();
    }

    #endregion

    #region --- GRAPHISMES ---

    public void SetAntiAliasing(int index)
    {
        PlayerPrefs.SetInt("AntiAliasing", index);
        PlayerPrefs.Save();

        ApplyAntiAliasingToAllCameras(index);
    }

    public static void ApplyAntiAliasingToAllCameras(int aaIndex)
    {
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.TryGetComponent<CameraSyncException>(out var exception))
            {
                if (exception.ignoreAntiAliasing)
                    continue;
            }

            var cameraData = cam.GetComponent<UniversalAdditionalCameraData>();
            if (cameraData != null)
            {
                cameraData.antialiasing = (AntialiasingMode)aaIndex;
            }
        }
    }

    public void SetRenderScale(float value)
    {
        var urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        if (urpAsset != null)
        {
            urpAsset.renderScale = value;

            PlayerPrefs.SetFloat("RenderScale", value);
            PlayerPrefs.Save();
        }
    }

    public void SetTextureQuality(int index)
    {
        QualitySettings.globalTextureMipmapLimit = index;

        PlayerPrefs.SetInt("TextureQuality", index);
        PlayerPrefs.Save();
    }

    #endregion

    #region --- SAUVEGARDE & CHARGEMENT ---

    private void LoadSettings()
    {
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        if (fullscreenToggle != null) fullscreenToggle.SetIsOnWithoutNotify(isFullscreen);
        Screen.fullScreen = isFullscreen;

        bool isVSync = PlayerPrefs.GetInt("VSync", QualitySettings.vSyncCount > 0 ? 1 : 0) == 1;
        if (vsyncToggle != null) vsyncToggle.SetIsOnWithoutNotify(isVSync);
        QualitySettings.vSyncCount = isVSync ? 1 : 0;

        int fpsIndex = PlayerPrefs.GetInt("FPSLimitIndex", 1);
        if (fpsDropdown != null) fpsDropdown.SetValueWithoutNotify(fpsIndex);
        SetFPSLimit(fpsIndex);

        int aaIndex = PlayerPrefs.GetInt("AntiAliasing", 2);
        if (aaDropdown != null) aaDropdown.SetValueWithoutNotify(aaIndex);
        SetAntiAliasing(aaIndex);

        float renderScale = PlayerPrefs.GetFloat("RenderScale", 1.0f);
        if (renderScaleSlider != null) renderScaleSlider.SetValueWithoutNotify(renderScale);
        SetRenderScale(renderScale);

        int texIndex = PlayerPrefs.GetInt("TextureQuality", 0);
        if (textureQualityDropdown != null) textureQualityDropdown.SetValueWithoutNotify(texIndex);
        SetTextureQuality(texIndex);
    }

    #endregion
}