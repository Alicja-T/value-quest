using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat {
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    [SerializeField] AnimatorOverrideController weaponOverride = null;
    [SerializeField] GameObject equippedPrefab = null;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float weaponDamage = 10f;

    public void Spawn(Transform handTransform, Animator animator){
        if (equippedPrefab != null) {
            Instantiate(equippedPrefab, handTransform);
        }
        //this is just to avoid error, but there must be another way...
        if (weaponOverride != null){ 
            animator.runtimeAnimatorController = weaponOverride;
        }
    }

    public float GetRange() {
        return weaponRange;
    }

    public float GetDamage() {
        return weaponDamage;
    }

}  
}//namespace
