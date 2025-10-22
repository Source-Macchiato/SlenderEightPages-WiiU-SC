using UnityEngine;

public class MenuData : MonoBehaviour
{
	[SerializeField] private SwitcherData analyticsSwitcher;
	[SerializeField] private SwitcherData fogSwitcher;

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

	// Fog
	public void SaveFogSwitcher()
	{
		SaveManager.saveData.settings.fogEnabled = fogSwitcher.currentOptionId == 1 ? false : true;
		SaveManager.Save();
	}

	public void LoadFogSwitcher()
	{
		bool fogEnabled = SaveManager.saveData.settings.fogEnabled;

		int switcherIndex = fogEnabled == true ? 0 : 1;

		if (switcherIndex >= 0 && switcherIndex < fogSwitcher.optionsName.Length)
		{
			fogSwitcher.currentOptionId = switcherIndex;
			fogSwitcher.UpdateText();
		}
	}
}
