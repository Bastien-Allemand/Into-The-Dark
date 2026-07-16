using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[ItemSpawner] GameManager instance not found in the scene.");
            return;
        }

        int currentChapter = GameManager.CurrentChapterIndex;
        int currentNight = GameManager.CurrentNightIndex;

        var nightsInChapter = GameManager.Instance.ChaptersSequence[currentChapter].nights;
        foreach (var night in nightsInChapter)
        {
            SetGroupActive(night.objectsToSpawn, false);
        }

        if (currentNight >= 0 && currentNight < nightsInChapter.Count)
        {
            for (int i = 0; i <= currentNight; i++)
            {
                SetGroupActive(nightsInChapter[i].objectsToSpawn, true);
            }
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

        if (GameManager.Instance == null) return allowed;

        int currentChapter = GameManager.CurrentChapterIndex;
        int currentNight = GameManager.CurrentNightIndex;
        var nightsInChapter = GameManager.Instance.ChaptersSequence[currentChapter].nights;

        if (currentNight >= 0 && currentNight < nightsInChapter.Count)
        {
            for (int i = 0; i <= currentNight; i++)
            {
                if (nightsInChapter[i].objectsToSpawn != null)
                {
                    allowed.AddRange(nightsInChapter[i].objectsToSpawn);
                }
            }
        }

        return allowed;
    }
}