using System.ComponentModel;
using GameDevTV.Inventories;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories {
public class RandomDropper : ItemDropper {
    [Tooltip("How far the items are scattered from the center")]
    [SerializeField] float scatterDistance = 1;
    [SerializeField] InventoryItem[] dropLibrary;

    const int ATTEMPTS = 20;

    public void RandomDrop() {
        var numItems = Mathf.FloorToInt(Random.value * 10);
        for (int i = 0; i < numItems; i++) {
            var itemsNum = Mathf.FloorToInt(Random.value * 10);
            var itemIndex = Mathf.FloorToInt(Random.Range(0, dropLibrary.Length));
            DropItem(dropLibrary[itemIndex], itemsNum);
        }
    }
    protected override Vector3 GetDropLocation() {
        for (int i = 0; i < ATTEMPTS; i++) {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas)) {
                return hit.position;
            }
        }
        return base.GetDropLocation();
    }

} // class
} // namespace