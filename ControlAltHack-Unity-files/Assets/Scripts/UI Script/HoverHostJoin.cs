using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is to allow there to be a pop up when a hostjoin button is hovered over
/// </summary>
public class HoverHostJoin : MonoBehaviour
{
    /// <summary>
    /// The image when the button is unhovered 
    /// </summary>
    public Sprite unhoverimage;
    /// <summary>
    /// The image when the button during hovering  
    /// </summary>
    public Sprite hoverimage;
    /// <summary>
    /// The game object of the button
    /// </summary>
    public GameObject btn;
    /// <summary>
    /// To hold the size before popup up
    /// </summary>
    Vector3 unhoverscale;
    /// <summary>
    /// While hovering above this game object
    /// </summary>
    public void OnHoverEnter()
    {
        Image image = btn.GetComponent<Image>();
        image.sprite = hoverimage;
        unhoverscale = btn.transform.localScale;
        btn.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    /// <summary>
    /// While not hovering above this game object
    /// </summary>
    public void OnHoverExit()
    {
        Image image = btn.GetComponent<Image>();
        image.sprite = unhoverimage;
        btn.transform.localScale = unhoverscale;
    }

}
