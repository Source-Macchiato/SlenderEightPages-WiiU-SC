using UnityEngine;
using UnityEngine.UI;

using BrewConnect;

public class PagesManager : MonoBehaviour
{
    [SerializeField] private GameObject pageFullScreen;
    
	[SerializeField] private Sprite lockedSprite;

    [SerializeField] private Image[] pageImages;
	[SerializeField] private Sprite[] pageSprites;
	
	private bool[] pageStatus = new bool[8];

	void Start()
	{
		// Set all pages to locked when game start
		foreach (Image pageImage in pageImages)
		{
			pageImage.sprite = lockedSprite;
		}

		// Disable full page by default
		pageFullScreen.SetActive(false);
	}

	public void PageUnlocked(Achievements.achievements achievement)
	{
		// Set the sprite linked to the page
		pageImages[(int)achievement].sprite = pageSprites[(int)achievement];

		// Page is now enabled (and the player could press it for display it)
		pageStatus[(int)achievement] = true;

		// Send analytics
		if (Analytics.analytics != null)
		{
			StartCoroutine(Analytics.analytics.UpdateAnalytics("pages", Analytics.analytics.GetPages()));
		}
	}

	public void DisplayPage(int achievementId)
	{
		if (pageStatus[achievementId])
		{
            pageFullScreen.SetActive(true);

            pageFullScreen.GetComponent<Image>().sprite = pageSprites[achievementId];
        }
	}

	public void HidePage()
	{
		pageFullScreen.SetActive(false);
	}
}
