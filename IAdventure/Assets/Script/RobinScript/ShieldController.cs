using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShieldController : MonoBehaviour
{
    public int countOfbound;
    public int boundMax = 5;
    public NavMeshAgent navMesh;

    public float shieldGive = 20;
    public int bounceRange = 4;
    public LayerMask allyLayer;
    public playerLife allyLife;
    public Transform center;
    public RobinIA me;
    private void Start()
    {
        me = FindObjectOfType<RobinIA>();
        navMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        for (int i = 0; i < Squadmanager.Instance.squadLife.Length; i++)
        {

            if (Squadmanager.Instance.squadLife[i].vie <= 90 % Squadmanager.Instance.squadLife[i].vieMax)
            {

                
                navMesh.SetDestination(Squadmanager.Instance.squadLife[i].transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {       
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
            allyLife = other.GetComponent<playerLife>();
            allyLife.armor += 20;
            BounceShield(transform.position, bounceRange);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            allyLife = null;
        }
    }

    void BounceShield(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].SendMessage("AddDamage");
            i++;
        }
    }


}
