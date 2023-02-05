using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class Projectile : MonoBehaviour
{
    
    [SerializeField] float speed = 1f;
    Health target = null;
    float damage = 0f;
    void Update()
    {   
        if (target == null) return;
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward*Time.deltaTime*speed);
        
    }

    public void SetTarget(Health newTarget, float damage) {
        this.target = newTarget;
        this.damage = damage;
    }

    private Vector3 GetAimLocation(){
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();
        if (collider == null) return target.transform.position;
        return target.transform.position + Vector3.up * collider.height/2f;
    }

    private void OnTriggerEnter(Collider other){
        if (other.GetComponent<Health>() != target) {
            return;
        }
        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}
