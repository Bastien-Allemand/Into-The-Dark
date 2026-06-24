using UnityEngine;

public class Raycast : MonoBehaviour
{
    static public GameObject CheckRaycast(Camera _cam,float _range)
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * _range, Color.red, 2f);
        if (Physics.Raycast(ray, out RaycastHit hit, _range))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}

