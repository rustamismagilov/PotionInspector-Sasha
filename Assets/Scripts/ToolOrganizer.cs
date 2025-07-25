using UnityEngine;

public class ToolOrganizer : MonoBehaviour
{
    [SerializeField] private Transform toolHolder;
    [SerializeField] private Transform toolMenu;
    [SerializeField] private GameObject minimizeButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (!Application.isPlaying || toolMenu == null || toolHolder == null) 
            return;

        if (!other.CompareTag("Tool"))
            return;

        Debug.Log("Tool removed");

        PrintAllToolStates();

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
        if (!other.CompareTag("Tool"))
            return;

        Debug.Log("Tool returned");

        PrintAllToolStates();

        var tool = other.gameObject;
        var toolScript = tool.GetComponent<ToolScript>();
        toolScript.ChangeState(false);

        if (tool.transform.parent != toolMenu)
        {
            tool.transform.SetParent(toolMenu, false);
            //minimizeButton.transform.SetAsLastSibling();
        }
    }

    public void PrintAllToolStates()
    {
        var tools = GameObject.FindGameObjectsWithTag("Tool");
        foreach (var tool in tools)
        {
            var toolScript = tool.GetComponent<ToolScript>();
            Debug.Log($"{tool.name} | OnDesk: {toolScript.OnDesk()} | Parent: {tool.transform.parent.name}");
        }
    }
}
