using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSoldatScript : MonoBehaviour
{
    [Header("Stats des drones")]

    public float Damage = 1f;
    public float Distance = 30f;
    public float Atk_Cooldown = 0.05f;
    public float Timer = 5f;
    public int AngleRange;

    [Header("Utilitaire")]

    [SerializeField] private float CDAtk;
    [SerializeField] private float CDSpawn;

    [SerializeField] private GameObject[] Spawn;

    public ScriptPlayer ScriptPlayerRef;

    public Collider[] Overlap;
    public LayerMask monlayer;
    public GameObject laser;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CDAtk = CDAtk + Time.deltaTime;
        CDSpawn = CDSpawn + Time.deltaTime;
        //Vector3 Position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        
        AngleRange = Random.Range(0, 3);
        //if (Physics.SphereCast(transform.position, 30, monlayer ))
        {
            if (CDAtk >= Atk_Cooldown)
            {
                //Debug.Log(AngleRange);
                Vector3 Position = Spawn[AngleRange].transform.position;
            
            
                //Quaternion rotation = new Quaternion(0, 25, 0, 360);
                //Debug.Log(rotation);
                GameObject Tir = Instantiate(laser, Position, transform.parent.parent.localRotation) as GameObject;
                if (ScriptPlayerRef.targetSHooter != null)
                    Tir.GetComponent<Bullet_Movement>().direction = ((ScriptPlayerRef.targetSHooter.transform.position + Vector3.up + new Vector3(Random.Range(-3,4),0,0)) - Position).normalized;
                else
                    Tir.GetComponent<Bullet_Movement>().direction = (transform.forward);
                //Tir.transform.position = gameObject.transform.position;
                //Tir.transform.rotation = new Quaternion (gameObject.transform.rotation.x,AngleRange, gameObject.transform.rotation.z , 1);

                CDAtk = 0;
            }
        }
        


        if (CDSpawn >= Timer)
        {
            gameObject.SetActive(false);
        }
    }
}
