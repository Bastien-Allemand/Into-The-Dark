using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Night1 : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    private string sceneToLoad = "Gameplay";

    private void OnEnable()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped += PlayableDirector_Stopped;
        }
    }
    private void OnDisable()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= PlayableDirector_Stopped;
        }
    }
    private void PlayableDirector_Stopped(PlayableDirector obj)
    {
        if (obj == playableDirector)
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Attention : Le nom de la scène à charger est vide !");
            }
        }
    }
}