using UnityEngine;

[CreateAssetMenu(fileName = "NouvelleDifficulte", menuName = "Horreur/Configuration Difficulte")]
public class DifficultySettings : ScriptableObject
{
    [Header("Statistiques du Joueur")]
    [Tooltip("Vitesse de récupération de la stamina")]
    public float recuperationStamina = 5f;

    [Tooltip("Vitesse de récupération de la folie")]
    public float recuperationFolie = 2f;

    [Header("Batteries des Équipements")]
    public float tailleBatterieCamera = 100f;
    public float tailleBatteriePhone = 100f;
    public float tailleBatterieDrone = 100f;

    [Header("Paramètres du Fantôme")]
    [Tooltip("Champ de vision (FOV) de la caméra du ghost")]
    public float fovCamGhost = 44.2f;
    public float nearCamGhost = 0.3f;
    public float farCamGhost = 13.6f;
}