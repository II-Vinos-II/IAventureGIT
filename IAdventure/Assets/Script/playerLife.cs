using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLife : MonoBehaviour {
    public int vie = 100;
    [HideInInspector] public int vieMax;
    private Animator anim;
    private MonoBehaviour[] scripts;
    private bool KO;

    void Awake() {
        vieMax = vie;
        if (TryGetComponent<Animator>(out Animator animator)) {
            anim = animator;
        }
        scripts = GetComponents<MonoBehaviour>();
    }

    public void takeDamage(int degats) {
        if (vie > 0) {
            vie -= degats;
        }

        if (vie <= 0 && !KO) {
            vie = 0;
            KO = true;
            foreach (MonoBehaviour truc in scripts) {
                if (truc != this) {
                    truc.enabled = false;
                }
            }
            anim.SetTrigger("coma");
        }
    }

    public void takeHeal(int heal) {
        if (!KO) {
            vie += heal;
            vie = Mathf.Clamp(vie, 0, vieMax);
        }
    }

    public void jesus() {
        KO = false;
        foreach (MonoBehaviour truc in scripts) {
            if (truc != this) {
                truc.enabled = true;
            }
        }
        vie = Mathf.RoundToInt(vieMax / 2);
    }
}
