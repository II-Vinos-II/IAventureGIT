using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float damage;
    private Rigidbody rgbd;
    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rgbd.velocity = transform.forward*15;
    }
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.SendMessage("takeDamage",damage);
        
        Destroy(gameObject);
    }
}
