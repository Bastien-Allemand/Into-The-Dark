using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int CurrentChapterIndex => currentChapterIndex;
    public static int CurrentNightIndex => currentNightIndex;

    [System.Serializable]

    public struct CharacterData
    {
        public GameObject PlayerObject;
        public Camera cam;
        public GameObject rightHand;
        public GameObject leftHand;

    }
    public struct NightData
    {
        public GameObject nightGO;
        public string nightName;
        public float duration;
        public bool changeScene;
        public string sceneToLoad;
        [Tooltip("Objects that will activate or spawn for this specific night")]
        public GameObject[] objectsToSpawn;
        public CharacterData characterOfTheNight;
    }

    [System.Serializable]
    public struct ChapterData
    {
        public string chapterName;
        [TextArea(3, 10)] public string chapterIntroText;
        public List<NightData> nights;
    }
    [Header("Night")]
    [SerializeField] public GameObject NightManager;

    [Header("Debug / Sandbox Settings")]
    [SerializeField] private bool infiniteMode = false;
    [SerializeField] private bool keepTimerInInfiniteMode = true;

    [Header("Story Progression")]
    [SerializeField] private List<ChapterData> chaptersSequence = new List<ChapterData>();
    [SerializeField] private List<NightData> NightSequence = new List<NightData>();
    [Header("Actual Difficulty")]
    public DifficultySettings activeDifficulty;

    [Header("Bank of Difficulties")]
    public DifficultySettings modeEasy;
    public DifficultySettings modeNormal;
    public DifficultySettings modeHard;

    [Header("Sequence Informations Settings")]
    [SerializeField] private TextMeshProUGUI nightNameText;
    [SerializeField] private TextMeshProUGUI timerText;
    private float timeToSurvive;

    private static int currentChapterIndex = 0;
    private static int currentNightIndex = 0;
    private static string activeSceneName = "";

    [Header("UI Panels (Nights)")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private GameObject transitionText;

    [Header("UI Panels (Chapters Intro)")]
    [SerializeField] private GameObject chapterTransitionPanel;
    [SerializeField] private TextMeshProUGUI chapterTitleText;
    [SerializeField] private TextMeshProUGUI chapterIntroTextUI;

    [SerializeField] private TextMeshProUGUI gameOverReasonText;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;

    [Header("Story Transition Settings")]
    [SerializeField] private float timeToWaitBeforeNextScene = 3f;
    [SerializeField] private float timeToWaitChapterTransition = 6f;

    [Header("Glitch Effect Settings")]
    [SerializeField] private float glitchTriggerTime = 0.5f;
    [SerializeField] private float minBlinkDelay = 0.05f;
    [SerializeField] private float maxBlinkDelay = 0.25f;

    [Header("Timer Polish Settings")]
    [SerializeField] private float startBlinkingAt = 10f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;

    [Header("Game Over Polish")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float textTypeSpeed = 0.05f;

    [Header("End Game Settings")]
    [SerializeField] private string endgameSceneName = "MainMenu";

    private float currentTime;
    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isTransitioning = false;

    private Coroutine blinkCoroutine;
    public NightData actualNight;
    public List<ChapterData> ChaptersSequence => chaptersSequence;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (activeDifficulty == null)
        {
            activeDifficulty = modeNormal;
        }
    }

    void Start()
    {
        if (infiniteMode)
        {
            activeSceneName = SceneManager.GetActiveScene().name;
            currentTime = (chaptersSequence != null && chaptersSequence.Count > 0 && chaptersSequence[0].nights.Count > 0)
                ? chaptersSequence[0].nights[0].duration
                : 300f;

            if (nightNameText != null) nightNameText.text = "Infinite / Sandbox";
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            if (transitionPanel != null) transitionPanel.SetActive(false);
            if (gameOverCanvasGroup != null) gameOverCanvasGroup.alpha = 0f;

            if (!keepTimerInInfiniteMode && timerText != null)
                timerText.gameObject.SetActive(false);

            return;
        }

        if (chaptersSequence == null || chaptersSequence.Count == 0 || chaptersSequence[currentChapterIndex].nights.Count == 0)
        {
            Debug.LogWarning("Empty chapter structure !");
            GenerateDefaultData();
        }

        if (currentChapterIndex >= chaptersSequence.Count) currentChapterIndex = chaptersSequence.Count - 1;
        if (currentNightIndex >= chaptersSequence[currentChapterIndex].nights.Count) currentNightIndex = chaptersSequence[currentChapterIndex].nights.Count - 1;

        ChapterData currentChapter = chaptersSequence[currentChapterIndex];
        NightData currentNight = currentChapter.nights[currentNightIndex];

        timeToSurvive = currentNight.duration;
        currentTime = timeToSurvive;

        DetermineActiveScene();

        if (nightNameText != null)
        {
            nightNameText.text = $"{currentChapter.chapterName} - {currentNight.nightName}";
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (transitionPanel != null) transitionPanel.SetActive(false);
        if (gameOverCanvasGroup != null) gameOverCanvasGroup.alpha = 0f;

        if (currentChapterIndex == 0 && currentNightIndex == 0)
        {
            StartCoroutine(StartGameWithIntroSequence(currentChapter));
        }
        ActivateGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != endgameSceneName)
        {
            isPaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void GenerateDefaultData()
    {
        GameObject nightManager = NightManager.GetComponentInChildren<Transform>().gameObject;

        int nightCount = 1;

        foreach (Transform nightTransform in nightManager.transform)
        {
            GameObject nightGO = nightTransform.gameObject;

            Transform characterRoot = nightTransform.GetChild(0);

            CharacterData defaultCharacter = new CharacterData
            {
                PlayerObject = characterRoot.GetChild(0).gameObject,
                cam = characterRoot.GetChild(1).GetComponent<Camera>(),
                rightHand = characterRoot.GetChild(2).gameObject,
                leftHand = characterRoot.GetChild(3).gameObject,
            };

            NightData defaultNight = new NightData
            {
                nightGO = nightGO,
                nightName = "Nuit " + nightCount,
                duration = 300f,
                changeScene = true,
                sceneToLoad = SceneManager.GetActiveScene().name,
                objectsToSpawn = new GameObject[0],
                characterOfTheNight = defaultCharacter
            };

            NightSequence.Add(defaultNight);
            nightCount++;
        }

        ChapterData defaultChapter = new ChapterData
        {
            chapterName = "Chapitre 1",
            nights = NightSequence
        };

        chaptersSequence = new List<ChapterData> { defaultChapter };
    }

    private void DetermineActiveScene()
    {
        int ch = currentChapterIndex;
        int n = currentNightIndex;

        while (ch >= 0)
        {
            while (n >= 0)
            {
                NightData night = chaptersSequence[ch].nights[n];
                if (night.changeScene && !string.IsNullOrEmpty(night.sceneToLoad))
                {
                    activeSceneName = night.sceneToLoad;
                    return;
                }
                n--;
            }
            ch--;
            if (ch >= 0) n = chaptersSequence[ch].nights.Count - 1;
        }

        activeSceneName = SceneManager.GetActiveScene().name;
    }

    public void PauseGame()
    {
        if (isGameOver || isTransitioning) return;
        isPaused = true;
        Time.timeScale = 0f;
        if (timerText != null) timerText.gameObject.SetActive(false);
        if (nightNameText != null) nightNameText.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        if (isGameOver || isTransitioning) return;
        isPaused = false;
        Time.timeScale = 1f;
        if (timerText != null) timerText.gameObject.SetActive(true);
        if (nightNameText != null) nightNameText.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.pause = false;
    }

    private void HandleTimer()
    {
        if (isTransitioning || isGameOver || isPaused)
        {
            if (isTransitioning || isGameOver)
            {
                if (timerText != null) timerText.gameObject.SetActive(false);
                if (nightNameText != null) nightNameText.gameObject.SetActive(false);
            }
            return;
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (timerText != null && !timerText.gameObject.activeSelf && (!infiniteMode || keepTimerInInfiniteMode))
                timerText.gameObject.SetActive(true);
            if (nightNameText != null && !nightNameText.gameObject.activeSelf)
                nightNameText.gameObject.SetActive(true);
        }
        else
        {
            currentTime = 0;

            if (infiniteMode)
            {
                UpdateTimerUI();
                return;
            }

            TriggerStoryTransition();
        }

        UpdateTimerUI();
        HandleBlinkTrigger();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
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

    private IEnumerator StartGameWithIntroSequence(ChapterData firstChapterData)
    {
        isTransitioning = true;

        if (timerText != null) timerText.gameObject.SetActive(false);
        if (nightNameText != null) nightNameText.gameObject.SetActive(false);

        yield return StartCoroutine(ShowChapterIntroductionSequence(firstChapterData));

        isTransitioning = false;

        if (timerText != null) timerText.gameObject.SetActive(true);
        if (nightNameText != null) nightNameText.gameObject.SetActive(true);
    }

    public void TriggerStoryTransition()
    {
        if (isGameOver || isTransitioning) return;
        isTransitioning = true;

        if (timerText != null) timerText.gameObject.SetActive(false);
        if (nightNameText != null) nightNameText.gameObject.SetActive(false);

        if (transitionPanel != null) transitionPanel.SetActive(true);
        if (transitionText != null) transitionText.SetActive(true);

        StartCoroutine(WaitAndLoadNextScene());
    }

    private IEnumerator WaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(timeToWaitBeforeNextScene);
        if (transitionPanel != null) transitionPanel.SetActive(false);

        bool hasNewChapter = false;
        bool isGameFullyFinished = false;

        if (currentNightIndex < chaptersSequence[currentChapterIndex].nights.Count - 1)
        {
            currentNightIndex++;
        }
        else if (currentChapterIndex < chaptersSequence.Count - 1)
        {
            currentChapterIndex++;
            currentNightIndex = 0;
            hasNewChapter = true;
        }
        else
        {
            isGameFullyFinished = true;
        }

        if (isGameFullyFinished)
        {
            if (!string.IsNullOrEmpty(endgameSceneName))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(endgameSceneName);
            }
            else
            {
                Debug.LogError("'endgameSceneName' is empty !");
            }
            yield break;
        }

        NightData nextNight = chaptersSequence[currentChapterIndex].nights[currentNightIndex];
        if (nextNight.changeScene && !string.IsNullOrEmpty(nextNight.sceneToLoad))
        {
            activeSceneName = nextNight.sceneToLoad;
        }
        else
        {
            DetermineActiveScene();
        }

        if (hasNewChapter)
        {
            yield return StartCoroutine(ShowChapterIntroductionSequence(chaptersSequence[currentChapterIndex]));
        }

        LoadCalculatedScene();
    }

    private IEnumerator ShowChapterIntroductionSequence(ChapterData dynamicChapterData)
    {
        if (chapterTransitionPanel == null) yield break;

        if (chapterTitleText != null) chapterTitleText.text = dynamicChapterData.chapterName;
        if (chapterIntroTextUI != null) chapterIntroTextUI.text = dynamicChapterData.chapterIntroText;

        chapterTransitionPanel.SetActive(true);
        yield return new WaitForSeconds(timeToWaitChapterTransition);
        chapterTransitionPanel.SetActive(false);
    }

    private void LoadCalculatedScene()
    {
        if (!string.IsNullOrEmpty(activeSceneName))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(activeSceneName);
        }
        else
        {
            Debug.LogError("Technical issue : the name of the active scene is empty !");
        }
    }

    public void TriggerGameOver(deathCause cause)
    {
        if (isGameOver || isTransitioning) return;
        isGameOver = true;

        if (timerText != null) timerText.gameObject.SetActive(false);
        if (nightNameText != null) nightNameText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        string deathMessage = "You died.";
        switch (cause)
        {
            case deathCause.Caught: deathMessage = "You got caught..."; break;
            case deathCause.Insanity: deathMessage = "Your mind has descended into profound madness."; break;
        }

        StartCoroutine(GameOverSequence(deathMessage));
    }

    private IEnumerator GameOverSequence(string message)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverReasonText != null) gameOverReasonText.text = "";

        if (gameOverCanvasGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                gameOverCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            gameOverCanvasGroup.alpha = 1f;
        }

        if (gameOverReasonText != null)
        {
            foreach (char letter in message.ToCharArray())
            {
                gameOverReasonText.text += letter;
                yield return new WaitForSecondsRealtime(textTypeSpeed);
            }
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(activeSceneName);
    }

    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("[CHEAT] Skip night.");
                isTransitioning = false;
                isGameOver = false;
                currentTime = 0f;
                return;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("[CHEAT] Trigger Game Over.");
                isTransitioning = false;
                TriggerGameOver(deathCause.Caught);
                return;
            }
        }

        if (isGameOver || isTransitioning) return;
        HandleTimer();
    }

    public void SetDifficultyByIndex(int index)
    {
        switch (index)
        {
            case 0:
                activeDifficulty = modeEasy;
                break;
            case 1:
                activeDifficulty = modeNormal;
                break;
            case 2:
                activeDifficulty = modeHard;
                break;
            default:
                Debug.LogWarning("Index de difficulté inconnu !");
                break;
        }
        Debug.Log("Difficulté modifiée par index. Mode actuel : " + activeDifficulty.name);
    }
    public void ActivateGame()
    {
        actualNight = chaptersSequence[currentChapterIndex].nights[currentNightIndex];
        transform.parent.GameObject().GetComponentInChildren<SetUIiToPlayer>().ActivateUI(actualNight);
        NightManager.GetComponent<PlayerStateMachine>().ActivateStateMachine(actualNight);
        actualNight.nightGO.SetActive(true);
    }
}