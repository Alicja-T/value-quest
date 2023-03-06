using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
namespace RPG.Attributes {
public class ExperienceDisplay : MonoBehaviour {
  Experience experience;

  private void Awake(){
    experience = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG).GetComponent<Experience>();
  }

  private void Update() {
    GetComponent<Text>().text = String.Format("{0}", experience.GetExperience());
  }
}
}