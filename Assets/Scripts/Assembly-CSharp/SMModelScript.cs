using UnityEngine;

public class SMModelScript : MonoBehaviour
{
	[SerializeField] private SanityManager sanityManager;
	
	public SlenderMan SM;

	public PlayerScript view;

	private void Update()
	{
		//sanityManager.cansee = false;
		//sanityManager.drain = 0f;
	}

	private void OnWillRenderObject()
	{
		SM.CheckSanity();
	}
}
