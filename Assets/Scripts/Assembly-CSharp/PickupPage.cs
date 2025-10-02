using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class PickupPage : MonoBehaviour
{
    [SerializeField] private PauseManager pauseManager;
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

    private void Start()
    {
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        pagesManager = FindObjectOfType<PagesManager>();
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
        if (base.GetComponent<Renderer>().isVisible)
        {
            if ((double)Vector3.Distance(base.transform.position, player.position) <= 2.0)
            {
                if (Physics.Raycast(player.position, (base.transform.position - player.position).normalized, out hitInfo, 2f, mask))
                {
                    withinrange = true;
                    view.nearpage = base.transform;
                }
            }
            else if (withinrange)
            {
                withinrange = false;
                view.nearpage = null;
            }
        }
        else if (withinrange)
        {
            withinrange = false;
            view.nearpage = null;
        }

        pickupText.SetActive(withinrange);
    }

	private void PickupPageActions()
	{
        if (!pauseManager.paused && withinrange && Physics.Raycast(player.position, (base.transform.position - player.position).normalized, out hitInfo, 2f, mask) && hitInfo.collider.gameObject == base.gameObject)
		{
            if ((view.pages == 0 || view.pages == 2 || view.pages == 4 || view.pages == 6 || view.pages == 7) && view.level == 0)
            {
                view.fadeinmusic = 0f;
            }
            view.pages++;
            view.fadeoutgui = 0;
            view.nearpage = null;
            view.toolong = 15000;
            view.sprscr.jogSpeed = 3.5f + (float)view.pages * 0.1f;
            if (view.pages < 8)
            {
                view.targetfog = 0.02f + (float)view.pages * 0.01f;
            }
            else
            {
                view.targetfog = 0.01f;
            }
            if (view.level > 0)
            {
                view.level--;
            }
            AudioSource.PlayClipAtPoint(pagesound, base.transform.position);
            if (view.pages < 8)
            {
                view.maxrange = 100 - (view.pages + view.level) * 11;
                view.minrange = 80 - (view.pages + view.level) * 10;
            }
            else
            {
                view.maxrange = 20f;
                view.minrange = 10f;
                view.finaldelay = 750;
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
