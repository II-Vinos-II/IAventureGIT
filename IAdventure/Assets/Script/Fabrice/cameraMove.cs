using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    [HideInInspector] public List<Transform> heros;

    private Vector3 positionTarget;
    private Bounds bounds;
    private Vector3 refVelocity;

    void LateUpdate() {
        if(heros.Count > 0) {
            positionTarget = GetCenterPoint();
            transform.position = Vector3.SmoothDamp(transform.position, positionTarget, ref refVelocity, 0.5f);
        }
    }

    //fonction pour calculer le centre de tout les personnage
    Vector3 GetCenterPoint() {
        if (heros.Count == 1) {
            return heros[0].position;
        }
        bounds = new Bounds(heros[0].position, Vector3.zero);
        for(int i = 0 ; i < heros.Count ; i++) {
            bounds.Encapsulate(heros[i].position);
        }

        return bounds.center;
    }
}
