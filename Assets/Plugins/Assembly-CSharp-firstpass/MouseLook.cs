using UnityEngine;
using WiiU = UnityEngine.WiiU;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	public float sensitivityX = 15f;

	public float sensitivityY = 15f;

	public float minimumX = -360f;

	public float maximumX = 360f;

	public float minimumY = -60f;

	public float maximumY = 60f;

    [SerializeField]
    private Transform playerTransform;
    private Vector2 currentRotation;

    private WiiU.GamePad gamePad;
    private WiiU.Remote remote;

    private void Start()
	{
		if ((bool)base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}

        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        currentRotation.x = playerTransform.localEulerAngles.y;
        currentRotation.y = transform.localEulerAngles.x;
    }

    private void Update()
    {
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        Vector2 input = Vector2.zero;

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


                break;
        }

        if (Application.isEditor)
        {
            input.x = Input.GetAxis("Mouse X");
            input.y = Input.GetAxis("Mouse Y");
        }

        // Apply rotation
        currentRotation.x += input.x * sensitivityX;
        currentRotation.y += input.y * sensitivityY;
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
