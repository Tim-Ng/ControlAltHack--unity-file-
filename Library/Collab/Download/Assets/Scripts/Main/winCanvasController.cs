using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace main
{
    /// <summary>
    /// This class is to control the elements that display the winner pop up 
    /// </summary>
    public class winCanvasController : MonoBehaviour
    {
        /// <summary>
        /// This is the game objects that holds all the element of the win canvas
        /// </summary>
        [SerializeField,Header("Game Objects for winners")]
        private GameObject WinCanvas = null;
        /// <summary>
        /// These are all the elements in the win canvas
        /// </summary>
        [SerializeField]
        private GameObject firstPlaceHolder = null, secondPlaceHolder = null, thirdPlaceHolder = null, PlayAgainButton = null
            , Avertar1 = null, firstPlaceNickName = null, amount1OfCred = null,
            Avertar2 = null, secondPlaceNickName = null, amount2OfCred = null,
            Avertar3 = null, thirdplaceNickName = null, amount3OfCred = null;
        /// <summary>
        /// This is to set/get if the WinCanvas game object set/if this game object is hidden or not
        /// </summary>
        public bool setWinCanvas
        {
            get { return WinCanvas.activeSelf; }
            set { WinCanvas.SetActive(value); }
        }
        /// <summary>
        /// This is to set the game object firstPlaceHolder to be hidden or not
        /// </summary>
        public bool setfirstPlaceHolder
        {
            set { firstPlaceHolder.SetActive(value); }
        }
        /// <summary>
        /// This is to set the image of the Avertar of the player in first place
        /// </summary>
        public Sprite setFirstPlaceAvertar
        {
            set { Avertar1.GetComponent<Image>().sprite = value; }
        }
        /// <summary>
        /// This is to set the name of the player in first place
        /// </summary>
        public string setFirstPlaceNickName
        {
            set { firstPlaceNickName.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// This is to set the amount of winning cred of the first place
        /// </summary>
        public int setFirstPlaceAmountOfCred
        {
            set { amount1OfCred.GetComponent<Text>().text = value.ToString(); }
        }
        /// <summary>
        /// This is to set the game object secondPlaceHolder to be hidden or not
        /// </summary>
        public bool setsecondPlaceHolder
        {
            set { secondPlaceHolder.SetActive(value); }
        }
        /// <summary>
        /// This is to set the image of the Avertar of the player in second place
        /// </summary>
        public Sprite setSecondPlaceAvertar
        {
            set { Avertar2.GetComponent<Image>().sprite = value; }
        }
        /// <summary>
        /// This is to set the name of the player in second place
        /// </summary>
        public string setSecondPlaceNickName
        {
            set { secondPlaceNickName.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// This is to set the amount of winning cred of the second place
        /// </summary>
        public int setSecondPlaceAmountOfCred
        {
            set { amount2OfCred.GetComponent<Text>().text = value.ToString(); }
        }
        /// <summary>
        /// This is to set the game object thirdPlaceHolder to be hidden or not
        /// </summary>
        public bool setThirdPlaceHolder
        {
            set { thirdPlaceHolder.SetActive(value); }
        }
        /// <summary>
        /// This is to set the image of the Avertar of the player in third place
        /// </summary>
        public Sprite setThirdPlaceAvertar
        {
            set { Avertar3.GetComponent<Image>().sprite = value; }
        }
        /// <summary>
        /// This is to set the name of the player in third place
        /// </summary>
        public string setThirdPlaceNickName
        {
            set { thirdplaceNickName.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// This is to set the amount of winning cred of the third place
        /// </summary>
        public int setThirdPlaceAmountOfCred
        {
            set { amount3OfCred.GetComponent<Text>().text = value.ToString(); }
        }
        /// <summary>
        /// This is to set of the button PlayAgainButton is interactable or not
        /// </summary>
        public bool setInteractablePlayAgainButton
        {
            set { PlayAgainButton.GetComponent<Button>().interactable = value; }
        }
    }
}
