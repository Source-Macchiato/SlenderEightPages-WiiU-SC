using UnityEngine;
using UnityEngine.SceneManagement;
using WiiU = UnityEngine.WiiU;

public class LoseScript : MonoBehaviour
{
	[SerializeField] private FogManager fogManager;
	[SerializeField] private SanityManager sanityManger;
	[SerializeField] private IntroScript introScript;
	[SerializeField] private SharedVar shared;
	[SerializeField] private FlashlightManager flashLightManager;
	public Transform player;

	public PlayerScript view;

	public bool onthistime;

	public int timeleft;

	public Light l1;

	public Light l2;

	public AudioSource san1;

	public AudioSource san2;

	public AudioSource san3;

	public Camera original;

	public GUIStyle credits;

	public bool quitted;

	public Light sun;

	public Material daysky;

	public Material nightsky;

	public Vector3 oldposition;

	public int mhdelay;

	[SerializeField] private Texture2D aIcon;

    private WiiU.GamePad gamePad;
    private WiiU.Remote remote;

    private void Start()
	{
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        nightsky = RenderSettings.skybox;
		base.transform.parent.GetComponent<Camera>().enabled = false;
		Color color = base.GetComponent<Renderer>().material.color;
		color.a = 0.4f;
		base.GetComponent<Renderer>().material.color = color;
	}

