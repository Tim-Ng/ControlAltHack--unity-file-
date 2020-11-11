using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionCardDisplay : MonoBehaviour
{
    public MissionCardScript mission_script;
    public Image artwork_back;
    public Image artwork_front_info;


    public GameObject InfoSide;
    public GameObject FrontSide;
    public void setUpdate()
    {
        Start();
    }
    private void Start()
    {
        artwork_back.sprite = mission_script.artwork_back;
        artwork_front_info.sprite = mission_script.artwork_front_info;
    }
}
