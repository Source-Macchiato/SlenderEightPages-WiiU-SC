using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class CameraController : MonoBehaviour
{
    [HideInInspector] public bool canLook = true;

	private float sensitivityX = 5f;
	private float sensitivityY = 5f;

	private float minimumY = -60f;
	private float maximumY = 60f;

    [SerializeField]
    private Transform playerTransform;
    private Vector2 currentRotation;

    private WiiU.GamePad gamePad;
    private WiiU.Remote remote;

    private void Start()
    {
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        currentRotation.x = playerTransform.localEulerAngles.y;
        currentRotation.y = transform.localEulerAngles.x;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Prevent camera for moving if canLook is false
        if (!canLook)
        {
            return;
        }

        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        Vector2 input = Vector2.zero;

        // When using the pointer (Wiimote) we will optionally map its Y directly to the camera pitch.
        bool usePointerY = false;
        float mappedPointerY = 0f;

        // Gamepad
        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            input = ReadStick(gamePadState.rStick, input);
        }

        // Remotes
        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                input = ReadStick(remoteState.pro.rightStick, input);
                break;
            case WiiU.RemoteDevType.Classic:
                input = ReadStick(remoteState.classic.rightStick, input);
                break;
            default:
                Vector2 pointerPosition = remoteState.pos;

                pointerPosition.x = ((pointerPosition.x + 1.0f) / 2.0f) * WiiU.Core.GetScreenWidth(WiiU.DisplayIndex.TV);
                pointerPosition.y = WiiU.Core.GetScreenHeight(WiiU.DisplayIndex.TV) - ((pointerPosition.y + 1.0f) / 2.0f) * WiiU.Core.GetScreenHeight(WiiU.DisplayIndex.TV);

                if (pointerPosition.x < 150f)
                {
                    input.x = -1f;
                }
                else if (pointerPosition.x > WiiU.Core.GetScreenWidth(WiiU.DisplayIndex.TV) - 150f)
                {
                    input.x = 1f;
                }

                float remoteRawY = remoteState.pos.y;
                float t = Mathf.Clamp01((remoteRawY + 1f) / 2f);
                mappedPointerY = Mathf.Lerp(maximumY, minimumY, t);
                usePointerY = true;
                break;
        }

        if (Application.isEditor)
        {
            input.x = Input.GetAxis("Mouse X");
            input.y = Input.GetAxis("Mouse Y");

            usePointerY = false;
        }

        // Apply rotation
        currentRotation.x += input.x * sensitivityX;
        currentRotation.y += input.y * sensitivityY;

        // If the Wiimote pointer is being used we override the pitch
        if (usePointerY)
        {
            currentRotation.y = mappedPointerY;
        }

        currentRotation.y = Mathf.Clamp(currentRotation.y, minimumY, maximumY);

        transform.localEulerAngles = new Vector3(-currentRotation.y, 0f, 0f);
        playerTransform.localEulerAngles = new Vector3(0f, currentRotation.x, 0f);
    }

    private Vector2 ReadStick(Vector2 stick, Vector2 current)
    {
        if (Mathf.Abs(stick.x) > 0.1f) current.x = stick.x;
        if (Mathf.Abs(stick.y) > 0.1f) current.y = stick.y;
        return current;
    }
}
