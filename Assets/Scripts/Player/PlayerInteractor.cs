using UnityEngine;
using UnityEngine.UI;

    public class PlayerInteractor : MonoBehaviour
    {
        [Header("UI")] [SerializeField] private Button  interactionPrompt;

        private IInteractable currentInteractable;

        private void Awake()
        {
            interactionPrompt.onClick.AddListener(HandleInteraction);
            interactionPrompt.gameObject.SetActive(false);
        }

        private void HandleInteraction()
        {
            if (currentInteractable == null)
                return;

            currentInteractable.Interact();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IInteractable interactable))
                return;
            currentInteractable = interactable;
            interactionPrompt.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out IInteractable interactable))
                return;
            if (currentInteractable==interactable)
            {
                currentInteractable = null;
                interactionPrompt.gameObject.SetActive(false);
            }
        }
        private void OnDestroy()
        {
            interactionPrompt.onClick.RemoveListener(HandleInteraction);
        }
    }
