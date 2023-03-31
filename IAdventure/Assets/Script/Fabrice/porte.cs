using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class porte : MonoBehaviour {

    [SerializeField] private Transform door;
    [SerializeField] private Vector3 targetMove;
    private bool go;

    void Update() {
        if (go) {
            door.localPosition = Vector3.MoveTowards(door.localPosition, targetMove, Time.deltaTime * 10f);
        }
    }

    public void activation() {
        targetMove += door.localPosition;
        go = true;
    }
}
