using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverAvatar : MonoBehaviour
{
    public GameObject avatarprofile;
    Vector3 unhoverscale;

    public void OnHoverEnter()
    {
        unhoverscale = avatarprofile.transform.localScale;
        avatarprofile.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void OnHoverExit()
    {
      
        avatarprofile.transform.localScale = unhoverscale;
    }
}
