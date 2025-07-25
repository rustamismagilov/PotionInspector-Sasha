using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogueWindow : MonoBehaviour
{
    [SerializeField] private Transform offscreenPos;
    [SerializeField] private Transform talkingPos;
    [SerializeField] private float moveSpeed = 2f;

    private bool onScreen = false;
    private TextMeshProUGUI dialogueText;
    
    //TODO: space to pass through dialogue from CustomerSO
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginInspection()
    {
        if (!onScreen)
        {
            transform.DOMove(talkingPos.position, moveSpeed);
            onScreen = true;
        }
        else
        {
            print("dialogue box already on screen");
        }

    }

    public void RetractWindow()
    {
        if (onScreen)
        {
            transform.DOMove(offscreenPos.position, moveSpeed);
            onScreen = false;
        }
        else return;
    }
    
    
}
