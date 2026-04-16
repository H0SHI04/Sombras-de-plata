using UnityEngine;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [Tooltip("Drag your Victory_Text UI object here")]
    public GameObject victoryUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Turn on the massive victory text
            if (victoryUI != null) victoryUI.SetActive(true);
            
            // 2. Freeze the game so the player can't fall off the edge
            Time.timeScale = 0f; 
            
            // 3. Hide the Heart to simulate collecting it
            gameObject.SetActive(false); 
        }
    }
}