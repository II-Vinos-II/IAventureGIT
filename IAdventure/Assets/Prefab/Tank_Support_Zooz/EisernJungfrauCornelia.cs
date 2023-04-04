using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EisernJungfrauCornelia : IaParent_Enzo
{
    #region(vars)
    [Header("Cooooneliaaaa")]
    [SerializeField] private Animator anims;
    [HideInInspector] public bool isAnimLocked;
    [SerializeField] private float updatePathInterval;
    private Player_Mylane scriptDeMylane;
    private float updatePathCooldown;
    private playerLife lifeManager;

    [Header("PrimaryFire")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunCannon;
    private float fireCooldown;
    [SerializeField] private float Cone = 45;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int numberOfCasts = 10;
    [HideInInspector] public Transform attackTarget;


    [Header("Capacité 1 : Bouclier énergetique")]
    [SerializeField] private float bonusHp;
    [SerializeField] private float actionRange_1 = 20;
    [SerializeField] private float cooldown_1 = 10;
    private float reloadC1;


    [Header("Capacité 2 : Blink Taunt")]
    [SerializeField] private float blinkRange_2 = 20;
    [SerializeField] private float actionRange_2 = 5;
    [SerializeField] private float cooldown_2 = 5;
    private float reloadC2;
    [SerializeField] private float durationTaunt_2 = 3;


    [Header("Capacité 3 : Renvoi")]
    [SerializeField] private Transform shield;
    [SerializeField] private float actionRange_3 = 2;
    [SerializeField] private float cooldown_3 = 1;
    private float reloadC3;
    //[SerializeField] private float duration_3 = 0.3f;
    [SerializeField] private float weakness_3 = 50;
    //[SerializeField] private float weaknessTime_3 = 0.7f;
    private bool inCap3;
    private List<Rigidbody> alreadyReversed;
    #endregion

    #region(mainfunct)
    private void Start()
    {
        lifeManager = GetComponent<playerLife>();
        lifeManager.vie = ((int)hp);
        nav = GetComponent<NavMeshAgent>();
        nav.speed = speed;
        rgbd = GetComponent<Rigidbody>();
        scriptDeMylane = FindObjectOfType<Player_Mylane>();
    }

    private void Update()
    {

        
        if (!isAnimLocked)
        {
            ChoseAction();
            anims.SetFloat("Speed", nav.velocity.magnitude); 
        } else
        {
            nav.SetDestination(transform.position);
            if (inCap3)
            {
                Capacite3();
            }
        }
        Cooldowns();
    }
    #endregion


    #region(secondaryFunctions)

    private void ChoseAction()
    {
        List<GameObject> potentialTargets = FindTargetsInRange(ennemiDetectRange, 0);
        if (cooldown_3 <= 0)
        {
            if (FindTargetsInRange(actionRange_3 * 1.2f, 2).Count > 0)
            {
                anims.SetTrigger("Reflect");
                isAnimLocked = true;
                return;
            }
        }
        if (cooldown_1 <= 0)
        {
            if (FindTargetsInRange(actionRange_1, 1).Count > 0)
            {
                anims.SetTrigger("Protec");
                isAnimLocked = true;
                return;
            }
        }
        if (cooldown_2 <= 0 && potentialTargets.Count > 0)
        {
            Capacite2(potentialTargets[GetClosestTarget(potentialTargets)].transform);
            anims.SetTrigger("Taunt");
            isAnimLocked = true;
            return;
        }
        if (potentialTargets.Count > 0 && fireCooldown <= 0)
        {
            List<GameObject> shootTargets = FindTargetsInRange(range, 0);
            if (shootTargets.Count > 0)
            {
                isAnimLocked = true;
                attackTarget = shootTargets[GetClosestTarget(shootTargets)].transform;
                anims.SetTrigger("Shoot");
                return;
            }
        }
        
        if (updatePathCooldown <= 0)
        {
            if (potentialTargets.Count > 0)
            {
                nav.SetDestination(potentialTargets[GetClosestTarget(potentialTargets)].transform.position);
                anims.SetFloat("Speed", nav.velocity.magnitude);
                updatePathCooldown = updatePathInterval;
            }
            else if (scriptDeMylane.onAvance)
            {
                Vector3 dir = transform.position - GameObject.FindGameObjectWithTag("Goal").transform.position;
                nav.destination = GameObject.FindGameObjectWithTag("Goal").transform.position - dir.normalized * 3;
                updatePathCooldown = updatePathInterval;
            } else 
            {
                nav.destination = transform.position;
            }
        }
    }

    
    private List<GameObject> FindTargetsInRange(float Range, int Target)
    {
        List<GameObject> toReturn = new();
        Collider[] inRange = Physics.OverlapSphere(transform.position, Range) ;
        for (int i = 0; i < inRange.Length; i++)
        {
            switch (Target)
            {
                case 0:
                    if (inRange[i].TryGetComponent<robotBig>(out _))
                    {
                        toReturn.Add(inRange[i].transform.gameObject);
                    }
                    break;
                case 1:
                    if (inRange[i].transform.CompareTag("AllyTag") && inRange[i].transform != transform)
                    {
                        toReturn.Add(inRange[i].transform.gameObject);
                    }
                    break;
                case 2:
                    if (inRange[i].TryGetComponent<robotLaser>(out _))
                    {
                        toReturn.Add(inRange[i].transform.gameObject);
                    }
                    break;
                default:
                    break;
            }
        }
        return toReturn;
    }

    private int GetClosestTarget(List<GameObject> targetsInRange)
    {
        int closest = 0;
        for (int i = 0; i < targetsInRange.Count; i++)
        {
            if (Vector3.Distance(targetsInRange[i].transform.position, transform.position) <= Vector3.Distance(targetsInRange[closest].transform.position, transform.position))
            {
                closest = i;
            }
        }
        return closest;
    }

    #endregion


    #region(Capacités)
    public void Fire()
    {
        
        List<Transform> targets = new();
        for (int i = 0; i < numberOfCasts; i++)
        {
            Vector3 Dir = new Vector3(transform.forward.x * Mathf.Cos(-Cone / 2 + i * (Cone / numberOfCasts)) - transform.forward.z * Mathf.Sin(-Cone / 2 + i * (Cone / numberOfCasts)),
                            transform.forward.y,
                            transform.forward.x * Mathf.Sin(-Cone / 2 + i * (Cone / numberOfCasts)) + transform.forward.z * Mathf.Cos(-Cone / 2 + i * (Cone / numberOfCasts)));
            
            if (Physics.Raycast(transform.position, Dir, out RaycastHit hit,range, ennemieDetect))
            {
                GameObject newBullet = Instantiate(bullet, gunCannon.position, transform.rotation, null);
                newBullet.GetComponent<Rigidbody>().velocity = Dir * bulletSpeed;
                print(hit.transform.name);
                if (hit.transform.TryGetComponent<robotBig>(out _))
                {
                    bool doExist = false;
                    for (int y = 0; y < targets.Count; y++)
                    {
                        if (hit.transform == targets[y])
                        {
                            doExist = true;
                        }
                    }
                    if (!doExist)
                    {
                        targets.Add(hit.transform);
                    }
                }
            }
        }
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i] != null)
            {
                targets[i].GetComponent<enemyLife>().takeDamage((int)damagePerHit);
            }
        }
        fireCooldown = fireRate;
    }

    public void Capacite1()
    {
        print("Shield");
        List<GameObject> targets = FindTargetsInRange(actionRange_1, 1);
        for (int i = 0; i < targets.Count; i++)
        {
            //addshieldToTarget
        }

        reloadC1 = cooldown_1;
    }

    private void Capacite2(Transform target)
    {
        print("blink");
        if (Vector3.Distance(transform.position, target.position) <= blinkRange_2)
        {
            transform.position = target.position - (transform.position - target.position).normalized;
            
        } else if (Vector3.Distance(transform.position, target.position) > blinkRange_2)
        {
            transform.position += (target.position - transform.position).normalized * blinkRange_2;
        }
        List<GameObject> targetsObject = FindTargetsInRange(actionRange_2, 0);
        for (int i = 0; i < targetsObject.Count; i++)
        {
            if (targetsObject[i].TryGetComponent<robotBig>(out robotBig script))
            {
                print("TauntIG");
                //script.targetPlayer = transform;
            }
        }
    }

    public void Capacite3()
    {
        List<GameObject> bulletInRange = FindTargetsInRange(actionRange_3, 2);

        for (int i = 0; i < bulletInRange.Count; i++)
        {
            if (bulletInRange[i].TryGetComponent<Rigidbody >(out Rigidbody target))
            {
                bool isHitted = false;
                foreach(Rigidbody alreadyHit in alreadyReversed)
                {
                    if (alreadyHit == target)
                    {
                        isHitted = true;
                    }
                }
                if (!isHitted)
                {
                    target.velocity = -target.velocity;
                    alreadyReversed.Add(target);
                }
            }
        }
    }

    public void StopCap3()
    {
        inCap3 = false;
        alreadyReversed.Clear();
    }

    private void Cooldowns()
    {
        if (updatePathCooldown > 0)
        {
            updatePathCooldown -= Time.deltaTime;
        }
        if (fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }
        if (reloadC1 > 0)
        {
            reloadC1 -= Time.deltaTime;
        }
        if (reloadC2 > 0)
        {
            reloadC2 -= Time.deltaTime;
        }
        if (reloadC3 > 0)
        {
            reloadC3 -= Time.deltaTime;
        }
    }

    

    public void EndVulnerability()
    {
        
    }
    #endregion
}