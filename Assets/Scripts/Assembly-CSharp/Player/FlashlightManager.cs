using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private StaminaManager staminaManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private LoseScript loseScript;
    [SerializeField] private SharedVar shared;

    [Header("Flashlight Manager")]
    public bool flashlightEnabled;
    public AudioSource flashlightSound;
    public Transform fldown;
    public Transform flup;
    public Light torch;
    public Light eyes;
    public float battery = 1f;
    public bool torchdying;
    private bool cranking;
    public LayerMask mask;

    private float flickerTimer;

    // Constants //
    private const float FLICKER_INTERVAL = 0.1f;            // - Interval for flicker effect
    private const float EYE_LIGHT_INCREASE = 0.15f;         // - Rate at which eye light increases
    private const float EYE_LIGHT_DECREASE = 0.5f;          // - Rate at which eye light decreases
    private const float BATTERY_DRAIN_TYPE0 = 1.8E-05f;     // - Battery drain rate for flashlight type 0
    private const float BATTERY_DRAIN_TYPE2 = 0.0002f;      // - Battery drain rate for flashlight type 2
    private const float BATTERY_RECHARGE = 0.001f;          // - Battery recharge rate when cranking

    private void Update()
    {
        if (pauseManager.paused) return;

        if (!introScript.introEnded) return;

        bool canInteract = !shared.lost || loseScript.timeleft > 250;

        UpdateEyeLight();
        UpdateBattery();
        UpdateFlickerEffect();

        if (canInteract)
        {
            UpdateTorchRotation();
            CheckDaytimeDisable();
        }
        else if (shared.lost && loseScript.timeleft < 250)
        {
            torch.enabled = false;
        }
    }

    // Updates the eye light intensity based on flashlight statu //
    private void UpdateEyeLight()
    {
        if (!torch.enabled && eyes.range < 120f)
        {
            eyes.range = Mathf.Min(eyes.range + EYE_LIGHT_INCREASE, 120f);
        }
        else if (torch.enabled && eyes.range > 30f)
        {
            eyes.range = Mathf.Max(eyes.range - EYE_LIGHT_DECREASE, 30f);
        }
    }

    // Updates the battery level based on flashlight usage //
    private void UpdateBattery()
    {
        if (!torch.enabled) return; // No battery drain if flashlight is off


        if (introScript.fltype == 0)
        {
            battery -= BATTERY_DRAIN_TYPE0;
        }
        else if (introScript.fltype == 2 && !cranking)
        {
            battery -= BATTERY_DRAIN_TYPE2;
        }

        if (battery <= 0.15f)
        {
            battery = 0f;
            torch.enabled = false;
        }
    }

    // Updates the flicker effect when battery is low //
    private void UpdateFlickerEffect()
    {
        flickerTimer -= Time.deltaTime;

        if (battery < 0.25f && flickerTimer <= 0f)
        {
            torchdying = !torchdying;
            flickerTimer = FLICKER_INTERVAL;
        }

        torch.intensity = torchdying ? battery - 0.015f : battery;
    }

    // Updates the torch rotation based on player actions and nearby pages //
    private void UpdateTorchRotation()
    {
        Quaternion targetRotation; // Target rotation for the flashlight

        // Prioritize looking down when running //
        if (playerController.canRun && playerController.direction.y > 0f)
        {
            targetRotation = Quaternion.LookRotation(fldown.position - torch.transform.position);
            torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, targetRotation, Time.deltaTime * 8f);
            return;
        }

        // Look down when cranking flashlight type 2 //
        if (cranking)
        {
            targetRotation = Quaternion.LookRotation(fldown.position - torch.transform.position);   // Look down
            torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, targetRotation, Time.deltaTime * 8f); // Smooth transition

            if (battery < 0.15f)
            {
                battery = 0.151f;
                torch.enabled = true;
            }
            battery = Mathf.Min(battery + BATTERY_RECHARGE, 1f);
            return;
        }

        // Idle rotation, considering nearby pages //
        targetRotation = GetIdleTargetRotation();
        float slerpSpeed = staminaManager.sprintcooldown <= 0 ? 8f : 2f + (60f - staminaManager.sprintcooldown) / 10f;
        torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, targetRotation, Time.deltaTime * slerpSpeed);
    }

    // Determines the target rotation when idle, considering nearby pages //
    private Quaternion GetIdleTargetRotation()
    {
        if (shared.nearpage == null)
        {
            return Quaternion.LookRotation(flup.position - torch.transform.position);
        }

        Vector3 directionToPage = (shared.nearpage.position - transform.position).normalized;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, directionToPage, out hitInfo, 2f, mask))
        {
            if (hitInfo.collider.gameObject == shared.nearpage.gameObject)
            {
                return Quaternion.LookRotation(shared.nearpage.position - torch.transform.position);
            }
        }
        return Quaternion.LookRotation(flup.position - torch.transform.position);
    }

    // Disables the flashlight at a specific time during daytime //
    private void CheckDaytimeDisable()
    {
        if (introScript.timer == 1599 && shared.daytime)
        {
            torch.enabled = false;
        }
    }

    public void ToggleFlashlight(bool status)
    {
        if (pauseManager.paused || introScript.fltype != 0 || !introScript.introEnded) return;

        bool canToggle = (!shared.lost && !shared.daytime) || 
                         (shared.lost && loseScript.timeleft > 250 && loseScript.timeleft < 950 && shared.daytime);

        if (!canToggle) return;

        if (torch.enabled)
        {
            torch.enabled = false;
        }
        else if (battery > 0f)
        {
            torch.enabled = true;
        }

        flashlightSound.Play();
        flashlightEnabled = status;
    }
}