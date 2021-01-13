using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverAvatar : MonoBehaviour
{
    [SerializeField]private GameObject avatarprofile = null;
    [SerializeField] private bool detectsHaveSprite;
    Vector3 unhoverscale;

    public void OnHoverEnter()
    {
        unhoverscale = avatarprofile.transform.localScale;
        if (detectsHaveSprite)
        {
            if(avatarprofile.GetComponent<Image>().sprite != null)
            {
                avatarprofile.transform.localScale = new Vector3(1.2f, 1.2f);
            }
        }
        else
        {
            avatarprofile.transform.localScale = new Vector3(1.2f, 1.2f);
        }
            
    }

    public void OnHoverExit()
    {
      
        avatarprofile.transform.localScale = unhoverscale;
    }
}
