using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gunther : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private NavMeshAgent agent;
    private Transform goal;
    void Awake() {
        agent= GetComponent<NavMeshAgent>();
        goal = GameObject.FindWithTag("Goal").transform;
        agent.SetDestination(goal.position);

        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 15f);
    }
}
