
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject pausePanel;

    [Header("Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button settingsBackButton;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject startMenuPanel;

    private bool isPaused;

    private void Awake()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(BackToMainMenu);
        settingsBackButton.onClick.AddListener(CloseSettings);
    }

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        isPaused = true;
        GameEvents.RaiseOpenPauseMenu(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.Instance.PlaySFX(SoundType.PanelOpen);
    }

    public void ResumeGame()
    {
        GameEvents.RaiseOpenPauseMenu(true);
        isPaused = false;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
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

    private void BackToMainMenu()
    {
        pausePanel.SetActive(false);
        GameEvents.RaiseBackToMainMenu();
    }
    private void OnDestroy()
    {
        pauseButton.onClick.RemoveListener(PauseGame);
        resumeButton.onClick.RemoveListener(ResumeGame);
        settingsButton.onClick.RemoveListener(OpenSettings);
        mainMenuButton.onClick.RemoveListener(BackToMainMenu);
        settingsBackButton.onClick.RemoveListener(CloseSettings);
    }
}
