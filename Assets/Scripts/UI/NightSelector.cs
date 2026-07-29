using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class NightSelector : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public TextMeshProUGUI Night;
    public int m_nightCount = 1;
    private void Start()
    {
        leftButton.onClick.AddListener(Previous);
        rightButton.onClick.AddListener(Next);

    }
    void Previous()
    {
        m_nightCount--;
        if (m_nightCount < 1)
        {
            m_nightCount = 1;
        }
        DisplayNight();
    }
    void Next()
    {
        m_nightCount++;
        if (m_nightCount > 3)
        {
            m_nightCount = 3;
        }
        DisplayNight();
    }
    void DisplayNight()
    {
        Night.text = "Night : " + m_nightCount;
    }

}
