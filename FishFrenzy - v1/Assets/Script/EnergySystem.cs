using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    public int maxEnergy = 5;
    public int currentEnergy = 5;

    public Image[] energyImages;

    private float runTimer = 0f;
    private float wrongEnvironmentTimer = 0f;

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateEnergyUI();
    }

    public bool HasEnergy()
    {
        return currentEnergy > 0;
    }

    public void UseRunningEnergy(float deltaTime)
    {
        runTimer += deltaTime;

        if (runTimer >= 1f)
        {
            LoseEnergy(1);
            runTimer = 0f;
        }
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateEnergyUI();
    }

    void LoseEnergy(int amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        UpdateEnergyUI();
    }

    void UpdateEnergyUI()
    {
        for (int i = 0; i < energyImages.Length; i++)
        {
            energyImages[i].gameObject.SetActive(i < currentEnergy);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("collectible")) return;

        AddEnergy(1);

        other.gameObject.SetActive(false);

        Debug.Log("Collected energy fish. Energy: " + currentEnergy);
    }

    public void UseWrongEnvironmentEnergy(float deltaTime)
    {
        wrongEnvironmentTimer += deltaTime;

        if (wrongEnvironmentTimer >= 2f)
        {
            wrongEnvironmentTimer = 0f;

            if (currentEnergy > 0)
            {
                currentEnergy--;
                UpdateEnergyUI();

                Debug.Log("Lost 1 energy from wrong environment");
            }
        }
    }

    public void ResetWrongEnvironmentTimer()
    {
        wrongEnvironmentTimer = 0f;
    }
}
