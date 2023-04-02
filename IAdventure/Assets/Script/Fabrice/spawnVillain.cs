using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class spawnVillain : MonoBehaviour
{
    [Header("C'est qui le villain a fumer ?")]
    [SerializeField] private GameObject villainPrefab;
    private GameObject villainSave;
    private List <GameObject> villains;

    [Header("Sinon, vous ça va ?")]    
    [SerializeField] private GameObject Beam;
    private GameObject beamSave;
    private Transform[] spawnPoints;
    public UnityEvent nextEvent;

    void Start() {
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0 ; i < transform.childCount ; i++) {
            spawnPoints[i] = transform.GetChild(i);
        }        
    }

    IEnumerator spawning() {
        for (int i = 0 ; i < villains.Count ; i++) {
            beamSave = Instantiate(Beam, spawnPoints[i].position, Quaternion.identity);
            Destroy(beamSave, 1f);
            villainSave = Instantiate(villainPrefab, spawnPoints[i].position, Quaternion.identity);
            villains.Add(villainSave);
            villainSave.GetComponent<enemyLife>().jeSuisTonPere(this);
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void jeSuisMouru(GameObject noob) {
        villains.Remove(noob);
        if (villains.Count == 0) {
            nextEvent.Invoke();
        }
    }

    public void activation() {
        StartCoroutine(spawning());
    }
}
