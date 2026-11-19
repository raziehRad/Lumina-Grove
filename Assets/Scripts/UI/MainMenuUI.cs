
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TMPro;
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
    
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    private static bool isRestarting;
    private void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(OpenSettings);
        quitButton.onClick.AddListener(QuitGame);
        backButton.onClick.AddListener(CloseSettings);
        doneButton.onClick.AddListener(CompleteGame);
        tutorialStartButton.onClick.AddListener(BeginQuest);
    }
    private void OnEnable()
    {
        GameEvents.OnQuestCompleted += ShowCompletePanel;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestCompleted -= ShowCompletePanel;
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

            gameManager.StartQuest();

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

        gameManager.StartQuest();
    }
    private void OpenSettings()
    {
        startMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(SoundType.PanelOpen);
    }
    private void CloseSettings()
    {
        settingsPanel.SetActive(false);
        startMenuPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(SoundType.PanelOpen);
    }
    private void ShowCompletePanel()
    {
        float finalTime = gameManager.GetElapsedTime();

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
    }
}
