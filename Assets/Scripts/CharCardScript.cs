using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawCards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "CharacterCard")]
    public class CharCardScript : ScriptableObject
    {
        public static string HardHack = "Hardware Hacking";
        public static string Crypt = "Cryptanalysis";
        public static string NetNinja = "Network Ninja";
        public static string SocialEng = "Social Engineering";
        public static string Kitchen = "Kitchen Sink";
        public static string SoftWiz = "Software Wizardry";
        public static string Barista = "Barista";
        public static string Connnections = "Connnections";
        public static string WebProcurement = "WebProcurement";
        public static string Forensics = "Forensics";
        public static string LockPicking = "LockPicking";
        public static string SearchFU = "SearchFU";
        public static string nully = "Null";
        public static List<string> allWorkName = new List<string>() { nully,HardHack, Crypt, NetNinja, SocialEng, Kitchen, SoftWiz, Barista, Connnections, WebProcurement, Forensics, LockPicking, SearchFU };
        public static List<string> allspecials = new List<string>() { nully,SoftWiz, Barista, Connnections, WebProcurement, Forensics, LockPicking, SearchFU };

        public int character_code;
        public string character_card_name;

        public Sprite artwork_back;
        public Sprite artwork_front_info;
        public Sprite image_Avertar;
        public int input_hardware;
        public int input_cryptanalysis;
        public int input_network;
        public int input_socal;
        public int input_kitchen;
        public int input_software;

        public string ad_disadvantages;

        [Dropdown("allspecials")]
        public string special;
        public int input_special;

        [Dropdown("allspecials")]
        public string special2;
        public int input_special2;

        public int find_which(string skill_name)
        {
            if (skill_name == HardHack)
            {
                return input_hardware;
            }
            else if (skill_name == Crypt)
            {
                return input_cryptanalysis;
            }
            else if (skill_name == NetNinja)
            {
                return input_network;
            }
            else if (skill_name == SocialEng)
            {
                return input_socal;
            }
            else if (skill_name == Kitchen)
            {
                return input_kitchen;
            }
            else if (skill_name == SoftWiz)
            {
                return input_software;
            }
            else if (skill_name == special)
            {
                return input_special;
            }
            else if (skill_name == special2)
            {
                return input_special2;
            }
            else
            {
                return find_which(Kitchen);
            }
        }
    }
}
