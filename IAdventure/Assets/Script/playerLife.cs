using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLife : MonoBehaviour {
    public int vie = 100;
    public int armor = 0;
    private int armorMax;
    [HideInInspector] public int vieBonus;
    [HideInInspector] public int vieMax;
    private Animator anim;
    private MonoBehaviour[] scripts;
    public bool KO;
    private bool regenArmor = true;

    void Awake() {
        vieMax = vie;
        armorMax = armor;
        if (TryGetComponent<Animator>(out Animator animator)) {
            anim = animator;
        }
        scripts = GetComponents<MonoBehaviour>();
    }

    void Update() {
        if (regenArmor && !KO && armorMax > 0 && armor < armorMax) {
            armor = Mathf.RoundToInt(Mathf.MoveTowards(armor, armorMax, Time.deltaTime * 10f));
        }
    }

    public void takeDamage(int degats) {
        if (vie > 0) {
            if (armor > 0) {
                armor -= degats;
                if (armor < 0) {
                    armor = 0;
                }
            }
            else if (vieBonus > 0) {
                vieBonus -= degats;
            } else {
                vie -= degats;
            }
            regenArmor = false;
            StopCoroutine(armorReset());
            StartCoroutine(armorReset());
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
        anim.SetTrigger("alive");
    }

    IEnumerator armorReset() {
        yield return new WaitForSeconds(3f);
        regenArmor = true;
    }
}
