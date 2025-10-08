using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class PlayerScript : MonoBehaviour
{
    // TEMPORARY NEW REFERENCES, DO NOT DELETE //
    [SerializeField] private StaminaManager staminaManager;
    [SerializeField] public ZoomManager zoomManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private SharedVar shared;


    // OLD REFERENCES //
    // public int pages;

    // public int level;

    public int toolong = 12000;

    public float sanity = 100f;

    public float stamina = 100f;

    public float maxstam = 100f;

    // public int scared;

    public bool cansee;

    public bool justsaw;

    public float drain;

    public bool lost;

    public bool caught;

    public float maxrange = 100f;

    public float minrange = 80f;

    public float fadeinmusic = 2f;

    public AudioSource san1;

    public AudioSource san2;

    public AudioSource san3;

    public AudioSource music1;

    public AudioSource music2;

    public AudioSource music3;

    public AudioSource music4;

    // public AudioSource staminaManager.breathing;

    public AudioClip s1;

    public AudioClip s2;

    public AudioClip s3;

    public AudioClip s4;

    public AudioClip s5;

    public AudioClip s6;

    public AudioClip s7;

    public AudioClip s8;

    public AudioClip s9;

    public AudioClip s10;

    public AudioClip s11;

    public AudioClip s12;

    public AudioClip t1;

    public AudioClip t2;

    public AudioClip t3;

    public AudioClip t4;

    public AudioClip t5;

    public AudioClip t6;

    public AudioClip t7;

    public AudioSource climbfence;

    public bool flraised = true;

    // public Transform flup;

    public Transform fldown;

    public int laststep;

    // public int staminaManager.stepcd = 120;

    public GameObject SM;

    public GUIStyle hint;

    public int fadeoutgui = 400;

    public int flicker;

    public bool endflicker;

    public bool lastflicker;

    public Transform statscale;

    //public AudioSource zoomManager.zsound;

    public LoseScript loseScript;

    public IntroScript introScript;

    public Transform chasetest;

    public Transform tentacles;

    public bool mh;

    public ParticleSystem dust;

    //public int sprintcooldown;

    //public SprintScript sprscr;

    public float targetfog = 0.02f;

    //public bool amrunning;

    //public bool cranking;

    //public Vector2 direction = Vector2.zero;

    

    private void OnGUI()
    {
        if (!pauseManager.paused && fadeoutgui < 400 && !mh)
        {
            if (shared.pages == 0)
            {
                GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "Collect all 8 pages", hint);
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "pages " + shared.pages + "/8", hint);
            }
        }
    }

    private void Update()
    {

        // Flicker //
        if (endflicker)
        {
            endflicker = false;
            lastflicker = true;
        }
    }

    private void FixedUpdate()
    {
        if (!pauseManager.paused)
        {
            chasetest.position = base.transform.position;
            Quaternion rotation = Quaternion.LookRotation(base.transform.position - SM.transform.position, Vector3.up);
            rotation.x = 0f;
            rotation.z = 0f;
            chasetest.rotation = rotation;

            // GUI //
            if (fadeoutgui < 400)
            {
                fadeoutgui++;
            }


            // Fog Manager //
            if ((double)targetfog + 0.001 < (double)RenderSettings.fogDensity)
            {
                RenderSettings.fogDensity -= 0.001f;
            }
            else if ((double)targetfog - 0.0002 > (double)RenderSettings.fogDensity)
            {
                RenderSettings.fogDensity += 0.0002f;
            }
            else
            {
                RenderSettings.fogDensity = targetfog;
            }


            if (toolong > 0 && introScript.introEnded && shared.pages < 8)
            {
                toolong--;
                if (toolong <= 0)
                {
                    toolong = 12000;
                    if (shared.pages + shared.level < 9)
                    {
                        shared.level++;
                        maxrange = 100 - (shared.pages + shared.level) * 11;
                        minrange = 80 - (shared.pages + shared.level) * 10;
                        if (shared.pages + shared.level == 1 || shared.pages + shared.level == 3 || shared.pages + shared.level == 5 || shared.pages + shared.level == 7)
                        {
                            fadeinmusic = 0f;
                        }
                    }
                }
            }

            if (shared.pages >= 8)
            {
                dust.startColor = new Color(0.5f, 0.5f, 0.5f, 0.125f);
            }
            else
            {
                dust.startColor = new Color(0.5f, 0.5f, 0.5f, 0.0625f + (float)(shared.pages + shared.level) * 0.045f);
            }
            /**if (caught && !lost)
            {
                playerController.mouseLook.enabled = false;
                playerController.cm.canControl = false;
                Vector3 vector = new Vector3(SM.transform.position.x, SM.transform.position.y + 1f, SM.transform.position.z);
                Quaternion to = Quaternion.LookRotation(vector - base.transform.parent.transform.position);
                base.transform.parent.transform.rotation = Quaternion.Slerp(base.transform.parent.transform.rotation, to, Time.deltaTime * 2f);
            } **/
            if (!lost || loseScript.timeleft > 250)
            {
                if (introScript.introEnded)
                {
                    zoomManager.ZoomToggle();
                    /**if (caught)
                    {
                        sanity -= 1f;
                        if (sanity < 0f)
                        {
                            lost = true;
                        }
                    }
                    if (!cansee && !caught)
                    {
                        if (sanity <= 100f)
                        {
                            sanity += 0.1f;
                            if (sanity > 100f)
                            {
                                sanity = 100f;
                            }
                        }
                    }
                    else if (drain > 0f)
                    {
                        if (!caught)
                        {
                            sanity -= drain;
                        }
                        if (sanity < 0f && !lost)
                        {
                            lost = true;
                        }
                    }
                    if (lost)
                    {
                        sanity = 100f;
                    }
                    if (sanity < 0f)
                    {
                        sanity = 0f;
                    }
                    justsaw = cansee;
                    if (loseScript.timeleft == 0)
                    {
                        if (sanity < 70f || mh)
                        {
                            san1.volume = 1f;
                            if (sanity < 40f)
                            {
                                san2.volume = 1f;
                                if (sanity < 10f)
                                {
                                    san3.volume = 1f;
                                }
                                else
                                {
                                    san3.volume = (40f - sanity) / 30f;
                                }
                            }
                            else
                            {
                                san2.volume = (70f - sanity) / 30f;
                                san3.volume = 0f;
                            }
                        }
                        else
                        {
                            san1.volume = (100f - sanity) / 30f;
                            san2.volume = 0f;
                            san3.volume = 0f;
                        }
                    }
                    else if (loseScript.timeleft == 0)
                    {
                        san1.volume = 0f;
                        san2.volume = 0f;
                        san3.volume = 0f;
                    }
                    if (flicker > 0 || endflicker || lastflicker)
                    {
                        san1.volume = 1f;
                        san2.volume = 1f;
                        san3.volume = 1f;
                        flicker--;
                        if (flicker == 0 && !lastflicker)
                        {
                            endflicker = true;
                        }
                        if (lastflicker)
                        {
                            lastflicker = false;
                        }
                    }
                    if (sanity > 80f || mh)
                    {
                        tentacles.localScale = new Vector3(0f, 0f, 0f);
                    }
                    else
                    {
                        tentacles.localScale = new Vector3((80f - sanity) * 0.01f, (80f - sanity) * 0.01f, (80f - sanity) * (1f / 160f));
                    }
                    if (lost && !mh)
                    {
                        tentacles.localScale = new Vector3(0.8f, 0.8f, 0.5f);
                    }
                    if (shared.pages + shared.level > 0 && loseScript.timeleft == 0 && !mh)
                    {
                        if (shared.pages + shared.level < 3)
                        {
                            music1.volume = fadeinmusic;
                        }
                        else if (shared.pages + shared.level < 5)
                        {
                            music1.volume = 2f - fadeinmusic;
                            music2.volume = fadeinmusic;
                        }
                        else if (shared.pages + shared.level < 7)
                        {
                            music1.volume = 0f;
                            music2.volume = 2f - fadeinmusic;
                            music3.volume = fadeinmusic;
                        }
                        else if (shared.pages + shared.level < 8)
                        {
                            music1.volume = 0f;
                            music2.volume = 0f;
                            music3.volume = 2f - fadeinmusic;
                            music4.volume = fadeinmusic;
                        }
                        else
                        {
                            music1.volume = 0f;
                            music2.volume = 0f;
                            music3.volume = 0f;
                            music4.volume = 1f - fadeinmusic / 2f;
                        }
                        fadeinmusic += 0.01f;
                        if (fadeinmusic > 2f)
                        {
                            fadeinmusic = 2f;
                        }
                    }
                    if (scared > 0)
                    {
                        scared--;
                    } **/

                    // STAMINA //
                    /** if (playerController.canRun && direction.y > 0f)
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
                                staminaManager.stepcd -= 4;
                            }
                            else
                            {
                                staminaManager.stepcd -= 6;
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
                                staminaManager.stepcd -= 4;
                            }
                            else
                            {
                                staminaManager.stepcd -= 5;
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

                            if (direction.y != 0f || direction.x != 0f)
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
                        if (direction.y != 0f || direction.x != 0f)
                        {
                            staminaManager.stepcd -= 4;
                        }
                        else
                        {
                            staminaManager.stepcd = 120;
                        }
                    }

                    statscale.localScale = new Vector3((zoomManager.zoom - 2.5f) / 57.5f, (zoomManager.zoom - 2.5f) / 57.5f, (zoomManager.zoom - 2.5f) / 57.5f);
                    if (stamina < 30f)
                    {
                        staminaManager.breathing.volume = (30f - stamina) / 20f;
                    }
                    else
                    {
                        staminaManager.breathing.volume = 0f;
                    } **/

                    // FOOTSTEPS SOUND //
                    if (staminaManager.stepcd <= 0 && loseScript.timeleft < 950)
                    {
                        staminaManager.stepcd = 120;
                        int num = 0;
                        if (base.transform.parent.transform.position.y <= 2.051f)
                        {
                            do
                            {
                                num = (int)(Random.value * 12f) + 1;
                            }
                            while (num == laststep);
                            switch (num)
                            {
                                case 1:
                                    AudioSource.PlayClipAtPoint(s1, base.transform.position);
                                    break;
                                case 2:
                                    AudioSource.PlayClipAtPoint(s2, base.transform.position);
                                    break;
                                case 3:
                                    AudioSource.PlayClipAtPoint(s3, base.transform.position);
                                    break;
                                case 4:
                                    AudioSource.PlayClipAtPoint(s4, base.transform.position);
                                    break;
                                case 5:
                                    AudioSource.PlayClipAtPoint(s5, base.transform.position);
                                    break;
                                case 6:
                                    AudioSource.PlayClipAtPoint(s6, base.transform.position);
                                    break;
                                case 7:
                                    AudioSource.PlayClipAtPoint(s7, base.transform.position);
                                    break;
                                case 8:
                                    AudioSource.PlayClipAtPoint(s8, base.transform.position);
                                    break;
                                case 9:
                                    AudioSource.PlayClipAtPoint(s9, base.transform.position);
                                    break;
                                case 10:
                                    AudioSource.PlayClipAtPoint(s10, base.transform.position);
                                    break;
                                case 11:
                                    AudioSource.PlayClipAtPoint(s11, base.transform.position);
                                    break;
                                case 12:
                                    AudioSource.PlayClipAtPoint(s12, base.transform.position);
                                    break;
                            }
                        }
                        else
                        {
                            do
                            {
                                num = (int)(Random.value * 7f) + 1;
                            }
                            while (num == laststep);
                            switch (num)
                            {
                                case 1:
                                    AudioSource.PlayClipAtPoint(t1, base.transform.position, 0.5f);
                                    break;
                                case 2:
                                    AudioSource.PlayClipAtPoint(t2, base.transform.position, 0.5f);
                                    break;
                                case 3:
                                    AudioSource.PlayClipAtPoint(t3, base.transform.position, 0.5f);
                                    break;
                                case 4:
                                    AudioSource.PlayClipAtPoint(t4, base.transform.position, 0.5f);
                                    break;
                                case 5:
                                    AudioSource.PlayClipAtPoint(t5, base.transform.position, 0.5f);
                                    break;
                                case 6:
                                    AudioSource.PlayClipAtPoint(t6, base.transform.position, 0.5f);
                                    break;
                                case 7:
                                    AudioSource.PlayClipAtPoint(t7, base.transform.position, 0.5f);
                                    break;
                            }
                        }
                        laststep = num;
                    }
                }
                else if (introScript.timer < 900 && introScript.timer > 0 && !mh)
                {
                    if (introScript.skintro)
                    {
                        introScript.timer = 1598;
                    }
                    else
                    {
                        staminaManager.stepcd -= 4;
                        if (staminaManager.stepcd <= 0)
                        {
                            staminaManager.stepcd = 120;
                            int num2 = 0;
                            do
                            {
                                num2 = (int)(Random.value * 12f) + 1;
                            }
                            while (num2 == laststep);
                            switch (num2)
                            {
                                case 1:
                                    AudioSource.PlayClipAtPoint(s1, base.transform.position);
                                    break;
                                case 2:
                                    AudioSource.PlayClipAtPoint(s2, base.transform.position);
                                    break;
                                case 3:
                                    AudioSource.PlayClipAtPoint(s3, base.transform.position);
                                    break;
                                case 4:
                                    AudioSource.PlayClipAtPoint(s4, base.transform.position);
                                    break;
                                case 5:
                                    AudioSource.PlayClipAtPoint(s5, base.transform.position);
                                    break;
                                case 6:
                                    AudioSource.PlayClipAtPoint(s6, base.transform.position);
                                    break;
                                case 7:
                                    AudioSource.PlayClipAtPoint(s7, base.transform.position);
                                    break;
                                case 8:
                                    AudioSource.PlayClipAtPoint(s8, base.transform.position);
                                    break;
                                case 9:
                                    AudioSource.PlayClipAtPoint(s9, base.transform.position);
                                    break;
                                case 10:
                                    AudioSource.PlayClipAtPoint(s10, base.transform.position);
                                    break;
                                case 11:
                                    AudioSource.PlayClipAtPoint(s11, base.transform.position);
                                    break;
                                case 12:
                                    AudioSource.PlayClipAtPoint(s12, base.transform.position);
                                    break;
                            }
                            laststep = num2;
                        }
                    }
                }
                else if (introScript.timer == 950)
                {
                    staminaManager.stepcd = 120;
                    if (!mh)
                    {
                        climbfence.Play();
                    }
                }
                else if (introScript.timer == 1599)
                {
                    playerController.cm.canControl = true;
                    playerController.mouseLook.enabled = true;
                }
            }
            if (!lost)
            {
                return;
            }
            music1.volume = 0f;
            music2.volume = 0f;
            music3.volume = 0f;
            music4.volume = 0f;
            if (loseScript.timeleft < 250)
            {
                staminaManager.breathing.volume = 0f;
                zoomManager.zsound.volume = 0f;
                return;
            }
            if (loseScript.timeleft >= 250)
            {
                playerController.cm.canControl = true;
                playerController.mouseLook.enabled = true;
            }
            if (loseScript.timeleft >= 950)
            {
                staminaManager.breathing.volume = 0f;
                zoomManager.zsound.volume = 0f;
                playerController.cm.canControl = false;
                playerController.mouseLook.enabled = false;
            }
            if (shared.pages < 8 || loseScript.timeleft < 1000 + loseScript.mhdelay)
            {
                return;
            }
            if (loseScript.timeleft < 2500 + loseScript.mhdelay)
            {
                music1.volume = fadeinmusic;
                fadeinmusic += 0.01f;
                if (fadeinmusic > 2f)
                {
                    fadeinmusic = 2f;
                }
            }
            else
            {
                music1.volume = 1f - fadeinmusic / 2f;
                fadeinmusic += 0.01f;
                if (fadeinmusic > 2f)
                {
                    fadeinmusic = 2f;
                }
            }
        }
    }
}
