using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capacity_3Controller : MonoBehaviour
{
    private bool damageable;
    public GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        damageable = true;
        StartCoroutine(ApplyDamage());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ApplyDamage()

    {
        damageable = true;
        Instantiate(Explosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.01f);
        damageable = false;
        yield return new WaitForSeconds(0.95f);
        StartCoroutine(ApplyDamage());
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (damageable)
        {
            other.gameObject.SendMessage("takeDamage", 20);
            Debug.Log("aradegat");
            //Destroy(gameObject);

        }
    }

}
