using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLife : MonoBehaviour
{
    [SerializeField] private int vie = 100;
    [SerializeField] GameObject EffectDeath;
    private robotBig scriptRobot;

    void Start() {
        scriptRobot = GetComponent<robotBig>();
    }

    public void takeDamage(int damage) {
        if(!scriptRobot.actif) {
            scriptRobot.actif = true;
        }

        vie-= damage;

        if (vie <= 0) {
            scriptRobot.actif = true;
            EffectDeath.SetActive(true);
            EffectDeath.transform.parent = null;
            Destroy(gameObject, 0.1f);
            Destroy(EffectDeath, 2f);
        }
    }
}
