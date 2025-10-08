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
    public int toolong = 12000;

    public float maxrange = 100f;

    public float minrange = 80f;

    public float fadeinmusic = 2f;

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

    public int laststep;

    public GameObject SM;

    public GUIStyle hint;

    public int fadeoutgui = 400;

    public LoseScript loseScript;

    public IntroScript introScript;

    public Transform chasetest;

    public Transform tentacles;

    public ParticleSystem dust;

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
            
            if (!lost || loseScript.timeleft > 250)
            {
                if (introScript.introEnded)
                {
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
