using main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawCards
{
    public class entropyCardDisplay : MonoBehaviour
    {
        private EntropyCardScript infoEntro = null;
        [SerializeField] private GameObject BackSide = null, InfoSide = null, gameInterfaceOBJ = null;
        public int turnPlayed = 0;
        private bool HasChangePosition= false;
        private void Update()
        {
            float posX=gameObject.GetComponent<RectTransform>().position.x;
            float posY=gameObject.GetComponent<RectTransform>().position.y;
            if (gameInterfaceOBJ.GetComponent<entropyCardPopup>().checkIfCanPlay(infoEntro, turnPlayed))
            {
                if (!HasChangePosition)
                {
                    gameObject.GetComponent<RectTransform>().position = new Vector2(posX, posY+10f);
                    HasChangePosition = true;
                }
            }
            else
            {
                if (HasChangePosition)
                {
                    gameObject.GetComponent<RectTransform>().position = new Vector2(posX, posY-10f);
                    HasChangePosition = false;
                }
            }
        }
        private void Start()
        {
            gameInterfaceOBJ = GameObject.Find("/MainGame/Game Interface");
        }
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
        public void clickOnInfo() => gameInterfaceOBJ.GetComponent<entropyCardPopup>().opendCharCard(infoEntro, this, turnPlayed);
        public void ifThisIsPlayed() => turnPlayed = gameInterfaceOBJ.GetComponent<TurnManager>().RoundNumber;
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
