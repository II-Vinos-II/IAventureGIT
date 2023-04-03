using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSuicide : MonoBehaviour
{
    public float t;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(boom());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator boom()
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
