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
    PlayableDirector playableDirector;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG);
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable() {
        playableDirector.played += DisableControl;
        playableDirector.stopped += EnableControl;
    }

    private void OnDisable() {
        playableDirector.played -= DisableControl;
        playableDirector.stopped -= EnableControl;
    }
    
    private void Start() {

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