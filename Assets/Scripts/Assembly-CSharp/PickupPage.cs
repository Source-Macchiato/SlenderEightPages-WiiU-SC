using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class PickupPage : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private FogManager fogManager;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private SprintScript sprscr;
    [SerializeField] private SharedVar shared;

    [Header("Settings")]
	public Transform player;

	public AudioClip pagesound;

	public bool withinrange;

	public LayerMask mask;

    [SerializeField] private GameObject pickupText;

    private RaycastHit hitInfo;

    public PlayerScript view;
    private PagesManager pagesManager;

    [SerializeField] private Achievements.achievements achievement;

    private WiiU.GamePad gamePad;
    private WiiU.Remote remote;

    private Renderer pageRenderer;

    private void Start()
    {
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        pagesManager = FindObjectOfType<PagesManager>();

        pageRenderer = GetComponent<Renderer>();
    }

	private void Update()
	{
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        // Gamepad
        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsTriggered(WiiU.GamePadButton.X))
            {
                PickupPageActions();
            }
        }

        // Remotes
        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.X))
                {
                    PickupPageActions();
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.X))
                {
                    PickupPageActions();
                }
                break;
            default:
                if (remoteState.IsTriggered(WiiU.RemoteButton.A))
                {
                    PickupPageActions();
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickupPageActions();
            }
        }

        // Detect player looking at pages
        if (pageRenderer.isVisible)
        {
            Debug.Log("Visible");
            if ((double)Vector3.Distance(base.transform.position, player.position) <= 2.0)
            {
                if (Physics.Raycast(player.position, (base.transform.position - player.position).normalized, out hitInfo, 2f, mask))
                {
                    withinrange = true;
                    shared.nearpage = base.transform;
                }
            }
            else if (withinrange)
            {
                withinrange = false;
                shared.nearpage = null;
            }
        }
        else if (withinrange)
        {
            withinrange = false;
            shared.nearpage = null;
        }

        pickupText.SetActive(withinrange);
    }

	private void PickupPageActions()
	{
        if (!pauseManager.paused && withinrange && Physics.Raycast(player.position, (base.transform.position - player.position).normalized, out hitInfo, 2f, mask) && hitInfo.collider.gameObject == base.gameObject)
		{
            if ((shared.pages == 0 || shared.pages == 2 || shared.pages == 4 || shared.pages == 6 || shared.pages == 7) && shared.level == 0)
            {
                view.fadeinmusic = 0f;
            }
            shared.pages++;
            view.fadeoutgui = 0;
            shared.nearpage = null;
            view.toolong = 15000;
            sprscr.jogSpeed = 3.5f + (float)shared.pages * 0.1f;
            if (shared.pages < 8)
            {
                fogManager.targetfog = 0.02f + (float)shared.pages * 0.01f;
            }
            else
            {
                fogManager.targetfog = 0.01f;
            }
            if (shared.level > 0)
            {
                shared.level--;
            }
            AudioSource.PlayClipAtPoint(pagesound, base.transform.position);
            if (shared.pages < 8)
            {
                view.maxrange = 100 - (shared.pages + shared.level) * 11;
                view.minrange = 80 - (shared.pages + shared.level) * 10;
            }
            else
            {
                view.maxrange = 20f;
                view.minrange = 10f;
                shared.finaldelay = 750;
            }

            if (MedalsManager.medalsManager != null)
            {
                MedalsManager.medalsManager.UnlockAchievement(achievement);
            }

            // Display page on GamePad
            if (pagesManager != null)
            {
                pagesManager.PageUnlocked(achievement);
            }

            Object.Destroy(base.gameObject);
        }
	}
}
