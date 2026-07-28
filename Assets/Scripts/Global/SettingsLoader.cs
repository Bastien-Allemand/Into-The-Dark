using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SettingsLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoInitialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        ApplyAllSettings();
    }
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyAllSettings();
    }
    public static void ApplyAllSettings()
    {
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        Screen.fullScreen = isFullscreen;
        Debug.Log(isFullscreen);

        int isVSync = PlayerPrefs.GetInt("VSync", QualitySettings.vSyncCount > 0 ? 1 : 0);
        QualitySettings.vSyncCount = isVSync;

        int fpsIndex = PlayerPrefs.GetInt("FPSLimitIndex", 1);
        int[] fpsLimits = { 30, 60, 120, 144, -1 };
        Application.targetFrameRate = fpsLimits[Mathf.Clamp(fpsIndex, 0, fpsLimits.Length - 1)];

        float renderScale = PlayerPrefs.GetFloat("RenderScale", 1.0f);
        var urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        if (urpAsset != null)
        {
            urpAsset.renderScale = renderScale;
        }

        int texIndex = PlayerPrefs.GetInt("TextureQuality", 0);
        QualitySettings.globalTextureMipmapLimit = texIndex;

        int aaIndex = PlayerPrefs.GetInt("AntiAliasing", 2);
        DisplayAndGraphicsSettings.ApplyAntiAliasingToAllCameras(aaIndex);
    }
}