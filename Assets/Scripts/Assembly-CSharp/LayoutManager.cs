using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class LayoutManager : MonoBehaviour
{
	[SerializeField] private Camera[] tvCameras;
    [SerializeField] private Camera[] gamepadCameras;
    [SerializeField] private Canvas[] canvas;

	void Start()
	{
        // ChangeLayout();
	}

#if UNITY_EDITOR
    void Update()
	{
        if (Input.GetKeyDown(KeyCode.P))
        {
            int layoutId = SaveManager.saveData.settings.layoutId;
            SaveManager.saveData.settings.layoutId = layoutId == -1 || layoutId == 0 ? 1 : 0;

            ChangeLayout();
        }
    }
#endif

    private void ChangeLayout()
    {
        if (SaveManager.saveData.settings.layoutId == 1)
        {
            foreach (Camera camera in tvCameras)
            {
                camera.targetDisplay = WiiU.DisplayIndex.GamePad;
            }

            foreach (Camera camera in gamepadCameras)
            {
                camera.targetDisplay = WiiU.DisplayIndex.TV;
            }

            canvas[0].targetDisplay = WiiU.DisplayIndex.GamePad;
            canvas[1].targetDisplay = WiiU.DisplayIndex.TV;
        }
        else
        {
            foreach (Camera camera in tvCameras)
            {
                camera.targetDisplay = WiiU.DisplayIndex.TV;
            }

            foreach (Camera camera in gamepadCameras)
            {
                camera.targetDisplay = WiiU.DisplayIndex.GamePad;
            }

            canvas[0].targetDisplay = WiiU.DisplayIndex.TV;
            canvas[1].targetDisplay = WiiU.DisplayIndex.GamePad;
        }
    }
}
