using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    [SerializeField] private GameObject light2d;

    public bool startFlicking = true;
    private EnemyTeleport enemyTeleport;

    // Start is called before the first frame update
    void Start()
    {
        enemyTeleport = FindObjectOfType<EnemyTeleport>();

        StartCoroutine(FlickerTimerLoop());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator FlickerTimerLoop()
    {
        while (true)
        {
            // Wait exactly 1 second before rolling again (fixes FPS differences!)
            yield return new WaitForSeconds(1.0f);

            if (startFlicking)
            {
                // Rolls a number once per second. Adjust the max number to make it more/less rare
                int flicker = Random.Range(1, 30); 

                if (flicker == 1)
                {
                    light2d.SetActive(false);
                    
                    // Teleport enemy immediately when lights go dark
                    if (enemyTeleport != null) enemyTeleport.TeleportToRandomPoint();
                    
                    // Pauses this loop until WaitTime finishes completely
                    yield return StartCoroutine(WaitTime());
                }
                else if (flicker == 2)
                {
                    light2d.SetActive(true);
                    
                    // Pauses this loop until WaitTime2 finishes completely
                    yield return StartCoroutine(WaitTime2());
                }
            }
        }
    }

    IEnumerator WaitTime()
    {
        startFlicking = false;
        yield return new WaitForSecondsRealtime(2.0f);
        light2d.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        startFlicking = true;

        yield return null;
    }

    IEnumerator WaitTime2()
    {
        startFlicking = false;
        yield return new WaitForSecondsRealtime(2.0f);
        light2d.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        startFlicking = true;

        yield return null;
    }
}
