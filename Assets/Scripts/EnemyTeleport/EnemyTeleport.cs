using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleport : MonoBehaviour
{
    public Transform[] teleportPoints;
    private EnemyPentrol patrolScript;

    // Start is called before the first frame update
    void Start()
    {
        patrolScript = GetComponent<EnemyPentrol>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeleportToRandomPoint()
    {
        if (teleportPoints == null || teleportPoints.Length == 0) return;

        // 1. Pick a random index and teleport
        int randomIndex = Random.Range(0, teleportPoints.Length);
        Vector3 targetPosition = teleportPoints[randomIndex].position;
        transform.position = targetPosition;

        if (patrolScript != null)
        {
            patrolScript.ResetAIOnTeleport();
        }

        // 2. FIND THE CLOSEST POINT (Fixed Loop & Logic)
        float closestPointDistance = Mathf.Infinity; 
        Transform closestPointTransform = null;

        

        // FIX: The loop condition must check against 'teleportPoints.Length'
        for (int i = 0; i < teleportPoints.Length; i++)
        {
            // Skip checking the exact point we just teleported to
            if (i == randomIndex) continue; 

            float distance = Vector2.Distance(transform.position, teleportPoints[i].position);

            if (distance < closestPointDistance)
            {
                closestPointDistance = distance;
                closestPointTransform = teleportPoints[i]; // Stores the actual closest spot
            }
        }

        // Now 'closestPointTransform' holds the closest point if you want to use it!
        if (closestPointTransform != null)
        {
            Debug.Log("The closest point to my new position is: " + closestPointTransform.name);
        }

        
    }
}
