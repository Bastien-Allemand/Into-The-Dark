using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScene : MonoBehaviour
{
    public Difficulty difficultyScript;
    public NightSelector nightScript;
    public void LoadGame()
    {
        //difficultyScript.m_difficultyCount;
        //nightScript.m_nightCount;
        //use info to set difficulty and night.

        SceneManager.LoadScene("GamePlay");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
