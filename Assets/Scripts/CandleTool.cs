using System;
using UnityEngine;
using UnityEngine.UI;

public class CandleTool : MonoBehaviour
{
    private GameObject flamePrefab;
    private Color flameColor;

    private PotionManager potionManager;
    private DropperTool dropper;
    [SerializeField] Transform flameSpawnPos;
    private GameObject currentFlame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        potionManager = FindFirstObjectByType<PotionManager>();
        dropper = FindFirstObjectByType<DropperTool>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetFlameInfo()
    {
        flamePrefab = potionManager.CurrentPotion().GetFlamePrefab();
        flameColor = potionManager.CurrentPotion().GetFlameColor();
    }

    public void IgniteFlame()
    {
        if (!dropper.UsingDropper() || !dropper.GotDrop())
            return;

        if (currentFlame != null)
        {
            Debug.Log("Flame already exists. Skipping spawn of the new one.");
            return;
        }

        currentFlame = Instantiate(flamePrefab, flameSpawnPos.position, Quaternion.identity, flameSpawnPos);

        var flameImage = currentFlame.GetComponentInChildren<Image>();
        if (flameImage != null)
        {
            flameImage.color = flameColor;
        }
    }
}
