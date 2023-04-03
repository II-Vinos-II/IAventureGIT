using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electroballmovementandreactions : MonoBehaviour
{
    public float damage = 10f;
    public float StunTime = 1f;
    public float speed = 3f;

    public float Propag_Distance = 2f;
    public float MaxEnnemyHit = 3;
    public float PropagCooldown = 0.2f;

    private float time; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        
        time = time + Time.deltaTime;
        if (time >= 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Destroy(gameObject);
            //Destroy(other.gameObject);
            other.gameObject.SendMessage("FREEZE");
        }
    }
}
