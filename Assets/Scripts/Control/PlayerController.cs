using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AI;
using UnityEngine.Experimental.UIElements;

namespace RPG.Control {
  public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    Health health;
    [SerializeField]
    float maxDistance = 0.5f;
    [SerializeField]
    float maxPathLength = 40f;
    [System.Serializable]
    struct CursorMapping {
      public CursorType type;
      public Vector2 hotspot;
      public Texture2D texture;
    }

    [SerializeField] CursorMapping[] cursorMappings = null;
    
    bool isDraggingUI = false;
    private void Awake() {
      health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update() {
      if (InteractWithUI()) {
        return;
      }
      if (health.IsDead()) {
        SetCursor(CursorType.None);
        return;
      }
      if (InteractWithComponent()){
        return;
      }

      if (InteractWithMovement()) {
        return;
      }
      SetCursor(CursorType.None);
    }

    private bool InteractWithComponent() {
      RaycastHit[] hits = GetSortedHits();
      foreach (RaycastHit hit in hits) {
        IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
        foreach (IRaycastable raycastable in raycastables) {
          if (raycastable.handleRaycast(this)) {
            SetCursor(raycastable.GetCursorType());
            return true;
          }
        }
      }
      return false;
    }

    private static RaycastHit[] GetSortedHits() {
      RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
      float[] distances = new float[hits.Length];
      for (int i = 0; i < hits.Length; i++) {
        distances[i] = hits[i].distance;
      }
      Array.Sort(distances, hits);
      return hits;
    }

    private bool InteractWithUI()
    { 
      if (Input.GetMouseButtonUp(0)) {
        isDraggingUI = false;
      }
      bool debug = EventSystem.current.IsPointerOverGameObject();
      if (debug) {
        if (Input.GetMouseButtonDown(0)) {
          isDraggingUI = true;
        }
        SetCursor(CursorType.UI);
      }
      if (isDraggingUI) {
        return true;
      }
      return debug;
    }

    private void SetCursor(CursorType type)
    {
      CursorMapping mapping = GetCursorMapping(type);
      Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
    }

    private CursorMapping GetCursorMapping(CursorType type) {
      foreach (CursorMapping mapping in cursorMappings) {
        if (mapping.type == type) {
          return mapping;
        }
      }
      return cursorMappings[0];
    }
    private bool InteractWithMovement() {

     
      Vector3 target = new Vector3();
      bool hasHit = RaycastNavMesh(out target);
      if (hasHit) {
        if (Input.GetMouseButton(0)) {
          GetComponent<Mover>().StartMoveAction(target, 1f);
        }
        SetCursor(CursorType.Movement);
        return true;     
      }
      return false;
    }

    private bool RaycastNavMesh(out Vector3 target){ 
      RaycastHit hit;
      target = new Vector3();
      bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
      NavMeshHit navMeshHit;
      if (!hasHit) return false;
      bool hasCastToMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, 
        maxDistance, NavMesh.AllAreas);
      if (!hasCastToMesh) return false;
      NavMeshPath path = new NavMeshPath();
      target = navMeshHit.position;
      bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
      if (!hasPath) return false;
      if (path.status != NavMeshPathStatus.PathComplete) return false;
      if (GetPathLength(path) > maxPathLength) return false;
      return true;
      
      
    }

    private float GetPathLength(NavMeshPath path) {
      Vector3[] corners = path.corners;
      float distance = 0;
      for (int i = 1; i < corners.Length; i++) {
        distance += Vector3.Distance(corners[i-1], corners[i]);
      }
      return distance;
    }

    private static Ray GetMouseRay() {
      return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
  }

} //namespace RPG.Control