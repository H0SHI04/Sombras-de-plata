using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Tooltip("The exact coordinate the player should teleport to. Usually an Empty GameObject.")]
    public Transform respawnCoordinate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                // Tell the player to update their saved location to this new one
                playerRespawn.UpdateCheckpoint(respawnCoordinate);
            }
        }
    }
}