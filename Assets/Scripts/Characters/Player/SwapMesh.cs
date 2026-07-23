using UnityEngine;

public class SwapMesh : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [SerializeField] private GameObject Night1;
    [SerializeField] private GameObject Night2;
    [SerializeField] private GameObject Night3;

    [SerializeField] private GameObject currentCharacter;

    public void Swap(int index)
    {
        if (currentCharacter != null)
            currentCharacter.SetActive(false);

        currentCharacter = index switch
        {
            1 => Night1,
            2 => Night2,
            3 => Night3,
            _ => currentCharacter
        };

        if (currentCharacter != null)
            currentCharacter.SetActive(true);

        playerStateMachine.RefreshAnimator();
    }
}