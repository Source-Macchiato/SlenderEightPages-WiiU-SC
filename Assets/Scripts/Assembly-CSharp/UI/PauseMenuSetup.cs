using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuSetup : MonoBehaviour
{
	void Start()
	{
		
	}

	public void Quit()
	{
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}
