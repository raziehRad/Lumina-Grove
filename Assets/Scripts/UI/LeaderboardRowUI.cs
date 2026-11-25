using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRowUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text timeText;

    [Header("Medal")]
    [SerializeField] private Image medalImage;
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite bronzeMedal;

    public void Setup(string rank, string username, long milliseconds)
    {
        rankText.text = rank;
        usernameText.text = string.IsNullOrEmpty(username)
            ? "Gardener"
            : username;

        long totalSeconds = milliseconds / 1000;
        timeText.text = $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";

        SetMedal(int.Parse(rank));
    }

    private void SetMedal(int rank)
    {
        Sprite medal = null;

        switch (rank)
        {
            case 1:
                medal = goldMedal;
                break;

            case 2:
                medal = silverMedal;
                break;

            case 3:
                medal = bronzeMedal;
                break;
        }

        medalImage.sprite = medal;
        medalImage.gameObject.SetActive(medal != null);
    }
}