using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ResolutionSelector : MonoBehaviour
{
    [Header("UI")]
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI resolutionText;

    private Resolution[] resolutions;
    private int currentIndex;

    void Start()
    {
        // Résolutions uniques
        List<Resolution> unique = new List<Resolution>();

        foreach (var r in Screen.resolutions)
        {
            bool exists = unique.Exists(x => x.width == r.width && x.height == r.height);

            if (!exists)
                unique.Add(r);
        }

        resolutions = unique.ToArray();

        // Trouver la résolution actuelle
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
                break;
            }
        }

        // Lier automatiquement les boutons
        leftButton.onClick.AddListener(PreviousResolution);
        rightButton.onClick.AddListener(NextResolution);

        UpdateText();
    }

    void PreviousResolution()
    {
        currentIndex = (currentIndex - 1 + resolutions.Length) % resolutions.Length;
        ApplyResolution();
    }

    void NextResolution()
    {
        currentIndex = (currentIndex + 1) % resolutions.Length;
        ApplyResolution();
    }

    void ApplyResolution()
    {
        Resolution r = resolutions[currentIndex];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
        UpdateText();
    }

    void UpdateText()
    {
        Resolution r = resolutions[currentIndex];
        resolutionText.text = $"{r.width} x {r.height}";
    }
}
