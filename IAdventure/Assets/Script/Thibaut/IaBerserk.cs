using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMesh))]
public class IaBerserk : playerLife
{
    [Header("Géneral")]
    protected Animator playerAnimator;
    protected Transform posTransform;
    protected NavMeshAgent nav;
    protected Rigidbody rgbd;
    protected bool isCasting;
    protected bool isMoving;

    [Header("Stat du Perso")]
    [Range(0f, 20f)]
    public float speed;
    [Range(0f, 100f)]
    public float baseArmor;

    [Header("Detect")]
    protected GameObject ennemieToShoot;
    protected float posdistance;
    protected bool isAtPos;
    public float detectRange;
    protected Collider[] hitColliders;
    public LayerMask ennemieDetect;


    [Header("PrimaryFire")]
    public float cooldown0;
    public float damage0;
    protected bool canuse0 = true;

    [Header("Capacity 1 : PifPafPouf")]
    public float cooldown1;
    public float damage1;
    public int hitNumber1;
    public float duration1;
    protected bool canuse1 = true;

    [Header("Capacity 2 : Com'here !")]
    public float cooldown2;
    public float damage2;
    public float maxDistance2;
    protected bool canuse2 = true;

    [Header("Capacity 3 : Army of the dead")]
    public int deadCounter;
    public GameObject textMeshTransform;
    public GameObject explosionPrefab;

