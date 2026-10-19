using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficultys : MonoBehaviour
{
    public static float playerSpeed = 5f;
    public static float chosenSpeed = 4f; 
    public static float chosenChaseSpeed = 8f;
    public static float detectionRange = 8f;
    public static float loseRange = 10f;

    [SerializeField] private GameObject[] texts;

    // Start is called before the first frame update
    void Start()
    {
        MediumDifficulty(); // Set default difficulty to medium.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDifficultyChanged(int index)
    {
        switch (index)
        {
            case 0:
                EasyDifficulty();
                break;
            case 1:
                MediumDifficulty();
                break;
            case 2:
                HardDifficulty();
                break;
        }
    }

    public void EasyDifficulty()
    {
        if (texts != null)
        {
            texts[0].SetActive(true);
            texts[1].SetActive(false);
            texts[2].SetActive(false);
        }

        chosenSpeed = 4f;
        chosenChaseSpeed = 5f;
        detectionRange = 5f;
        loseRange = 7f;
    }

    public void MediumDifficulty()
    {
        if (texts != null)
        {
            texts[1].SetActive(true);
            texts[0].SetActive(false);
            texts[2].SetActive(false);
        }

        chosenSpeed = 6f;
        chosenChaseSpeed = 7f;
        detectionRange = 7f;
        loseRange = 9f;
    }

    public void HardDifficulty()
    {
        if (texts != null)
        {
            texts[2].SetActive(true);
            texts[1].SetActive(false);
            texts[0].SetActive(false);
        }

        chosenSpeed = 8f;
        chosenChaseSpeed = 9f;
        detectionRange = 9f;
        loseRange = 11f;
    }

}
