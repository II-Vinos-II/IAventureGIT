using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class robotBigSuicide : MonoBehaviour
{
    [SerializeField] private GameObject rocket1, rocket2, kaboom;
    private bool go;
    private Collider[] targets;
    private Transform victime;
    public Transform targetMove;
    private float speed;

    void Update() {
        if (go) {
            speed = Mathf.MoveTowards(speed, 10f, Time.deltaTime);
            targetMove.position = Vector3.MoveTowards(targetMove.position, victime.position, Time.deltaTime * speed * 10);
            transform.position = Vector3.MoveTowards(transform.position, targetMove.position, Time.deltaTime * speed * 10);
            transform.LookAt(victime);
            transform.Rotate(90, 0, 0);

            if(transform.position == victime.position) {
                victime.GetComponent<enemyLife>().takeDamage(100);
                kaboom.transform.parent = null;
                kaboom.SetActive(true);
                GetComponent<enemyLife>().takeDamage(1000);                
            }
        }
    }

    public void killYourselfNoob() {
        go = true;
        rocket1.SetActive(true);
        rocket2.SetActive(true);
        GetComponent<robotBig>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        targets = Physics.OverlapSphere(transform.position, 20f, LayerMask.GetMask("Enemy"));
        if (targets.Length > 0) {
            victime = targets[UnityEngine.Random.Range(0, targets.Length)].transform;
        } else {
            kaboom.transform.parent = null;
            kaboom.SetActive(true);
            GetComponent<enemyLife>().takeDamage(1000);
        }
        GameObject coincoin = new GameObject("canard");
        targetMove = coincoin.transform;
        targetMove.position = victime.position + Vector3.up * 10f;
    }
}
