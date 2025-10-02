using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class PlayerScript : MonoBehaviour
{
    // TEMPORARY NEW REFERENCES, DO NOT DELETE //
    [SerializeField] public ZoomManager zoomManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PauseManager pauseManager;


    // OLD REFERENCES //
    public bool debug;

    public int pages;

    public int level;

    public int toolong = 12000;

    public float sanity = 100f;

    public float stamina = 100f;

    public float maxstam = 100f;

    public int scared;

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

    public AudioSource breathing;

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

    public AudioSource flashlight;

    public Light torch;

    public Light eyes;

    public float battery = 1f;

    public bool torchdying;

    public bool flraised = true;

    public Transform flup;

    public Transform fldown;

    public int laststep;

    public int stepcd = 120;

    public GameObject SM;

    public GUIStyle hint;

    public int fadeoutgui = 400;

    // public mouseLook;

    // public CharacterMotor playerController.cm;

    public int flicker;

    public bool endflicker;

    public bool lastflicker;

    // public float zoom = 60f;

    public Transform statscale;

    public AudioSource zsound;

    public LoseScript loseScript;

    public IntroScript introScript;

    public int finaldelay;

    public Transform chasetest;

    public Transform tentacles;

    public bool daytime;

    public bool mh;

    public Transform nearpage;

    public Transform endfix;

    public ParticleSystem dust;

    public bool dustyair = true;

    // public bool backedup;

    public int sprintcooldown;

    public SprintScript sprscr;

    // public bool paused;

    public float targetfog = 0.02f;

    public bool amrunning;

    public bool cranking;

    public AudioSource cranksound;

    public LayerMask mask;

    // public bool playerController.canRun;
    public bool flashlightEnabled;
    /**
	private bool zoomIn;
	private bool zoomOut; **/

    public Vector2 direction = Vector2.zero;

    /**
	private MenuManager menuManager;

    private WiiU.GamePad gamePad;
    private WiiU.Remote remote; **/

    private void Start()
    {
        /** playerController.cm.canControl = false;
        playerController.mouseLook.enabled = false;

		// menuManager = FindObjectOfType<MenuManager>();

        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0); **/
    }

    private void OnGUI()
    {
        if (!pauseManager.paused && fadeoutgui < 400 && !mh)
        {
            if (pages == 0)
            {
                GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "Collect all 8 pages", hint);
            }
            else
            {
                GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "Pages " + pages + "/8", hint);
            }
        }
    }

    private void Update()
    {
        /**
        // Reset direction at each frames
        direction = Vector2.zero;

        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        // Gamepad
        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            Vector2 leftStickGamepad = gamePadState.lStick;

			if (Mathf.Abs(leftStickGamepad.x) > 0.1f)
			{
				direction.x = leftStickGamepad.x;
			}

			if (Mathf.Abs(leftStickGamepad.y) > 0.1f)
			{
				direction.y = leftStickGamepad.y;
			}

			// Toggle run
            if (gamePadState.IsTriggered(WiiU.GamePadButton.ZR))
            {
                playerController.canRun = true;
            }
            else if (gamePadState.IsReleased(WiiU.GamePadButton.ZR))
            {
                playerController.canRun = false;
            }

			// Toggle flashlight
			if (gamePadState.IsTriggered(WiiU.GamePadButton.StickR))
			{
				ToggleFlashlight(!flashlightEnabled);
			}
			else if (gamePadState.IsTriggered(WiiU.GamePadButton.Y))
			{
                ToggleFlashlight(!flashlightEnabled);
            }

            // Zoom in and out
            if (gamePadState.IsTriggered(WiiU.GamePadButton.ZL))
            {
                zoomIn = true;
                zoomOut = false;
            }
            else if (gamePadState.IsReleased(WiiU.GamePadButton.ZL))
            {
                zoomIn = false;
                zoomOut = true;
            }

			if (gamePadState.IsTriggered(WiiU.GamePadButton.Up))
			{
				zoomIn = true;
				zoomOut = false;
			}
			else if (gamePadState.IsReleased(WiiU.GamePadButton.Up))
			{
				zoomIn = false;
				zoomOut = false;
			}

			if (gamePadState.IsTriggered(WiiU.GamePadButton.Down))
			{
				zoomIn = false;
				zoomOut = true;
			}
			else if (gamePadState.IsReleased(WiiU.GamePadButton.Down))
			{
				zoomIn = false;
				zoomOut = false;
			}

			// Skip Intro
			if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
			{
				SkipIntro();
			}

			if (gamePadState.IsTriggered(WiiU.GamePadButton.B))
			{
				SetGamePlayed();
			}

			if (gamePadState.IsTriggered(WiiU.GamePadButton.Plus))
			{
                if (pauseManager.paused)
                {
                    SetGamePlayed();
                }
                else
                {
                    SetGamePauseManager.paused();
                }
            }
        }

        // Remotes
        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                Vector2 leftStickProController = remoteState.pro.leftStick;

				if (Mathf.Abs(leftStickProController.x) > 0.1f)
				{
					direction.x = leftStickProController.x;
				}

				if (Mathf.Abs(leftStickProController.y) > 0.1f)
				{
					direction.y = leftStickProController.y;
				}

				// Toggle run
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.ZR))
                {
                    playerController.canRun = true;
                }
                else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.ZR))
                {
                    playerController.canRun = false;
                }

				// Toggle flashlight
				if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.StickR))
				{
                    ToggleFlashlight(!flashlightEnabled);
                }
				else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Y))
				{
                    ToggleFlashlight(!flashlightEnabled);
                }

                // Zoom in and out
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.ZL))
                {
                    zoomIn = true;
                    zoomOut = false;
                }
                else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.ZL))
                {
                    zoomIn = false;
                    zoomOut = true;
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Up))
                {
                    zoomIn = true;
                    zoomOut = false;
                }
                else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.Up))
                {
                    zoomIn = false;
                    zoomOut = false;
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Down))
                {
                    zoomIn = false;
                    zoomOut = true;
                }
                else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.Down))
                {
                    zoomIn = false;
                    zoomOut = false;
                }

                // Skip intro
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.A))
				{
					SkipIntro();
				}

				if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.B))
				{
					SetGamePlayed();
				}

				if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Plus))
				{
                    if (pauseManager.paused)
                    {
                        SetGamePlayed();
                    }
                    else
                    {
                        SetGamePauseManager.paused();
                    }
                }
                break;
            case WiiU.RemoteDevType.Classic:
                Vector2 leftStickClassicController = remoteState.classic.leftStick;

				if (Mathf.Abs(leftStickClassicController.x) > 0.1f)
				{
					direction.x = leftStickClassicController.x;
				}

				if (Mathf.Abs(leftStickClassicController.y) > 0.1f)
				{
					direction.y = leftStickClassicController.y;
				}

				// Toggle run
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.R))
                {
                    playerController.canRun = true;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.R))
                {
                    playerController.canRun = false;
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.ZR))
                {
                    playerController.canRun = true;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.ZR))
                {
                    playerController.canRun = false;
                }

                // Toggle flashlight
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Y))
				{
                    ToggleFlashlight(!flashlightEnabled);
                }

                // Zoom in and out
				if (remoteState.classic.IsTriggered(WiiU.ClassicButton.L))
				{
                    zoomIn = true;
                    zoomOut = false;
				}
				else if (remoteState.classic.IsReleased(WiiU.ClassicButton.L))
				{
					zoomIn = false;
					zoomOut = true;
				}
				
				if (remoteState.classic.IsTriggered(WiiU.ClassicButton.ZL))
				{
                    zoomIn = true;
                    zoomOut = false;
				}
				else if (remoteState.classic.IsReleased(WiiU.ClassicButton.ZL))
				{
					zoomIn = false;
					zoomOut = true;
				}

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Up))
                {
                    zoomIn = true;
                    zoomOut = false;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.Up))
                {
                    zoomIn = false;
                    zoomOut = false;
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Down))
                {
                    zoomIn = false;
                    zoomOut = true;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.Down))
                {
                    zoomIn = false;
                    zoomOut = false;
                }

                // Skip intro
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.A))
				{
					SkipIntro();
				}

				if (remoteState.classic.IsTriggered(WiiU.ClassicButton.B))
				{
					SetGamePlayed();
				}

                    if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Plus))
                    {
                        if (pauseManager.paused)
                        {
                            SetGamePlayed();
                        }
                        else
                        {
                            SetGamePauseManager.paused();
                        }
                    }
                break;
            default:
                Vector2 stickNunchuk = remoteState.nunchuk.stick;

				if (Mathf.Abs(stickNunchuk.x) > 0.1f)
				{
					direction.x = stickNunchuk.x;
				}

				if (Mathf.Abs(stickNunchuk.y) > 0.1f)
				{
					direction.y = stickNunchuk.y;
				}

                if (remoteState.IsTriggered(WiiU.RemoteButton.B))
                {
                    playerController.canRun = true;
					SetGamePlayed();
                }
                else if (remoteState.IsReleased(WiiU.RemoteButton.B))
                {
                    playerController.canRun = false;
                }

				// Toggle flashlight
				if (remoteState.IsTriggered(WiiU.RemoteButton.One))
				{
                    ToggleFlashlight(!flashlightEnabled);
                }

                // Zoom in and out
                if (remoteState.IsTriggered(WiiU.RemoteButton.Up))
                {
                    zoomIn = true;
                    zoomOut = false;
                }
                else if (remoteState.IsReleased(WiiU.RemoteButton.Up))
                {
                    zoomIn = false;
                    zoomOut = false;
                }

                if (remoteState.IsTriggered(WiiU.RemoteButton.Down))
				{
					zoomIn = false;
					zoomOut = true;
				}
				else if (remoteState.IsReleased(WiiU.RemoteButton.Down))
				{
					zoomIn = false;
					zoomOut = false;
				}

				// Skip intro
				if (remoteState.IsTriggered(WiiU.RemoteButton.A))
				{
					SkipIntro();
				}

				// Set game pauseManager.paused
				if (remoteState.IsTriggered(WiiU.RemoteButton.Plus))
				{
                    if (pauseManager.paused)
                    {
                        SetGamePlayed();
                    }
                    else
                    {
                        SetGamePauseManager.paused();
                    }
                }
				break;
        }

		if (Application.isEditor)
		{
			// Y axis
			if (Input.GetKey(KeyCode.W))
			{
				direction.y = 1;
			}

			if (Input.GetKey(KeyCode.S))
			{
				direction.y = -1;
			}

			// X axis
			if (Input.GetKey(KeyCode.D))
			{
				direction.x = 1;
			}

			if (Input.GetKey(KeyCode.A))
			{
				direction.x = -1;
			}

			// Toggle run
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                playerController.canRun = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                playerController.canRun = false;
            }

			// Skip intro
			if (Input.GetMouseButtonDown(0))
			{
				SkipIntro();
			}

			// Toggle flashlight
            if (Input.GetMouseButtonDown(1))
			{
                ToggleFlashlight(!flashlightEnabled);
            }

            // Zoom in and out
            if (Input.GetKeyDown(KeyCode.E))
            {
                zoomIn = true;
                zoomOut = false;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                zoomIn = false;
                zoomOut = false;
            }

            if (Input.GetKeyDown(KeyCode.Q))
			{
				zoomIn = false;
				zoomOut = true;
			}
			else if (Input.GetKeyUp(KeyCode.Q))
			{
				zoomIn = false;
				zoomOut = false;
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
                if (pauseManager.paused)
				{
                    SetGamePlayed();
                }
				else
				{
                    SetGamePauseManager.paused();
                }

                SkipIntro();
            }
		} **/

        if (!pauseManager.paused)
        {
            if (debug)
            {
                if (Input.GetKeyDown("f1"))
                {
                    pages = 7;
                    maxrange = 30f;
                    minrange = 10f;
                    targetfog = 0.1f;
                    RenderSettings.fogDensity = 0.1f;
                }
                if (Input.GetKeyDown("f2"))
                {
                    base.transform.parent.transform.position = new Vector3(0f, 2f, 0f);
                }
                if (Input.GetKeyDown("f3"))
                {
                    pages = 8;
                    lost = true;
                }
                if (Input.GetKeyDown("f4"))
                {
                    toolong = 60;
                }
            }
            if (introScript.fltype == 2)
            {
                if (Input.GetMouseButton(1) && introScript.timer >= 1600 && ((!lost && !daytime) || (loseScript.timeleft > 250 && loseScript.timeleft < 950 && daytime)) && !Input.GetButton("Jog/Sprint") && stamina >= 10f)
                {
                    cranking = true;
                    cranksound.volume = 1f;
                }
                else
                {
                    cranking = false;
                    cranksound.volume = 0f;
                }
            }
            if (introScript.gamestarted)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
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


            if (toolong > 0 && introScript.timer >= 1600 && pages < 8)
            {
                toolong--;
                if (toolong <= 0)
                {
                    toolong = 12000;
                    if (pages + level < 9)
                    {
                        level++;
                        maxrange = 100 - (pages + level) * 11;
                        minrange = 80 - (pages + level) * 10;
                        if (pages + level == 1 || pages + level == 3 || pages + level == 5 || pages + level == 7)
                        {
                            fadeinmusic = 0f;
                        }
                    }
                }
            }

            // TO DELETE // 
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
            if (pages >= 8)
            {
                dust.startColor = new Color(0.5f, 0.5f, 0.5f, 0.125f);
            }
            else
            {
                dust.startColor = new Color(0.5f, 0.5f, 0.5f, 0.0625f + (float)(pages + level) * 0.045f);
            }
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
                if (introScript.timer >= 1600)
                {
                    zoomManager.ZoomToggle();
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
                    if (playerController.canRun && direction.y > 0f)
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
                        Quaternion to2 = Quaternion.LookRotation(fldown.position - torch.transform.position);
                        torch.transform.rotation = Quaternion.Slerp(torch.transform.rotation, to2, Time.deltaTime * 8f);
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
                            stamina -= 0.025f;
                            if (stamina < 10f)
                            {
                                stamina = 0f;
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
                    if (stepcd <= 0 && loseScript.timeleft < 950)
                    {
                        stepcd = 120;
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
                        stepcd -= 4;
                        if (stepcd <= 0)
                        {
                            stepcd = 120;
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
                    stepcd = 120;
                    if (!mh)
                    {
                        climbfence.Play();
                    }
                }
                else if (introScript.timer == 1599)
                {
                    playerController.cm.canControl = true;
                    playerController.mouseLook.enabled = true;

                    if (daytime)
                    {
                        torch.enabled = false;
                    }
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
                breathing.volume = 0f;
                zsound.volume = 0f;
                torch.enabled = false;
                return;
            }
            if (loseScript.timeleft >= 250)
            {
                playerController.cm.canControl = true;
                playerController.mouseLook.enabled = true;
            }
            if (loseScript.timeleft >= 950)
            {
                breathing.volume = 0f;
                zsound.volume = 0f;
                playerController.cm.canControl = false;
                playerController.mouseLook.enabled = false;
            }
            if (pages < 8 || loseScript.timeleft < 1000 + loseScript.mhdelay)
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

    /**private void SkipIntro()
	{
		if (!pauseManager.paused && introScript.gamestarted && introScript.timer < 1598 && introScript.timer > 0)
		{
            introScript.timer = 1598;
            climbfence.Stop();
        }
	} **/

    /** private void SetGamePauseManager.paused()
	{
        if (!pauseManager.paused)
		{
            if (introScript.gamestarted)
            {
				if (!lost && introScript.timer >= 1598)
				{
                    if (sanity == 100f)
                    {
                        pauseManager.paused = true;
                        Time.timeScale = 0f;

                        menuManager.DisplayMenu();

                        playerController.cm.canControl = false;
                        playerController.mouseLook.enabled = false;
                    }
                    else
                    {
                        flicker = 3;
                    }
                }
            }
            else
            {
                backedup = true;
            }
        }
    }

	public void SetGamePlayed()
	{
		if (pauseManager.paused)
		{
			menuManager.HideMenu();

            pauseManager.paused = false;
            Time.timeScale = 1f;

            playerController.cm.canControl = true;
            playerController.mouseLook.enabled = true;
        }
	} **/

    /** private void ZoomToggle()
	{
        if (zoomIn && !zoomOut && zoom > 20f)
        {
            zoom -= 0.75f;
            if (zoom < 20f)
            {
                zoom = 20f;
            }
            zsound.volume = 1f;
        }
        else if (zoomOut && !zoomIn && zoom < 60f)
        {
            zoom += 0.75f;
            if (zoom > 60f)
            {
                zoom = 60f;
            }
            zsound.volume = 1f;
        }
        else
        {
            zsound.volume = 0f;
        }

        base.GetComponent<Camera>().fieldOfView = zoom;
    } **/
    
    public void ToggleFlashlight(bool status)
    {
        if (!pauseManager.paused && introScript.fltype == 0 && introScript.timer >= 1600 && ((!lost && !daytime) || (loseScript.timeleft > 250 && loseScript.timeleft < 950 && daytime)))
        {
            if (torch.enabled)
            {
                torch.enabled = false;
            }
            else if (battery > 0f)
            {
                torch.enabled = true;
            }
            flashlight.Play();

            flashlightEnabled = status;
        }
    } 
}
