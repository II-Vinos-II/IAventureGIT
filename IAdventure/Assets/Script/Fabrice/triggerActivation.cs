using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerActivation : MonoBehaviour
{
    [SerializeField] private GameObject activationObject;
    [SerializeField] private moveGoal goalToMove;
    private int nombreHero;
    private int heroMax;

    private void Start() {
        heroMax = GameObject.FindWithTag("Spawn").GetComponent<spawnHero>().heroCount;
    }

    private void OnTriggerEnter(Collider truc) {
        if (truc.gameObject.layer == LayerMask.NameToLayer("Player")) {
            nombreHero++;
            if(nombreHero == heroMax) {
                activationObject.SendMessage("activation");
                if (goalToMove != null) {
                    goalToMove.nextPoint();
                }
            }
        }
    }
}
