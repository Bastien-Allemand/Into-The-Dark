using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class Difficulty : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI difficulty;
    public int m_difficultyCount = 1;
    private void Start()
    {
        leftButton.onClick.AddListener(Previous);
        rightButton.onClick.AddListener(Next);


    }
    void Previous()
    {
        m_difficultyCount--;
        if (m_difficultyCount < 1)
        {
            m_difficultyCount = 1;
        }
        DisplayNight();
    }
    void Next()
    {
        m_difficultyCount++;
        if (m_difficultyCount > 3)
        {
            m_difficultyCount = 3;
        }
        DisplayNight();
    }
    void DisplayNight()
    {
        difficulty.text = "Difficulty : " + m_difficultyCount;
    }

}
