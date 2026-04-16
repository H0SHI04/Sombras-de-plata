using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Tooltip("Drag your Tutorial_Text UI object here")]
    public GameObject tutorialUI;
    
    // We use this to know when the player is allowed to clear the text
    private bool isPlayerInZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialUI != null) tutorialUI.SetActive(true);
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // This is still here just in case they walk backward out of the box instead of dashing
        if (other.CompareTag("Player"))
        {
            if (tutorialUI != null) tutorialUI.SetActive(false);
            isPlayerInZone = false;
        }
    }

    private void Update()
    {
        // If the player is in the zone and actually presses Left Click to dash...
        if (isPlayerInZone && Input.GetMouseButtonDown(0))
        {
            // Instantly turn off the text
            if (tutorialUI != null) tutorialUI.SetActive(false);
            
            // Destroy this invisible trigger box so the tutorial never pops up again!
            Destroy(gameObject);
        }
    }
}