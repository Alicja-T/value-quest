using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;
namespace RPG.Cinematics {
public class CinematicsControlRemover : MonoBehaviour
{
     GameObject player;
    private void Start() {
        player = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG);
        PlayableDirector playableDirector = GetComponent<PlayableDirector>();
        playableDirector.played += DisableControl;
        playableDirector.stopped += EnableControl;
    }
   void DisableControl(PlayableDirector pDirector) {
    player.GetComponent<ActionScheduler>().CancelCurrentAction();
    player.GetComponent<PlayerController>().enabled = false;
    print("disabled control");
   }
   void EnableControl(PlayableDirector pDirector) {
    print("enabled control");
    player.GetComponent<PlayerController>().enabled = true;
   }
}
}