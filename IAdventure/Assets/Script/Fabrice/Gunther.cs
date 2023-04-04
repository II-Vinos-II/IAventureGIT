using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class Gunther : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int degats = 5;
    private NavMeshAgent agent;
    private Transform goal;
    private Collider[] batardsColl;
    private Animator anim;

    private bool combat;
    private bool jeTabasse;
    private bool punch;
    private Transform boloss;
    private float distanceBatard;

    void Awake() {
        anim = GetComponent<Animator>();
        agent= GetComponent<NavMeshAgent>();
        goal = GameObject.FindWithTag("Goal").transform;
        agent.SetDestination(goal.position);
        agent.speed = speed;
        StartCoroutine(checkGoal());
        StartCoroutine(checkBatard());
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("velocity", agent.velocity.magnitude);
        if (combat && boloss != null) {
            tabasser();
        } 
        else if(!combat){
            agent.SetDestination(goal.position);
        }
    }

    void tabasser() {
        if(Vector3.Distance(transform.position, boloss.position) > 2f) {
            agent.SetDestination(boloss.position);
            jeTabasse = false;
        } else {
            agent.isStopped = true;
            jeTabasse = true;
        }

        if (jeTabasse && !punch) {
            punch = true;
            anim.SetTrigger("punch");
        }
    }

    IEnumerator checkBatard() {
        yield return new WaitForSeconds(0.5f);
        batardsColl = Physics.OverlapSphere(transform.position, 20f, LayerMask.GetMask("Enemy"));
        distanceBatard = 20f;
        if (batardsColl.Length > 0 ) {
            foreach (Collider truc in batardsColl) {
                if (Vector3.Distance(transform.position, truc.transform.position) < distanceBatard) {
                    distanceBatard = Vector3.Distance(transform.position, truc.transform.position);
                    boloss = truc.transform;
                }
            }
            combat = true;
        } else {
            combat = false;
        }
        StartCoroutine(checkBatard());
    }

    IEnumerator checkGoal() {
        yield return new WaitForSeconds(1f);
        agent.SetDestination(goal.position);
        StartCoroutine(checkGoal());
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 15f);
    }

    public void PAF() {
        if (boloss != null) {
            boloss.GetComponent<enemyLife>().takeDamage(degats);
        }        
    }
    public void punchEnd() {
        punch = false;
    }
}
