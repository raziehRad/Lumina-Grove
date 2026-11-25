
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIController : MonoBehaviour
{
    [Header("Rank Medals")]
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite bronzeMedal;
    [Header("References")]
    [SerializeField] private NakamaManager nakamaManager;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Transform content;
    [SerializeField] private LeaderboardRowUI recordPrefab;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text statusText;
    private GameObject previousPanel;
    private void Awake()
    {
        leaderboardPanel.SetActive(false);
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseLeaderboard);
        GameEvents.OnOpenLeaderboard += OpenLeaderboard;
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(CloseLeaderboard);
        GameEvents.OnOpenLeaderboard -= OpenLeaderboard;
    }

    public async void OpenLeaderboard(GameObject returnPanel)
    {
        previousPanel = returnPanel;

        if (previousPanel != null)
            previousPanel.SetActive(false);

        leaderboardPanel.SetActive(true);
        statusText.text = "Loading...";
        ClearRecords();

        IApiLeaderboardRecordList result =
            await nakamaManager.GetLeaderboard();

        if (!leaderboardPanel.activeInHierarchy)
            return;

        if (result == null)
        {
            statusText.text = "Failed to load leaderboard.";
            return;
        }

        statusText.text = "";
        DisplayRecords(result);
    }
    private void DisplayRecords(IApiLeaderboardRecordList result)
    {
        foreach (var record in result.Records)
        {
            LeaderboardRowUI row = Instantiate(recordPrefab, content);

            if (row == null)
            {
                Debug.LogError("LeaderboardRowUI is missing!");
                Destroy(row);
                continue;
            }

            row.Setup(
                record.Rank,
                string.IsNullOrEmpty(record.Username)
                    ? "Gardener"
                    : record.Username,
                long.Parse(record.Score)
            );
        }
    }
    private void ClearRecords()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);

        if (previousPanel != null)
            previousPanel.SetActive(true);

        previousPanel = null;
    }
}
