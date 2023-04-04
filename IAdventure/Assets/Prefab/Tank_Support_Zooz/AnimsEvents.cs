using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimsEvents : MonoBehaviour
{
    [SerializeField] private EisernJungfrauCornelia coooneliaaaaa;

    public void Fire()
    {
        coooneliaaaaa.Fire(coooneliaaaaa.attackTarget);
    }

    public void Reflect()
    {
        coooneliaaaaa.StopCap3();
    }

    public void EndVulerability()
    {
        
    }

    public void gaaw()
    {
        coooneliaaaaa.Capacite1();
    }

    public void UnlockAnims()
    {
        coooneliaaaaa.isAnimLocked = false;
    }
}
