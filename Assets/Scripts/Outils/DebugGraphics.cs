using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DebugGraphics : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("=== VERIFICATION GRAPHISMES SCENE B ===");
        Debug.Log($"• FPS Cible : {Application.targetFrameRate} (-1 = Illimité)");
        Debug.Log($"• VSync : {(QualitySettings.vSyncCount > 0 ? "Activé" : "Désactivé")}");
        Debug.Log($"• Résolution : {Screen.width}x{Screen.height} (Plein Écran : {Screen.fullScreen})");
        Debug.Log($"• Qualité Textures : Mipmap Limit = {QualitySettings.globalTextureMipmapLimit} (0 = Élevée)");

        var urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        if (urpAsset != null)
        {
            Debug.Log($"• Render Scale URP : {urpAsset.renderScale}");
        }

        Camera mainCam = Camera.main;
        if (mainCam != null && mainCam.TryGetComponent<UniversalAdditionalCameraData>(out var camData))
        {
            Debug.Log($"• Anti-Aliasing Caméra : {camData.antialiasing}");
        }
        else
        {
            Debug.LogWarning("Caméra ou UniversalAdditionalCameraData introuvable !");
        }
    }
}