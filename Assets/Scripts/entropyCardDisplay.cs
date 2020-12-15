using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawCards
{
    public class entropyCardDisplay : MonoBehaviour
    {
        private EntropyCardScript infoEntro = null;
        [SerializeField] private GameObject BackSide = null, InfoSide = null;
        public void setID(int which)
        {
            infoEntro = entropyCardDeck.cardDeck[which-1];
            BackSide.GetComponent<Image>().sprite = infoEntro.artwork_back;
            InfoSide.GetComponent<Image>().sprite = infoEntro.artwork_info;
            setBackSide(true);
            setInfoSide(false);
        }
        public EntropyCardScript getInfo()
        {
            return infoEntro;
        }
        public void clickOnBackSide()
        {
            setBackSide(false);
            setInfoSide(true);
        }
        public void clickOnInfo()
        {
            GameObject popUp = GameObject.Find("/MainGame/Game Interface");
            popUp.GetComponent<entropyCardPopup>().opendCharCard(infoEntro, false);
        }
        public void setBackSide(bool TorF)
        {
            BackSide.SetActive(TorF);
        }
        public void setInfoSide(bool TorF)
        {
            InfoSide.SetActive(TorF);
        }
    }
}
