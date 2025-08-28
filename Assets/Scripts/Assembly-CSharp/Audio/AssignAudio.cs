using UnityEngine;
using WiiU = UnityEngine.WiiU;

[RequireComponent(typeof(AudioSource))]
public class AssignAudio : MonoBehaviour
{
    [SerializeField] private bool tv = false;
    [SerializeField] private bool gamepad = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (tv && gamepad)
        {
            WiiU.AudioSourceOutput.Assign(audioSource, WiiU.AudioOutput.TV | WiiU.AudioOutput.GamePad);
        }
        else if (tv)
        {
            WiiU.AudioSourceOutput.Assign(audioSource, WiiU.AudioOutput.TV);
        }
        else if (gamepad)
        {
            WiiU.AudioSourceOutput.Assign(audioSource, WiiU.AudioOutput.GamePad);
        }
        else
        {
            WiiU.AudioSourceOutput.Assign(audioSource, WiiU.AudioOutput.TV | WiiU.AudioOutput.GamePad);
        }
    }
}