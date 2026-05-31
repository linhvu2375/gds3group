using UnityEngine;
using UnityEngine.UI;

public class FishSwitchManager : MonoBehaviour
{
    [Header("Fish Children")]
    public GameObject[] fishes;

    [Header("UI Buttons")]
    public Button switchButton;
    public Button[] fishOptionButtons;

    [Header("Optional UI Text")]
    public Text fishNameText;

    private int currentIndex = 0;
    private bool optionsVisible = false;

    [Header("Sea Zone")]
    public SeaLevelsManager seaZoneManager;

    void Start()
    {
        if (switchButton != null)
            switchButton.onClick.AddListener(ToggleOptions);

        for (int i = 0; i < fishOptionButtons.Length; i++)
        {
            int index = i;

            if (fishOptionButtons[i] != null)
                fishOptionButtons[i].onClick.AddListener(() => SelectFish(index));
        }

        SetOptionsVisible(false);
        ShowFish(currentIndex);
    }

    void ToggleOptions()
    {
        optionsVisible = !optionsVisible;
        SetOptionsVisible(optionsVisible);
    }

    void SetOptionsVisible(bool visible)
    {
        for (int i = 0; i < fishOptionButtons.Length; i++)
        {
            if (fishOptionButtons[i] != null)
                fishOptionButtons[i].gameObject.SetActive(visible);
        }
    }

    void SelectFish(int index)
    {
        ShowFish(index);

        optionsVisible = false;
        SetOptionsVisible(false);
    }

    void ShowFish(int index)
    {
        currentIndex = index;

        UpdateSeaZoneFishType(currentIndex);

        for (int i = 0; i < fishes.Length; i++)
        {
            if (fishes[i] != null)
                fishes[i].SetActive(i == currentIndex);
        }

        if (fishNameText != null && fishes[currentIndex] != null)
            fishNameText.text = fishes[currentIndex].name;
    }

    void UpdateSeaZoneFishType(int index)
    {
        if (seaZoneManager == null) return;

        if (index == 0)
            seaZoneManager.fishType = SeaLevelsManager.FishType.Anglerfish;
        else if (index == 1)
            seaZoneManager.fishType = SeaLevelsManager.FishType.Frogfish;
        else if (index == 2)
            seaZoneManager.fishType = SeaLevelsManager.FishType.Shark;

        seaZoneManager.UpdateActiveAbility();
    }
}