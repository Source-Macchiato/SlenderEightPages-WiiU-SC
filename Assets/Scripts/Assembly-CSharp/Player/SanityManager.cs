using UnityEngine;

public class SanityManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private LoseScript loseScript;

    [Header("Sanity Manager")]
    public AudioSource san1;
    public AudioSource san2;
    public AudioSource san3;
    public Transform tentacles;
    public AudioSource music1;
    public AudioSource music2;
    public AudioSource music3;
    public AudioSource music4;
    public float sanity = 100f;
    public bool cansee;
    public bool justsaw;
    public bool caught;
    public float drain;
    public int flicker;
    public bool endflicker;
    public bool lastflicker;
    public bool lost;
    public bool mh;
    public int scared;
    public int pages;
    public int level;
    private float fadeinmusic;

    private void Update()
    {

        if (!pauseManager.paused)
        {
            if (caught && !lost)
            {
                playerController.mouseLook.enabled = false;
                playerController.cm.canControl = false;
                Vector3 vector = new Vector3(SM.transform.position.x, SM.transform.position.y + 1f, SM.transform.position.z);
                Quaternion to = Quaternion.LookRotation(vector - base.transform.parent.transform.position);
                base.transform.parent.transform.rotation = Quaternion.Slerp(base.transform.parent.transform.rotation, to, Time.deltaTime * 2f);
            }
            
            if (!lost || loseScript.timeleft > 250)
            {
                if (introScript.introEnded)
                {
                    if (caught)
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
                    if (pages + level > 0 && loseScript.timeleft == 0 && !mh)
                    {
                        if (pages + level < 3)
                        {
                            music1.volume = fadeinmusic;
                        }
                        else if (pages + level < 5)
                        {
                            music1.volume = 2f - fadeinmusic;
                            music2.volume = fadeinmusic;
                        }
                        else if (pages + level < 7)
                        {
                            music1.volume = 0f;
                            music2.volume = 2f - fadeinmusic;
                            music3.volume = fadeinmusic;
                        }
                        else if (pages + level < 8)
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
                    }
                }
            }
        }
    }
}