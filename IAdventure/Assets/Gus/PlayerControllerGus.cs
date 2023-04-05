using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using static UnityEngine.GraphicsBuffer;

public class PlayerControllerGus : MonoBehaviour
{
    [Header("Controller")]
    public float playerSpeed = 5;
    public float playerLife = 100;
    public Transform akimbo1;
    public Transform akimbo2;

    public float distance = 30;
    public float degats = 8;
    public float cooldownShoot = .5f;
    public float coneVisée = 30;
    public bool reload;
    public GameObject bulletPrefab;

    public Drone drone;
    public DroneWTF droneWTF;
    public AuraRevigorante auraRevigorante;



    [Header("Capacité 1 : Drone soin qui roule")]
    public Transform droneSpawnPoint;
    public GameObject droneObj;
    [SerializeField] private GameObject droneSave;
    [SerializeField] private bool coolDownDone;
    [SerializeField] private Transform lookDroneTarget;
    [SerializeField] private Transform healingTarget;
    [SerializeField] private Transform targetPotes;
    [SerializeField] private NavMeshAgent navMeshAgentDrone;




    private NavMeshAgent agent;
    private Transform ennemi;
    public Collider[] targets; 
    private float[] targetDistance;

    private bool combat;
    private Transform targetPlayer;
    [SerializeField] private float distanceAggro = 5f;
    [SerializeField] private LayerMask EnemyMask;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Transform aimTarget2;
    [SerializeField] private Transform canon1;
    [SerializeField] private Transform canon2;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Rig aimRig2;

    //[SerializeField] private Transform canon;
    [SerializeField] private float cooldown;
    [SerializeField] private float projectilSpeed = 15f;
    [SerializeField] private GameObject laser;
    private GameObject laserSave;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        searchEnnemi();
        StartCoroutine(checkTarget());
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPotes != null)
        {
            lookDroneTarget.position = targetPotes.position;
        }
        for (int i = 0; i < Squadmanager.Instance.squadLife.Length; i++)
        {

            if (Squadmanager.Instance.squadLife[i].vie <= 90 % Squadmanager.Instance.squadLife[i].vieMax)
            {
                healingTarget = Squadmanager.Instance.squadLife[i].transform;
                lookDroneTarget = Squadmanager.Instance.squadLife[i].transform;
                navMeshAgentDrone.SetDestination(healingTarget.position);
            }
        }
        if (!coolDownDone)
        {
            coolDownDone = true;
            StartCoroutine(SpawnDroneCoroutine());
        }








        if (combat && targetPlayer != null)
        {
            lookTarget.position = targetPlayer.position;
            aimTarget.position = Vector3.MoveTowards(aimTarget.position, targetPlayer.position, Time.deltaTime * 20);
            aimTarget2.position = Vector3.MoveTowards(aimTarget2.position, targetPlayer.position, Time.deltaTime * 20);
            if (aimRig.weight != 1)
            {
                aimRig.weight = Mathf.MoveTowards(aimRig.weight, 1, Time.deltaTime * 10);
            }
            if (aimRig2.weight != 1)
            {
                aimRig2.weight = Mathf.MoveTowards(aimRig.weight, 1, Time.deltaTime * 10);
            }
        }
       
        if (combat && !reload)
        {
            reload = true;
            if (targetPlayer != null)
            {
                StartCoroutine(AkimboShoot());
            }
        }
    }

    public void Akimbo()
    {

    }


    void searchEnnemi()
    {
        ennemi = GameObject.FindWithTag("Enemy").transform;
        agent.SetDestination(ennemi.position);
    }
    public void RobotInvoke()
    {

    }
    IEnumerator checkTarget()
    {
        targets = Physics.OverlapSphere(transform.position, distanceAggro, EnemyMask);
        if (targets.Length > 0)
        {
            targetDistance = new float[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                targetDistance[i] = Vector3.Distance(transform.position, targets[i].transform.position);
            }
            Array.Sort(targetDistance, targets);

            combat = true;
            targetPlayer = targets[0].transform;
            agent.isStopped = true;
        }
        else
        {
            combat = false;
            targetPlayer = null;
            agent.isStopped = false;
            searchEnnemi();
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(checkTarget());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceAggro);
    }

    public IEnumerator AkimboShoot()
    {
        laserSave = Instantiate(bulletPrefab, akimbo1.position, Quaternion.identity);
        Destroy(laserSave, 2f);
        rb = laserSave.GetComponent<Rigidbody>();
        rb.velocity = (targets[0].transform.position - canon1.position).normalized * projectilSpeed;
        yield return new WaitForSeconds(.5F);
        laserSave = Instantiate(bulletPrefab, akimbo2.position, Quaternion.identity);
        Destroy(laserSave, 2f);
        rb = laserSave.GetComponent<Rigidbody>();
        rb.velocity = (targets[0].transform.position - canon2.position).normalized * projectilSpeed;
        yield return new WaitForSeconds(.5f);
        reload = false;
    }
    public IEnumerator SpawnDroneCoroutine()
    {
        droneSave = Instantiate(droneObj, droneSpawnPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(3);
        coolDownDone = false;
    }
}