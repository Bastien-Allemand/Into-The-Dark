using UnityEngine;

public class ViewModelFollow : MonoBehaviour
{
    [SerializeField] private Transform targetCamera;

    void LateUpdate()
    {
        transform.position = targetCamera.position;
        transform.rotation = targetCamera.rotation;
    }
}