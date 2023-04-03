using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMesh))]
public class IaParent_Mylane : MonoBehaviour
{
    #region(variable)
    [Header("Géneral")]
    public Transform posTransform;
    protected NavMeshAgent nav;
    protected Rigidbody rgbd;
    protected bool isCasting;
    protected bool isMoving;
    protected bool isAtPos;

    [Header("Stat du Perso")]
    [Range(0f, 20f)]
    public float speed;
    public float hp;
    float maxHp;
    [Range(0f, 100f)]
    public float baseArmor;

    [Header("GestionLife")]
    protected bool isAlive;
    protected bool takeDamage;
    protected bool healing;
    

    [Header("PrimaryFire")]
    public float fireRate;
    public float damagePerHit;
    public float distance;
    public  GameObject ennemieToShoot;
    public float range;
    public Collider[] hitColliders;
    public LayerMask ennemieDetect;

    [Header("capo")]
    protected static bool capoIsDown;
    #endregion

    void Awake()
    {
        isAlive = true;
        nav = GetComponent<NavMeshAgent>();
        rgbd = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isAlive)
        {
            LifeGestion();
        }

    }

    #region(fonction)
    /*protected void Move(Vector3 posToGo)
    {

        nav.destination = posToGo;

        if (transform.position == posToGo)
        {
            isAtPos = true;
        }
        else
        {
            isAtPos = false;
        }
    }*/

    #region(ce qui touche à la vie)
    public void LifeGestion()
    {
        if(hp <= 0)
        {
            isAlive = false;
            hp = 0;
        }
        else if(hp > 0)
        {
            isAlive = true;
        }
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public void Heal(float heal)
    {
        hp += heal;

        if(hp > maxHp)
        {
            hp = maxHp;
        }
    }
    #endregion

    /*protected Transform AttackRadius()
    {
        hitColliders = Physics.OverlapSphere(transform.position, range, ennemieDetect);
        Transform tempTarget = null;
        if (hitColliders.Length > 0)
        {

            float distance = Vector3.Distance(transform.position, hitColliders[0].transform.position);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                float tempDistance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (tempDistance <= distance)
                {
                    distance = tempDistance;
                    tempDistance = 0;
                    tempTarget = hitColliders[i].transform;
                }

            }

        }
        return tempTarget;
    }*/
    #endregion
}
