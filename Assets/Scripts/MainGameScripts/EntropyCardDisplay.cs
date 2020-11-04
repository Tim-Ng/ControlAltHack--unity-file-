using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntropyCardDisplay : MonoBehaviour
{
    public GameObject sucoffail_obj;
    public GameObject usage_obj;
    public GameObject text_RollVS_obj;
    public GameObject cost_obj;
    public GameObject lighting_discription;
    public GameObject close_success_amount;
    public EntropyCardScript entropyData;
    // for bag of tricks
    public Text Title;
    public Text discription;
    public Text Cost;
    public Text usage;

    public Image artwork_info;
    public Image artwork_background;
    public GameObject InfoSide;
    public GameObject FrontSide;
    // for Lighting strike 
    public Text Sucess;
    public Text Failure;
    public Text minus_how_much_cred;
    public Text text_RollVS;

    public Button playEntorypyButton;
    public void setUpdate()
    {
        Start();
    }
    void Start()
    {
        if (entropyData.IsBagOfTricks)
        {
            close_success_amount.SetActive(true);
            cost_obj.SetActive(true);
            sucoffail_obj.SetActive(false);
            usage_obj.SetActive(true);
            text_RollVS_obj.SetActive(false);
            Title.text = entropyData.Title;
            discription.text = entropyData.discription;
            usage.text = entropyData.usage;
            Cost.text = entropyData.Cost.ToString();

            artwork_info.sprite = entropyData.artwork_info;
            artwork_background.sprite = entropyData.artwork_back;
        }
        else if (entropyData.IsLigthingStrikes)
        {
            cost_obj.SetActive(false);
            Title.text = entropyData.Title;
            discription.text = entropyData.discription;
            Cost.text = entropyData.Cost.ToString();

            artwork_info.sprite = entropyData.artwork_info;
            artwork_background.sprite = entropyData.artwork_back;
            if (entropyData.UseSucFailLighting)
            {
                close_success_amount.SetActive(false);
                text_RollVS_obj.SetActive(true);
                sucoffail_obj.SetActive(true);
                usage_obj.SetActive(false);
                Sucess.text = entropyData.Success;
                Failure.text = entropyData.Failure;
                minus_how_much_cred.text = entropyData.minus_how_much_cred.ToString();
                text_RollVS.text = entropyData.RollVSWhich;
            }
            else
            {
                text_RollVS_obj.SetActive(false);
                sucoffail_obj.SetActive(false);
                usage_obj.SetActive(true);
                usage.text = entropyData.usage;
            }
        }
    }
}
