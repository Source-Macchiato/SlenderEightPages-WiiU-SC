using UnityEngine;

public class ZoomManager : MonoBehaviour 
{
    [Header("Shared Components")]
    public Camera palyerCamera;

    [Header("Zoom")]
    public AudioSource zsound;
    public bool zoomIn;
	public bool zoomOut;
    public float zoom = 60f;

    private void Update()
    {
        ZoomToggle();
    }    
    // ZoomSystem //
    public void ZoomToggle()
    {
        if (zoomIn && !zoomOut && zoom > 20f)
        {
            zoom -= 0.75f;
            if (zoom < 20f)
            {
                zoom = 20f;
            }
            zsound.volume = 1f;
        }
        else if (zoomOut && !zoomIn && zoom < 60f)
        {
            zoom += 0.75f;
            if (zoom > 60f)
            {
                zoom = 60f;
            }
            zsound.volume = 1f;
        }
        else
        {
            zsound.volume = 0f;
        }

        palyerCamera.fieldOfView = zoom;
    }
}