using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squadmanager : MonoBehaviour
{
    private static Squadmanager instance = null;
    public static Squadmanager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        squadLife = new playerLife[9];
        squad = new GameObject[9];
        vieMaxPote = new int[9];

        squadLife = FindObjectsOfType<playerLife>();
        for(int i = 0; i<squadLife.Length; i++)
        {
            squad[i] = squadLife[i].gameObject;
            vieMaxPote[i] = squadLife[i].vie;
        }

    }

    public Collider[] hitColliders;
    public GameObject[] squad;
    public playerLife[] squadLife;

    //public int[] viepote;
    public int[] vieMaxPote;
    public List<GameObject> squadHeal = new List<GameObject>();
    public List<GameObject> squadDeath = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        squadHeal.Clear();
        squadDeath.Clear();


        for(int i = 0; i < squadLife.Length; i++)
        {
            if(vieMaxPote[i]-squadLife[i].vie > 0)
            {
                squadHeal.Add(squad[i]);
            }
        }


        for (int i = 0; i < squadLife.Length; i++)
        {
            if (vieMaxPote[i] - squadLife[i].vie == vieMaxPote[i])
            {
                
                squadDeath.Add(squad[i]);
            }
        }

    }

    


}
