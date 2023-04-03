using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;



public class Player_Mylane : IaParent_Mylane
{

    bool canShoot,shoot,onAvance;



    //public List<Transform> ennemie  = new List<Transform>();
    //public List<GameObject> mylane_Enemy = new List<GameObject>();
    public List<Transform> ally = new List<Transform>();
    public GameObject bullet;
    public Transform bulletPoint;
    private Animator animator;

    [Header("Capacité 1 : Pouf Magie ")]
    public  bool canuseCap1;
    public float actionRange_1 = 25;
    public float cooldown_1 = 6;
    public  GameObject objectToTP;
    public  Vector3 telePortposition;



    [Header("Capacité 2 : La loi c'est moi  ")]
    public bool canuseCap2;
    public float actionRange_2 = 30;
    public float cooldown_2 = 5;
    public float duration_2 = 4;
    public float damageMultiplicator_2 = 1.25f;
    public  GameObject orderTarget;
    public  List<GameObject> allyToAplly;


    [Header("Capacité 3 : La mort vient du ciel ")]

    public bool canuseCap3;
    public float actionRange_3 = 50;
    public float cooldown_3 = 60;
    public float duration_3 = 3;
    public float attackRay_3 = 5;
    public GameObject explosion;
    public Vector3 capacityPos_3;

    public Collider[] targets;
    [SerializeField] private LayerMask ennemieMask;
    private float[] targetDistance;



    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        canuseCap1 = true;
        canuseCap2 = true;
        canuseCap3 = true;
        canShoot = true;
        StartCoroutine(checkTarget());
    }

    // Update is called once per frame
    void Update()
    {
        Move(posTransform.position);
        PrimaryFire();
        Capacity_3();
        Capacity_2();
        Capacity_1();
    }
    void DesiscionMaking()
    {
        if(targets.Length<=2)
        {
            onAvance = true;
        }
        if(onAvance)
        {
            if(canuseCap3)
            {
                Capacity_3();
            }
        }
    }

    private void MoveOrder()
    {

    }
    public void AddEnemy(Transform Enemytrans)
    {
       
    }
    void Move(Vector3 posToGo)
    {
        if(!shoot)
        {
            nav.destination = posToGo;
            animator.SetBool("Shoot", false);
            animator.SetBool("Walk", true);
        }
        else if(shoot)
        {
            nav.destination = transform.position;
            animator.SetBool("Shoot", true);
            animator.SetBool("Walk", false);
        }
        

        if (transform.position == posToGo)
        {
            isAtPos = true;
        }
        else
        {
            isAtPos = false;
        }
    }   
    private void PrimaryFire()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            shoot = true;
            transform.LookAt(ennemieToShoot.transform.position);
        }
        else
        {
            shoot = false;
        }
        if (canShoot && shoot)
        {

           
            StartCoroutine(ShootTimer(fireRate));


        }
    }
    private void Capacity_1()
    {
        if (canuseCap1 && Input.GetKeyDown(KeyCode.Space ))
        {
            canuseCap1 = false;
            objectToTP = ally[1].gameObject;
            telePortposition = new Vector3(-15,1,15);
            objectToTP.transform.position = telePortposition;
            
            StartCoroutine(Wait(cooldown_1, 1));
        }
      
    }
    private void Capacity_2()
    {
        if(canuseCap2)
        {
            canuseCap2 = false;
            StartCoroutine(Wait(cooldown_2, 2));
        }

        
    }
    private void Capacity_3()
    {
        if (canuseCap3 )
        {
            StartCoroutine(RayTimer());
            canuseCap3 = false;
            StartCoroutine(Wait(cooldown_3, 3));
        }

     
    }
    IEnumerator Wait(float timeToWait , int capacitytoCharge )
    {
        yield return new WaitForSeconds(timeToWait);
        if(capacitytoCharge == 1)
        {
            canuseCap1 = true;
        }
        else if (capacitytoCharge == 2)
        {
            canuseCap2 = true;
        }
        else if (capacitytoCharge == 3)
        {
            canuseCap3 = true;
        }
    }
    IEnumerator ShootTimer(float shootingRate)
    {
        canShoot = false;
        Instantiate(bullet, bulletPoint.position, transform.rotation);
        yield return new WaitForSeconds(shootingRate);
        canShoot = true;
    }
    IEnumerator RayTimer()
    {
        GameObject go =  Instantiate(explosion, capacityPos_3, transform.rotation) ;
        yield return new WaitForSeconds(3);
        Destroy(go);
      
    }
    IEnumerator checkTarget()
    {
        Debug.Log("uoi");
        targets = Physics.OverlapSphere(transform.position, 40,ennemieMask);
        
        if (targets.Length > 0)
        {
            targetDistance = new float[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                targetDistance[i] = Vector3.Distance(transform.position, targets[i].transform.position);
            }
            Array.Sort(targetDistance, targets);
            ennemieToShoot = targets[0].transform.gameObject;
        }
        else
        {

            ennemieToShoot = null;
            
           
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(checkTarget());
    }


}
