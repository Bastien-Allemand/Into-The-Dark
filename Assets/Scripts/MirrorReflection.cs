using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class MirrorReflection : MonoBehaviour
{
    [Header("Références")]
    public Camera reflectionCamera;

    [Header("Paramètres")]
    public int textureSize = 1024;
    [Tooltip("Cochez si c'est un Quad Unity. Décochez si c'est un Plane.")]
    public bool isQuad = true;

    private RenderTexture reflectionTexture;
    private Renderer mirrorRenderer;

    private void OnEnable()
    {
        mirrorRenderer = GetComponent<Renderer>();
        RenderPipelineManager.beginCameraRendering += UpdateReflectionCamera;
        CreateRenderTexture();
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= UpdateReflectionCamera;
        if (reflectionTexture)
        {
            if (reflectionCamera) reflectionCamera.targetTexture = null;
            RenderTexture.active = null;
            reflectionTexture.Release();
            DestroyImmediate(reflectionTexture);
        }
    }

    private void CreateRenderTexture()
    {
        if (reflectionTexture == null || reflectionTexture.width != textureSize)
        {
            if (reflectionTexture)
            {
                if (reflectionCamera) reflectionCamera.targetTexture = null;
                reflectionTexture.Release();
            }

            reflectionTexture = new RenderTexture(textureSize, textureSize, 24);
            reflectionTexture.name = "MirrorTexture_" + GetInstanceID();
            reflectionTexture.hideFlags = HideFlags.DontSave;

            if (reflectionCamera) reflectionCamera.targetTexture = reflectionTexture;

            if (mirrorRenderer && mirrorRenderer.sharedMaterial)
            {
                mirrorRenderer.sharedMaterial.SetTexture("_BaseMap", reflectionTexture);
                mirrorRenderer.sharedMaterial.SetTexture("_MainTex", reflectionTexture);
            }
        }
    }

    private void UpdateReflectionCamera(ScriptableRenderContext context, Camera mainCam)
    {
        if (!reflectionCamera || !mirrorRenderer || !mirrorRenderer.isVisible) return;

        // SÉCURITÉ 1 : Éviter la boucle infinie et ignorer les caméras de preview/shadows
        if (mainCam == reflectionCamera ||
            mainCam.cameraType == CameraType.Reflection ||
            mainCam.cameraType == CameraType.Preview) return;

        // SÉCURITÉ 2 : En jeu, on ne veut refléter QUE la caméra principale du joueur
        if (Application.isPlaying && mainCam != Camera.main) return;

        // 1. Définir la normale du miroir (la direction vers laquelle le miroir regarde)
        // Un Quad Unity regarde vers -transform.forward
        Vector3 mirrorNormal = isQuad ? -transform.forward : transform.up;
        Vector3 mirrorPos = transform.position;

        // SÉCURITÉ 3 : Si la caméra est DERRIÈRE le miroir, on ne calcule rien (évite les rendus bizarres)
        Vector3 camToMirror = mainCam.transform.position - mirrorPos;
        if (Vector3.Dot(camToMirror, mirrorNormal) < 0) return;

        // 2. Calcul de la Position réfléchie (Symétrie parfaite)
        float distance = Vector3.Dot(mirrorNormal, camToMirror);
        reflectionCamera.transform.position = mainCam.transform.position - 2 * distance * mirrorNormal;

        // 3. Calcul de la Rotation réfléchie
        Vector3 reflectForward = Vector3.Reflect(mainCam.transform.forward, mirrorNormal);
        Vector3 reflectUp = Vector3.Reflect(mainCam.transform.up, mirrorNormal);
        reflectionCamera.transform.rotation = Quaternion.LookRotation(reflectForward, reflectUp);

        // 4. Synchronisation optique
        reflectionCamera.fieldOfView = mainCam.fieldOfView;
        reflectionCamera.aspect = mainCam.aspect;

        // 5. Matrice de projection oblique (Plan de découpe pour masquer l'arrière du miroir)
        Vector4 clipPlane = CameraSpacePlane(reflectionCamera, mirrorPos, mirrorNormal, 1.0f);
        reflectionCamera.projectionMatrix = mainCam.CalculateObliqueMatrix(clipPlane);
    }

    private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
    {
        Vector3 offsetPos = pos + normal * 0.01f; // Léger décalage pour éviter le Z-fighting
        Matrix4x4 m = cam.worldToCameraMatrix;
        Vector3 cpos = m.MultiplyPoint(offsetPos);
        Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;
        return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
    }
}