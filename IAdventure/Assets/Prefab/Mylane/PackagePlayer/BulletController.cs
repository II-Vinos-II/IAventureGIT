using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float damage;
    private Rigidbody rgbd;
    public GameObject impact;
    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        rgbd.velocity = transform.forward*speed;       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            collision.gameObject.SendMessage("takeDamage", 1);
            Debug.Log("chech");
            Instantiate(impact, transform.position, transform.rotation);
            Destroy(gameObject);

        }
       
        
       
    }

}
