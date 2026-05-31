using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapCheckpointSystem : MonoBehaviour
{
    public int totalLaps = 3;

    public TMP_Text lapText;
    public TMP_Text finishText;

    private GameObject[] checkpoints;
    private int checkpointCount = 0;
    private int currentLap = 1;

    //[Header("Collectibles")]
    //public GameObject[] energyFish;

    void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("checkpoint");

        if (finishText != null)
            finishText.gameObject.SetActive(false);

        UpdateUI();

        Debug.Log("Checkpoints found: " + checkpoints.Length);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("checkpoint")) return;

        checkpointCount++;
        Debug.Log("Checkpoint hit: " + checkpointCount + "/" + checkpoints.Length);

        other.gameObject.SetActive(false);

        if (checkpointCount >= checkpoints.Length)
        {
            CompleteLap();
        }
    }

    void CompleteLap()
    {
        currentLap++;
        checkpointCount = 0;

        ReactivateCheckpoints();

        ReactivateEnergyFish();

        if (currentLap > totalLaps)
        {
            FinishRace();
        }
        else
        {
            UpdateUI();
            Debug.Log("Lap started: " + currentLap + "/" + totalLaps);
        }
    }

    void ReactivateCheckpoints()
    {
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.SetActive(true);
        }
    }

    void UpdateUI()
    {
        if (lapText != null)
            lapText.text = "Lap: " + currentLap + "/" + totalLaps;
    }

    void FinishRace()
    {
        Debug.Log("FINISH!");

        if (finishText != null)
        {
            finishText.gameObject.SetActive(true);
            finishText.text = "FINISH!";
        }
    }

    void ReactivateEnergyFish()
    {
        GameObject[] collectibles =
            Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in collectibles)
        {
            if (obj.CompareTag("collectible"))
            {
                obj.SetActive(true);
            }
        }
    }
}