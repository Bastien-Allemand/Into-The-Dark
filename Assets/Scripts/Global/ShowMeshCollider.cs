using UnityEngine;

[ExecuteInEditMode]
public class ShowMeshCollider : MonoBehaviour
{
    public Color gizmoColor = new Color(0f, 1f, 0f, 0.4f); 
    public bool drawWireframe = true;

    private MeshCollider meshCollider;

    void OnDrawGizmos()
    {
        if (meshCollider == null)
            meshCollider = GetComponent<MeshCollider>();

        if (meshCollider != null && meshCollider.sharedMesh != null)
        {
            Gizmos.color = gizmoColor;
            Gizmos.matrix = transform.localToWorldMatrix; 

            if (drawWireframe)
            {
                Gizmos.DrawWireMesh(meshCollider.sharedMesh);
            }
            else
            {
                Gizmos.DrawMesh(meshCollider.sharedMesh);
            }
        }
    }
}