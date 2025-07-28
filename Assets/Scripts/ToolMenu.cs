
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToolMenu : MonoBehaviour
{
    [Header("Menu Snap Locations")]
    [SerializeField] private Transform offscreenPosition;
    [SerializeField] private Transform openPosition;

    [Header("Button")]
    [SerializeField] private GameObject menuButton;
    [SerializeField] private Transform buttonHide;
    [SerializeField] private Transform buttonReturn;

    [Header("Buttons to Hide")]
    [Tooltip("remove interactability on buttons beneath menu while menu is open")]
    [SerializeField] private List<Button> hideableButtons;

    [Header("Movement Speed")]
    [SerializeField] private float moveSpeed = 1f;

    //holding onto this in case i need these when i reparent tools to tool menu at end of day
    // [Header("Tool Snap Locations")]
    // [SerializeField] Transform[] toolSpots;

    private bool menuOpen = false;

    public bool MenuOpen()
    {
        return menuOpen;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // gameObject.transform.position = offscreenPosition.position; //ensure menu starts offscreen
    }

    public void OpenMenu()
    {
        GetComponentInChildren<Button>().interactable = true; //make minimize button interactable
        menuButton.transform.DOMove(buttonHide.position, moveSpeed); //move initial menu button offscreen
        transform.DOMove(openPosition.position, moveSpeed); //menu slides into frame
        menuOpen = true;

        foreach (Button button in hideableButtons) //make the buttons that get hidden by the open menu NOT interactable to prevent misclicks
        {
            button.interactable = false;
        }
    }

    public void CloseMenu()
    {

        GetComponentInChildren<Button>().interactable = false; //once minimize button has been clicked, it cannot be clicked again
        transform.DOMove(offscreenPosition.position, moveSpeed); //menu slides offscreen
        menuButton.transform.DOMove(buttonReturn.position, moveSpeed); //menu button returns to screen
        menuOpen = false;

        foreach (Button button in hideableButtons) //re-enable interactability on buttons that were hidden by the menu
        {
            button.interactable = true;
        }
    }

}
