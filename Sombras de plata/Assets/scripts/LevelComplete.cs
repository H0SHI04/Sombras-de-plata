using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            
            Debug.Log("LEVEL COMPLETE! El Búho has reclaimed the Heart of the Pueblo!");
            
           
            Time.timeScale = 0f; 
            
            
            gameObject.SetActive(false); 
        }
    }
}