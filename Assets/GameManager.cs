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
    [SerializeField] private GameObject transitionText;

    [SerializeField] private TextMeshProUGUI gameOverReasonText;

    [Header("Timer Settings")]
    [SerializeField] private float timeToSurvive = 5f;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Story Transition Settings")]
    [SerializeField] private float timeToWaitBeforeNextScene = 3f;
    //[SerializeField] private string nextSceneName = "NextSceneHistoire";

    [Header("Glitch Effect Settings")]
    [SerializeField] private float glitchTriggerTime = 0.5f;
    [SerializeField] private float minBlinkDelay = 0.05f;
    [SerializeField] private float maxBlinkDelay = 0.25f;

    [Header("Timer Polish Settings")]
    [SerializeField] private float startBlinkingAt = 10f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;

    private float currentTime;
    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isTransitioning = false;


    // Coroutine reference to ensure we don't start multiple blinking loops
    private Coroutine blinkCoroutine;

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

    public void TogglePause()
    {
        if (isGameOver || isTransitioning) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }


    private void HandleTimer()
    {

        if (isTransitioning || isGameOver || isPaused)
        {
            if ((isTransitioning || isGameOver) && timerText != null)
            {
                timerText.gameObject.SetActive(false);
            }
            return;
        }


        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (timerText != null && !timerText.gameObject.activeSelf)
            {
                timerText.gameObject.SetActive(true);
            }
        }
        else
        {
            currentTime = 0;
            TriggerStoryTransition();
        }

        UpdateTimerUI();

        HandleBlinkTrigger();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Minutes and secondes
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            // Format MM:SS
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void HandleBlinkTrigger()
    {
        if (timerText == null) return;

        if (currentTime <= startBlinkingAt && currentTime > 0 && !isTransitioning && !isGameOver)
        {
            if (blinkCoroutine == null)
            {
                // Smooth transition to red initially
                blinkCoroutine = StartCoroutine(BlinkTimerRed());
            }
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;

                timerText.color = normalColor;
            }
        }
    }

    private IEnumerator BlinkTimerRed()
    {
        float duration = 1f;

        while (true)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (isTransitioning || isGameOver) yield break;

                elapsed += Time.unscaledDeltaTime;

                float t = (Mathf.Sin(elapsed * Mathf.PI * 2f / duration) + 1f) / 2f;

                if (timerText != null)
                {
                    timerText.color = Color.Lerp(normalColor, warningColor, t);
                }

                yield return null;
            }
        }
    }

    public void TriggerStoryTransition()
    {
        if (isGameOver || isTransitioning) return;

        isTransitioning = true;

        // Unable timer once transition starts
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false);
        }

        if (transitionPanel != null)
        {
            transitionPanel.SetActive(true);
        }

        if (transitionText != null)
        {
            transitionText.SetActive(true);
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

        if (gameOverReasonText != null)
        {
            switch (cause)
            {
                case deathCause.Caught:
                    gameOverReasonText.text = "Vous avez été attrapé...";
                    break;

                case deathCause.Insanity:
                    gameOverReasonText.text = "Votre esprit a sombré dans la folie profonde.";
                    break;

                default:
                    gameOverReasonText.text = "Vous avez péri.";
                    break;
            }
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(true);

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
