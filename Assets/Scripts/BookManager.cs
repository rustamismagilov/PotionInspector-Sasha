using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    [SerializeField] List<GameObject> snapPositions;
    [SerializeField] private float moveSpeed = 1.0f;
    private GameObject bookCanvas;
    private bool isOpen;
    private Coroutine moveRoutine;

    public bool IsOpen()
    {
        return isOpen;
    }

    private void Awake()
    {
        bookCanvas = GameObject.FindWithTag("BookUI");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveToOpen()
    {
        StartMove(snapPositions[2].transform.position, OnMoveFinishedOpen);
    }

    public void MoveOffscreen()
    {
        StartMove(snapPositions[0].transform.position, null);
    }

    public void MoveToHover()
    {
        StartMove(snapPositions[1].transform.position, null);
    }

    public void CloseBook()
    {
        bookCanvas.SetActive(false);
        isOpen = false;
    }
    
    

    private void OnMoveFinishedOpen()
    {
        bookCanvas.SetActive(true);
        isOpen = true;
    }

    private void StartMove(Vector3 targetPos, System.Action onComplete)
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }

        moveRoutine = StartCoroutine(MoveRoutine(targetPos, onComplete));
    }

    private IEnumerator MoveRoutine(Vector3 targetPos, System.Action onComplete)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        onComplete?.Invoke();
        moveRoutine = null;
    }
}
