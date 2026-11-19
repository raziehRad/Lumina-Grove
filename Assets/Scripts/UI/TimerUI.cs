using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private void OnEnable()
    {
        GameEvents.OnTimerUpdated += UpdateTimer;
    }

    private void OnDisable()
    {
        GameEvents.OnTimerUpdated -= UpdateTimer;
    }

    private void UpdateTimer(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}