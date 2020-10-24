using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverContinueButton : MonoBehaviour
{
    //public GameObject continuebtn;
    public Sprite unhoverimage;
    public Sprite hoverimage;
    public Image image;

    public void OnHoverEnter()
    {
        Image image = GameObject.Find("Continue").GetComponent<Image>();
        image.sprite = hoverimage;
    }

    public void OnHoverExit()
    {
        Image image = GameObject.Find("Continue").GetComponent<Image>();
        image.sprite = unhoverimage;
    }
}
