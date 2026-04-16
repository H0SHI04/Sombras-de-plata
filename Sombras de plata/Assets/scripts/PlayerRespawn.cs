using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRespawn : MonoBehaviour
{
    [Tooltip("Drag the first respawn point here so the player has a default start.")]
    public Transform currentCheckpoint;
    
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // This method is called by the hazard floor when the player touches it
    public void RespawnPlayer()
    {
        if (currentCheckpoint != null)
        {
            // You MUST disable the controller to manually change the position
            characterController.enabled = false;
            
            // Move player and match the rotation so they face the right way
            transform.position = currentCheckpoint.position;
            transform.rotation = currentCheckpoint.rotation;
            
            characterController.enabled = true;
            Debug.Log("Player Respawned!");
        }
        else
        {
            Debug.LogWarning("No Checkpoint Set! Cannot Respawn.");
        }
    }

    // This method is called when the player walks through a new checkpoint
    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint Updated!");
    }
}