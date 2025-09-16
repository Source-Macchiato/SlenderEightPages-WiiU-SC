using UnityEngine;

public class FramerateLimit : MonoBehaviour
{
	[SerializeField] private int framerate = 60;

	void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = framerate;
	}
}
