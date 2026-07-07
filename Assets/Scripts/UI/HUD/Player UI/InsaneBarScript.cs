using UnityEngine;

public class InsaneBarScript : UI
{

    [SerializeField] private RectTransform insaneMeterTransform;
    [SerializeField] private InsaneMeterScript insaneMeterScript;
    private float initalInsaneMeterWidth;
    void Start()
    {
        if (insaneMeterTransform != null)
        {
            initalInsaneMeterWidth = insaneMeterTransform.rect.width;
        }
    }

    void Update()
    {
        UpdateInsaneUI();
    }

    void UpdateInsaneUI()
    {
        if (insaneMeterTransform == null) return;
        float percentLeft = insaneMeterScript.insaneMeterRatio;
        insaneMeterTransform.sizeDelta = new Vector2(initalInsaneMeterWidth * percentLeft, insaneMeterTransform.rect.height);
    }
}
