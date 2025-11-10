using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Credits : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI descriptionText;
	[SerializeField] private TextMeshProUGUI thanksForPlayingText;

	void Start()
	{
		StartCoroutine(CreditsSequence());
	}

	private IEnumerator CreditsSequence()
	{
        titleText.text = "Game Design & Programming";
		descriptionText.text = "Mark J. Hadley";
        thanksForPlayingText.text = string.Empty;

        yield return new WaitForSeconds(1.5f);

		titleText.text = string.Empty;
		descriptionText.text = string.Empty;

		yield return new WaitForSeconds(0.5f);

		titleText.text = "Music & Sound";
		descriptionText.text = "Mark J. Hadley";

		yield return new WaitForSeconds(1.5f);

        titleText.text = string.Empty;
        descriptionText.text = string.Empty;

		yield return new WaitForSeconds(0.5f);

		titleText.text = "Models";
		descriptionText.text = "Pau Cano";

		yield return new WaitForSeconds(2f);

		descriptionText.text = "Universal Image";

		yield return new WaitForSeconds(2f);

		descriptionText.text = "VIS Games";

		yield return new WaitForSeconds(2f);

		descriptionText.text = "Profi Developers";

		yield return new WaitForSeconds(2f);

		descriptionText.text = "Unity Technology";

		yield return new WaitForSeconds(1.5f);

        titleText.text = string.Empty;
        descriptionText.text = string.Empty;

        yield return new WaitForSeconds(0.5f);

        titleText.text = "Port by Source Macchiato";
        descriptionText.text = "Alexis L.";

		yield return new WaitForSeconds(2f);

		descriptionText.text = "Alyx Lihoreau";

		yield return new WaitForSeconds(1.5f);

        titleText.text = string.Empty;
        descriptionText.text = string.Empty;

		yield return new WaitForSeconds(0.5f);

		titleText.text = "Decompilation";
		descriptionText.text = "Gamejolt @Real_JackOfficial_Demon";

		yield return new WaitForSeconds(1.5f);

        titleText.text = string.Empty;
        descriptionText.text = string.Empty;

        yield return new WaitForSeconds(0.5f);

        thanksForPlayingText.text = "Thanks for playing";

		yield return new WaitForSeconds(1.5f);

		thanksForPlayingText.text = string.Empty;

		yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene("MainMenu");
    }
}
