using UnityEngine;

public class Raycast : MonoBehaviour
{
    public class RaycastResult
    {
        public GameObject hitObject;
        public Vector3 hitPoint;
        public Vector3 hitNormal;
        public float hitDistance;
        public RaycastResult(GameObject _hitObject, Vector3 _hitPoint, Vector3 _hitNormal, float _hitDistance)
        {
            hitObject = _hitObject;
            hitPoint = _hitPoint;
            hitNormal = _hitNormal;
            hitDistance = _hitDistance;
        }
    }
    static public RaycastResult CheckRaycast(Camera _cam,float _range)
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * _range, Color.red, 2f);
        if (Physics.Raycast(ray, out RaycastHit hit, _range))
        {
            return new RaycastResult(hit.collider.gameObject, hit.point, hit.normal, hit.distance);
        }
        return null;
    }

    static public RaycastResult CheckBoxCast(Transform _transform, Vector3 _extents
        , Vector3 _dir, Quaternion _orientation,float _range)
    {
        Vector3 origin = _transform.position;
        Vector3 halfExtents = _extents * .5f;


        Debug.DrawRay(origin, _dir * _range, Color.red, 2f);
        if (Physics.BoxCast(origin, halfExtents, _dir, out RaycastHit hit, _orientation, _range))
        {
            return new RaycastResult(hit.collider.gameObject, hit.point, hit.normal, hit.distance);
        }
        return null;
    }


}

