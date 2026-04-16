using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SpiritDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashDistance = 10f;
    public int maxDashes = 3;
    private int currentDashes;

    [Header("References & Targeting")]
    public Camera playerCamera;
    
    [Tooltip("Check everything EXCEPT the layer your cages/bars are on.")]
    public LayerMask solidObstaclesMask; 

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        // Auto-assign the main camera if left empty
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        currentDashes = maxDashes;
    }

    void Update()
    {
        // 1. Ground Check to Recharge Dashes
        // The CharacterController has a built-in isGrounded boolean we can utilize
        if (characterController.isGrounded)
        {
            if (currentDashes != maxDashes)
            {
                currentDashes = maxDashes;
            }
        }

        // 2. Input Listener (Left Click = 0)
        if (Input.GetMouseButtonDown(0) && currentDashes > 0)
        {
            PerformDash();
        }
    }

    private void PerformDash()
    {
        currentDashes--;

        Vector3 direction = playerCamera.transform.forward;
        Vector3 targetPos = transform.position + (direction * dashDistance);

        // Get the actual width of El Búho's collider
        float playerRadius = characterController.radius;
        
        // Cast from the physical center of the capsule, not the camera
        Vector3 centerPos = transform.position + characterController.center;

        // Upgrade to SphereCast. It acts like a thick battering ram instead of a laser.
        if (Physics.SphereCast(centerPos, playerRadius, direction, out RaycastHit hit, dashDistance, solidObstaclesMask))
        {
            // hit.distance is the exact safe distance the sphere traveled before touching the wall.
            // We use that distance minus a tiny 0.1f buffer so we don't stick to the geometry.
            float safeDistance = Mathf.Max(0, hit.distance - 0.1f);
            
            targetPos = transform.position + (direction * safeDistance);
        }

        characterController.enabled = false;
        transform.position = targetPos;
        characterController.enabled = true;

        Debug.Log("Spirit Dash Used! Dashes remaining: " + currentDashes);
    }
}