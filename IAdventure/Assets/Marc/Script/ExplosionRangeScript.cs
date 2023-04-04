using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRangeScript : MonoBehaviour
{
    private float time;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        Destroy(gameObject, 0.001f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Enemy"))
        {

            //other.GetComponent<enemyLife>().takeDamage([50])
        }
    }
}
