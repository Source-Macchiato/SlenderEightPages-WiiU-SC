using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSetup : MonoBehaviour
{
	private MenuManager menuManager;
	private MenuData menuData;

	void Start()
	{
		menuManager = FindObjectOfType<MenuManager>();
		menuData = FindObjectOfType<MenuData>();

        menuManager.ChangeMenu(0);

		menuManager.SetBackCallback(2, OnBackFromBrewConnect);
		menuManager.SetBackCallback(3, OnBackFromVideo);

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
    }
	
	// Button functions
	public void StartGame()
	{
		if (menuManager.canNavigate)
		{
			menuManager.canNavigate = false;

			GameMode.isCoop = false;

            SceneManager.LoadSceneAsync("Loading");
        }
	}

	public void StartCoop()
	{
        if (menuManager.canNavigate)
        {
            menuManager.canNavigate = false;

            GameMode.isCoop = true;

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

	public void BrewConnect()
	{
		menuManager.ChangeMenu(2);

        menuData.LoadAnalyticsAndUpdateSwitcher();
    }

	public void Video()
	{
		menuManager.ChangeMenu(3);

		menuData.LoadFogSwitcher();
	}

	// Callback functions
	public void OnBackFromBrewConnect()
	{
		menuData.SaveAndUpdateAnalytics();
	}

	public void OnBackFromVideo()
	{
		menuData.SaveFogSwitcher();
	}
}
