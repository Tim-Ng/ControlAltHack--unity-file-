using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avertars
{
    /// <summary>
    /// This is a class to hold all the data of the Avertar Images
    /// </summary>
    public class AvertarList: MonoBehaviour
    {
        /// <summary>
        /// The gameobject for the popup to select the Avertar
        /// </summary>
        [SerializeField] private GameObject selectPopup = null;
        /// <summary>
        /// The Sprites for all the character
        /// </summary>
        [SerializeField] private Sprite
            Avertar1 =null,
            Avertar2 =null,
            Avertar3 =null,
            Avertar4 =null,
            Avertar5 =null,
            Avertar6 =null,
            Avertar7 =null,
            Avertar8 =null,
            Avertar9 =null,
            Avertar10 =null;
        /// <summary>
        /// Holds the list of the Avertar sprites.
        /// This is static so this list can be accessed on other scripts.
        /// </summary>
        public static List<Sprite> AvertarLists = new List<Sprite>();
        /// <summary>
        /// Input all the sprites into the list
        /// </summary>
        void Awake()
        {
            AvertarLists.Add(Avertar1);
            AvertarLists.Add(Avertar2);
            AvertarLists.Add(Avertar3);
            AvertarLists.Add(Avertar4);
            AvertarLists.Add(Avertar5);
            AvertarLists.Add(Avertar6);
            AvertarLists.Add(Avertar7);
            AvertarLists.Add(Avertar8);
            AvertarLists.Add(Avertar9);
            AvertarLists.Add(Avertar10);
        }
        /// <summary>
        /// To open the popup to select Avertar.
        /// </summary>
        public void onClickPopupSelect()
        {
            selectPopup.SetActive(true);
        }
        /// <summary>
        /// To close the popup to select Avertar.
        /// </summary>
        public void onClickClosePopup()
        {
            selectPopup.SetActive(false);
        }
    }
}

