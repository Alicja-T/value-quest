﻿using RPG.Core;
using GameDevTV.Saving;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement {
  public class Mover : MonoBehaviour, IAction, ISaveable {

    [SerializeField] Transform target;
    [SerializeField] float maxSpeed = 6f;
    Health health;
    NavMeshAgent navMeshAgent;
    void Awake() {
      navMeshAgent = GetComponent<NavMeshAgent>();
      health = GetComponent<Health>();
    }

    void Update() {
      navMeshAgent.enabled = !health.IsDead();
      UpdateAnimator();
    }

    public void StartMoveAction(Vector3 destination, float speedFraction) {
      GetComponent<ActionScheduler>().StartAction(this);
      MoveTo(destination, speedFraction);
    }

    public void MoveTo(Vector3 destination, float speedFraction) {
      navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
      navMeshAgent.destination = destination;
      navMeshAgent.isStopped = false;
    }

    public void Cancel(){
      navMeshAgent.isStopped = true;
    }
    
    private void UpdateAnimator() {
      Vector3 velocity = navMeshAgent.velocity;
      Vector3 localVelocity = transform.InverseTransformDirection(velocity);
      float speed = localVelocity.z;
      GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
    }
    
    [System.Serializable]
    struct MoverState {
      public SerializableVector3 position;
      public SerializableVector3 rotation;
    }
    public object CaptureState() {
      MoverState data = new MoverState();
      data.position = new SerializableVector3(transform.position);
      data.rotation = new SerializableVector3(transform.eulerAngles);
      return data;
    }

    public void RestoreState(object state) {
      MoverState data = (MoverState)state;
      //TO_DO check if position is from the same scene to get rid of warning
      navMeshAgent.enabled = false;
      transform.position = data.position.ToVector();
      //transform.eulerAngles = data.rotation.ToVector();
      navMeshAgent.enabled = true;
    }
  }

} //RPG.Movement
