using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropperTool : MonoBehaviour
{
    [SerializeField] private float dropperFollowSpeed = 0.5f;
    [SerializeField] private GameObject droplet;

    [SerializeField] private GraphicRaycaster uiRaycaster;

    // event system reference for UI interactions
    [SerializeField] private EventSystem eventSystem;

    private ToolScript thisTool;

    private PotionManager potionManager;

    // is the dropper currently being used? (needed for RMB interaction)
    private bool usingDropper = false;

    private Vector3 mousePosition;

    // does the dropper currently hold a droplet?
    private bool gotDrop = false;

    // used to check if dropper has a droplet
    public bool GotDrop()
    {
        return gotDrop;
    }

    // used by other scripts to know if dropper is active
    public bool UsingDropper()
    {
        return usingDropper;
    }

    // assign references and hide droplet as Image (not the entire GameObject) at start
    void Start()
    {
        if (uiRaycaster == null)
            uiRaycaster = FindFirstObjectByType<GraphicRaycaster>();

        if (eventSystem == null)
            eventSystem = FindFirstObjectByType<EventSystem>();

        thisTool = GetComponent<ToolScript>();
        potionManager = FindFirstObjectByType<PotionManager>();

        // instead of disabling the entire droplet GameObject, just disable (and the re-enable) its Image component
        droplet.GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        // toggle dropper with right-click
        if (Input.GetMouseButtonDown(1))
        {
            // if already using the dropper, disable it
            if (usingDropper)
            {
                ToggleDropper();
            }
            else
            {
                // if right-clicked on this dropper while it's on the desk, enable it
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject && thisTool.OnDesk())
                {
                    ToggleDropper();
                }
            }
        }

        // dropper follows the mouse and checks for left click
        if (usingDropper)
        {
            // update dropper position to follow mouse with smoothing
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, dropperFollowSpeed);

            // handle left-click while using dropper
            if (Input.GetMouseButtonDown(0))
            {
                // prepare pointer data for UI raycast (to check how many things are in front of raycast)
                PointerEventData pointerData = new PointerEventData(eventSystem);
                pointerData.position = Input.mousePosition;

                // perform UI raycast
                var results = new List<RaycastResult>();
                uiRaycaster.Raycast(pointerData, results);

                // iterate through all UI hits
                foreach (var result in results)
                {
                    Debug.Log("UI Hit: " + result.gameObject.name);

                    // clicked on a potion = get droplet color
                    if (result.gameObject.CompareTag("Potion"))
                    {
                        GetDropletColor();
                        break;
                    }

                    // clicked on a candle with droplet = ignite the flame
                    if (result.gameObject.CompareTag("Candle") && GotDrop())
                    {
                        Debug.Log("Candle clicked with dropper and droplet active");
                        result.gameObject.GetComponent<CandleTool>()?.IgniteFlame();
                        break;
                    }
                }

                // no valid UI object found under mouse
                if (results.Count == 0)
                {
                    Debug.Log("UI raycast found nothing.");
                }
            }
        }
    }

    // toggle the dropper mode and handle its collider/image state
    private void ToggleDropper()
    {
        // flips the boolean value of usingDropper between true and false each time it's called
        usingDropper = !usingDropper;

        if (usingDropper)
        {
            // disable image raycast and collider to prevent blocking input
            GetComponent<Image>().raycastTarget = false;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            // re-enable interaction
            GetComponent<Image>().raycastTarget = true;
            GetComponent<Collider2D>().enabled = true;

            // reset droplet if it was active
            if (gotDrop)
            {
                droplet.GetComponent<Image>().enabled = false;
                gotDrop = false;
            }
        }
    }

    // get "color" (currently sprites of the potions) of the potion and apply it to the droplet
    private void GetDropletColor()
    {
        Debug.Log("GetDropletColor() called");

        var colorType = potionManager.CurrentPotion().GetPotionColorType();
        var colorSprite = potionManager.GetColorSprite(colorType);

        if (colorSprite != null)
        {
            var image = droplet.GetComponent<Image>();
            image.sprite = colorSprite;
            droplet.GetComponent<Image>().enabled = true;
            gotDrop = true;

            Debug.Log($"Droplet shown with sprite {colorSprite} for color: {colorType}");
        }
        else
        {
            Debug.LogWarning("No sprite found for color: " + colorType);
        }
    }

    // show a droplet
    public void MakeDroplet()
    {
        if (droplet != null)
        {
            droplet.GetComponent<Image>().enabled = true;
            gotDrop = true;
        }
    }
}
