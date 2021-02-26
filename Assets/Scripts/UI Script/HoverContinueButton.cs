using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is to allow there to be a pop up when a button is hovered over
/// </summary>
public class HoverContinueButton : MonoBehaviour
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
    /// The image component of the button 
    /// </summary>
    public Image image;
    /// <summary>
    /// While hovering above this game object
    /// </summary>
    public void OnHoverEnter()
    {
        Image image = GetComponent<Image>();
        image.sprite = hoverimage;
    }
    /// <summary>
    /// While not hovering above this game object
    /// </summary>
    public void OnHoverExit()
    {
        Image image = GetComponent<Image>();
        image.sprite = unhoverimage;
    }
}
