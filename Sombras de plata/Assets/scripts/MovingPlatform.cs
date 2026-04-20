using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Pathing")]
    public Transform posA;
    public Transform posB;
    public float speed = 3f;

    private Vector3 nextPosition;
    private Vector3 lastPosition;
    
    // We will store the player's controller here when they land
    private CharacterController activePlayer;

    void Start()
    {
        if (posA != null && posB != null)
        {
            nextPosition = posB.position;
        }
        lastPosition = transform.position;
    }

    // CHANGED TO LATEUPDATE: This forces the platform to move the player 
    // AFTER the player's own gravity and walking scripts finish!
    void LateUpdate() 
    {
        if (posA == null || posB == null) return;

        // 1. Move the platform
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, nextPosition) <= 0.05f)
        {
            nextPosition = (nextPosition == posA.position) ? posB.position : posA.position;
        }

        // 2. Calculate exactly how far the platform moved this frame
        Vector3 platformDeltaMove = transform.position - lastPosition;

        // 3. If the player is on the platform, physically shove their controller by the same amount
        if (activePlayer != null)
        {
            activePlayer.Move(platformDeltaMove);
        }

        // 4. Update the last position for the next frame
        lastPosition = transform.position;
    }

    // When the player touches the trigger box, save their CharacterController
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activePlayer = other.GetComponent<CharacterController>();
        }
    }

    // When they jump or dash away, clear it out
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activePlayer = null;
        }
    }
}