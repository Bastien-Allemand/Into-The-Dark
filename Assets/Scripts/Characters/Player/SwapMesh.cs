using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class SwapMesh : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [SerializeField] private GameObject Night1;
    [SerializeField] private GameObject Night2;
    [SerializeField] private GameObject Night3;

    [SerializeField] private GameObject currentCharacter;

    private List<GameObject> NightList  = new List<GameObject>();
    private void Awake()
    {

        if (currentCharacter != null)
        {
            currentCharacter.SetActive(true);
        }
    }

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