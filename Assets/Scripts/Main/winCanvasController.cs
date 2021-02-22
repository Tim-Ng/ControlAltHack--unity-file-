using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace main
{
    public class winCanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameObject WinCanvas = null, firstPlaceHolder = null, secondPlaceHolder = null, thirdPlaceHolder = null, PlayAgainButton = null
            , Avertar1 = null, firstPlaceNickName = null, amount1OfCred = null,
            Avertar2 = null, secondPlaceNickName = null, amount2OfCred = null,
            Avertar3 = null, thirdplaceNickName = null, amount3OfCred = null;
        public bool setWinCanvas
        {
            get { return WinCanvas.activeSelf; }
            set { WinCanvas.SetActive(value); }
        }
        public bool setfirstPlaceHolder
        {
            set { firstPlaceHolder.SetActive(value); }
        }
        public Sprite setFirstPlaceAvertar
        {
            set { Avertar1.GetComponent<Image>().sprite = value; }
        }
        public string setFirstPlaceNickName
        {
            set { firstPlaceNickName.GetComponent<Text>().text = value; }
        }
        public int setFirstPlaceAmountOfCred
        {
            set { amount1OfCred.GetComponent<Text>().text = value.ToString(); }
        }
        public bool setsecondPlaceHolder
        {
            set { secondPlaceHolder.SetActive(value); }
        }
        public Sprite setSecondPlaceAvertar
        {
            set { Avertar2.GetComponent<Image>().sprite = value; }
        }
        public string setSecondPlaceNickName
        {
            set { secondPlaceNickName.GetComponent<Text>().text = value; }
        }
        public int setSecondPlaceAmountOfCred
        {
            set { amount2OfCred.GetComponent<Text>().text = value.ToString(); }
        }
        public bool setThirdPlaceHolder
        {
            set { thirdPlaceHolder.SetActive(value); }
        }
        public Sprite setThirdPlaceAvertar
        {
            set { Avertar3.GetComponent<Image>().sprite = value; }
        }
        public string setThirdPlaceNickName
        {
            set { thirdplaceNickName.GetComponent<Text>().text = value; }
        }
        public int setThirdPlaceAmountOfCred
        {
            set { amount3OfCred.GetComponent<Text>().text = value.ToString(); }
        }
        public bool setInteractablePlayAgainButton
        {
            set { PlayAgainButton.GetComponent<Button>().interactable = value; }
        }
    }
}
