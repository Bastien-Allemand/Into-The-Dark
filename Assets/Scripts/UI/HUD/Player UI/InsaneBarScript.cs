using UnityEngine;

public class InsaneBarScript : UI
{
    GameManager.NightData? night;
    [SerializeField] private RectTransform insaneMeterTransform;
    [SerializeField] private InsaneMeterScript insaneMeterScript;
    private float initalInsaneMeterWidth;
    private bool InGame = false;
    void Awake()
    {
        if (!night.HasValue)
        {
            InGame = false;
            return;
        }
        insaneMeterScript = night.Value.nightGO.GetComponent<InsaneMeterScript>();
        if (insaneMeterTransform != null)
        {
            initalInsaneMeterWidth = insaneMeterTransform.rect.width;
        }
    }

    void Update()
    {
        if (!InGame)
            return;
        UpdateInsaneUI();
    }

    void UpdateInsaneUI()
    {
        if (insaneMeterTransform == null) return;
        float percentLeft = insaneMeterScript.insaneMeterRatio;
        insaneMeterTransform.sizeDelta = new Vector2(initalInsaneMeterWidth * percentLeft, insaneMeterTransform.rect.height);
    }
    public void ActivateUI(GameManager.NightData nightSelection)
    {
        night = nightSelection;
    }
}
