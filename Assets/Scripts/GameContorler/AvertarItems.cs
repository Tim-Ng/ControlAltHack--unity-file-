using MainMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Avertars
{
    public class AvertarItems : MonoBehaviour
    {
        [SerializeField] private GameObject ImageAvertar = null;
        private GameObject NickNameScriptOBJ = null;
        public string whichAvertar=null;
        private void Start()
        {
            ImageAvertar.GetComponent<Image>().sprite = AvertarList.AvertarLists[int.Parse(whichAvertar)];
            NickNameScriptOBJ = GameObject.Find("JoinHost");
        }
        public void onClickOnAvertar()
        {
            NickNameScriptOBJ.GetComponent<NickNameRoom>().setCharacter(whichAvertar);
        }
    }
}

