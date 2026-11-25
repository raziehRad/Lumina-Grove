
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
using UI_InputSystem.Base;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_Text finalTimeText;
    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private GameObject tutorialPanel;
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button doneButton;
    [SerializeField] private Button tutorialStartButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button completeLeaderboardButton;
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private NakamaManager nakamaManager;
    [SerializeField] private LeaderboardUIController leaderboardUIController;
    private static bool isRestarting;
    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        backButton.onClick.AddListener(CloseSettings);
        doneButton.onClick.AddListener(CompleteGame);
        tutorialStartButton.onClick.AddListener(BeginQuest);
        leaderboardButton.onClick.AddListener(OpenLeaderboard);
        completeLeaderboardButton.onClick.AddListener(OpenLeaderboardFromComplete);
    }
    private void OnEnable()
    {
        GameEvents.OnQuestCompleted += ShowCompletePanel;
        GameEvents.OnBackToMainMenu += ShowStartMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestCompleted -= ShowCompletePanel;
        GameEvents.OnBackToMainMenu -= ShowStartMenu;
    }
    
    public void ShowStartMenu()
    {
        startMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        completePanel.SetActive(false);
        tutorialPanel.SetActive(false);

        Time.timeScale = 0f;
    }

    private void Start()
    {
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        completePanel.SetActive(false);

        if (isRestarting)
        {
            isRestarting = false;

            startMenuPanel.SetActive(false);

            GameEvents.RaiseStartQuest();
          //  gameManager.StartQuest();

            AudioManager.Instance.PlayMusic();
        }
        else
        {
            startMenuPanel.SetActive(true);
        }
    }

    private void StartGame()
    {
        startMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        AudioManager.Instance.PlayMusic();
        AudioManager.Instance.PlaySFX(SoundType.PanelOpen);
    }
    private void BeginQuest()
    {
        tutorialPanel.SetActive(false);
        GameEvents.RaiseStartQuest();
      //  gameManager.StartQuest();
    }
    private void OpenLeaderboard()
    {
        GameEvents.RaiseOpenLeaderboard(startMenuPanel);
    }
    private void OpenLeaderboardFromComplete()
    {
        GameEvents.RaiseOpenLeaderboard(startMenuPanel);
    }
    private void OpenSettings()
    {
        settingsPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(SoundType.PanelOpen);
    }
    private void CloseSettings()
    {
        settingsPanel.SetActive(false);
        AudioManager.Instance.PlaySFX(SoundType.PanelOpen);
    }
    private void ShowCompletePanel()
    {
        float finalTime = gameManager.GetElapsedTime();
        int score = Mathf.RoundToInt(finalTime * 1000f);

        nakamaManager!.SubmitScore(score);
        int minutes = Mathf.FloorToInt(finalTime / 60);
        int seconds = Mathf.FloorToInt(finalTime % 60);

        finalTimeText.text = $"Your Time: {minutes:00}:{seconds:00}";
        completePanel.SetActive(true);
    }
    private void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    private void RestartGame()
    {
        isRestarting = true;
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
    private void CompleteGame()
    {
        completePanel.SetActive(false);
        startMenuPanel.SetActive(true);
        
        startButton.onClick.RemoveListener(StartGame);
        startButton.onClick.AddListener(RestartGame);
    }
    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(StartGame);
        settingsButton.onClick.RemoveListener(OpenSettings);
        quitButton.onClick.RemoveListener(QuitGame);
        backButton.onClick.RemoveListener(CloseSettings);
        doneButton.onClick.RemoveListener(CompleteGame);
        tutorialStartButton.onClick.RemoveListener(BeginQuest);
         leaderboardButton.onClick.RemoveListener(OpenLeaderboard);
         completeLeaderboardButton.onClick.RemoveListener(OpenLeaderboardFromComplete);
    }
}
