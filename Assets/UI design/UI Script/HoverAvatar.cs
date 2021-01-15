using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverAvatar : MonoBehaviour
{
    private GameObject avatarprofile = null;
    [SerializeField] private bool detectsHaveSprite;
    public float x = 1.2f,y=1.2f;
    Vector3 unhoverscale;
    private void Start()
    {
        avatarprofile =gameObject;
        unhoverscale = avatarprofile.transform.localScale;
    }
    public void OnHoverEnter()
    {
        if (!avatarprofile.GetComponent<Button>().interactable)
        {
            return;
        }
        if (detectsHaveSprite)
        {
            if(avatarprofile.GetComponent<Image>().sprite != null)
            {
                avatarprofile.transform.localScale = new Vector3(x, y);
            }
        }
        else
        {
            avatarprofile.transform.localScale = new Vector3(x, y);
        } 
    }

    public void OnHoverExit()
    {
      
        avatarprofile.transform.localScale = unhoverscale;
    }
}
