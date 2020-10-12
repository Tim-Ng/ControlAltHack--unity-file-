using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private Vector2 startPosition;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public void StartDrag()
    {
        startPosition = transform.position;
        isDragging = true;
    }
    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
        }
        else
        {
            transform.position = startPosition;
        }

    }
}
