using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserGate : MonoBehaviour
{
    [SerializeField] private Transform murOpen;
    [SerializeField] private Transform murClose;
    private Transform poteaux1;
    private Transform poteaux2;
    private float distancePoteaux;

    void Start() {
        poteaux1 = transform.GetChild(0);
        poteaux2 = transform.GetChild(1);
        distancePoteaux = Vector3.Distance(poteaux1.position, poteaux2.position);

        murOpen.position = Vector3.Lerp(poteaux1.position, poteaux2.position, 0.5f);
        murClose.position = Vector3.Lerp(poteaux1.position, poteaux2.position, 0.5f);

        murOpen.position += Vector3.up;
        murClose.position += Vector3.up;

        murOpen.localScale = new Vector3(0.2f, 5f, distancePoteaux * 0.99f);
        murClose.localScale = new Vector3(0.2f, 5f, distancePoteaux * 0.99f);
    }

    public void activation() {
        murOpen.gameObject.SetActive(true);
        murClose.gameObject.SetActive(true);
    }
}
