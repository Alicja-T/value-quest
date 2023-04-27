using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
using RPG.Attributes;
namespace RPG.Combat {
public class EnemyHealthDisplay : MonoBehaviour {
  Fighter fighter;

  private void Awake(){
    fighter = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG)
                        .GetComponent<Fighter>();
  }

  private void Update() {
    Health target = fighter.GetTarget();
    if (target == null) {
      GetComponent<Text>().text = "N/A";
    }
    else {
      GetComponent<Text>().text = String.Format("{0:0}/{1:0}", target.GetHealthPoints(), target.GetMaxHealthPoints());
    }
    
  }
}
}