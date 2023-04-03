using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class zapPropagation : MonoBehaviour
{
    public int bounce;
    private Collider[] victimes;
    private Transform nextTarget;
    private Transform maTarget;
    public Transform targetVictime;
    private GameObject newZap;

    void Start() {
        maTarget = transform.GetChild(0);
        StartCoroutine(waitBounce());
    }

    IEnumerator waitBounce() {
        yield return new WaitForSeconds(0.2f);
        if (bounce > 0) {
            victimes = Physics.OverlapSphere(maTarget.position, 2f, LayerMask.GetMask("Enemy"));
            foreach(Collider truc in victimes) {
                if (truc.transform != targetVictime) {
                    newZap = Instantiate(gameObject, maTarget.position + Vector3.up, Quaternion.identity);
                    bounce--;
                    nextTarget = newZap.transform.GetChild(0);
                    nextTarget.position = truc.transform.position;
                    newZap.GetComponent<zapPropagation>().bounce = bounce;
                    newZap.GetComponent<zapPropagation>().targetVictime = truc.transform;
                    Destroy(newZap, 2f);
                    break;
                }
            }
        }
    }
}
