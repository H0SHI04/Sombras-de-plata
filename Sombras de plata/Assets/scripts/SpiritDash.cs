using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

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
    public Image[] dashIndicators; 
    public Color activeColor = Color.white;
    public Color emptyColor = new Color(1f, 1f, 1f, 0.2f);

    [Header("Dash Polish (Juice)")]
    public float dashFOV = 110f; // How much the screen stretches during the dash
    public float effectDuration = 0.15f; // How long the effect lasts
    public Image shadowOverlay; // Drag your new full-screen image here
    private float normalFOV;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (playerCamera == null) playerCamera = Camera.main;
        
        normalFOV = playerCamera.fieldOfView; // Save their default FOV
        currentDashes = maxDashes;
        UpdateDashUI();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            if (currentDashes != maxDashes)
            {
                currentDashes = maxDashes;
                UpdateDashUI();
            }
        }

        if (Input.GetMouseButtonDown(0) && currentDashes > 0)
        {
            PerformDash();
        }
    }

    private void PerformDash()
    {
        currentDashes--;
        UpdateDashUI();

        Vector3 direction = playerCamera.transform.forward;
        Vector3 targetPos = transform.position + (direction * dashDistance);
        float playerRadius = characterController.radius;
        Vector3 centerPos = transform.position + characterController.center;

        if (Physics.SphereCast(centerPos, playerRadius, direction, out RaycastHit hit, dashDistance, solidObstaclesMask))
        {
            float safeDistance = Mathf.Max(0, hit.distance - 0.1f);
            targetPos = transform.position + (direction * safeDistance);
        }

        // Instead of just teleporting, we trigger the visual effect sequence
        StartCoroutine(DashVisualsCoroutine(targetPos));
    }

    private IEnumerator DashVisualsCoroutine(Vector3 targetPosition)
    {
        // 1. THE SNAP: Instantly stretch the FOV and turn the screen shadowy
        playerCamera.fieldOfView = dashFOV;
        if (shadowOverlay != null) shadowOverlay.color = new Color(0, 0, 0, 0.7f); // 70% opacity

        // 2. THE TELEPORT: Move the player instantly to avoid clipping through walls
        characterController.enabled = false;
        transform.position = targetPosition;
        characterController.enabled = true;

        // 3. THE LERP: Smoothly return the FOV to normal and fade out the shadow
        float elapsed = 0f;
        while (elapsed < effectDuration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / effectDuration;

            // Lerp FOV back down
            playerCamera.fieldOfView = Mathf.Lerp(dashFOV, normalFOV, percentComplete);

            // Lerp Shadow Alpha from 0.7 down to 0
            if (shadowOverlay != null)
            {
                shadowOverlay.color = new Color(0, 0, 0, Mathf.Lerp(0.7f, 0f, percentComplete));
            }

            yield return null; // Wait for the next frame
        }

        // 4. CLEANUP: Ensure everything is perfectly reset
        playerCamera.fieldOfView = normalFOV;
        if (shadowOverlay != null) shadowOverlay.color = new Color(0, 0, 0, 0f);
    }

    private void UpdateDashUI()
    {
        for (int i = 0; i < dashIndicators.Length; i++)
        {
            if (i < currentDashes) dashIndicators[i].color = activeColor;
            else dashIndicators[i].color = emptyColor;
        }
    }
}