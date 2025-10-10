using UnityEngine;

public class SpawnPages : MonoBehaviour
{
	public Transform page1;
	public Transform page2;
	public Transform page3;
	public Transform page4;
	public Transform page5;
	public Transform page6;
	public Transform page7;
	public Transform page8;
	public int whichpage;

	private GameObject[] finishSpawns;

	private void Start()
	{
		finishSpawns = GameObject.FindGameObjectsWithTag("Finish");
		SpawnAllPages();
	}

	// (can you believe it ? this shit was on an update and called a FindGameObjectWithTag at EVERY FUCKING FRAMES WTF -- this was also 150 lines long...uhh...)
	private void SpawnAllPages()
	{
		Transform[] pages = { page1, page2, page3, page4, page5, page6, page7, page8 };

		for (int i = 0; i < pages.Length; i++)
		{
			FindSpawn(pages[i]);
		}

		// Cleanup remaining spawns
		for (int i = 0; i < finishSpawns.Length; i++)
		{
			if (finishSpawns[i] != null)
            {
                Destroy(finishSpawns[i]);
            }
			
		}
		finishSpawns = null;
	}

	private void FindSpawn(Transform target)
	{
		if (finishSpawns == null || finishSpawns.Length == 0)
		{
			Debug.LogWarning("PAGEFAIL");
			return;
		}

		int count = finishSpawns.Length;
		for (int i = 0; i < finishSpawns.Length; i++)
		{
			if (finishSpawns[i] == null) continue;

			if (Random.value <= 1f / count)
			{
				Transform spawn = finishSpawns[i].transform;
				target.position = spawn.position;
				target.rotation = spawn.rotation;

				for (int j = finishSpawns.Length - 1; j >= 0; j--)
				{
					if (finishSpawns[j] != null && 
					    Vector3.Distance(target.position, finishSpawns[j].transform.position) <= 35f)
					{
						Destroy(finishSpawns[j]);
						finishSpawns[j] = null;
					}
				}
				return;
			}
			count--;
		}

		Debug.LogWarning("PAGEFAIL");
	}
}