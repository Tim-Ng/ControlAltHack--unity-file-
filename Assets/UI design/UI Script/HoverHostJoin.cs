using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverHostJoin : MonoBehaviour
{
    public Sprite unhoverimage;
    public Sprite hoverimage;
    public GameObject btn;
    Vector3 unhoverscale;

    public void OnHoverEnter()
    {
        Image image = btn.GetComponent<Image>();
        image.sprite = hoverimage;
        unhoverscale = btn.transform.localScale;
        btn.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnHoverExit()
    {
        Image image = btn.GetComponent<Image>();
        image.sprite = unhoverimage;
        btn.transform.localScale = unhoverscale;
    }

}
