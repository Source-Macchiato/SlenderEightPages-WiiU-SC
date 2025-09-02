using UnityEngine;
using UnityEngine.UI;

public class PagesManager : MonoBehaviour
{
	[SerializeField] private Image[] pageImages;
	[SerializeField] private Sprite[] pageSprites;
	[SerializeField] private Sprite lockedSprite;
	private bool[] pageStatus = new bool[8];

	void Start()
	{
		// Set all pages to locked when game start
		foreach (Image pageImage in pageImages)
		{
			pageImage.sprite = lockedSprite;
		}
	}

	public void PageUnlocked(Achievements.achievements achievement)
	{
		// Set the sprite linked to the page
		pageImages[(int)achievement].sprite = pageSprites[(int)achievement];

		// Page is now enabled (and the player could press it for display it)
		pageStatus[(int)achievement] = true;

		// Send analytics
		if (AnalyticsData.analyticsData != null)
		{
			StartCoroutine(AnalyticsData.analyticsData.UpdateAnalytics("pages", AnalyticsData.analyticsData.GetPages()));
		}
	}
}
