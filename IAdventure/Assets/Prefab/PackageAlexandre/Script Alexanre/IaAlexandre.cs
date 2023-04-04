using System.Collections;
using Unity.VisualScripting;
using UnityEngine;  
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[RequireComponent(typeof(NavMesh))]
public class IaAlexandre : playerLife
{
    #region(variabes)
    [Header("Alexandre\n")]
    [SerializeField] GameObject me;
    [SerializeField] GameObject weapon, fireEffect;
    [SerializeField] Animator myAnim;

    [Header("General\n")]
    [SerializeField] NavMeshAgent nav;
    [SerializeField] Transform Objectif;
    [SerializeField] float speed, baseArmor;
    
    [Header("Attacks basic value")]
    float baseFireRate;
    float baseCoolDownCap1, baseCoolDownCap2, baseCoolDownCap3;

    [Header ("Attacks value\n")]
    [SerializeField] float fireRate;
    [SerializeField, Range(0, 40)] float damagePerHit, dotTime, autoSkillLauncher,  coolDownCap1, coolDownCap2, coolDownCap3;
    [SerializeField] bool canUseBasicAtk, canUseCapacity1, canUseCapacity2, canUseCapacity3;

    [Header("FindClosestEnnemy\n")]
    [SerializeField] Collider[] ennemyArray;
    [SerializeField] Transform closestEnemy;

    [Header("HitBoxSword\n")]
    [SerializeField] float RadiusWeapon;
    [SerializeField] float rangeRadius;
    [SerializeField] GameObject handleOfWeapon, weaponTip;
    [SerializeField] LayerMask iWant;
    [SerializeField] Collider[] TouchByWeapon, InRange;
    #endregion

    void Start()
    {
        #region(searchComponent)
        nav = GetComponent<NavMeshAgent>();
        myAnim = GetComponent<Animator>();
        #endregion

        #region(set de variable)
        closestEnemy = null;
        rangeRadius = 2;
        baseFireRate = fireRate;
        baseCoolDownCap1 = coolDownCap1;
        baseCoolDownCap2 = coolDownCap2;
        baseCoolDownCap3 = coolDownCap3;
        #endregion
    }

    void Update()
    {
        Objectif = GameObject.Find("OBJECTIF").GetComponent<Transform>();

        if (!KO)
        {
            #region(décrémentation cooldown)
            if (fireRate > 0) fireRate -= Time.deltaTime;
            if (coolDownCap1 > 0) coolDownCap1 -= Time.deltaTime;
            if (coolDownCap2 > 0) coolDownCap2 -= Time.deltaTime;
            if (coolDownCap3 > 0) coolDownCap3 -= Time.deltaTime;
        #endregion

        if (canUseCapacity1)
        {
                autoSkillLauncher += Time.deltaTime;
                Capacity1();
            }
        else
            {
                closestEnemy = GetClosestEnnemy();

                Move(closestEnemy.position);
            }
            AttackGestion();
        }
        else
        {
            Move(transform.position);
        }
    }

    #region(fonctions)
    #region(déplacement)
    public Transform GetClosestEnnemy()
    {
        float closestDistance = 30;
        Transform trans = null;

        ennemyArray = Physics.OverlapSphere(transform.position, closestDistance, iWant);

        foreach (Collider go in ennemyArray)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }

