using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnItem : MonoBehaviour
{
    [SerializeField] private GameObject[] itemsToTeleport;
    [SerializeField] private Transform[] transformsToTeleport;

    // This runs automatically as soon as you press the Play button!
    void Start()
    {
        Activate();
    }

    public void Activate()
    {   
        // Safety Check
        if (transformsToTeleport == null || transformsToTeleport.Length == 0 || itemsToTeleport == null || itemsToTeleport.Length == 0)
        {
            Debug.LogWarning("Make sure both Items and Transforms arrays are filled in the Inspector!");
            return;
        }

        // Create a list of available positions so we can keep track of them
        List<Transform> availablePositions = new List<Transform>(transformsToTeleport);

        // Go through each item one by one and give it a random position
        foreach (GameObject item in itemsToTeleport)
        {
            if (item == null) continue;

            // If we run out of spots, stop teleporting items
            if (availablePositions.Count == 0)
            {
                Debug.LogWarning("Ran out of unique spawn positions for all items!");
                break;
            }

            // Pick a random index from our remaining available positions list
            int randomIndex = Random.Range(0, availablePositions.Count);
            Transform chosenPosition = availablePositions[randomIndex];

            // Teleport the item to that spot!
            item.transform.position = chosenPosition.position;

            // Remove this spot from the list so the NEXT object cannot choose it
            availablePositions.RemoveAt(randomIndex);
        }
    }
}
