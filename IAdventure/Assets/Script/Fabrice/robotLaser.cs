using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotLaser : MonoBehaviour
{
    public int degats = 5;
    [SerializeField] private GameObject effect;
    private GameObject effectSave;

    private void OnTriggerEnter(Collider truc) {
        if (truc.tag == "Player") {
            effectSave = Instantiate(effect, transform.position, Quaternion.identity);
            truc.SendMessage("takeDamage", degats, SendMessageOptions.DontRequireReceiver);
            Destroy(effectSave, 3);
            Destroy(gameObject);
        }
    }
}
