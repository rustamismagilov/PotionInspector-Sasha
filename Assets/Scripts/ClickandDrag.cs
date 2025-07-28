using System;
using UnityEngine;

public class ClickandDrag : MonoBehaviour
{
    private Vector3 mousePosOffset; //to drag object from whatever point it is clicked

    private void Update()
    {
        if (FindFirstObjectByType<DropperTool>().UsingDropper())
            return;
    }

    Vector3 GetMouseWorldPos()
    {
       return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mousePosOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mousePosOffset;
    }

}
