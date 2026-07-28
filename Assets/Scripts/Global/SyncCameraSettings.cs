using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SyncCameraSettings : MonoBehaviour
{
    private void OnEnable()
    {
        if (TryGetComponent<CameraSyncException>(out var exception))
        {
            if (exception.ignoreAntiAliasing)
                return;
        }

        int savedAA = PlayerPrefs.GetInt("AntiAliasing", 2);

        var cameraData = GetComponent<UniversalAdditionalCameraData>();
        if (cameraData != null)
        {
            cameraData.antialiasing = (AntialiasingMode)savedAA;
        }
    }
}