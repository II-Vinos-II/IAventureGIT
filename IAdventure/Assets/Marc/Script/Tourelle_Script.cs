using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tourelle_Script : MonoBehaviour
{
    [Header("Stats de la tourelle")]
    public float damage = 10f;
    public float Cooldown = 5f;
    public float Distance = 30f;
    public float StunTime = 1f;

    public float Propag_Distance = 2f;
    public float MaxEnnemyHit = 3;
    public float PropagCooldown = 0.2f;

    [SerializeField] private LayerMask ennemyLayer;
    private float timer;
    [SerializeField] private GameObject ElectroBall;
    [SerializeField] private GameObject CanoonEmbout;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;
        if (timer >= Cooldown)
        {
            closestTarget();
            GameObject Tir = Instantiate(ElectroBall, CanoonEmbout.transform.position, CanoonEmbout.transform.rotation);
            timer = 0;
        }
    }

    private void closestTarget()
    {
        Collider[] availableTargets = Physics.OverlapSphere(transform.position, Distance, ennemyLayer);
        int closest = 0;

        for (int i = 0; i < availableTargets.Length; i++)
        {
            if (Vector3.Distance(gameObject.transform.position, availableTargets[i].transform.position) < Vector3.Distance(gameObject.transform.position, availableTargets[closest].transform.position))
            {
                closest = i;
            }
        }
        gameObject.transform.LookAt(availableTargets[closest].transform.position + Vector3.up);
    }
}