        if (trans != null)
        {
            return trans;
        }
        else
        {
            return Objectif;
        }
    }

    private void Move(Vector3 posToGo)
    {
        nav.SetDestination(posToGo);

        if (transform.position == posToGo)
        {
            myAnim.SetBool("moving?", false);
            nav.speed = 0;
        }
        else
        {
            myAnim.SetBool("moving?", true);
            nav.speed = speed;
        }
    }
    #endregion

    #region(Atk)

    #region(1fonction bcp trop longue qui fait quasi toute la gestion du launch de capacité)
    void AttackGestion()
    {
        if (fireRate <= 1) myAnim.SetBool("canSlash", false);
        if (fireRate <= 0) canUseBasicAtk = true;
        if (coolDownCap1 <= 0) canUseCapacity1 = true;
        if (coolDownCap2 <= 0) canUseCapacity2 = true;
        if (coolDownCap3 <= 0) canUseCapacity3 = true;

        InRange = Physics.OverlapSphere(transform.position, rangeRadius, iWant);

        for (int i = 0; i < InRange.Length; i++)
        {
            if (InRange[i].gameObject.CompareTag("Enemy"))
            {

                if (canUseBasicAtk)
                {
                    BasicAttack();
                    canUseBasicAtk = false;
                }

                else if (canUseCapacity2)
                {
                    Capacity2();
                    StartCoroutine(DotFlamme(4));
                }

                else if (canUseCapacity3) Capacity3();
            }
        }

        if (!canUseCapacity2) rangeRadius = 2;
    }
    #endregion

    #region(capacity)
    void BasicAttack()
    {
        fireRate = baseFireRate;
        myAnim.SetBool("canSlash", true);
        canUseBasicAtk = false;
    }

    void Capacity1()
    {
        if (autoSkillLauncher < 5)
        {
            rangeRadius = 3;
            Move(transform.position);
            rangeRadius = 3;
            speed = 0;
        }
        else if (autoSkillLauncher >= 5)
        {
            rangeRadius = 2;
            speed = 12;
            Move(Objectif.position);
            autoSkillLauncher = 0;
            coolDownCap1 = baseCoolDownCap1;
            canUseCapacity1 = false;
        }
    }

    void Capacity2()
    {
        rangeRadius = 10;
        fireEffect.SetActive(true);
        myAnim.SetBool("canSlash", true);
        coolDownCap2 = baseCoolDownCap2;
        canUseCapacity2 = false;
    }

    void Capacity3()
    {
        rangeRadius = 15;
        for (int i = 0; i < ennemyArray.Length; i++)
        {
            if(i <= 9)
            {
                ennemyArray[i].GetComponent<enemyLife>().takeDamage((int)damagePerHit);
            }

            else if (i >= 10)
            {
                canUseCapacity3 = false;
                rangeRadius = 2;
                coolDownCap3 = baseCoolDownCap3;
            }
        }
    }
    #endregion

    #region(hitBox)
    public void HitingSomething()
    {

        TouchByWeapon = Physics.OverlapCapsule(handleOfWeapon.transform.position, weaponTip.transform.position, RadiusWeapon, iWant);

        for (int i = 0; i < TouchByWeapon.Length; i++)
        {
            if (TouchByWeapon[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                myAnim.SetBool("canSlash", false);
                TouchByWeapon[i].GetComponent<enemyLife>().takeDamage((int)damagePerHit);
            }
        }
    }
/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 2f);
        Gizmos.DrawLine(handleOfWeapon.transform.position, weaponTip.transform.position);
        Gizmos.DrawLine(new Vector3(handleOfWeapon.transform.position.x + RadiusWeapon, handleOfWeapon.transform.position.y, handleOfWeapon.transform.position.z), new Vector3(weaponTip.transform.position.x + RadiusWeapon, weaponTip.transform.position.y, weaponTip.transform.position.z));
        Gizmos.DrawLine(new Vector3(handleOfWeapon.transform.position.x - RadiusWeapon, handleOfWeapon.transform.position.y, handleOfWeapon.transform.position.z), new Vector3(weaponTip.transform.position.x - RadiusWeapon, weaponTip.transform.position.y, weaponTip.transform.position.z));
        Gizmos.DrawLine(new Vector3(handleOfWeapon.transform.position.x, handleOfWeapon.transform.position.y, handleOfWeapon.transform.position.z + RadiusWeapon), new Vector3(weaponTip.transform.position.x, weaponTip.transform.position.y, weaponTip.transform.position.z + RadiusWeapon));
        Gizmos.DrawLine(new Vector3(handleOfWeapon.transform.position.x, handleOfWeapon.transform.position.y, handleOfWeapon.transform.position.z - RadiusWeapon), new Vector3(weaponTip.transform.position.x, weaponTip.transform.position.y, weaponTip.transform.position.z - RadiusWeapon));
    }*/
    #endregion
    #endregion
    #endregion

    #region(coroutine)

    IEnumerator DotFlamme(int dotDamage)
    {
        dotTime += Time.deltaTime;
        if (dotTime <= 4)
        {
            ennemyArray[0].GetComponent<enemyLife>().takeDamage(dotDamage);
            yield return new WaitForSeconds(1);
            StartCoroutine(DotFlamme(dotDamage));
        }
        else 
        {
            fireEffect.SetActive(false);
            canUseCapacity2 = false;
        }
    }
    #endregion
}