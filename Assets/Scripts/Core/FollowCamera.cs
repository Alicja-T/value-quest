using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] Transform target;
    void LateUpdate()
    {
        transform.position = target.transform.position;

    }
}
