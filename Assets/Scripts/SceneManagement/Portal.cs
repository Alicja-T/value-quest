﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Core;
namespace RPG.SceneManagement {
public class Portal : MonoBehaviour {
    [SerializeField] int sceneIndex = -1;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == CoreConstants.PLAYER_TAG) {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }
    }
}
}//namespace