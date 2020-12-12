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

    public int find_which(string skill_name)
    {
        if (skill_name == "Hardware Hacking")
        {
            return input_hardware;
        }
        else if (skill_name == "Cryptanalysis")
        {
            return input_cryptanalysis;
        }
        else if (skill_name == "Network Ninja")
        {
            return input_network;
        }
        else if (skill_name == "Social Engineering" )
        {
            return input_socal;
        }
        else if (skill_name == "Kitchen Sink")
        {
            return input_kitchen;
        }
        else if (skill_name == "Software Wizardry")
        {
            return input_software;
        }
        else
        {
            if (skill_name == special)
            {
                return input_special;
            }
            else if (skill_name == special2)
            {
                return input_special2;
            }
            else
            {
                return find_which("Kitchen Sink");
            }
        }
    }
}
