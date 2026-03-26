using UnityEngine;

[CreateAssetMenu(fileName = "New Page", menuName = "Guidebook Page")]
public class PageSO : ScriptableObject
{
    [Tooltip("Determines order of pages within book")] [SerializeField] private int pageIndex;
    
    [Header("Left Page")]
    [TextArea(1, 2)] 
    [SerializeField] private string titleL = "Left page title...";
    
    [TextArea(2, 10)]
    [SerializeField] private string bodyL = "Left page body text...";
    
    [Header("Right Page")]
    [TextArea(1, 2)] 
    [SerializeField] private string titleR = "Right page title...";

    [TextArea(2, 10)] 
    [SerializeField] private string bodyR = "Right page body text...";


    public int GetPageNumber()
    {
        return pageIndex;
    }
    public string GetLeftTitle()
    {
        return titleL;
    }

    public string GetLeftBody()
    {
        return bodyL;
    }

    public string GetRightTitle()
    {
        return titleR;
    }

    public string GetRightBody()
    {
        return bodyR;
    }



}
