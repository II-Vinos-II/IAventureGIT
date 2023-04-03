using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float range;
    [SerializeField] private LayerMask ennemieDetect;
    public Vector3 mouvement;

    RaycastHit hit;
    void Start()
    {
        rb = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        if(rb != null)
        {
            
            rb.velocity = -transform.forward * currentSpeed;
        }
        Shoot(AttackRadius());
    }

    protected Transform AttackRadius()
    {


        Squadmanager.Instance.hitColliders = Physics.OverlapSphere(transform.position, range, ennemieDetect);
        Transform tempTarget = null;
        if (Squadmanager.Instance.hitColliders.Length > 0)
        {

            float distance = Vector3.Distance(transform.position, Squadmanager.Instance.hitColliders[0].transform.position);

            for (int i = 0; i < Squadmanager.Instance.hitColliders.Length; i++)
            {
                float tempDistance = Vector3.Distance(transform.position, Squadmanager.Instance.hitColliders[i].transform.position);
                if (tempDistance <= distance)
                {
                    distance = tempDistance;
                    tempDistance = 0;
                    tempTarget = Squadmanager.Instance.hitColliders[i].transform;
                }

            }

        }
        return tempTarget;
    }

    protected void Shoot(Transform target)
    {


        transform.LookAt(target);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ennemieDetect))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

    }

}
