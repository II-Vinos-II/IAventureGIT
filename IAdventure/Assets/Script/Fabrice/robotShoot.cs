using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotShoot : MonoBehaviour
{
    [SerializeField] private Transform canon;
    [SerializeField] private float cooldown;
    [SerializeField] private float projectilSpeed = 15f;
    [SerializeField] private GameObject laser;
    private GameObject laserSave;
    private Rigidbody rb;

    public void shoot(Transform target, int _degats) {
        laserSave = Instantiate(laser, canon.position, Quaternion.identity);
        rb = laserSave.GetComponent<Rigidbody>();
        laserSave.GetComponent<robotLaser>().degats = _degats;
        rb.velocity = ((target.position + Vector3.up) - canon.position).normalized * projectilSpeed;
    }
}
