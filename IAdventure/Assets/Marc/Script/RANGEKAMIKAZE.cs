using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RANGEKAMIKAZE : MonoBehaviour
{
    public float Timer = 10f;
    public float Damage = 30f;
    private float time;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        time = time + Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (time >= Timer)
            {
                Destroy(other.gameObject);
            }
        }

    }
}
