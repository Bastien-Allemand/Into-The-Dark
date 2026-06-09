using UnityEngine;

[ExecuteInEditMode]
public class MirrorReflection : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera mirrorCamera;

    private void LateUpdate()
    {
        if (playerCamera == null || mirrorCamera == null) return;

        // L'avant du miroir
        Vector3 mirrorNormal = transform.forward;
        Plane mirrorPlane = new Plane(mirrorNormal, transform.position);

        // Position du joueur
        Vector3 playerPos = playerCamera.transform.position;
        float distance = mirrorPlane.GetDistanceToPoint(playerPos);

        // La caméra est maintenant projetée directement SUR le plan du miroir.
        mirrorCamera.transform.position = playerPos - distance * mirrorNormal;

        Vector3 playerForward = playerCamera.transform.forward;
        Vector3 reflectedForward = Vector3.Reflect(playerForward, mirrorNormal);
        Vector3 playerUp = playerCamera.transform.up;
        Vector3 reflectedUp = Vector3.Reflect(playerUp, mirrorNormal);
        mirrorCamera.transform.rotation = Quaternion.LookRotation(reflectedForward, reflectedUp);

    }
}