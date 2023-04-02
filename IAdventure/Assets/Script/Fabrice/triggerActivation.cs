using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class triggerActivation : MonoBehaviour
{
    [SerializeField] private UnityEvent nextAction;
    private int nombreHero;
    private int heroMax;

    private void Start() {
        heroMax = GameObject.FindWithTag("Spawn").GetComponent<spawnHero>().heroCount;
    }

    private void OnTriggerEnter(Collider truc) {
        if (truc.gameObject.layer == LayerMask.NameToLayer("Player")) {
            nombreHero++;
            if(nombreHero == heroMax) {
                nextAction.Invoke();
            }
        }
    }
}
