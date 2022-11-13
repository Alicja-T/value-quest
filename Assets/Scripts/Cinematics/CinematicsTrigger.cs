using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
namespace RPG.Cinematics {
public class CinematicsTrigger : MonoBehaviour
{   
    bool wasPlayed = false;
    private void OnTriggerEnter(Collider other) {
        if (!wasPlayed && other.gameObject.tag == CoreConstants.PLAYER_TAG) {
            GetComponent<PlayableDirector>().Play();
            wasPlayed = true;
        }
    }
}
}