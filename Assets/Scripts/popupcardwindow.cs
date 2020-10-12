using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupcardwindow : MonoBehaviour
{
    private GameObject panel;
    public GameObject CardInfo;
    public Sprite imageSprite;
    

    private void Start()
    {
        panel= GameObject.Find("/MainGameStart/Game Interface/Panel");
        CardInfo = GameObject.Find("/MainGameStart/Game Interface/Panel/CardInfo");
    }
    public void clickOn()
    {
        panel.SetActive(true);
        CardInfo.gameObject.GetComponent<Image>().sprite = imageSprite;
    }
    public void clickExit()
    {
        panel.SetActive(false);
        CardInfo.gameObject.GetComponent<Image>().sprite = null;
    }

}
