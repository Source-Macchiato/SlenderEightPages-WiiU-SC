using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class PlayerController : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private IntroScript introScript;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private ZoomManager zoomManager;
    [SerializeField] private PlayerScript playerScript; // TEMPORARY

    [Header("Player References")]
    public CharacterMotor cm;
    public MouseLook mouseLook;
    public bool paused;
    public bool canRun;
    public bool flashlightEnabled;
    public Vector2 direction = Vector2.zero;
    private WiiU.GamePad gamePad;
    private WiiU.Remote remote;

    private void Start()
    {
        cm.canControl = false;
        mouseLook.enabled = false;

        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);
    }

    private void Update()
    {
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
                canRun = true;
            }
            else if (gamePadState.IsReleased(WiiU.GamePadButton.ZR))
            {
                canRun = false;
            }

            // Toggle flashlight
            if (gamePadState.IsTriggered(WiiU.GamePadButton.StickR))
            {
                playerScript.ToggleFlashlight(!flashlightEnabled);
            }
            else if (gamePadState.IsTriggered(WiiU.GamePadButton.Y))
            {
                playerScript.ToggleFlashlight(!flashlightEnabled);
            }

            // Zoom in and out
            if (gamePadState.IsTriggered(WiiU.GamePadButton.ZL))
            {
                zoomManager.zoomIn = true;
                zoomManager.zoomOut = false;
            }
            else if (gamePadState.IsReleased(WiiU.GamePadButton.ZL))
            {
                zoomManager.zoomIn = false;
                zoomManager.zoomOut = true;
            }

            if (gamePadState.IsTriggered(WiiU.GamePadButton.Up))
            {
                zoomManager.zoomIn = true;
                zoomManager.zoomOut = false;
            }
            else if (gamePadState.IsReleased(WiiU.GamePadButton.Up))
            {
                zoomManager.zoomIn = false;
                zoomManager.zoomOut = false;
            }

            if (gamePadState.IsTriggered(WiiU.GamePadButton.Down))
            {
                zoomManager.zoomIn = false;
                zoomManager.zoomOut = true;
            }
            else if (gamePadState.IsReleased(WiiU.GamePadButton.Down))
            {
                zoomManager.zoomIn = false;
                zoomManager.zoomOut = false;
            }

            // Skip Intro
            if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
            {
                introScript.SkipIntro();
            }

            if (gamePadState.IsTriggered(WiiU.GamePadButton.B))
            {
                pauseManager.ToggleGamePause();
            }

            if (gamePadState.IsTriggered(WiiU.GamePadButton.Plus))
            {
                pauseManager.ToggleGamePause();
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
                    canRun = true;
                }
                else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.ZR))
                {
                    canRun = false;
                }

                // Toggle flashlight
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.StickR))
                {
                    playerScript.ToggleFlashlight(!flashlightEnabled);
                }
                else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Y))
                {
                    playerScript.ToggleFlashlight(!flashlightEnabled);
                }

                // Zoom in and out
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.ZL))
                {
                    zoomManager.zoomIn = true;
                    zoomManager.zoomOut = false;
                }
                else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.ZL))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = true;
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Up))
                {
                    zoomManager.zoomIn = true;
                    zoomManager.zoomOut = false;
                }
                else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.Up))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = false;
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Down))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = true;
                }
                else if (remoteState.pro.IsReleased(WiiU.ProControllerButton.Down))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = false;
                }

                // Skip intro
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.A))
                {
                    introScript.SkipIntro();
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.B))
                {
                    pauseManager.ToggleGamePause();
                }

                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Plus))
                {
                    pauseManager.ToggleGamePause();
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
                    canRun = true;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.R))
                {
                    canRun = false;
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.ZR))
                {
                    canRun = true;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.ZR))
                {
                    canRun = false;
                }

                // Toggle flashlight
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Y))
                {
                    playerScript.ToggleFlashlight(!flashlightEnabled);
                }

                // Zoom in and out
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.L))
                {
                    zoomManager.zoomIn = true;
                    zoomManager.zoomOut = false;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.L))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = true;
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.ZL))
                {
                    zoomManager.zoomIn = true;
                    zoomManager.zoomOut = false;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.ZL))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = true;
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Up))
                {
                    zoomManager.zoomIn = true;
                    zoomManager.zoomOut = false;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.Up))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = false;
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Down))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = true;
                }
                else if (remoteState.classic.IsReleased(WiiU.ClassicButton.Down))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = false;
                }

                // Skip intro
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.A))
                {
                    introScript.SkipIntro();
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.B))
                {
                    pauseManager.ToggleGamePause();
                }

                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Plus))
                {
                    pauseManager.ToggleGamePause();
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
                    canRun = true;
                    pauseManager.ToggleGamePause();
                }
                else if (remoteState.IsReleased(WiiU.RemoteButton.B))
                {
                    canRun = false;
                }

                // Toggle flashlight
                if (remoteState.IsTriggered(WiiU.RemoteButton.One))
                {
                    playerScript.ToggleFlashlight(!flashlightEnabled);
                }

                // Zoom in and out
                if (remoteState.IsTriggered(WiiU.RemoteButton.Up))
                {
                    zoomManager.zoomIn = true;
                    zoomManager.zoomOut = false;
                }
                else if (remoteState.IsReleased(WiiU.RemoteButton.Up))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = false;
                }

                if (remoteState.IsTriggered(WiiU.RemoteButton.Down))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = true;
                }
                else if (remoteState.IsReleased(WiiU.RemoteButton.Down))
                {
                    zoomManager.zoomIn = false;
                    zoomManager.zoomOut = false;
                }

                // Skip intro
                if (remoteState.IsTriggered(WiiU.RemoteButton.A))
                {
                    introScript.SkipIntro();
                }

                // Set game paused
                if (remoteState.IsTriggered(WiiU.RemoteButton.Plus))
                {
                    pauseManager.ToggleGamePause();
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
                canRun = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                canRun = false;
            }

            // Skip intro
            if (Input.GetMouseButtonDown(0))
            {
                introScript.SkipIntro();
            }

            // Toggle flashlight
            if (Input.GetMouseButtonDown(1))
            {
                playerScript.ToggleFlashlight(!flashlightEnabled);
            }

            // Zoom in and out
            if (Input.GetKeyDown(KeyCode.E))
            {
                zoomManager.zoomIn = true;
                zoomManager.zoomOut = false;
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                zoomManager.zoomIn = false;
                zoomManager.zoomOut = false;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                zoomManager.zoomIn = false;
                zoomManager.zoomOut = true;
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                zoomManager.zoomIn = false;
                zoomManager.zoomOut = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseManager.ToggleGamePause();

                introScript.SkipIntro();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!pauseManager.paused)
        {
            playerScript.chasetest.position = base.transform.position;
            Quaternion rotation = Quaternion.LookRotation(base.transform.position - playerScript.SM.transform.position, Vector3.up);
            rotation.x = 0f;
            rotation.z = 0f;
            playerScript.chasetest.rotation = rotation;
        }
    }
}