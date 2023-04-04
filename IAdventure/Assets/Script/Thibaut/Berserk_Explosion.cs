using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserk_Explosion : IaBerserk
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == ennemieDetect)
        {
            Debug.Log("KABOOM");
            other.gameObject.GetComponent<enemyLife>().takeDamage((int)damage0);
            takeHeal((int) hpBack3);
        }
    }
}
