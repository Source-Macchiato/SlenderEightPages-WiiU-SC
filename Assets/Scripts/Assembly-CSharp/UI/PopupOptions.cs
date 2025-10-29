using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using BrewConnect;

public class PopupOptions : MonoBehaviour
{
	public Button[] buttons;
	public GameObject[] cursors;
	
	void Update()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			cursors[i].SetActive(EventSystem.current.currentSelectedGameObject == buttons[i].gameObject);
        }
	}

	public void ShareAnalytics(bool share)
	{
		Analytics.analytics.ShareAnalytics(share);
	}
}
