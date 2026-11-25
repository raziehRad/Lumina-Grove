using System.Collections;
using UnityEngine;

public class CelebrationController : MonoBehaviour
{
    [Header("Celebration Objects")]
    [SerializeField] private GameObject prize;
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private ParticleSystem snow;
   // [Header("Game Manager")]
    //[SerializeField] private GameManager gameManager;

    [Header("Completion Settings")]
    [SerializeField] private float completionPanelDelay = 4f;
    private bool hasPlayed;

    private void Awake()
    {
        if (prize != null)
            prize.SetActive(false);

        if (confetti != null)
            confetti.gameObject.SetActive(false);

        if (snow != null)
            snow.gameObject.SetActive(false);
    }

    public void PlayCelebration()
    {
        if (hasPlayed)
            return;

        hasPlayed = true;
        StartCoroutine(CelebrationSequence());
    }

    private IEnumerator CelebrationSequence()
    {
        if (prize != null)
            prize.SetActive(true);

        if (confetti != null)
        {
            confetti.gameObject.SetActive(true);
            confetti.Play();
        }

        if (snow != null)
        {
            snow.gameObject.SetActive(true);
            snow.Play();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(SoundType.TreeCompleted);
        }

        // Wait before showing completion UI
        yield return new WaitForSeconds(completionPanelDelay);
        
          //  gameManager.CompleteQuest();
            GameEvents.RaiseCompleteQuest();
    }
}