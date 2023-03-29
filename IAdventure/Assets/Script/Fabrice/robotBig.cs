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
        if (combat && targetPlayer != null) {
            lookTarget.position = targetPlayer.position + Vector3.up;
            aimTarget.position = Vector3.MoveTowards(aimTarget.position, targetPlayer.position + Vector3.up, Time.deltaTime * 20);
            if (aimRig.weight != 1) {
                aimRig.weight = Mathf.MoveTowards(aimRig.weight, 1, Time.deltaTime * 10);
            }            
        }

        if (combat && !reload) {
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
        
        yield return new WaitForSeconds(1f);
        StartCoroutine(checkTarget());
    }

    IEnumerator shooting() {
        yield return new WaitForSeconds(1f);
        reload = false;
    }

    void animCheck() {
        anim.SetFloat("velocity", agent.velocity.magnitude);
        anim.SetBool("combat", combat);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceAggro);
    }
}
