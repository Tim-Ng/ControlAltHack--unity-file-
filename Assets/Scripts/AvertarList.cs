using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avertars
{
    public class AvertarList: MonoBehaviour
    {
        [SerializeField] private Sprite
            Avertar1 =null,
            Avertar2 =null,
            Avertar3 =null,
            Avertar4 =null,
            Avertar5 =null,
            Avertar6 =null;
        public static List<Sprite> AvertarLists = new List<Sprite>();
        void Awake()
        {
            AvertarLists.Add(Avertar1);
            AvertarLists.Add(Avertar2);
            AvertarLists.Add(Avertar3);
            AvertarLists.Add(Avertar4);
            AvertarLists.Add(Avertar5);
            AvertarLists.Add(Avertar6);
        }
    }
}

