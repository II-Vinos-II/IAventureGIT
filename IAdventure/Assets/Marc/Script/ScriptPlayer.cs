using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class ScriptPlayer : MonoBehaviour
{
    public bool IsAlive;
    public bool isAtPosition;
    public Collider[] EnnemyArray;
    public Transform objectif;
    [SerializeField] private Transform ClosedEnemy;
    public float[] Hauteur;

    public LayerMask monlayer;

    private RaycastHit raycast;
    [SerializeField] private GameObject balle;
    [SerializeField] private GameObject Canon;
    [SerializeField] private bool SpawnBalle = true;
    public Transform targetSHooter = null;

    private float distanceArray;
    [Header("Stats principales")]

    public float speed = 7;
    public float hp = 150;
    public float baseArmor = 10;

    [SerializeField] private NavMeshAgent Agent;
    
    
    [Header("Stats d'arme")]

    public float fireRate = 0.5f;
    public float damagePerHit = 2;
    public float distance = 500;
    private GameObject ennemieToShoot;

    [Header("Spawn des tourelles")]

    [SerializeField] private GameObject tourelle;
    [SerializeField] private float Cooldown_Tourelle = 20f;

    //[SerializeField] private GameObject Drone_Soldats;
    [SerializeField] private float Cooldown_Drone_Soldats = 30f;
    [SerializeField] private float During_Drone_Soldat = 5f;
    [SerializeField] private GameObject DroneBase;

    [SerializeField] private GameObject Drone_Kamikaze;
    [SerializeField] private float Cooldown_Drone_Kamikaze = 30f;
    [SerializeField] private GameObject KamikazeGO;
    private bool Kamikazobool = true;
    private bool dronebool = true;
    private bool tourellebool = true; 

    // Start is called before the first frame update
    void Start()
    {
        IsAlive = true;
        Agent = GetComponent<NavMeshAgent>();
        ClosedEnemy = null;
        
    }

    // Update is called once per frame
    void Update()
    {
        //objectif = GameObject.Find("Objectif").GetComponent<Transform>();

        //takeDamage();
        if (IsAlive)
        {
            ClosedEnemy = GetClosedEnnemy();
            Move(new Vector3(ClosedEnemy.position.x, Hauteur[0], ClosedEnemy.position.z));
            ShootDamage();

            float time2 = 0;
            //Debug.Log(time2);
            if (!DroneBase)
            {
                time2 += Time.deltaTime;
                
            }
            Debug.Log(time2);

            if (time2 >= Cooldown_Drone_Soldats)
            {
                DroneBase.SetActive(true);
                time2 = 0;
            }
            if(Kamikazobool)
            {
                StartCoroutine(Kamikaze());
            }
            
            if(dronebool)
            {
                StartCoroutine(SpawnDrone());
            }

            if(tourellebool)
            {
                StartCoroutine(SpawnTourelle());
            }
            //StartCoroutine(SpawnDrone());
            
        }
        Debug.Log(SpawnBalle);

    }

    public void ShootDamage()
    {
        if (SpawnBalle)
        {
            StartCoroutine(Tir());
        }
    }
    public Transform GetClosedEnnemy()
    {
        EnnemyArray = Physics.OverlapSphere(transform.position, 30f,monlayer);
        distanceArray = 30;
        
        //Agent.stoppingDistance = 20;

        foreach (Collider go in EnnemyArray)
        {
            if (Vector3.Distance(transform.position, go.transform.position)< distanceArray)
            {
                distanceArray = Vector3.Distance(transform.position, go.transform.position);
                targetSHooter = go.transform;
            }
        }

        if (targetSHooter != null)
        {
            return targetSHooter;
        }
        else
        {
            return objectif;
        }
    }

    private void Move(Vector3 GoToPosition)
    {
        Agent.SetDestination(GoToPosition);
        //Agent.stoppingDistance = 0;

        if (transform.position == GoToPosition)
        {
            isAtPosition = true;
            Agent.speed = 0;
        }
        else
        {
            isAtPosition = false;
            Agent.speed = speed;
        }
    }

    private IEnumerator Tir()
    {
        SpawnBalle = false;
        GameObject laser = Instantiate(balle, Canon.transform.position, Canon.transform.rotation) as GameObject;
        laser.GetComponent<Bullet_Movement>().direction = ((targetSHooter.position + Vector3.up) - Canon.transform.position).normalized;
        yield return new WaitForSeconds(fireRate);
        //laser.transform.position = Canon.transform.position;
        SpawnBalle = true;
    }

    private IEnumerator Kamikaze()
    {
        Debug.Log("Spawn");
        Kamikazobool = false;
        GameObject Drone = Instantiate(KamikazeGO) as GameObject;
        //Drone.transform.position = transform.position + new Vector3(0, 0, - 3);
        //Kamikazobool = true;
        yield return new WaitForSeconds(Cooldown_Drone_Kamikaze);
        Drone.transform.position = transform.position + new Vector3(0, 0, -3);
        //GameObject Drone = Instantiate(KamikazeGO) as GameObject;
        //Drone.transform.position = transform.position + new Vector3(0, 0, 3);
        Kamikazobool = true;
    }

    private IEnumerator SpawnDrone()
    {
        dronebool = false;
        yield return new WaitForSeconds(Cooldown_Drone_Soldats);
        DroneBase.SetActive(true);
        yield return new WaitForSeconds(During_Drone_Soldat);
        DroneBase.SetActive(false);
        dronebool = true;
        
    }

    private IEnumerator SpawnTourelle()
    {
        tourellebool = false;
        yield return new WaitForSeconds(Cooldown_Tourelle);
        GameObject tesla = Instantiate(tourelle);
        tesla.transform.position = transform.position + new Vector3(0, 1, -2);
        yield return new WaitForSeconds(10f);
        Destroy(tesla);
        tourellebool = true;
    }
    
    public void OnDrawGizmos()
    {
        //Gizmos.DrawLine(Canon.transform.position, Vector3.forward + new Vector3(0, 0, 20));
        Gizmos.DrawLine(Canon.transform.localPosition, transform.forward);
    }
}
