using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    /*[Header("Scripts")]
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private LoseScript loseScript;

    [Header("Flashlight Manager")]

    public AudioSource flashlightSound;
    public Light torch;
    public Light eyes;
	public float battery = 1f;
	public bool torchdying;

    private void Update()
    {
        if (introScript.timer >= 1600)
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
    }
    private void ToggleFlashlight(bool status)
    {
        if (!PauseManager.paused && introScript.fltype == 0 && introScript.timer >= 1600 && ((!lost && !daytime) || (loseScript.timeleft > 250 && loseScript.timeleft < 950 && daytime)))
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
    } */
}