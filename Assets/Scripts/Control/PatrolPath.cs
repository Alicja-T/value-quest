using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control {
public class PatrolPath : MonoBehaviour
{

   private void OnDrawGizmos() {
    for (int i = 0; i < transform.childCount; i++) {
       Gizmos.color = Color.white;
       int j = (i + 1) % transform.childCount;
       Gizmos.DrawSphere(GetWaypoint(i), 0.3f);
       Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
    }
   }

   public Vector3 GetWaypoint(int i) {
     return transform.GetChild(i).position;
   }

   public int GetNextWaypoint(int i) {
      if (i + 1 == transform.childCount) {
         return 0;
      }
      else {
         return i + 1;
      }
   }



}

}
