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

    private void Update()
    {
        if (!pauseManager.paused)
        {
            if (introScript.introEnded)
            {
                if (!torch.enabled && eyes.range < 120f)
                {
                    eyes.range += 0.15f;
                    if (eyes.range >= 120f)
                    {
                        eyes.range = 120f;
                    }
                }
                else if (torch.enabled)
                {
                    if (introScript.fltype == 0)
                    {
                        battery -= 1.8E-05f;
                    }
                    else if (introScript.fltype == 2 && !cranking)
                    {
                        battery -= 0.0002f;
                    }
                    if (battery <= 0.15f)
                    {
                        battery = 0f;
                        Debug.Log("Flashlight battery dead");
                        torch.enabled = false;
                    }
                    if (eyes.range > 30f)
                    {
                        eyes.range -= 0.5f;
                        if (eyes.range <= 30f)
                        {
                            eyes.range = 30f;
                        }
                    }
                }
                if (battery < 0.25f && Random.value < 0.2f)
                {
                    if (torchdying)
                    {
                        torchdying = false;
                    }
                    else
                    {
                        torchdying = true;
                    }
                }
                if (torchdying)
                {
                    torch.intensity = battery - 0.015f;
                }
                else
                {
                    torch.intensity = battery;
                }
            }

            if (!shared.lost || loseScript.timeleft > 250)
            {
                if (introScript.introEnded)
                {
                    if (playerController.canRun && playerController.direction.y > 0f)
                    {
                        Quaternion to2 = Quaternion.LookRotation(fldown.position - torch.transform.position);
                        torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, to2, Time.deltaTime * 8f);
                    }
                    else
                    {
                        if (cranking)
                        {
                            Quaternion to2 = Quaternion.LookRotation(fldown.position - torch.transform.position);
                            torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, to2, Time.deltaTime * 8f);
                            if (battery < 0.15f)
                            {
                                battery = 0.151f;
                                torch.enabled = true;
                            }
                            battery += 0.001f;
                            if (battery > 1f)
                            {
                                battery = 1f;
                            }
                        }
                        else
                        {
                            RaycastHit hitInfo;
                            Quaternion to2 = ((shared.nearpage == null) ? Quaternion.LookRotation(flup.position - torch.transform.position) : ((!Physics.Raycast(base.transform.position, (shared.nearpage.position - base.transform.position).normalized, out hitInfo, 2f, mask)) ? Quaternion.LookRotation(flup.position - torch.transform.position) : ((!(hitInfo.collider.gameObject == shared.nearpage.gameObject)) ? Quaternion.LookRotation(flup.position - torch.transform.position) : Quaternion.LookRotation(shared.nearpage.position - torch.transform.position))));
                            if (staminaManager.sprintcooldown <= 0)
                            {
                                torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, to2, Time.deltaTime * 8f);
                            }
                            else
                            {
                                torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, to2, Time.deltaTime * (2f + (60f - (float)staminaManager.sprintcooldown) / 10f));
                            }
                        }
                    }
                }
            }

            if (!shared.lost || loseScript.timeleft > 250)
            {
                if (introScript.timer == 1599)
                {
                    if (shared.daytime)
                    {
                        Debug.Log("Flashlight disabled by daytime");
                        torch.enabled = false;
                    }
                }
            }
            if (!shared.lost)
            {
                return;
            }
            if (loseScript.timeleft < 250)
            {
                Debug.Log("Flashlight disabled");
                torch.enabled = false;
                return;
            }
        }
    }
    public void ToggleFlashlight(bool status)
    {
        if (!pauseManager.paused && introScript.fltype == 0 && introScript.introEnded && ((!shared.lost && !shared.daytime) || (loseScript.timeleft > 250 && loseScript.timeleft < 950 && shared.daytime)))
        {
            if (torch.enabled)
            {
                Debug.Log("Flashlight off");
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
}