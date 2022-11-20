using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Core;
namespace RPG.SceneManagement {
public class Portal : MonoBehaviour {

    enum DestinationIdentifier {
        A, B, C, D, E
    }

    [SerializeField] int sceneIndex = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destination;
    [SerializeField] float fadeOutTime = 0.5f;
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float waitTime = 0.5f;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == CoreConstants.PLAYER_TAG) {
            StartCoroutine(Transition());
        }    
    }

    private IEnumerator Transition() {
         DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        
        print("Scene loaded");
        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
        
        yield return new WaitForSeconds(waitTime);
        yield return fader.FadeIn(fadeInTime);
        Destroy(gameObject);
    }
    private void UpdatePlayer(Portal otherPortal) {
        GameObject player = GameObject.FindGameObjectWithTag(CoreConstants.PLAYER_TAG);
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        //player.transform.position = otherPortal.spawnPoint.position;
        player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal() {
        foreach (Portal portal in FindObjectsOfType<Portal>()) {
            if (portal == this) continue;
            if (portal.destination != destination) continue;
            return portal;
        }
        return null;
    }
    
    
    }

    
}//namespace