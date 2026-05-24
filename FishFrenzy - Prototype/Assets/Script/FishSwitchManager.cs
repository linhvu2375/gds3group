using UnityEngine;
using UnityEngine.UI;

public class FishSwitchManager : MonoBehaviour
{
    public GameObject[] fishes;
    public Transform playerCamera;
    public Button switchButton;
    public Text fishNameText;

    private int currentIndex = 0;

    void Start()
    {
        if (switchButton != null)
            switchButton.onClick.AddListener(SwitchFish);

        ShowFish(currentIndex);
    }

    void SwitchFish()
    {
        currentIndex++;

        if (currentIndex >= fishes.Length)
            currentIndex = 0;

        ShowFish(currentIndex);
    }

    void ShowFish(int index)
    {
        // Move camera to the new active fish BEFORE disabling others
        if (playerCamera != null)
        {
            playerCamera.SetParent(fishes[index].transform);
            playerCamera.localPosition = new Vector3(0f, 1f, -3f);
            playerCamera.localRotation = Quaternion.identity;
        }

        for (int i = 0; i < fishes.Length; i++)
        {
            fishes[i].SetActive(i == index);
        }

        if (fishNameText != null)
            fishNameText.text = fishes[index].name;
    }
}