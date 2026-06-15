using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject transitionPanel;

    [Header("Timer Settings")]
    [SerializeField] private float timeToSurvive = 60f;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Story Transition Settings")]
    [SerializeField] private float timeToWaitBeforeNextScene = 3.5f;
    [SerializeField] private string nextSceneName = "NextSceneHistoire";

    private float currentTime;
    private bool isGameOver = false;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = timeToSurvive;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (transitionPanel != null) transitionPanel.SetActive(false);
    }

    private void HandleTimer()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(currentTime / 60);
                int seconds = Mathf.FloorToInt(currentTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
        else
        {
            currentTime = 0;
            TriggerStoryTransition();
        }
    }

    public void TriggerStoryTransition()
    {
        if (isGameOver || isTransitioning) return;

        isTransitioning = true;
        Debug.Log("Transition en cours.");

        if (transitionPanel != null)
        {
            transitionPanel.SetActive(true);
        }

        StartCoroutine(WaitAndLoadNextScene());
    }

    private IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(timeToWaitBeforeNextScene);

        if (transitionPanel != null)
        {
            transitionPanel.SetActive(false);
        }

        //SceneManager.LoadScene(nextSceneName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TriggerGameOver(deathCause cause)
    {
        if (isGameOver || isTransitioning) return;

        isGameOver = true;
        Debug.Log("Game Over. Cause : " + cause);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    // Update is called once per frame
    void Update()
    {
        if (isGameOver || isTransitioning) return;

        HandleTimer();
    }
}
