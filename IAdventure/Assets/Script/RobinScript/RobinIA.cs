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



    RaycastHit hit;
    //public Collider[] hitColliders;
    public LayerMask ennemieDetect;


    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        goal = GameObject.FindGameObjectWithTag("Goal");
        anim = GetComponent<Animator>();
        canShoot = true;
        
    }

    private void Update()
    {
        navMesh.SetDestination(goal.transform.position);
        
        Shoot(AttackRadius());
        
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
            navMesh.speed = 0;
            lineRenderer.enabled = true;

            if (canShoot)
            {
                print("azzaeaeaeeaea");
                lifeE.takeDamage(damage);
                StartCoroutine(shootCoolDown());
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            
            anim.SetBool("shoot", false);
            navMesh.speed = 5;
            lineRenderer.enabled = false;
        }

    }
   
    IEnumerator shootCoolDown()
    {
        print("aaza");
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
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
        Instantiate(shield, spawnSpell.transform);
            if(shieldTarget == null)
            {
                Destroy(shield);
            }
            else
            {

                shield.transform.position = shieldTarget.transform.position.normalized;
            }
    }
}
