using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class Projectile : MonoBehaviour
{
    
    [SerializeField] float speed = 1f;
    Health target = null;
    void Update()
    {   
        if (target == null) return;
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward*Time.deltaTime*speed);
        
    }

    public void SetTarget(Health newTarget) {
        this.target = newTarget;
    }

    private Vector3 GetAimLocation(){
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
        if (collider == null) return target.transform.position;
        return target.transform.position + Vector3.up * collider.height/2f;
    }
}
