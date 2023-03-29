using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class porte : MonoBehaviour {

    [SerializeField] private Transform door;
    [SerializeField] private Vector3 targetMove;
    private bool go;

    void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            activation();
        }

        if (go) {
            door.position = Vector3.MoveTowards(door.position, targetMove, Time.deltaTime * 10f);
        }
    }

    public void activation() {
        targetMove += door.position;
        go = true;
    }
}
