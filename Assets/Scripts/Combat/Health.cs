using UnityEngine;

namespace RPG.Combat {
public class Health : MonoBehaviour {
   
    [SerializeField] float health = 100f;

       // Update is called once per frame

    public void TakeDamage(float damage) {
       health = Mathf.Max(health-damage,0);
       print(health);
    }
 
}//class
}//namespace RPG.Combat