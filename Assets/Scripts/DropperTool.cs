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
    [SerializeField] private EventSystem eventSystem;


    private ToolScript thisTool;
    private PotionManager potionManager;
    private bool usingDropper = false;
    private Vector3 mousePosition;
    private bool gotDrop = false;

    public bool GotDrop()
    {
        return gotDrop;
    }

    public bool UsingDropper()
    {
        return usingDropper;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (uiRaycaster == null) uiRaycaster = FindFirstObjectByType<GraphicRaycaster>();
        if (eventSystem == null) eventSystem = FindFirstObjectByType<EventSystem>();
        thisTool = GetComponent<ToolScript>();
        potionManager = FindFirstObjectByType<PotionManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (usingDropper)
            {
                ToggleDropper();
            }
            else
            {
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject && thisTool.OnDesk())
                {
                    ToggleDropper();
                }
            }
        }

        if (usingDropper)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, dropperFollowSpeed);

            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerData = new PointerEventData(eventSystem);
                pointerData.position = Input.mousePosition;

                var results = new List<RaycastResult>();
                uiRaycaster.Raycast(pointerData, results);

                foreach (var result in results) // just because we can have more than 1 thing in the way of raycast
                                                // we will iterate through all of them and find the one with tag "Potion"
                {
                    Debug.Log("UI Hit: " + result.gameObject.name);

                    if (result.gameObject.CompareTag("Potion"))
                    {
                        GetDropletColor();
                        break;
                    }
                }

                if (results.Count == 0) // if there is nothing in the way of raycast = do nothing (print message)
                {
                    Debug.Log("UI raycast found nothing.");
                }
            }
        }
    }

    private void ToggleDropper()
    {
        usingDropper = !usingDropper;

        if (usingDropper)
        {
            GetComponent<Image>().raycastTarget = false;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            GetComponent<Image>().raycastTarget = true;
            GetComponent<Collider2D>().enabled = true;

            if (gotDrop)
            {
                droplet.SetActive(false);
                gotDrop = false;
            }
        }
    }


    private void GetDropletColor()
    {
        Debug.Log("GetDropletColor() called");

        var colorType = potionManager.CurrentPotion().GetPotionColorType();
        var colorSprite = potionManager.GetColorSprite(colorType);

        if (colorSprite != null)
        {
            var image = droplet.GetComponent<Image>();
            image.sprite = colorSprite;
            droplet.SetActive(true);
            gotDrop = true;

            Debug.Log($"Droplet shown with sprite {colorSprite} for color: {colorType}");
        }
        else
        {
            Debug.LogWarning("No sprite found for color: " + colorType);
        }
    }


    public void MakeDroplet()
    {
        if (droplet != null)
        {
            droplet.SetActive(true);
            gotDrop = true;
        }
    }
}
