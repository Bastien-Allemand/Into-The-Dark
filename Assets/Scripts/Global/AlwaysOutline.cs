using UnityEngine;

public class AlwaysOutline : MonoBehaviour
{
    [Tooltip("Glisse ton matériau d'outline ici")]
    public Material outlineMaterial;

    void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer == null) return;

        // On récupère le matériau de base (ton bleu transparent)
        Material[] originalMaterials = renderer.sharedMaterials;

        // On crée une liste de 2 slots
        Material[] newMaterials = new Material[originalMaterials.Length + 1];

        // Slot 0 = Ton bleu transparent
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            newMaterials[i] = originalMaterials[i];
        }

        // Slot 1 = L'outline
        newMaterials[newMaterials.Length - 1] = outlineMaterial;

        // On applique le tout
        renderer.materials = newMaterials;
    }
}