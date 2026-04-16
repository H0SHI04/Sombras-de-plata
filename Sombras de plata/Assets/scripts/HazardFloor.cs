using UnityEngine;

public class HazardFloor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that touched the floor is the Player
        if (other.CompareTag("Player"))
        {
            // Find the respawn script and trigger it
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.RespawnPlayer();
            }
        }
    }
}