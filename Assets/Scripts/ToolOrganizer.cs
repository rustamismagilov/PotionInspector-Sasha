using UnityEngine;

public class ToolOrganizer : MonoBehaviour
{
    [SerializeField] private RectTransform toolHolder;
    [SerializeField] private RectTransform toolMenu;
    [SerializeField] private GameObject minimizeButton;

    public void OnTriggerExit2D(Collider2D other)
    {
        if (!Application.isPlaying || toolMenu == null || toolHolder == null)
            return;

        if (!(
    other.CompareTag("Stamp") ||
    other.CompareTag("Dropper") ||
    other.CompareTag("Candle")))
        {
            return;
        }


        Debug.Log("Tool removed");

        var tool = other.gameObject;
        var toolScript = tool.GetComponent<ToolScript>();
        toolScript.ChangeState(true);

        if (tool.transform.parent != toolHolder)
        {
            tool.transform.SetParent(toolHolder, false);
        }

        var pos = tool.transform.position;

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!(
    other.CompareTag("Stamp") ||
    other.CompareTag("Dropper") ||
    other.CompareTag("Candle")))
        {
            return;
        }

        Debug.Log("Tool returned");

        var tool = other.gameObject;
        var toolScript = tool.GetComponent<ToolScript>();
        toolScript.ChangeState(false);

        if (tool.transform.parent != toolMenu)
        {
            tool.transform.SetParent(toolMenu, false);
        }

    }
}
