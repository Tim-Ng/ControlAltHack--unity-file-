using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avertars
{
    public class AvertarList: MonoBehaviour
    {
        [SerializeField] private GameObject selectPopup = null;
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
        public static List<Sprite> AvertarLists = new List<Sprite>();
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
        public void onClickPopupSelect()
        {
            selectPopup.SetActive(true);
        }
        public void onClickClosePopup()
        {
            selectPopup.SetActive(false);
        }
    }
}

