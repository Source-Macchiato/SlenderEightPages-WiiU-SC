using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        if (GameMode.isCoop)
        {
            StartCoroutine(LoadAsynchronously("Coop"));
        }
        else
        {
            StartCoroutine(LoadAsynchronously("Slender"));
        }
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operationLoadLevel = SceneManager.LoadSceneAsync(sceneName);

        while (!operationLoadLevel.isDone)
        {
            float progress = Mathf.Clamp01(operationLoadLevel.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
    }
}