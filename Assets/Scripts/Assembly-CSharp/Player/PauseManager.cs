using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private SanityManager sanityManager;
    [SerializeField] private SharedVar shared;

    public bool paused;
    public bool backedup;

    public void ToggleGamePause()
    {
        // Pause logic
        if (!paused)
        {
            // If the game hasn't started yet, keep the original "backedup = true" behavior
            if (!introScript.gamestarted)
            {
                backedup = true;
                return;
            }

            // Require not shared.lost and timer threshold
            if (shared.lost || introScript.timer < 1598)
            {
                return;
            }

            // Only pause when sanity is full otherwise trigger playerScript.flicker
            if (Mathf.Approximately(sanityManager.sanity, 100f))
            {
                paused = true;
                Time.timeScale = 0f;

                menuManager.DisplayMenu();

                playerController.canMove = false;
                cameraController.canLook = false;
            }
            else
            {
                sanityManager.flicker = 3;
            }

            return;
        }

        // Unpause logic
        menuManager.HideMenu();

        paused = false;
        Time.timeScale = 1f;

        playerController.canMove = true;
        cameraController.canLook = true;
    }

    public void SetGamePlay()
    {
        if (paused)
        {
            ToggleGamePause();
        }
    }
}