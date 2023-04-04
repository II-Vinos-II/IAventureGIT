using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneKamikazeScript : MonoBehaviour
{
    [Header("Stats du drone Kamikaze")]

    public float speed = 10f;
    public float RangeDetection = 1f;
    public float Timer = 10f;
    //public float DamageRange = 5f;
    //public float Damage = 30f;

    private float time;
    [SerializeField] private GameObject Explosionrange;

    [Header("Utilitaire pour le déplacement")]

    private RaycastHit _RCH;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _RCH, 1))
        {
            transform.Rotate(Vector3.up * Random.Range(50, 200));
        } 
        time = time + Time.deltaTime;
        if (time >= Timer)
        {
            GameObject explode = Instantiate(Explosionrange);
            explode.transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }
    }
}
