using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
namespace RPG.Attributes {
public class HealthDisplay : MonoBehaviour {
  Health health;

  private void Awake(){
    health = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG).GetComponent<Health>();
  }

  private void Update() {
    GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
  }
}
}