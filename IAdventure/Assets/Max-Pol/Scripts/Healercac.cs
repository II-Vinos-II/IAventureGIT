using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healercac : IAparentHealercac
{
    //public GameObject zone;
    //private bool canHeal;

    /*[Header("Debout")]
    public float coolDown = 60f;
    public float MaxDistance = 1f;
    public float HealMax = 200f;*/



    public LayerMask Player;

    public Transform pt;
    public playerLife Pl;
    public GameObject Pd;

    public bool jesoigne;
    public float distancePL;
    private Animator anim;
    private float timerJesus = 60f;
    public bool CanRez = true;

    //attaque
    public bool Attacking = true;
    public Transform CaptnTarget;
    public Transform Target;
    public Vector3 HitBoxSize;
    public Vector3 HitBoxOffset;
    public Collider[] Hit;
    public List<GameObject> enemyHit;
    [SerializeField]
    private float speedAttack, AttackTimer, AttackSpeed;
    private int Dmg = 10;
    public bool jetape;


    //zone Competence
    public delegate void PlayerEnteredZoneEvent(GameObject player);
    public event PlayerEnteredZoneEvent OnPlayerEnteredZone;

    public float activationTime = 10.0f;
    public float cooldownTime = 40.0f;

    private bool isActive = true;
    private float timer = 0.0f;
    public float gizmoRadius = 10.0f;

    public Player_Mylane My;
    private GameObject goal;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        CanRez = true;
        Attacking = false;
        jetape = false;
        
        goal = GameObject.FindGameObjectWithTag("Goal");

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            timer += Time.deltaTime;
            if (timer >= cooldownTime)
            {
                isActive = true;
                timer = 0.0f;
            }
        }


        if (Attacking && jetape)
        {
            Attack();

        }

        Detection();

        nav.SetDestination(posTransform.position);
        //nav.SetDestination(goal.transform.position);
        nav.speed = speed;



        if (Pl != null && Pl.KO && !CanRez)
        {
            Pl = null;
        }




        for (int i = 0; i < Squadmanager.Instance.squadDeath.Count; i++)
        {


            //Pd = Squadmanager.Instance.squadDeath[i];

            if (CanRez)
            {
                posTransform.position = Squadmanager.Instance.squadDeath[i].transform.position;
                StartCoroutine(jesus(Squadmanager.Instance.squadDeath[i].GetComponent<playerLife>()));
            }




        }




        for (int i = 0; i < Squadmanager.Instance.squadLife.Length; i++)
        {
            if (Pl != null)
            {
                jetape = false;
                print("ya des gens a heal");
                break;
            }
            if (Squadmanager.Instance.squadLife[i].vie <= 70 % Squadmanager.Instance.squadLife[i].vieMax && !Squadmanager.Instance.squadLife[i].KO)
            {
                posTransform.position = Squadmanager.Instance.squadLife[i].transform.position;
                Pl = Squadmanager.Instance.squadLife[i];
                break;
            }

        }
        if (Pl != null)
        {
            distancePL = Vector3.Distance(transform.position, Pl.transform.position);
            if (distancePL > 2)
            {
                posTransform.position = Pl.transform.position;
            }
            if (Pl.vie > 80 % Pl.vieMax)
            {
                Pl = null;
            }
        }

        if (Pl == null)
        {


            jetape = true;
            print("personne a besoin de heal");
            anim.SetBool("isAttacking", false);

        }

        if (!jesoigne && Pl != null && distancePL <= 2)
        {

            anim.SetBool("isAttacking", true);
            nav.isStopped = true;
            jesoigne = true;
            StartCoroutine(soin());
        }



        if (nav.velocity.magnitude > 0f)
        {
            anim.SetBool("isWalking", true);
            //print("cours");
            //anim.SetBool("isAttacking", false);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }


        if (Pl.vie <= 0 && distancePL <= 2 && CanRez)
        {
            StartCoroutine(jesus(Pl));
        }





        if (Squadmanager.Instance.squadHeal.Count == 0)
        {
            posTransform.position = pt.position;
        }



    }


    void Attack()
    {
        if (Attacking && AttackTimer < 0)
        {

            AttackTimer = AttackSpeed;
            Hit = Physics.OverlapBox(transform.position + HitBoxOffset, HitBoxSize, transform.rotation, LayerMask.GetMask("Enemy"));
            {
                if (Hit.Length > 0)
                {
                    print("enemy touché");
                    for (int i = 0; i < Hit.Length; i++)
                    {

                        transform.LookAt(Hit[i].gameObject.transform.position);
                        if (!enemyHit.Contains(Hit[i].gameObject)) Hit[i].GetComponent<enemyLife>().takeDamage(Dmg);
                        anim.SetTrigger("Attack");
                        hp += 10;
                        if (hp >= 100)
                        {
                            hp = 100;
                        }
                        enemyHit.Add(Hit[i].gameObject);
                    }
                }
            }
        }

        else
        {
            AttackTimer -= Time.deltaTime;
            enemyHit.Clear();
        }
    }




    IEnumerator soin()
    {
        Pl.takeHeal(10);
        yield return new WaitForSeconds(0.5f);
        jesoigne = false;
        nav.isStopped = false;
    }

    IEnumerator jesus(playerLife PLife)
    {
        PLife.jesus();
        CanRez = false;
        yield return new WaitForSeconds(60f);
        CanRez = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + HitBoxOffset, HitBoxSize * 2);
       
            
            Gizmos.DrawWireSphere(transform.position, gizmoRadius);
        
    }

    void Detection()
    {
        if (jetape == true)
        {
            Squadmanager.Instance.hitColliders = Physics.OverlapSphere(transform.position, range, ennemieDetect);

            Transform tempTarget = null;

            if (Squadmanager.Instance.hitColliders.Length > 0)
            {

                float distance = Vector3.Distance(transform.position, Squadmanager.Instance.hitColliders[0].transform.position);


                for (int i = 0; i < Squadmanager.Instance.hitColliders.Length; i++)
                {
                    float tempDistance = Vector3.Distance(transform.position, Squadmanager.Instance.hitColliders[i].transform.position);



                    if (tempDistance <= distance)
                    {
                        distance = tempDistance;
                        tempDistance = 0;
                        tempTarget = Squadmanager.Instance.hitColliders[i].transform;

                        posTransform.position = Squadmanager.Instance.hitColliders[i].transform.position;
                        Attacking = true;
                    }

                }
            }


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= gizmoRadius)
            {
                OnPlayerEnteredZone?.Invoke(player);
                print("un joueur est la");
            }
        }
    }


}
