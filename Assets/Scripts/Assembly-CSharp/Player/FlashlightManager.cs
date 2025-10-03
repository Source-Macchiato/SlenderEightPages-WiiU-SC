using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private LoseScript loseScript;

    [Header("Flashlight Manager")]
    public AudioSource flashlightSound;
    public Light torch;
    public Light eyes;
	public float battery = 1f;
	public bool torchdying;
    private bool cranking;

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

            if (!lost || loseScript.timeleft > 250)
            {
                if (introScript.introEnded)
                {
                    if (playerController.canRun && direction.y > 0f)
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
                            Quaternion to2 = ((nearpage == null) ? Quaternion.LookRotation(flup.position - torch.transform.position) : ((!Physics.Raycast(base.transform.position, (nearpage.position - base.transform.position).normalized, out hitInfo, 2f, mask)) ? Quaternion.LookRotation(flup.position - torch.transform.position) : ((!(hitInfo.collider.gameObject == nearpage.gameObject)) ? Quaternion.LookRotation(flup.position - torch.transform.position) : Quaternion.LookRotation(nearpage.position - torch.transform.position))));
                            if (sprintcooldown <= 0)
                            {
                                torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, to2, Time.deltaTime * 8f);
                            }
                            else
                            {
                                torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, to2, Time.deltaTime * (2f + (60f - (float)sprintcooldown) / 10f));
                            }
                        }
                    }
                }
            }

            if (!lost || loseScript.timeleft > 250)
            {
                if (introScript.timer == 1599)
                {
                    if (daytime)
                    {
                        torch.enabled = false;
                    }
                }
            }

            if (loseScript.timeleft < 250) // TO BE MOVED DIRECTLY TO LOSESCRIPT
            {
                torch.enabled = false;
                return;
            }
        }
    }
    public void ToggleFlashlight(bool status)
    {
        if (!PauseManager.paused && introScript.fltype == 0 && introScript.introEnded && ((!lost && !daytime) || (loseScript.timeleft > 250 && loseScript.timeleft < 950 && daytime)))
        {
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
}