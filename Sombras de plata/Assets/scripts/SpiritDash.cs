using UnityEngine;
using UnityEngine.UI; // CRITICAL: This line allows us to talk to the Canvas!

[RequireComponent(typeof(CharacterController))]
public class SpiritDash : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashDistance = 10f;
    public int maxDashes = 3;
    private int currentDashes;

    [Header("References & Targeting")]
    public Camera playerCamera;
    public LayerMask solidObstaclesMask;

    [Header("UI Elements")]
    [Tooltip("Drag your 3 UI Images here from the Hierarchy")]
    public Image[] dashIndicators; 
    public Color activeColor = Color.white;
    public Color emptyColor = new Color(1f, 1f, 1f, 0.2f); // Same color, but 20% opacity (faded)

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (playerCamera == null) playerCamera = Camera.main;
        
        currentDashes = maxDashes;
        UpdateDashUI(); // Make sure circles are full when we spawn
    }

    void Update()
    {
        // 1. Ground Check to Recharge Dashes
        if (characterController.isGrounded)
        {
            if (currentDashes != maxDashes)
            {
                currentDashes = maxDashes;
                UpdateDashUI(); // Light the circles back up!
            }
        }

        // 2. Dash Execution
        if (Input.GetMouseButtonDown(0) && currentDashes > 0)
        {
            PerformDash();
        }
    }

    private void PerformDash()
    {
        currentDashes--;
        UpdateDashUI(); // Fade a circle out immediately 

        Vector3 direction = playerCamera.transform.forward;
        Vector3 targetPos = transform.position + (direction * dashDistance);
        float playerRadius = characterController.radius;
        Vector3 centerPos = transform.position + characterController.center;

        if (Physics.SphereCast(centerPos, playerRadius, direction, out RaycastHit hit, dashDistance, solidObstaclesMask))
        {
            float safeDistance = Mathf.Max(0, hit.distance - 0.1f);
            targetPos = transform.position + (direction * safeDistance);
        }

        characterController.enabled = false;
        transform.position = targetPos;
        characterController.enabled = true;
    }

    // --- NEW UI METHOD ---
    private void UpdateDashUI()
    {
        // Loop through our 3 UI circles
        for (int i = 0; i < dashIndicators.Length; i++)
        {
            // If this circle's index is less than our current dashes, light it up
            if (i < currentDashes)
            {
                dashIndicators[i].color = activeColor;
            }
            // Otherwise, fade it out to show it's empty
            else
            {
                dashIndicators[i].color = emptyColor;
            }
        }
    }
}