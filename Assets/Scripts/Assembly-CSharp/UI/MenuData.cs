using UnityEngine;

public class MenuData : MonoBehaviour
{
	[SerializeField] private SwitcherData analyticsSwitcher;

	void Start()
	{

	}

	// Share analytics
	public void SaveAndUpdateAnalytics()
	{
		SaveManager.saveData.settings.shareAnalytics = analyticsSwitcher.currentOptionId == 1 ? 0 : 1;
		SaveManager.Save();

		if (AnalyticsData.analyticsData != null)
		{
            AnalyticsData.analyticsData.CanShareAnalytics();
        }
	}

	public void LoadAnalyticsAndUpdateSwitcher()
	{
		// Get share analytics
		int shareAnalytics = SaveManager.saveData.settings.shareAnalytics;

		int switcherIndex = shareAnalytics == 1 ? 0 : 1;

		if (switcherIndex >= 0 && switcherIndex < analyticsSwitcher.optionsName.Length)
		{
			analyticsSwitcher.currentOptionId = switcherIndex;
			analyticsSwitcher.UpdateText();
		}
	}
}
