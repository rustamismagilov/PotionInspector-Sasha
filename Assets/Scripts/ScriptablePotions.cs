using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/ScriptablePotions")]
public class ScriptablePotions : ScriptableObject
{
    [Header("Base Information")]
    [TextArea(1, 3)]
    [SerializeField] private string potionName = "Potion name...";
    [Tooltip("Choose type based on color")][SerializeField] private PotionType potionType;
    // instead of using actual Color, use PotionColorType enum to define potion "color" (which is sprite)
    [SerializeField] private PotionColorType colorType;
    [SerializeField] private Sprite potionSprite;

    [Header("Customer/Merchant Name")]
    [Tooltip("Which alchemist made the potion")][SerializeField] private PotionMaker alchemist;

    [Header("Testable attributes")]
    [SerializeField] private List<AromaType.AromaOptions> aromas = new List<AromaType.AromaOptions>(3);
    [Tooltip("choose size + color of flame")][SerializeField] private GameObject flameType;
    [SerializeField] private Color flameColor;
    [TextArea(1, 3)][SerializeField] private string tasteEffect = "Describe the taste & effect of this potion...";

    private bool approvable;

    public string GetPotionName()
    {
        return potionName;
    }

    public Sprite GetPotionSprite()
    {
        return potionSprite;
    }

    public PotionType GetPotionType()
    {
        return potionType;
    }

    public PotionMaker GetPotionCreator()
    {
        return alchemist;
    }

    // Return the sprite for a specific potion color type
    public PotionColorType GetPotionColorType()
    {
        return colorType;
    }

    public List<AromaType.AromaOptions> GetAromas()
    {
        return aromas;
    }

    public GameObject GetFlamePrefab()
    {
        return flameType;
    }

    public Color GetFlameColor()
    {
        return flameColor;
    }

    public string GetTasteEffect()
    {
        return tasteEffect;
    }

    public bool IsApprovable()
    {
        return approvable;
    }
}
