using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Drone : MonoBehaviour
{
    public int soinGiven1 = 30;
    public NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject droneSave;
    [SerializeField] private Transform healingTarget;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < Squadmanager.Instance.squadLife.Length; i++)
            {
                Squadmanager.Instance.squadLife[i].takeHeal(soinGiven1);   

            }
            Destroy(droneSave);
        }
    }

    public void searchPote()
    {
        navMeshAgent.SetDestination(healingTarget.position);
    }
}
