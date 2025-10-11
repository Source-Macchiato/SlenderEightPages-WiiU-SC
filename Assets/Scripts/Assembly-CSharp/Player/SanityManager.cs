using UnityEngine;

public class SanityManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private IntroScript introScript;
    [SerializeField] private LoseScript loseScript;
    [SerializeField] private SlenderMan slenderMan;
    [SerializeField] private SharedVar shared;

    [Header("Sanity Manager")]
    public AudioSource san1;
    public AudioSource san2;
    public AudioSource san3;
    public Transform tentacles;
    public float sanity = 100f;
    public bool cansee;
    public bool justsaw;
    public float drain;
    public int flicker;
    public bool endflicker;
    public bool lastflicker;

    // Constants //
    private const float SANITY_MAX = 100f;              // - Maximum sanity value
    private const float SANITY_REGEN = 0.1f;            // - Sanity regeneration rate when not seeing
    private const float SANITY_CAUGHT_DRAIN = 1f;       // - Sanity drain rate when caught
    private const float MUSIC_FADE_RATE = 0.01f;        // - Rate at which music fades in/out
    private const float ROTATION_SPEED = 2f;            // - Speed of rotation towards Slender
    private const float TENTACLE_SCALE_FACTOR = 0.01f;  // - Tentacle scaling factor
    private const float TENTACLE_LOST_SCALE = 0.8f;     // - Tentacle scale when lost

    // Private Variables //
    private Transform parentTransform; // Player's parent transform
    private Transform slenderTransform; // Slender

    private void Start()
    {   
        // Initialize references //
        parentTransform = transform.parent;
        slenderTransform = slenderMan.SM.transform;
    }

    private void Update()
    {
        if (endflicker)
        {
            endflicker = false;
            lastflicker = true;
        }

        if (pauseManager.paused) return;

        ResetMusicVolumes();

        if (!introScript.introEnded) return;

        HandleCaughtState();

        if (shared.lost && loseScript.timeleft <= 250) return;

        UpdateSanity();
        UpdateAudioVolumes();
        UpdateFlicker();
        UpdateTentacles();
        UpdateBackgroundMusic();

        if (shared.scared > 0)
        {
            shared.scared--;
        }  
    }

    // Resets all background music //
    private void ResetMusicVolumes()
    {
        shared.music1.volume = 0f;
        shared.music2.volume = 0f;
        shared.music3.volume = 0f;
        shared.music4.volume = 0f;
    }

    // Handles player rotation and control when caught by Slender //
    private void HandleCaughtState()
    {
        if (shared.caught && !shared.lost)
        {
            playerController.canMove = false;
            cameraController.canLook = false;

            Vector3 targetPos = slenderTransform.position;
            targetPos.y += 1f;
            Quaternion targetRot = Quaternion.LookRotation(targetPos - parentTransform.position);
            parentTransform.rotation = Quaternion.Slerp(parentTransform.rotation, targetRot, Time.deltaTime * ROTATION_SPEED);
        }
    }

    // Updates sanity based on current conditions //
    private void UpdateSanity()
    {
        if (shared.caught)
        {
            sanity -= 1f;
            if (sanity < 0f)
                shared.lost = true;
        }
        else if (!cansee)
        {
            if (sanity < SANITY_MAX)
            {
                sanity = Mathf.Min(sanity + SANITY_REGEN, SANITY_MAX);
            }
        }
        else if (drain > 0f)
        {
            sanity -= drain;
            Debug.Log("Draining sanity by " + drain);
            if (sanity < 0f)
                shared.lost = true;
        }

        if (shared.lost)
            sanity = SANITY_MAX;
        else if (sanity < 0f)
            sanity = 0f;

        justsaw = cansee;
    }

    // Updates audio volumes based on current sanity level //
    private void UpdateAudioVolumes()
    {
        if (loseScript.timeleft != 0)
        {
            san1.volume = 0f;
            san2.volume = 0f;
            san3.volume = 0f;
            return;
        }

        if (sanity < 70f || shared.mh)
        {
            san1.volume = 1f;
            if (sanity < 40f)
            {
                san2.volume = 1f;
                san3.volume = sanity < 10f ? 1f : (40f - sanity) / 30f;
            }
            else
            {
                san2.volume = (70f - sanity) / 30f;
                san3.volume = 0f;
            }
        }
        else
        {
            san1.volume = (SANITY_MAX - sanity) / 30f;
            san2.volume = 0f;
            san3.volume = 0f;
        }
    }

    // Updates flicker state and audio during flicker //
    private void UpdateFlicker()
    {
        if (flicker > 0 || endflicker || lastflicker)
        {
            san1.volume = 1f;
            san2.volume = 1f;
            san3.volume = 1f;

            if (flicker > 0)
            {
                flicker--;
                if (flicker == 0 && !lastflicker)
                    endflicker = true;
            }

            if (lastflicker)
                lastflicker = false;
        }
    }

    // Updates tentacle scaling based on sanity and lost state //
    private void UpdateTentacles()
    {
        if (shared.lost && !shared.mh)
        {
            tentacles.localScale = new Vector3(TENTACLE_LOST_SCALE, TENTACLE_LOST_SCALE, 0.5f);
        }
        else if (sanity > 80f || shared.mh)
        {
            tentacles.localScale = Vector3.zero;
        }
        else
        {
            float scale = (80f - sanity) * TENTACLE_SCALE_FACTOR;
            tentacles.localScale = new Vector3(scale, scale, scale * 0.5f);
        }
    }

    // Updates background music based on game progress //
    private void UpdateBackgroundMusic()
    {
        int progress = shared.pages + shared.level;
        if (progress <= 0 || loseScript.timeleft != 0 || shared.mh) return;

        if (progress < 3)           // 0-2 Pages
        {
            shared.music1.volume = shared.fadeinmusic;
        }
        else if (progress < 5)      // 3-4 Pages
        {
            shared.music1.volume = 2f - shared.fadeinmusic;
            shared.music2.volume = shared.fadeinmusic;
        }
        else if (progress < 7)      // 5-6 Pages
        {
            shared.music2.volume = 2f - shared.fadeinmusic;
            shared.music3.volume = shared.fadeinmusic;
        }
        else if (progress < 8)      // 7 Pages
        {
            shared.music3.volume = 2f - shared.fadeinmusic;
            shared.music4.volume = shared.fadeinmusic;
        }
        else                        // 8 Pages
        {
            shared.music4.volume = 1f - shared.fadeinmusic * 0.5f;
        }

        shared.fadeinmusic = Mathf.Min(shared.fadeinmusic + MUSIC_FADE_RATE, 2f); // Cap fade-in value
    }
}