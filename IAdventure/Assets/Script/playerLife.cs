using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLife : MonoBehaviour
{
    public int vie = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void takeDamage(int degats)
    {
        vie -= degats;
        if (vie <= 0)
        {
            Destroy(gameObject);
        }
    }
}
