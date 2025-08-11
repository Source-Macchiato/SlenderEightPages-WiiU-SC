using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class PickupPage : MonoBehaviour
{
	public Transform player;

	public PlayerScript view;

	public AudioClip pagesound;

	public bool withinrange;

	public LayerMask mask;

    private WiiU.GamePad gamePad;
    private WiiU.Remote remote;

    private void Start()
    {
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);
    }

	private void Update()
	{
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        // Gamepad
        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsTriggered(WiiU.GamePadButton.Y))
            {
                PickupPageActions();
            }
        }

        // Remotes
        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Y))
                {
                    PickupPageActions();
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Y))
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
	}

	private void PickupPageActions()
	{
        RaycastHit hitInfo;
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

        if (withinrange && Physics.Raycast(player.position, (base.transform.position - player.position).normalized, out hitInfo, 2f, mask) && hitInfo.collider.gameObject == base.gameObject)
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
            Object.Destroy(base.gameObject);
        }
	}
}
