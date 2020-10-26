using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card",menuName ="CharacterCard")]
public class CharCardScript : ScriptableObject
{
    public int character_code;
    public string character_card_name;
    public string discription;

    public Sprite artwork_back;
    public Sprite artwork_front_info;
    public Sprite char_info;

    public int input_hardware;
    public int input_cryptanalysis;
    public int input_network;
    public int input_socal;
    public int input_kitchen;
    public int input_software;

    public string ad_disadvantages;

    public string special;
    public int input_special;
    public Sprite image_special;

    public string special2;
    public int input_special2;
    public Sprite image_special2;

    public Sprite image_Avertar;

   
}
