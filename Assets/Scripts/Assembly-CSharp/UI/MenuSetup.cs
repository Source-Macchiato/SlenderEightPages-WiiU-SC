using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSetup : MonoBehaviour
{
	private MenuManager menuManager;

	// Use this for initialization
	void Start()
	{
		menuManager = FindObjectOfType<MenuManager>();
	}
	
	// Button functions
	public void StartGame()
	{
		if (menuManager.canNavigate)
		{
			menuManager.canNavigate = false;

            SceneManager.LoadSceneAsync("Slender");
        }
	}
}
