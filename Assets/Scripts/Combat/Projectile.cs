using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class Projectile : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] float speed = 1f;
    
    void Update()
    {   
        if (target == null) return;
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward*Time.deltaTime);
        
    }

    private Vector3 GetAimLocation(){
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
        if (collider == null) return target.position;
        return target.position + Vector3.up * collider.height/2f;
    }
}
