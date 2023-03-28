using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLife : MonoBehaviour
{
    [SerializeField] private int vie = 100;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void takeDamage(int damage)
    {
        vie-= damage;
        if (vie <= 0)
        {
            anim.SetTrigger("death");
        }
    }
}
