using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avertars {
    /// <summary>
    /// This script is to allow there to be a pop up when an avertar is hovered over
    /// </summary>
    public class HoverAvatar : MonoBehaviour
    {
        /// <summary>
        /// The game object of the avertar profile
        /// </summary>
        private GameObject avatarprofile = null;
        /// <summary>
        /// Do we need to detect for a sprite to popup
        /// </summary>
        [SerializeField] private bool detectsHaveSprite = false;
        /// <summary>
        /// The amount to be changed
        /// </summary>
        public float x = 1.2f, y = 1.2f;
        /// <summary>
        /// To hold the size before popup up
        /// </summary>
        Vector3 unhoverscale;
        /// <summary>
        /// When this script is rendered this function is called.To set the value of avatarprofile & unhoverscale
        /// </summary>
        private void Awake()
        {
            avatarprofile = gameObject;
            unhoverscale = avatarprofile.transform.localScale;
        }
        /// <summary>
        /// While hovering above this game object
        /// </summary>
        public void OnHoverEnter()
        {
            if (!avatarprofile.GetComponent<Button>().interactable)
            {
                return;
            }
            if (detectsHaveSprite)
            {
                if (avatarprofile.GetComponent<Image>().sprite != null)
                {
                    avatarprofile.transform.localScale = new Vector3(x, y);
                }
            }
            else
            {
                avatarprofile.transform.localScale = new Vector3(x, y);
            }
        }
        /// <summary>
        /// While not hovering above this game object
        /// </summary>
        public void OnHoverExit()
        {

            avatarprofile.transform.localScale = unhoverscale;
        }
    }
}

