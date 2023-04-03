using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobinIA : MonoBehaviour
{
    private NavMeshAgent navMesh;
    private GameObject goal;
    private Animator anim;
    public LineRenderer lineRenderer;
    public enemyLife lifeE;
    

    float fireTimer;

    [Header("Shoot")]
    public GameObject weapon;
    public int damage;
    public Transform laserOrigin;
    public float fireRate;
    public float range;
    public float laserDuration;
    private bool canShoot;

    [Header("Spell1")]
    public Transform spawnDrone;
    public GameObject drone;

    [Header("Spell2")]
    public Transform spawnSpell;
    public GameObject shield;
    public Transform shieldTarget;
    public bool shieldIsOut;
    private float spell2Cooldown= 10f;

    public Vector3 shieldTarget2;

    RaycastHit hit;
    //public Collider[] hitColliders;
    public LayerMask ennemieDetect;


    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        goal = GameObject.FindGameObjectWithTag("Goal");
        anim = GetComponent<Animator>();
        canShoot = true;
        shieldIsOut = true;


    }

    private void Update()
    {
        navMesh.SetDestination(goal.transform.position);
        if(transform.position != goal.transform.position)
        {
            anim.SetBool("run", true);
        }
        else if(transform.position == goal.transform.position)
        {
            anim.SetBool("run", false);
        }
        
        Shoot(AttackRadius());
        for (int i=0; i < Squadmanager.Instance.squadLife.Length; i++)
        {

            if(Squadmanager.Instance.squadLife[i].vie <= 90% Squadmanager.Instance.squadLife[i].vieMax)
            {
                if (shieldIsOut)
                {
                    StartCoroutine(shieldCooldown());
                    ThrowShield();

                }
            }
        }
       


    }
    IEnumerator shieldCooldown()
    {
        yield return new WaitForSeconds(spell2Cooldown);
        ThrowShield();
    }
    protected void Shoot(Transform target)
    {
        
        lineRenderer.SetPosition(0, laserOrigin.position);
        transform.LookAt(target);
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ennemieDetect))
        {
            lifeE = hit.transform.GetComponent<enemyLife>();
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            
            
            lineRenderer.SetPosition(1, hit.point);
            anim.SetBool("shoot", true);
            anim.SetBool("run", false);
            navMesh.speed = 0;
            lineRenderer.enabled = true;

            if (canShoot)
            {
                
                lifeE.takeDamage(damage);
                StartCoroutine(shootCoolDown());
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            
            anim.SetBool("shoot", false);
            anim.SetBool("run", true);
            navMesh.speed = 5;
            lineRenderer.enabled = false;
        }

    }
   
    IEnumerator shootCoolDown()
    {
        print("aaza");
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }


    protected void Melee(Transform target)
    {
        
    }

    protected Transform AttackRadius()
    {

       
        Squadmanager.Instance.hitColliders = Physics.OverlapSphere(transform.position, range, ennemieDetect);
        

        Transform tempTarget = null;
        
        if (Squadmanager.Instance.hitColliders.Length > 0)
        {
            
            float distance = Vector3.Distance(transform.position, Squadmanager.Instance.hitColliders[0].transform.position);
           

            for (int i = 0; i< Squadmanager.Instance.hitColliders.Length;i++)
            {
                float tempDistance = Vector3.Distance(transform.position, Squadmanager.Instance.hitColliders[i].transform.position);
                


                if (tempDistance <= distance)
                {
                    distance = tempDistance;
                    tempDistance = 0;
                    tempTarget = Squadmanager.Instance.hitColliders[i].transform;
                   
                }
            
             }
        
        }
        return tempTarget;
    }

    void ThrowShield()
    {
        if (shieldIsOut)
        {

            Instantiate(shield, spawnSpell.transform);
            shieldIsOut = false;
        }
           
    }
}
