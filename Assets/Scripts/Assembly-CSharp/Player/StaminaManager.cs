using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private LoseScript loseScript;
    [SerializeField] private SharedVar shared;
    [SerializeField] private ZoomManager zoomManager;

    [Header("Stamina Manager")]
    public int sprintcooldown;
    public bool amrunning;
    public bool flraised;
    public bool cranking;
    public float stamina = 100f;
    public float maxstam = 100f;
    public Transform statscale;
    public AudioSource breathing;
    public int stepcd = 120;

    private Vector3 cachedScale;
    private const float SCALE_DIVISOR = 57.5f;  // Divisor for scaling calculation
    private const float SCALE_OFFSET = 2.5f;    // Offset for scaling calculation

    private void Update()
    {
        // Early exits for performance
        if (pauseManager.paused || (shared.lost && loseScript.timeleft <= 250) || !introScript.introEnded) return;

        bool isMoving = playerController.direction.y != 0f || playerController.direction.x != 0f;
        bool canSprint = playerController.canRun && playerController.direction.y > 0f;

        if (canSprint)
        {
            HandleSprinting(isMoving);
        }
        else
        {
            HandleRecovery(isMoving);
        }

        UpdateStepCooldown(canSprint, isMoving);
        UpdateVisuals();
    }

    // Handles stamina depletion when sprinting //
    private void HandleSprinting(bool isMoving)
    {
        // Prevent sprinting if stamina is too low or on cooldown //
        if (!amrunning && stamina >= 10f)
        {
            amrunning = true;
            stamina -= 5f;
        }

        flraised = false; // Flashlight is lowered when sprinting

        // Immediate stop if stamina is too low //
        bool isLowStamina = stamina < 10f;
        if (isLowStamina)
        {
            stamina = 0f;
        }
        else
        {
            stamina -= shared.scared > 0 ? 0.1125f : playerController.jogSpeed / 105f;

            if (shared.scared > 0)
            {
                maxstam -= 0.009f;
                if (maxstam < 45f)
                    maxstam = 45f;
            }
        }
    }

    // Handles stamina recovery when not sprinting //
    private void HandleRecovery(bool isMoving)
    {
        amrunning = false; // Not sprinting

        // Decrease sprint cooldown if flashlight was recently lowered //
        if (sprintcooldown > 0)
        {
            sprintcooldown--;
        }

        // Flashlight raised when not sprinting //
        if (!flraised)
        {
            flraised = true;
            sprintcooldown = 60;
        }

        // cranking drains stamina faster //
        if (cranking)
        {
            stamina -= 0.025f;
            if (stamina < 10f)
                stamina = 0f;
        }
        else
        {
            stamina += isMoving ? 0.05f : 0.1f;
            if (stamina > maxstam)
                stamina = maxstam;
        }
    }
    
    // Updates the step cooldown based on player actions //
    private void UpdateStepCooldown(bool canSprint, bool isMoving)
    {
        if (canSprint)
        {
            int reduction = stamina < 10f ? 4 : (shared.scared > 0 ? 6 : 5);
            stepcd -= reduction;
        }
        else
        {
            stepcd = isMoving ? stepcd - 4 : 120;
        }
    }

    private void UpdateVisuals()
    {
        // Reuse Vector3 to avoid GC allocation
        float scaleValue = (zoomManager.zoom - SCALE_OFFSET) / SCALE_DIVISOR;
        cachedScale.x = scaleValue;
        cachedScale.y = scaleValue;
        cachedScale.z = scaleValue;
        statscale.localScale = cachedScale;

        breathing.volume = stamina < 30f ? (30f - stamina) / 20f : 0f;
    }
}