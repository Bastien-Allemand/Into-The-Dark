using System.Collections.Generic;
using UnityEngine;


public class ItemSpawner : MonoBehaviour
{
    [Header("Groups of objects for each night")]
    [Tooltip("Objects exclusives to night 1 (index 0)")]
    [SerializeField] private GameObject[] objectsNight1;

    [Tooltip("Objects exclusives to night 2 (index 1)")]
    [SerializeField] private GameObject[] objectsNight2;

    [Tooltip("Objects spawning from night 3 (index 2)")]
    [SerializeField] private GameObject[] objectsNight3;

    void Start()
    {
        int currentNight = GameManager.CurrentNightIndex;

        SetGroupActive(objectsNight1, false);
        SetGroupActive(objectsNight2, false);
        SetGroupActive(objectsNight3, false);

        if (currentNight == 0)
        {
            SetGroupActive(objectsNight1, true);
        }
        else if (currentNight == 1)
        {
            SetGroupActive(objectsNight2, true);
        }
        else if (currentNight >= 2)
        {
            SetGroupActive(objectsNight1, true);
            SetGroupActive(objectsNight2, true);
            SetGroupActive(objectsNight3, true);
        }
    }

    private void SetGroupActive(GameObject[] group, bool isActive)
    {
        if (group == null) return;
        foreach (GameObject obj in group)
        {
            if (obj != null) obj.SetActive(isActive);
        }
    }

    public List<GameObject> GetAllowedPrefabsForCurrentNight()
    {
        List<GameObject> allowed = new List<GameObject>();
        int currentNight = GameManager.CurrentNightIndex;

        if (currentNight == 0) // Nuit 1
        {
            allowed.AddRange(objectsNight1);
        }
        else if (currentNight == 1) // Nuit 2
        {
            allowed.AddRange(objectsNight2);
        }
        else if (currentNight >= 2) // Nuit 3 et +
        {
            allowed.AddRange(objectsNight1);
            allowed.AddRange(objectsNight2);
            allowed.AddRange(objectsNight3);
        }

        return allowed;
    }
}