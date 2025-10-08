using UnityEngine;

public class FogManager : MonoBehaviour 
{
    public float targetfog = 0.02f;
    [Header("Scripts")]
    [SerializeField] private PauseManager pauseManager;

    private void Update()
    {

        if (!pauseManager.paused)
        {
            // Fog Manager //
            if ((double)targetfog + 0.001 < (double)RenderSettings.fogDensity)
            {
                RenderSettings.fogDensity -= 0.001f;
            }
            else if ((double)targetfog - 0.0002 > (double)RenderSettings.fogDensity)
            {
                RenderSettings.fogDensity += 0.0002f;
            }
            else
            {
                RenderSettings.fogDensity = targetfog;
            }
        }
    }
}