	private void OnGUI()
	{
		if (shared.pages >= 8)
		{
			if (shared.mh)
			{
				if (timeleft >= 1100 && timeleft < 1350)
				{
					GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "The tape ends there.", credits);
				}
				else if (timeleft >= 1500 && timeleft < 1900)
				{
					GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 65, 600f, 50f), "There was no sign of who had", credits);
					GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "filmed it, only a label on the", credits);
					GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 + 15, 600f, 50f), "tape which read 'WATCH THIS'.", credits);
				}
				else if (timeleft >= 2050 && timeleft < 2300)
				{
					GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "Someone is trying to help me.", credits);
				}
			}
		}
		if (shared.mh)
		{
			if (timeleft >= 300 && timeleft < 550)
			{
				GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "The tape ends there.", credits);
			}
			else if (timeleft >= 700 && timeleft < 1100)
			{
				GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 65, 600f, 50f), "There was no label on the", credits);
				GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "tape, nor any indication", credits);
				GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 + 15, 600f, 50f), "as to who had filmed it.", credits);
			}
			else if (timeleft >= 1250 && timeleft < 1500)
			{
				GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 25, 600f, 50f), "But I intend to find out.", credits);
			}
		}
		if (shared.pages < 8 && timeleft >= 250 + mhdelay && !quitted)
		{
			GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 75, 600f, 50f), "Pages: " + shared.pages + "/8", credits);
            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 + 25, 600f, 50f), new GUIContent(" Press to continue", aIcon), credits);
        }
	}

	private void Update()
	{
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

		if (gamePadState.gamePadErr == WiiU.GamePadError.None)
		{
			if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
			{
				GameOver();
			}
		}

        switch (remoteState.devType)
		{
			case WiiU.RemoteDevType.ProController:
				if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.A))
				{
					GameOver();
				}
				break;
			case WiiU.RemoteDevType.Classic:
				if (remoteState.classic.IsTriggered(WiiU.ClassicButton.A))
				{
					GameOver();
				}
				break;
			default:
				if (remoteState.IsTriggered(WiiU.RemoteButton.A))
				{
					GameOver();
				}
				break;
		}

		if (Application.isEditor)
		{
			if (Input.GetMouseButtonDown(0))
			{
				GameOver();
			}
		}
	}

	private void FixedUpdate()
	{
		base.transform.parent.position = new Vector3(0f, -180f, 0f);
		if (introScript.timer == 1599 && shared.daytime)
		{
			RenderSettings.skybox = daysky;
		}
		if (!shared.lost)
		{
			return;
		}
		if (timeleft == 0)
		{
			if (MedalsManager.medalsManager != null)
			{
				MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.SLENDER);
			}

			original.enabled = false;
			base.transform.parent.GetComponent<Camera>().enabled = true;
			oldposition = player.position;
			player.position = new Vector3(0f, -2000f, 0f);

            // Disable skybox
            RenderSettings.skybox = null;
            DynamicGI.UpdateEnvironment();

            sun.enabled = false;
			if (shared.mh)
			{
				mhdelay = 1400;
			}
			view.dust.Stop();
		}
		base.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Random.value, Random.value);
		if (timeleft < 20 || timeleft > 120)
		{
			onthistime = false;
		}
		else if ((double)Random.value < 0.7)
		{
			onthistime = true;
		}
		else
		{
			onthistime = false;
		}
		if (onthistime)
		{
			l1.intensity = 1f;
			l2.intensity = 1f;
			san1.volume = 1f;
			san2.volume = 1f;
			san3.volume = 1f;
		}
		else if (timeleft < 250)
		{
			l1.intensity = 0f;
			l2.intensity = 0f;
			san1.volume = 0f;
			san2.volume = 0f;
			san3.volume = 0f;
		}
		if (timeleft < 250 + mhdelay || (timeleft < 2700 + mhdelay && shared.pages >= 8))
		{
			timeleft++;
		}
		if (timeleft == 250)
		{
			san1.volume = 0f;
			san2.volume = 0f;
			san3.volume = 0f;
		}
		if (timeleft == 251 && shared.pages >= 8)
		{
			onthistime = false;
			if (shared.daytime)
			{
                // Disable skybox
                RenderSettings.skybox = null;
                DynamicGI.UpdateEnvironment();

                flashLightManager.torch.enabled = true;
				sun.enabled = false;
			}
			else
			{
				// Disable the fog
				fogManager.targetfog = 0f;

                // Disable skybox
                RenderSettings.skybox = daysky;
                DynamicGI.UpdateEnvironment();
				sun.enabled = true;
				
				if (MedalsManager.medalsManager != null)
				{
					MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.THE_EIGHT_PAGES);
				}
			}
			if (shared.dustyair && shared.daytime)
			{
				view.dust.Play();
			}
			original.enabled = true;
			base.transform.parent.GetComponent<Camera>().enabled = false;
			player.position = oldposition;
			player.LookAt(new Vector3(shared.endfix.position.x, player.position.y, shared.endfix.position.z));
		}
		if (timeleft >= 950 && shared.pages >= 8)
		{
			original.enabled = false;
			base.transform.parent.GetComponent<Camera>().enabled = true;
			l1.intensity = 0f;
			l2.intensity = 0f;
			sun.enabled = false;

            // Disable skybox
            RenderSettings.skybox = null;
            DynamicGI.UpdateEnvironment();

            player.position = new Vector3(0f, -2000f, 0f);

            quitted = true;
            SceneManager.LoadScene("Credits");
        }
		if (timeleft > 250 && shared.pages >= 8)
		{
			if (shared.mh)
			{
				if ((float)timeleft < 425f)
				{
					san1.volume = (450f - (float)timeleft) / 100f;
					san2.volume = (350f - (float)timeleft) / 100f;
					san3.volume = 0f;
				}
				else if (timeleft < 950)
				{
					san1.volume = 0.25f;
					san2.volume = 0f;
					san3.volume = 0f;
				}
				else
				{
					san1.volume = 0f;
					san2.volume = 0f;
					san3.volume = 0f;
				}
			}
			else
			{
				san1.volume = (450f - (float)timeleft) / 100f;
				san2.volume = (350f - (float)timeleft) / 100f;
				san3.volume = 0f;
			}
		}
		if (timeleft >= 2700 + mhdelay && shared.pages >= 8)
		{
			quitted = true;
			SceneManager.LoadScene("MainMenu");
		}
		if (timeleft == 250 + mhdelay && shared.pages >= 8)
		{
			view.fadeinmusic = 0f;
		}
		if (timeleft == 2500 + mhdelay && shared.pages >= 8)
		{
			view.fadeinmusic = 0f;
		}
	}

	private void GameOver()
	{
        if (shared.pages < 8 && timeleft >= 250)
        {
            quitted = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
