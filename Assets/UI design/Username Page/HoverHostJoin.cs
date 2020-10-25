using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverHostJoin : MonoBehaviour
{
    public Sprite unhoverimage;
    public Sprite hoverimage;
    public GameObject btn;

    public void OnHoverEnter()
    {
        Image image = btn.GetComponent<Image>();
        image.sprite = hoverimage;
    }

    public void OnHoverExit()
    {
        Image image = btn.GetComponent<Image>();
        image.sprite = unhoverimage;
    }

}
