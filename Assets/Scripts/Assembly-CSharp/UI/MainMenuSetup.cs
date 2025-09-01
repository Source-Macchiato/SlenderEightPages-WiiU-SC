using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSetup : MonoBehaviour
{
	private MenuManager menuManager;

	void Start()
	{
		menuManager = FindObjectOfType<MenuManager>();

        menuManager.ChangeMenu(0);

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
    }
	
	// Button functions
	public void StartGame()
	{
		if (menuManager.canNavigate)
		{
			menuManager.canNavigate = false;

            SceneManager.LoadSceneAsync("Loading");
        }
	}

	public void Credits()
	{
		if (menuManager.canNavigate)
		{
			menuManager.canNavigate = false;

			SceneManager.LoadSceneAsync("Credits");
		}
	}
}
