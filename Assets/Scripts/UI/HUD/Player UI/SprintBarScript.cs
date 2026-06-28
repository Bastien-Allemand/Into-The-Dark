using UnityEngine;

public class SprintBarScript : UI
{
    [SerializeField] private RectTransform sprintBarTransform;
    [SerializeField] private PlayerStateMachine playerStateMachine;
    private float sprintBarInitialWidth;
    void Start()
    {
        sprintBarInitialWidth = sprintBarTransform.rect.width;
    }

    void Update()
    {
        UpdateSprintUI();
    }

    void UpdateSprintUI()
    {
        if (sprintBarTransform == null) return;

        float percentLeft = playerStateMachine.StaminaRatio;
        sprintBarTransform.sizeDelta = new Vector2(sprintBarInitialWidth * percentLeft, sprintBarTransform.rect.height);
    }
}
