using System;
using UnityEngine;

public class ClickandDrag : MonoBehaviour
{
    private Vector3 mousePosOffset; //to drag object from whatever point it is clicked

    Vector3 GetMouseWorldPos()
    {
       return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        //Debug.Log("Clicked on " + gameObject.name);
        mousePosOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        //Debug.Log("Dragging " + gameObject.name);
        transform.position = GetMouseWorldPos() + mousePosOffset;
    }

}
