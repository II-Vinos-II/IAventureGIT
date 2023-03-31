using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class robotBig : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int degats = 5;
    [SerializeField] private float distanceAggro = 10f;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask crazyMask;

    [SerializeField] private GameObject redLight;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Rig aimRig;
    private NavMeshAgent agent;

    private bool combat;
    [HideInInspector] public bool actif;
    private Transform targetPlayer;

    private Animator anim;
    private Transform player;
    public Collider[] targets;
    private float[] targetDistance;

    //gestion attaque
    private bool reload;
    private robotShoot shootScript;
    private bool vueClear;

    //Gestion Hack
    private bool hacked;
    private int hackChoice;
    private bool crazy;
    private bool crazyTargetChange;
    [SerializeField] private GameObject miniBoom;

    void Start() {        
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;        
        shootScript = GetComponent<robotShoot>();
        StartCoroutine(checkTarget());
    }

    void searchPlayer() {
        player = GameObject.FindWithTag("Player").transform;
        agent.SetDestination(player.position);
    }

    
    void Update() {
        if(Input.GetKeyDown(KeyCode.K)) {
            TayMaBitch();
        }

        if (combat && targetPlayer != null && !hacked) {
            lookTarget.position = targetPlayer.position + Vector3.up;
            aimTarget.position = Vector3.MoveTowards(aimTarget.position, targetPlayer.position + Vector3.up, Time.deltaTime * 20);
            if (aimRig.weight != 1) {
                aimRig.weight = Mathf.MoveTowards(aimRig.weight, 1, Time.deltaTime * 10);
            }
        }

        if (combat && !reload && !hacked) {
            reload = true;
            if (targetPlayer != null) {
                shootScript.shoot(targetPlayer, degats);
            }            
            StartCoroutine(shooting());
        }

        if (crazy && !reload) {
            reload = true;
            if (targetPlayer != null) {
                shootScript.shoot(targetPlayer, degats);
            }
            StartCoroutine(shooting());
        }

        if (!combat) {
            if (aimRig.weight != 0) {
                aimRig.weight = Mathf.MoveTowards(aimRig.weight, 0, Time.deltaTime * 10);
            }
        }
        animCheck();
    }

    IEnumerator checkTarget() {
        if (!actif) {
            targets = Physics.OverlapSphere(transform.position, distanceAggro, playerMask);
            if (targets.Length > 0) {
                actif = true;
                agent.SetDestination(targets[0].transform.position);
            }
        }

        if (actif) {
            if (!crazy) {
                targets = Physics.OverlapSphere(transform.position, distanceAggro * 0.7f, playerMask);
                if (targets.Length > 0) {
                    targetDistance = new float[targets.Length];
                    for (int i = 0 ; i < targets.Length ; i++) {
                        targetDistance[i] = Vector3.Distance(transform.position, targets[i].transform.position);
                    }
                    Array.Sort(targetDistance, targets);

                    combat = true;
                    redLight.SetActive(true);
                    targetPlayer = targets[0].transform;
                    agent.isStopped = true;
                } else {
                    combat = false;
                    redLight.SetActive(false);
                    targetPlayer = null;
                    agent.isStopped = false;
                    searchPlayer();
                }
            }

            if (crazy && !crazyTargetChange) {
                crazyTargetChange = true;
                targets = Physics.OverlapSphere(transform.position, distanceAggro * 0.7f, crazyMask);
                if (targets.Length > 0) {
                    combat = true;
                    redLight.SetActive(true);
                    targetPlayer = targets[UnityEngine.Random.Range(0, targets.Length)].transform;
                    agent.isStopped = true;
                } else {
                    combat = false;
                    redLight.SetActive(false);
                    targetPlayer = null;
                    agent.isStopped = false;
                    searchPlayer();
                }

                StartCoroutine(crazyTarget());
            }
        }
        
        yield return new WaitForSeconds(1f);
        StartCoroutine(checkTarget());
    }

    public void TayMaBitch() {
        hackChoice = UnityEngine.Random.Range(0, 4);
        switch(hackChoice) {
            case 0:
                print("BOOM");
                anim.SetTrigger("boom");
                hacked = true;
                StartCoroutine(returnCombat(3));
                break;
            case 1:
                print("I'M CRAZYYYYYY");
                crazy = true;
                hacked = true;
                break;
            case 2:
                print("BANZAIE");
                combat = false;
                anim.SetTrigger("suicide");
                GetComponent<robotBigSuicide>().killYourselfNoob();
                break;
            case 3:
                print("Chaud cacao");
                anim.SetTrigger("dance");
                hacked = true;
                StartCoroutine(returnCombat(5));
                break;
        }
    }

    IEnumerator returnCombat(float time) {
        yield return new WaitForSeconds(time);
        hacked = false;
    }

    IEnumerator shooting() {
        yield return new WaitForSeconds(1f);
        reload = false;
    }

    IEnumerator crazyTarget() {
        yield return new WaitForSeconds(2f);
        crazyTargetChange = false;
    }

    void animCheck() {
        anim.SetFloat("velocity", agent.velocity.magnitude);
        anim.SetBool("combat", combat);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceAggro);
    }
    public void petitBoom() {
        targets = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Enemy"));
        foreach(Collider truc in targets) {
            truc.SendMessage("takeDamage", 30f);
        }
        miniBoom.SetActive(true);
        StartCoroutine(miniWait());
    }

    IEnumerator miniWait() {
        yield return new WaitForSeconds(2f);
        miniBoom.SetActive(false);
    }
}
