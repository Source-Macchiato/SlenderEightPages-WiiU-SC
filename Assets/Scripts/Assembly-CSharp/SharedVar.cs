using UnityEngine;

public class SharedVar : MonoBehaviour
{
    [Header("Shared Variables")]
    public bool daytime;
    public bool dustyair;
    public Transform endfix;
    public Transform nearpage;
    public int finaldelay;
    public bool lost;
    public int scared;
    public int pages;
    public int level;
    public bool mh;
    public bool caught;
    public bool flraised = true;
    public float fadeinmusic = 2f;
    public int toolong = 12000;
    public float maxrange = 100f;
    public float minrange = 80f;
    public GameObject SM;

    [Header("Audio")]
    public AudioSource music1;
    public AudioSource music2;
    public AudioSource music3;
    public AudioSource music4;

}