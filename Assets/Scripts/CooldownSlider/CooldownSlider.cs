using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CooldownSlider : MonoBehaviour
{
    public Slider slider;
    public Slider slider2;
    
    // Set how many seconds you want the full bar to last (e.g., 300 seconds = 5 minutes)
    [SerializeField] private float totalDurationInSeconds = 1000f; 
    public bool ifRunning = false;
    public bool ifOn = false;

    private Coroutine timeStopRoutine;

    void Start()
    {
        slider.value = 100;
        slider2.value = 100;

        ifOn = true;

        start();
    }

    public void start()
    {
        if (timeStopRoutine != null) StopCoroutine(timeStopRoutine);
        timeStopRoutine = StartCoroutine(TimeStops());
    }

    IEnumerator TimeStops()
    {
        // This calculates exactly how much to subtract per second based on your total duration
        float drainSpeed = 100f / totalDurationInSeconds;

        while (slider.value > 0 && ifOn || slider2.value > 0 && ifRunning || slider.value > 0 && ifOn && slider2.value > 0 && ifRunning)
        {
            if (ifOn && ifRunning)
            {
                slider.value -= drainSpeed * Time.deltaTime;
                slider2.value -= drainSpeed * Time.deltaTime;
            }
            else if (ifOn)
            {
                slider.value -= drainSpeed * Time.deltaTime;
            }
            else if (ifRunning)
            {
                slider2.value -= drainSpeed * Time.deltaTime;
            }

            
            
            yield return null;
        }
        
        timeStopRoutine = null;
    }
}
