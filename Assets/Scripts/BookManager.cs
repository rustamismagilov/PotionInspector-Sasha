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
        StartMoveToOpen();
    }

    public void MoveOffscreen()
    {
        StartMoveTo(snapPositions[0].transform.position);
    }

    public void MoveToHover()
    {
        StartMoveTo(snapPositions[1].transform.position);
    }

    public void CloseBook()
    {
        bookCanvas.SetActive(false);
        isOpen = false;
    }
    
    

    private void StartMoveToOpen()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveRoutineToOpen(snapPositions[2].transform.position));
    }

    private void StartMoveTo(Vector3 targetPos)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveRoutine(targetPos));
    }

    private IEnumerator MoveRoutineToOpen(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        bookCanvas.SetActive(true);
        isOpen = true;
        moveRoutine = null;
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        moveRoutine = null;
    }
}
