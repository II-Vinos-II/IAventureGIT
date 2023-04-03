using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShieldController : MonoBehaviour
{
    public int countOfbound;
    public int boundMax = 5;
    public float speed= 12;

    public List<Transform> allyHistorique = new List<Transform>();
    public float shieldGive = 20;
    public int bounceRange = 4;
    public LayerMask allyLayer;
    public playerLife allyLife;
    public Transform center;
    Transform target;
    public RobinIA me;

    Rigidbody rb;
    private void Start()
    {
        me = FindObjectOfType<RobinIA>();
       
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < Squadmanager.Instance.squadLife.Length; i++)
        {

            if (Squadmanager.Instance.squadLife[i].vie <= 90 % Squadmanager.Instance.squadLife[i].vieMax)
            {

                target = Squadmanager.Instance.squadLife[i].transform;
                allyHistorique.Add(target);

            }
        }
    }

    private void Update()
    {


        

        if(target != null)
        {
            rb.velocity = (target.position + Vector3.up - transform.position).normalized * speed;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print(other.name);
            allyLife = other.GetComponent<playerLife>();

            allyLife.vieBonus += 20;
            if (countOfbound < boundMax)
            {

                BounceShield();
            }
            if(countOfbound > boundMax)
            {
                Destroy(this.gameObject);
                me.shieldIsOut = true;
            }
        }
    }

    


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            allyLife = null;
        }
    }

    void BounceShield()
    {
        countOfbound++;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, bounceRange, allyLayer);
       

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].transform != target.transform && !allyHistorique.Contains(hitColliders[i].transform))
            {
                
                target = hitColliders[i].transform;
                allyHistorique.Add(target);

                break;
            }
            

        }
        
    }


}
