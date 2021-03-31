using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avatars
{
    /// <summary>
    /// This is a class to hold all the data of the Avatar Images
    /// </summary>
    public class AvatarList: MonoBehaviour
    {
        /// <summary>
        /// The gameobject for the popup to select the Avatar
        /// </summary>
        [SerializeField] private GameObject selectPopup = null;
        /// <summary>
        /// The Sprites for all the character
        /// </summary>
        [SerializeField] private Sprite
            Avatar1 =null,
            Avatar2 =null,
            Avatar3 =null,
            Avatar4 =null,
            Avatar5 =null,
            Avatar6 =null,
            Avatar7 =null,
            Avatar8 =null,
            Avatar9 =null,
            Avatar10 =null;
        /// <summary>
        /// Holds the list of the Avatar sprites.
        /// This is static so this list can be accessed on other scripts.
        /// </summary>
        public static List<Sprite> AvatarLists = new List<Sprite>();
        /// <summary>
        /// Input all the sprites into the list
        /// </summary>
        void Awake()
        {
            AvatarLists.Add(Avatar1);
            AvatarLists.Add(Avatar2);
            AvatarLists.Add(Avatar3);
            AvatarLists.Add(Avatar4);
            AvatarLists.Add(Avatar5);
            AvatarLists.Add(Avatar6);
            AvatarLists.Add(Avatar7);
            AvatarLists.Add(Avatar8);
            AvatarLists.Add(Avatar9);
            AvatarLists.Add(Avatar10);
        }
        /// <summary>
        /// To open the popup to select Avatar.
        /// </summary>
        public void onClickPopupSelect()
        {
            selectPopup.SetActive(true);
        }
        /// <summary>
        /// To close the popup to select Avatar.
        /// </summary>
        public void onClickClosePopup()
        {
            selectPopup.SetActive(false);
        }
    }
}

