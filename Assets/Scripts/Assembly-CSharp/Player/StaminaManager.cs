using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private LoseScript loseScript;
    [SerializeField] private SharedVar shared;
    [SerializeField] private ZoomManager zoomManager;
    [SerializeField] private SprintScript sprscr;
    public int sprintcooldown;
    public bool amrunning;
    public bool flraised;
    public bool cranking;
    public float stamina = 100f;
    public float maxstam = 100f;
    public Transform statscale;
    public AudioSource breathing;
    public int stepcd = 120;

    private void Update()
    {

        if (!pauseManager.paused)
        {
            if (!lost || loseScript.timeleft > 250)
            {
                if (introScript.introEnded)
                {
                    if (playerController.canRun && playerController.direction.y > 0f)
                    {
                        if (!amrunning && stamina >= 10f)
                        {
                            amrunning = true;
                            stamina -= 5f;
                        }
                        if (flraised)
                        {
                            flraised = false;
                        }
                        if (scared > 0)
                        {
                            stamina -= 0.1125f;
                            if (stamina < 10f)
                            {
                                stamina = 0f;
                                stepcd -= 4;
                            }
                            else
                            {
                                stepcd -= 6;
                                maxstam -= 0.009f;
                                if (maxstam <= 45f)
                                {
                                    maxstam = 45f;
                                }
                            }
                        }
                        else
                        {
                            stamina -= sprscr.jogSpeed / 105f;
                            if (stamina < 10f)
                            {
                                stamina = 0f;
                                stepcd -= 4;
                            }
                            else
                            {
                                stepcd -= 5;
                            }
                        }
                    }
                    else
                    {
                        amrunning = false;
                        if (sprintcooldown > 0)
                        {
                            sprintcooldown--;
                        }
                        if (!flraised)
                        {
                            flraised = true;
                            sprintcooldown = 60;
                        }
                        if (cranking)
                        {
                            stamina -= 0.025f;
                            if (stamina < 10f)
                            {
                                stamina = 0f;
                            }
                        }
                        else
                        {

                            if (playerController.direction.y != 0f || playerController.direction.x != 0f)
                            {
                                stamina += 0.05f;
                            }
                            else
                            {
                                stamina += 0.1f;
                            }
                            if (stamina > maxstam)
                            {
                                stamina = maxstam;
                            }
                        }
                        if (playerController.direction.y != 0f || playerController.direction.x != 0f)
                        {
                            stepcd -= 4;
                        }
                        else
                        {
                            stepcd = 120;
                        }
                    }

                    statscale.localScale = new Vector3((zoomManager.zoom - 2.5f) / 57.5f, (zoomManager.zoom - 2.5f) / 57.5f, (zoomManager.zoom - 2.5f) / 57.5f);
                    if (stamina < 30f)
                    {
                        breathing.volume = (30f - stamina) / 20f;
                    }
                    else
                    {
                        breathing.volume = 0f;
                    }
                }
            }
        }
    }
}