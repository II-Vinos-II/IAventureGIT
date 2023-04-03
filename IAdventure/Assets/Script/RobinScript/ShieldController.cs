using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public int countOfbound;
    public int boundMax = 5;

    public float shieldGive = 20;
    public int bounceRange = 4;
    public LayerMask allyLayer;
    public playerLife allyLife;
    public Transform center;

    private void OnTriggerEnter(Collider other)
    {       
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            
            allyLife = other.GetComponent<playerLife>();
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
