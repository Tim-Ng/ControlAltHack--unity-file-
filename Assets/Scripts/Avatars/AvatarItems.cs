using MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Avatars
{
    /// <summary>
    /// For displaying the Avatar on an Object by inputting the Avatar code.
    /// </summary>
    public class AvatarItems : MonoBehaviour
    {
        [Header("Game object of the Avertar Image")]
        ///<summary>To hold the gameobject that would hold the Image</summary>
        [SerializeField] private GameObject ImageAvertar = null;
        ///<summary>To hold the gameobject that holds the NickNameRoom script which is "JoinHost"</summary>
        private GameObject NickNameScriptOBJ = null;
        /// <summary> To input the code for the Avertar </summary>
        public string whichAvertar=null;
        /// <summary> To load the image to the object as soon as the object is loaded </summary>
        private void Start()
        {
            ImageAvertar.GetComponent<Image>().sprite = AvatarList.AvatarLists[int.Parse(whichAvertar)];
            NickNameScriptOBJ = GameObject.Find("JoinHost");
        }
        /// <summary> This is for choosing your character </summary>
        /// <remarks> By sending the info to the NickNameRoom Script</remarks>
        public void onClickOnAvertar()
        {
            NickNameScriptOBJ.GetComponent<NickNameRoom>().setCharacter(whichAvertar);
        }
    }
}

