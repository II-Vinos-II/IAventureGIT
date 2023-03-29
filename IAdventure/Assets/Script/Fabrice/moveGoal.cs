using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveGoal : MonoBehaviour {

    [SerializeField] private Transform goalObject;
    private Transform[] points;
    private int pointCount;

    void Start() {
        points = new Transform[transform.childCount];
        for(int i = 0 ; i < points.Length ; i++) {
            points[i] = transform.GetChild(i);
        }
        goalObject.position= points[0].position;
    }

    public void nextPoint() {
        pointCount++;
        goalObject.position = points[pointCount].position;
    }
}
