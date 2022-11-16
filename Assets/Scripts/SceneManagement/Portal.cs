using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Core;
namespace RPG.SceneManagement {
public class Portal : MonoBehaviour {
    [SerializeField] int sceneIndex = -1;
    [SerializeField] Transform spawnPoint;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == CoreConstants.PLAYER_TAG) {
            StartCoroutine(Transition());
        }    
    }

    private IEnumerator Transition() {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        print("Scene loaded");
        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
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
            return portal;
        }
        return null;
    }
    
    
    }

    
}//namespace