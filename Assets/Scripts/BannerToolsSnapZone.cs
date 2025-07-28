using UnityEngine;

// this script is mostly ToolOrganizer + some new functionality
// should be assigned to every new part of the banner where tools can snap to (e.g. top, bottom, etc.)
public class BannerToolsSnapZone : MonoBehaviour
{
    // dropdown menu to define the location of the snap zone
    public enum ZoneLocation { Top, Mid, Bottom }
    // defines the zone location
    public ZoneLocation zone;

    // tag that must be assigned to the tool in order to be allowed in this zone
    public string allowedToolTag = "";
    // snap target for the tools to snap to
    public RectTransform snapTarget;

    [SerializeField] private RectTransform toolHolder;
    [SerializeField] private RectTransform toolMenu;
    [SerializeField] private GameObject minimizeButton;

    // currently hovered tool
    private GameObject hoveredTool;
    // current zone that the hovered tool is in
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
        // if hoveredTool is not null and currentZone is not null...
        if (hoveredTool != null && currentZone != null)
        {
            // ...check for mouse button up events
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                // if the tool is not already in the snap target, snap it
                hoveredTool.transform.SetParent(currentZone.snapTarget, false);
                hoveredTool.transform.localPosition = Vector3.zero;
                Debug.Log($"Snapped {hoveredTool.name} to {currentZone.zone}");

                // reset hoveredTool and currentZone to interact with another tool
                hoveredTool = null;
                currentZone = null;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        // if the scene is not loaded, exit early
        if (gameObject.scene.isLoaded == false)
            return;

        // check if the other collider has a tag matching any of the allowed tools
        if (!(
    other.CompareTag("Stamp") ||
    other.CompareTag("Dropper") ||
    other.CompareTag("Candle")))
        {
            return;
        }

        Debug.Log("Tool removed");

        // get reference to the tool game object
        var tool = other.gameObject;

        // call ChangeState(true) to mark tool as inactive (or "not in snap zone")
        var toolScript = tool.GetComponent<ToolScript>();
        toolScript.ChangeState(true);

        // if the tool is not already parented to toolHolder, reparent it
        if (tool.transform.parent != toolHolder)
        {
            tool.transform.SetParent(toolHolder, false);
        }

        // store tools position
        var pos = tool.transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // ignore collisions with anything except the valid tool tags
        if (!(
    other.CompareTag("Stamp") ||
    other.CompareTag("Dropper") ||
    other.CompareTag("Candle")))
        {
            return;
        }

        Debug.Log("Tool returned");

        // get reference to the BannerToolsSnapZone script on this GameObject
        if (TryGetComponent(out BannerToolsSnapZone zone))
        {
            // only allow tool if its tag matches the allowedToolTag for this zone
            if (other.CompareTag(zone.allowedToolTag))
            {
                // set hoveredTool to the entered tool
                hoveredTool = other.gameObject;

                // store current snap zone to allow snapping on mouse release
                currentZone = zone;

                // call ChangeState(false) to mark tool as active (or "snapped in")
                var toolScript = other.GetComponent<ToolScript>();
                toolScript.ChangeState(false);
            }
        }
    }
}
