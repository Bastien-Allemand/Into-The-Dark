using UnityEngine;

public class PlayCinematic : MonoBehaviour
{
    [SerializeField] private string scene1;
    [SerializeField] private string scene2;
    [SerializeField] private string scene3;
    [SerializeField] private NightSelector nightSelector;

    public void PlayScene()
    {
        switch (nightSelector.m_nightCount)
        {
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene1);
                break;
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene2);
                break;
            case 3:
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene3);
                break;
            default:
                Debug.LogError("Invalid night selected.");
                break;
        }
    }
}
