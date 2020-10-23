using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardDispaly : MonoBehaviour
{
    public CharCardScript CharCard;

    public Text input_hardware;
    public Text input_cryptanalysis;
    public Text input_network;
    public Text input_socal;
    public Text input_kitchen;
    public Text input_software;
    public Text input_special;
    public Text input_special2;
    public Text discription;

    public Text character_card_name;
    public Text ad_disadvantages;
    public Text special;
    public Text special2;


    public Image image_special;
    public Image image_special2;
    public Image image_character_info;
    public Image artwork_back;
    public Image artwork_front;

    public GameObject InfoSide;
    public GameObject FrontSide;


    void Start()
    {
        character_card_name.text = CharCard.character_card_name;
        ad_disadvantages.text = CharCard.ad_disadvantages;
        special.text = CharCard.special;

        input_hardware.text = CharCard.input_hardware.ToString();
        input_cryptanalysis.text = CharCard.input_cryptanalysis.ToString();
        input_network.text = CharCard.input_network.ToString();
        input_socal.text = CharCard.input_socal.ToString();
        input_kitchen.text = CharCard.input_kitchen.ToString();
        input_software.text = CharCard.input_software.ToString();
        input_special.text = CharCard.input_special.ToString();
        input_special2.text = CharCard.input_special2.ToString();
        discription.text = CharCard.discription;

        special.text = CharCard.special;
        special2.text = CharCard.special2;

        image_special.sprite = CharCard.image_special;
        image_special2.sprite = CharCard.image_special2;

        image_character_info.sprite = CharCard.char_info;

        artwork_back.sprite = CharCard.artwork_back;
        artwork_front.sprite = CharCard.artwork_front;

    }
    
  

}
