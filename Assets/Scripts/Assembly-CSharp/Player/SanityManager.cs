using UnityEngine;

public class SanityManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private LoseScript loseScript;
    [SerializeField] private SlenderMan slenderMan;
    [SerializeField] private SharedVar shared;

    [Header("Sanity Manager")]
    public AudioSource san1;
    public AudioSource san2;
    public AudioSource san3;
    public Transform tentacles;
    public float sanity = 100f;
    public bool cansee;
    public bool justsaw;
    public float drain;
    public int flicker;
    public bool endflicker;
    public bool lastflicker;

    private void Update()
    {
        // Flicker //
        if (endflicker)
        {
            endflicker = false;
            lastflicker = true;
        }

        if (!pauseManager.paused)
        {
            shared.music1.volume = 0f;
            shared.music2.volume = 0f;
            shared.music3.volume = 0f;
            shared.music4.volume = 0f;

            if (shared.caught && !shared.lost)
            {
                playerController.mouseLook.enabled = false;
                playerController.cm.canControl = false;
                Vector3 vector = new Vector3(slenderMan.SM.transform.position.x, slenderMan.SM.transform.position.y + 1f, slenderMan.SM.transform.position.z);
                Quaternion to = Quaternion.LookRotation(vector - base.transform.parent.transform.position);
                base.transform.parent.transform.rotation = Quaternion.Slerp(base.transform.parent.transform.rotation, to, Time.deltaTime * 2f);
            }

            if (!shared.lost || loseScript.timeleft > 250)
            {
                if (introScript.introEnded)
                {
                    if (shared.caught)
                    {
                        sanity -= 1f;
                        if (sanity < 0f)
                        {
                            shared.lost = true;
                        }
                    }
                    if (!cansee && !shared.caught)
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
                        if (!shared.caught)
                        {
                            sanity -= drain;
                        }
                        if (sanity < 0f && !shared.lost)
                        {
                            shared.lost = true;
                        }
                    }
                    if (shared.lost)
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
                        if (sanity < 70f || shared.mh)
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
                    if (sanity > 80f || shared.mh)
                    {
                        tentacles.localScale = new Vector3(0f, 0f, 0f);
                    }
                    else
                    {
                        tentacles.localScale = new Vector3((80f - sanity) * 0.01f, (80f - sanity) * 0.01f, (80f - sanity) * (1f / 160f));
                    }
                    if (shared.lost && !shared.mh)
                    {
                        tentacles.localScale = new Vector3(0.8f, 0.8f, 0.5f);
                    }
                    if (shared.pages + shared.level > 0 && loseScript.timeleft == 0 && !shared.mh)
                    {
                        if (shared.pages + shared.level < 3)
                        {
                            shared.music1.volume = shared.fadeinmusic;
                        }
                        else if (shared.pages + shared.level < 5)
                        {
                            shared.music1.volume = 2f - shared.fadeinmusic;
                            shared.music2.volume = shared.fadeinmusic;
                        }
                        else if (shared.pages + shared.level < 7)
                        {
                            shared.music1.volume = 0f;
                            shared.music2.volume = 2f - shared.fadeinmusic;
                            shared.music3.volume = shared.fadeinmusic;
                        }
                        else if (shared.pages + shared.level < 8)
                        {
                            shared.music1.volume = 0f;
                            shared.music2.volume = 0f;
                            shared.music3.volume = 2f - shared.fadeinmusic;
                            shared.music4.volume = shared.fadeinmusic;
                        }
                        else
                        {
                            shared.music1.volume = 0f;
                            shared.music2.volume = 0f;
                            shared.music3.volume = 0f;
                            shared.music4.volume = 1f - shared.fadeinmusic / 2f;
                        }
                        shared.fadeinmusic += 0.01f;
                        if (shared.fadeinmusic > 2f)
                        {
                            shared.fadeinmusic = 2f;
                        }
                    }
                    if (shared.scared > 0)
                    {
                        shared.scared--;
                    }
                }
            }
        }
    }
}