    public float cooldown3 = 5f;
    public float hpBack3 = 5f;
    public float blastSize3 = 2.5f;
    public float blastPerS3 = 2f;
    public float blastRange3 = 20f;
    protected bool canuse3 = true;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rgbd = GetComponent<Rigidbody>();
        isCasting = false;
        isMoving = false;
        posTransform = transform;
    }

    void Update()
    {
        //set up de l'ennemis
        if (AttackRadius() != null)
        {
            if (ennemieToShoot == null)
            {
                deadCounter++;
                textMeshTransform.GetComponent<TextMeshPro>().text = deadCounter + "+";
                textMeshTransform.GetComponent<Animation>().Play("DeathUp");

                ennemieToShoot = AttackRadius().gameObject;
            }
            else
            {
                float smollcheck = Vector3.Distance(AttackRadius().position, transform.position);

                if (smollcheck < posdistance && smollcheck > 3f)
                {
                    ennemieToShoot = AttackRadius().gameObject;
                }
            }

        }
        //textfacecam
        textMeshTransform.transform.rotation = Quaternion.LookRotation(textMeshTransform.transform.position - Camera.main.transform.position);

        //déplacement vers l'ennemis
        if (ennemieToShoot != null)
        {
            posTransform = ennemieToShoot.transform;
        }
        else if (GameObject.FindGameObjectWithTag("Goal") != null)
        {
            posTransform = GameObject.FindGameObjectWithTag("Goal").transform;
        }
        else
        {
            posTransform = transform;
        }

        //calcule de la distance
        DistanceCalcul();

        //move et attack
        if (vie > 0 && !isCasting)
        {
            Move(posTransform.position, speed);

            if (ennemieToShoot != null)
            {
                Attack();
            }
        }
    }

    public void DistanceCalcul()
    {
        posdistance = Vector3.Distance(posTransform.position, transform.position);

        if (posdistance >= 2f)
        {
            isAtPos = false;
            isMoving = true;
        }
        else if (posdistance <= 2f)
        {
            isAtPos = true;
            isMoving = false;
        }

        playerAnimator.SetBool("IsMoving", isMoving);
    }

    public void Move(Vector3 posToGo, float movespeed)
    {
        nav.destination = posToGo;
        nav.speed = movespeed;
    }

    public void Attack()
    {
        Collider[] smolhitcollid;

        //CAPACITY 3
        smolhitcollid = Physics.OverlapSphere(transform.position, 20f, ennemieDetect);
        if (deadCounter > 5 && smolhitcollid.Length >= 15)
        {
            if (canuse3)
            {
                canuse3 = false;
                isCasting = true;
                StartCoroutine(DeadArmy());
                Debug.Log("Power 3 Worked");
            }
            else
            {
                Debug.Log("Power 3 FAILED");
            }
        }

        //CAPACITY 2
        smolhitcollid = Physics.OverlapSphere(ennemieToShoot.transform.position, 3f, ennemieDetect);
        if ((posdistance < maxDistance2 + 1) && (posdistance > maxDistance2 - 1) && smolhitcollid.Length >= 3)
        {
            if (canuse2)
            {
                canuse2 = false;
                isCasting = true;
                StartCoroutine(ComHere());
                Debug.Log("Power 2 Worked");
            }
            else
            {
                Debug.Log("Power 2 FAILED");
            }

        }

        if (isAtPos)
        {
            smolhitcollid = Physics.OverlapSphere(ennemieToShoot.transform.position, 3f, ennemieDetect);
            //Debug.Log(smolhitcollid.Length + " enemy around me");
            if (smolhitcollid.Length >= 3)
            {
                //CAPACITY 1
                if (canuse1)
                {
                    canuse1 = false;
                    isCasting = true;
                    StartCoroutine(PPP());
                    Debug.Log("Power 1 Worked");
                }
                else
                {
                    Debug.Log("Power 1 FAILED");
                }

            }
            else
            {
                //CAPACITY 0
                if (!isCasting && canuse0)
                {
                    canuse0 = false;
                    isCasting = true;
                    StartCoroutine(Fire());
                }

            }
        }

    }

    public IEnumerator Fire()
    {
        ennemieToShoot.GetComponent<enemyLife>().takeDamage((int)damage0);

        playerAnimator.SetTrigger("Attack");

        isCasting = false;
        yield return new WaitForSeconds(cooldown0);
        canuse0 = true;
    }

    public IEnumerator PPP()
    {
        playerAnimator.SetTrigger("Capacity1");

        float timeSpace = duration1 / hitNumber1;

        for (int i = 0; i < hitNumber1; i++)
        {
            if (isAtPos)
            {
                ennemieToShoot.GetComponent<enemyLife>().takeDamage((int)damage1);
                takeHeal((int) damage1);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(timeSpace);
        }

        isCasting = false;
        yield return new WaitForSeconds(cooldown1);
        canuse1 = true;
    }

    public IEnumerator ComHere()
    {
        Move(ennemieToShoot.transform.position, speed * 3);
        playerAnimator.SetTrigger("Capacity2");

        while (!isAtPos)
        {
            yield return null;
        }

        StartCoroutine(ConstantFocus(ennemieToShoot));
        ennemieToShoot.GetComponent<enemyLife>().takeDamage((int)damage2);
        playerAnimator.SetTrigger("HasLanded");

        isCasting = false;
        yield return new WaitForSeconds(cooldown2);
        canuse2 = true;
    }

    public IEnumerator DeadArmy()
    {
        playerAnimator.SetBool("Capacity3", true);

        for (int i = 0; i < deadCounter; i++)
        {
            Vector3 randomrot = new Vector3(transform.rotation.x, Random.Range(0f, 361f), transform.rotation.z);
            GameObject boom = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(randomrot));
            boom.transform.localPosition = new Vector3(boom.transform.localPosition.x, boom.transform.localPosition.y, Random.Range(1f, 21f));

            yield return new WaitForSeconds(0.5f);
            Destroy(boom);
        }

        playerAnimator.SetBool("Capacity3", false);
        deadCounter = 0;

        isCasting = false;
        yield return new WaitForSeconds(cooldown3);
        canuse3 = true;
    }

    public IEnumerator ConstantFocus(GameObject focusedEnemy)
    {
        //focusedEnemy.GetComponent<robotBig>().Taunt(1f); //C'EST LE TAUNT
        yield return new WaitForSeconds(1);

        if (focusedEnemy)
        {
            StartCoroutine(ConstantFocus(focusedEnemy));
        }
    }

    public Transform AttackRadius()
    {
        hitColliders = Physics.OverlapSphere(transform.position, detectRange, ennemieDetect);
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
    }
}
