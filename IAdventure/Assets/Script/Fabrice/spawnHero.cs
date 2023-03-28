using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnHero : MonoBehaviour
{
    [Header("Mettez vos personnages dans ce tableau")]
    [SerializeField] private GameObject[] heros;

    [Header("Ne pas toucher")]
    [SerializeField] private GameObject Beam;
    private GameObject beamSave;

    private Transform[] spawnPoints;

    void Start() {
        spawnPoints = new Transform[transform.childCount];
        for(int i = 0 ; i < transform.childCount ; i++) {
            spawnPoints[i] = transform.GetChild(i);
        }
        StartCoroutine(spawning());
    }

    IEnumerator spawning() {
        for (int i = 0 ; i < heros.Length ; i++) {
            beamSave = Instantiate(Beam, spawnPoints[i].position, Quaternion.identity);
            Destroy(beamSave, 1f);

            yield return new WaitForSeconds(0.15f);

            Instantiate(heros[i], spawnPoints[i].position, Quaternion.identity);
        }
    }
}
