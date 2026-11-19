using UnityEngine;

public class OrnamentCollectible : MonoBehaviour, IInteractable
{
    [Header("Quest Reference")]
    [SerializeField] private QuestManager questManager;
    public string InteractionMessage => "Collect Ornament";
    public void Interact()
    {
        Collect();
    }

    private void Collect()
    {
        if (questManager == null)
        {
            Debug.LogError("QuestManager not found!");
            return;
        }
        if (questManager.IsQuestCompleted)
            return;

        if (questManager.CollectedOrnaments >= questManager.TotalOrnaments)
            return;
        
        AudioManager.Instance.PlaySFX(SoundType.CollectItem);
        questManager.CollectOrnament();

        gameObject.SetActive(false);
    }
}