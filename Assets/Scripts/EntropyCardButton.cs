using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntropyCardButton : MonoBehaviour
{
    public GameObject cardFront;
    public EntropyCardDisplay myParent;
    private GameObject PopUp;
    private popupcardwindowEntropy popUpScript;
    private EntropyCardScript EntroyScript;
    private void Start()
    {
        PopUp = GameObject.Find("/MainGame/Game Interface/PopUpBackGroundEntropy");
        popUpScript = PopUp.GetComponent<popupcardwindowEntropy>();
    }
    public void clickOnEntropy()
    {
        PopUp.SetActive(true);
        EntroyScript = myParent.entropyData;
        popUpScript.openEntropyCard(EntroyScript);
    }
    public void clickToFlip()
    {
        cardFront.SetActive(false);
    }
}
