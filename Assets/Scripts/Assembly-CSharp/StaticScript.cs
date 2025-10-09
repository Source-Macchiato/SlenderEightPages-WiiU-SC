using UnityEngine;

public class StaticScript : MonoBehaviour
{
	[SerializeField] private SharedVar shared;
	[SerializeField] private SanityManager sanityManager;
	public LoseScript final;

	private Renderer rend;

	private void Start()
	{
		rend = GetComponent<Renderer>();
	}

	private void FixedUpdate()
	{
		Color color = rend.material.color;
		if (!shared.lost)
		{
			if (sanityManager.flicker <= 0)
			{
				color.a = 0.25f - sanityManager.sanity / 400f;
			}
			else
			{
				color.a = 1f;
			}
		}
		else if (final.timeleft >= 250)
		{
			color.a = (450f - (float)final.timeleft) / 200f;
		}
		else
		{
			color.a = 0f;
		}
		rend.material.color = color;
		rend.material.mainTextureOffset = new Vector2(Random.value, Random.value);
	}
}
