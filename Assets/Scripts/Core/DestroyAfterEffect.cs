﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core {
public class DestroyAfterEffect : MonoBehaviour {
    
    [SerializeField] GameObject targetToDestroy = null;
    void Update() {
        if (!GetComponent<ParticleSystem>().IsAlive()) {
            if (targetToDestroy) {
                Destroy(targetToDestroy);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
}//namespace
