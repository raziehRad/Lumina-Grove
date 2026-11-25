using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Serialization;

public class NicknameUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject nicknamePanel;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TMP_Text errorText;
    
    [Header("References")]
    [SerializeField] private LootLockerManager lootLockerManager;

    private void Awake()
    {
        nicknamePanel.SetActive(false);
        errorText.gameObject.SetActive(false);
        confirmButton.onClick.AddListener(OnConfirmClicked);
    }

    private void OnEnable()
    {
        GameEvents.OnShowNicknamePanel += ShowNicknamePanel;
    }

    private void OnDisable()
    {
        GameEvents.OnShowNicknamePanel -= ShowNicknamePanel;
    }

    public async void ShowNicknamePanel()
    {
       bool hasNickname = await lootLockerManager.HasChosenNickname();

        nicknamePanel.SetActive(!hasNickname);

        if (!hasNickname)
        {
            nicknameInput.text = "";
            errorText.gameObject.SetActive(false);
            confirmButton.interactable = true;
        }
        else
        {
            Debug.Log("Welcome back!");
        }
    }

    private async void OnConfirmClicked()
    {
        string nickname = nicknameInput.text.Trim();

        if (nickname.Length < 3 || nickname.Length > 16)
        {
            ShowError("Nickname must be 3-16 characters.");
            return;
        }

        confirmButton.interactable = false;
        errorText.gameObject.SetActive(false);

        await SaveNickname(nickname);
    }

    private async Task SaveNickname(string nickname)
    {
        bool success = await lootLockerManager.SetNickname(nickname);

        if (success)
        {
            nicknamePanel.SetActive(false);
            Debug.Log("Nickname registered successfully!");
        }
        else
        {
            ShowError("Could not save nickname. Try again.");
            confirmButton.interactable = true;
        }
    }

    private void ShowError(string message)
    {
        errorText.text = message;
        errorText.gameObject.SetActive(true);
    }
}
