using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
namespace RPG.Stats {
public class LevelDisplay : MonoBehaviour {
  BaseStats stats;

  private void Awake(){
    stats = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG).GetComponent<BaseStats>();
  }

  private void Update() {
    GetComponent<Text>().text = String.Format("{0}", stats.GetLevel());
  }
}
}