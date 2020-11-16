using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntropyCardDisplay : MonoBehaviour
{
    public EntropyCardScript entropyData;
    public Image artwork_info;
    public Image artwork_background;
    public GameObject InfoSide;
    public GameObject FrontSide;
    public Button playEntorypyButton;
    public void setUpdate()
    {
        Start();
    }
    void Start()
    {
        artwork_info.sprite = entropyData.artwork_info;
        artwork_background.sprite = entropyData.artwork_back;
    }
}
