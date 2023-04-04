using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;



public class Player_Mylane : IaParent_Mylane
{

    bool canShoot,shoot,inDanger,canability;
    public bool WasIncombat, onAvance;


    private  Bounds bounds;
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
    public GameObject tp_particle;
    public  Transform telePortposition;



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
    public GameObject InsaneBot;



    // Start is called before the first frame update
    void Start()
    {
        posTransform = GameObject.FindGameObjectWithTag("Goal").transform;
        canability = true;
        canShoot = true;
        shoot = true;
        onAvance = true;
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
        DesiscionMaking();
       
        SpecialEnnemylist();
      /*PrimaryFire();
        Capacity_3();
        Capacity_2();
        Capacity_1();*/
    }
    void DesiscionMaking()
    {
        if(targets.Length<=0)
        {
            if(WasIncombat && !onAvance)
            {
                StartCoroutine(ReloadCooldown());
                
            }
            else if(onAvance)
            {

                Move(posTransform.position);
                Debug.Log("EnterForwardMode");
            }
           
        }
        else
        {
            onAvance = false;
            WasIncombat = true;
        }
        if(!onAvance)
        {
            if(canability)
            {
                if (targets.Length <= 3)
                {
                    //Debug.Log("EnterAttaclMode");
                    if (canuseCap1)
                    {

                        Capacity_1(ennemieToShoot);
                        Debug.Log("bbbbbbbbb");

                    }
                    else if (!canuseCap1)
                    {
                        PrimaryFire();
                    }

                }
                else if (targets.Length > 3)
                {
                    if (canuseCap3)
                    {



                        Capacity_3();


                    }
                    else if (canuseCap2 && canuseCap1 /*&& InsaneBot != null*/ )
                    {
                        Debug.Log("aaaaaaa");
                        Combo_1_2(ennemieToShoot);
                    }
                    else if (canuseCap1)
                    {
                        Capacity_1(ennemieToShoot);
                    }
                    else
                    {
                        PrimaryFire();
                    }
                }
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
        Vector3 dir = this.transform.position - posTransform.position;
        if(onAvance)
        {
            nav.destination = posToGo + dir.normalized*3;
            animator.SetBool("Shoot", false);
            animator.SetBool("Walk", true);
        }
        else if(!onAvance)
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

        
        
        transform.LookAt(ennemieToShoot.transform.position);
        animator.SetBool("Shoot", true);
        animator.SetBool("Walk", false);

        if (canShoot && shoot)
        {

           
            StartCoroutine(ShootTimer(fireRate));


        }
    }
    private void Capacity_1(GameObject objectToapply)
    {
        if (canuseCap1)
        {
            objectToTP = objectToapply;
            if(InsaneBot != null)
            {
                objectToTP = InsaneBot;
            }
            /*else if(objectToapply == null)
            {
                objectToTP = ennemieToShoot;
            }*/

            canuseCap1 = false;
            if(targets.Length<=3)
            {
                objectToTP.transform.position = telePortposition.position;
                Instantiate(tp_particle, objectToTP.transform.position, transform.rotation);
            }
            else if (targets.Length > 3)
            {
                objectToTP.transform.position = telePortposition.position; // un spawn dvant un tank pour que le robot prenne les balles
                Instantiate(tp_particle, objectToTP.transform.position, transform.rotation);
            }
            
            if(canuseCap2)
            {
                Capacity_2(objectToapply);
            }
            StartCoroutine(Wait(cooldown_1, 1));
        }
      
    }
    private void Capacity_2(GameObject ennemieAimed )
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
            StartCoroutine( CoolDown());
        }

     
    }
    Vector3 GetCenterPoint()
    {

        bounds = new Bounds(targets[0].transform.position, Vector3.zero);
        for (int i = 0; i < targets.Length/2; i++)
        {
            bounds.Encapsulate(targets[i].transform.position);
        }

        return bounds.center;
    }
    private void Combo_1_2(GameObject ennemietoAim)
    {
        Capacity_2(ennemieToShoot);
        Capacity_1(ennemietoAim);
    }
    private void Combo_1_2_3(GameObject ennemietoAim)
    {
        Capacity_2(ennemieToShoot);
        Capacity_1(ennemietoAim);
        capacityPos_3 = InsaneBot.transform.position;
        Capacity_3();

    }
    private void SpecialEnnemylist()
    {
        if(InsaneBot == null)
            for(int i = 0; i<targets.Length;i++)
            {
                if(targets[i].gameObject.tag == "Elite")
                {
                    InsaneBot = targets[i].gameObject;
                    Debug.Log("aaa");
                }
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
        capacityPos_3 = GetCenterPoint();
        GameObject go =  Instantiate(explosion, capacityPos_3, transform.rotation) ;
        yield return new WaitForSeconds(3);
        Destroy(go);
      
    }
    IEnumerator checkTarget()
    {
        //Debug.Log("uoi");
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

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(checkTarget());
    }
    IEnumerator CoolDown()
    {
        canability = false; 
        yield return new WaitForSeconds(0.5f);
        canability = true; 
    }
    IEnumerator ReloadCooldown()
    {
        WasIncombat = false;
        yield return new WaitForSeconds(30f);
        onAvance = true;
    }
    


}
