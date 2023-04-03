using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private Vector3 spawnPoint;
    private RaycastHit hit;
    void Start() {
        if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
            spawnPoint = hit.point;
        }
    }

    
    void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            Instantiate(enemy, spawnPoint, Quaternion.identity);
        }
    }
}
