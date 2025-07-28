using UnityEngine;

public class BannerToolsSnapZone : MonoBehaviour
{
    public enum ZoneLocation { Top, Mid, Bottom }
    public ZoneLocation zone;

    public string allowedToolTag = "";
    public RectTransform snapTarget;

    [SerializeField] private RectTransform toolHolder;
    [SerializeField] private RectTransform toolMenu;
    [SerializeField] private GameObject minimizeButton;

    private GameObject hoveredTool;
    private BannerToolsSnapZone currentZone;

    void Start()
    {
        // find all tools in the scene only with allowed tags
        GameObject[] tools = GameObject.FindGameObjectsWithTag(allowedToolTag);

        // snap them to their respective positions
        foreach (GameObject tool in tools)
        {
            tool.transform.SetParent(snapTarget, false);
            tool.transform.localPosition = Vector3.zero;

            Debug.Log($"Snapped {tool.name} to {zone} at start");
        }
    }

    void Update()
    {
        if (hoveredTool != null && currentZone != null)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                hoveredTool.transform.SetParent(currentZone.snapTarget, false);
                hoveredTool.transform.localPosition = Vector3.zero;
                Debug.Log($"Snapped {hoveredTool.name} to {currentZone.zone}");

                hoveredTool = null;
                currentZone = null;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.scene.isLoaded == false)
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

        if (TryGetComponent(out BannerToolsSnapZone zone))
        {
            if (other.CompareTag(zone.allowedToolTag))
            {
                hoveredTool = other.gameObject;
                currentZone = zone;

                var toolScript = other.GetComponent<ToolScript>();
                toolScript.ChangeState(false);
            }
        }
    }
}
