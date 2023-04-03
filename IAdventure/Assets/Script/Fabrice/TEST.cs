using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public GameObject zap;
    public Transform Enemy;
    private Transform targetZap;
    public Transform canon;
    private GameObject zapSave;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            zapSave = Instantiate(zap, canon.transform.position, Quaternion.identity);
            targetZap = zapSave.transform.GetChild(0).transform;
            targetZap.position = Enemy.position + Vector3.up;
            Destroy(zapSave, 2f);
        }
    }
}
