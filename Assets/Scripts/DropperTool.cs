using System;
using UnityEngine;
using UnityEngine.UI;

public class DropperTool : MonoBehaviour
{
    [SerializeField] private float dropperFollowSpeed = 0.5f;
    [SerializeField] private GameObject droplet;
    private ToolScript thisTool;
    private PotionManager potionManager;
    private bool usingDropper = false;
    private Vector3 mousePosition;
    private bool gotDrop = false;
    private Color dropletColor;

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
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("Potion"))
                {
                    Debug.Log("Hit: " + hit.collider.name);
                    Debug.Log("Tag on hit: " + hit.collider.tag);
                    GetDropletColor();
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
            image.color = Color.white; // Ensure alpha is visible
            droplet.SetActive(true);
            gotDrop = true;

            Debug.Log("Droplet shown with sprite for color: " + colorType);
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
