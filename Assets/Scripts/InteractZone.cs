using UnityEngine;

public class InteractZone : MonoBehaviour
{
    [SerializeField] private GameObject interactPrompt;
    private bool playerInRange = false;
    public bool PlayerInRange => playerInRange;

    void Start()
    {
        SetPromptVisible(false);
    }
    private void SetPromptVisible(bool visible)
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(visible);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            SetPromptVisible(true);

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            SetPromptVisible(false);
        }
    }
}
