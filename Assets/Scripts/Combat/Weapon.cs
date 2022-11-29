﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat {
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    [SerializeField] AnimatorOverrideController weaponOverride = null;
    [SerializeField] GameObject weaponPrefab = null;

    public void Spawn(Transform handTransform, Animator animator){
        Instantiate(weaponPrefab, handTransform);
        //this is just to avoid error, but there must be another way...
        animator.runtimeAnimatorController = weaponOverride.runtimeAnimatorController;
    }



}  
}//namespace