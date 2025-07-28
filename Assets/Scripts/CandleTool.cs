using System;
using UnityEngine;
using UnityEngine.UI;

public class CandleTool : MonoBehaviour
{
    private GameObject flamePrefab;
    private Color flameColor;

    private PotionManager potionManager;
    private DropperTool dropper;
    // The position where the flame will be spawned
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
        // ensure we have the flame prefab and color set, otherwise return
        if (!dropper.UsingDropper() || !dropper.GotDrop())
            return;

        // prevent multiple flames from being spawned
        if (currentFlame != null)
        {
            Debug.Log("Flame already exists. Skipping spawn of the new one.");
            return;
        }

        // otherwise, spawn the flame
        currentFlame = Instantiate(flamePrefab, flameSpawnPos.position, Quaternion.identity, flameSpawnPos);

        // set the flame color by getting the Image component in the flame prefab and then changing its color
        var flameImage = currentFlame.GetComponentInChildren<Image>();
        if (flameImage != null)
        {
            flameImage.color = flameColor;
        }
    }
}
