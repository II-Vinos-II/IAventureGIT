using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLife : MonoBehaviour
{
    [SerializeField] private int vie = 100;
    private Animator anim;
    private robotBig scriptRobot;

    void Start() {
        anim = GetComponent<Animator>();
        scriptRobot = GetComponent<robotBig>();
    }

    public void takeDamage(int damage) {
        if(!scriptRobot.actif) {
            scriptRobot.actif = true;
        }
        vie-= damage;
        if (vie <= 0) {
            anim.SetTrigger("death");
        }
    }
}
