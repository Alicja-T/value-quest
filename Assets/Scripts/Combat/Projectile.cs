using UnityEngine;
using UnityEngine.Events;
using RPG.Attributes;

namespace RPG.Combat{
public class Projectile : MonoBehaviour
{
    
    [SerializeField] float speed = 1f;
    [SerializeField] bool isHoming = false;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float maxLifeTime = 10f;

    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterImpact = 2f;
    Health target = null;
    float damage = 0f;
    GameObject instigator = null;
    
    Vector3 shootingDirection; 

    [SerializeField] UnityEvent OnHit;

    void Start() {
        transform.LookAt(GetAimLocation());
    }

    void Update()
    {   
        if (target == null) return;
        if (isHoming && !target.IsDead()) {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward*Time.deltaTime*speed);
        
    }

    public void SetTarget(GameObject instigator, Health newTarget, float damage) {
        this.target = newTarget;
        this.damage = damage;
        this.instigator = instigator;
        Destroy(gameObject, maxLifeTime);
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
        if (target.IsDead()) return;
        target.TakeDamage(instigator, damage);
        speed = 0;
        OnHit.Invoke();
        if (hitEffect != null) {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }

        foreach( GameObject toDestroy in destroyOnHit) {
            Destroy(toDestroy);
        }
        Destroy(gameObject, lifeAfterImpact);
    }
}
}