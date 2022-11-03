using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control {
public class PatrolPath : MonoBehaviour
{
   private void OnDrawGizmos() {
    for (int i = 0; i < transform.childCount; i++) {
       Transform childTransform = transform.GetChild(i);
       Gizmos.color = Color.white;
       Gizmos.DrawSphere(childTransform.position, 0.3f);
    }
   }
}

}
