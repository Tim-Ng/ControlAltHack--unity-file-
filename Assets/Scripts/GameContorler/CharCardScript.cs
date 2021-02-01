using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawCards
{
     public enum  AllJobs
    {
        Null,
        HardHack,
        Crypt,
        NetNinja,
        SocialEng,
        Kitchen,
        SoftWiz,
        Barista,
        Connnections,
        WebProcurement,
        Forensics,
        LockPicking,
        SearchFU
    }
    public static class GetStringOfTask
    {
        private static string HardHackSTR = "Hardware Hacking";
        public static string CryptSTR = "Cryptanalysis";
        public static string NetNinjaSTR = "Network Ninja";
        public static string SocialEngSTR = "Social Engineering";
        public static string KitchenSTR = "Kitchen Sink";
        public static string SoftWizSTR = "Software Wizardry";
        public static string BaristaSTR = "Barista";
        public static string ConnnectionsSTR = "Connnections";
        public static string WebProcurementSTR = "WebProcurement";
        public static string ForensicsSTR = "Forensics";
        public static string LockPickingSTR = "LockPicking";
        public static string SearchFUSTR = "SearchFU";
        public static string nullySTR = "Null ERROR";
        public static string get_string_of_job(AllJobs WhichJob)
        {
            if (WhichJob == AllJobs.HardHack)
            {
                return HardHackSTR;
            }
            else if (WhichJob == AllJobs.Crypt)
            {
                return CryptSTR;
            }
            else if (WhichJob == AllJobs.NetNinja)
            {
                return NetNinjaSTR;
            }
            else if (WhichJob == AllJobs.SocialEng)
            {
                return SocialEngSTR;
            }
            else if (WhichJob == AllJobs.Kitchen)
            {
                return KitchenSTR;
            }
            else if (WhichJob == AllJobs.SoftWiz)
            {
                return SoftWizSTR;
            }
            else if (WhichJob == AllJobs.Barista)
            {
                return BaristaSTR;
            }
            else if (WhichJob == AllJobs.Connnections)
            {
                return ConnnectionsSTR;
            }
            else if (WhichJob == AllJobs.WebProcurement)
            {
                return WebProcurementSTR;
            }
            else if (WhichJob == AllJobs.Forensics)
            {
                return ForensicsSTR;
            }
            else if (WhichJob == AllJobs.LockPicking)
            {
                return LockPickingSTR;
            }
            else if (WhichJob == AllJobs.SearchFU)
            {
                return SearchFUSTR;
            }
            else
            {
                return nullySTR;
            }
        }
        public static AllJobs get_AllJobs_usingString(string WhichJob)
        {
            if (WhichJob == HardHackSTR)
            {
                return AllJobs.HardHack;
            }
            else if (WhichJob == CryptSTR )
            {
                return AllJobs.Crypt;
            }
            else if (WhichJob == NetNinjaSTR)
            {
                return AllJobs.NetNinja;
            }
            else if (WhichJob == SocialEngSTR)
            {
                return AllJobs.SocialEng ;
            }
            else if (WhichJob == KitchenSTR )
            {
                return AllJobs.Kitchen;
            }
            else if (WhichJob == SoftWizSTR )
            {
                return AllJobs.SoftWiz;
            }
            else if (WhichJob == BaristaSTR )
            {
                return AllJobs.Barista;
            }
            else if (WhichJob == ConnnectionsSTR)
            {
                return AllJobs.Connnections;
            }
            else if (WhichJob == WebProcurementSTR)
            {
                return AllJobs.WebProcurement;
            }
            else if (WhichJob == ForensicsSTR )
            {
                return AllJobs.Forensics;
            }
            else if (WhichJob == LockPickingSTR)
            {
                return AllJobs.LockPicking;
            }
            else if (WhichJob == SearchFUSTR )
            {
                return AllJobs.SearchFU;
            }
            else
            {
                return AllJobs.Null;
            }
        }
    }
    [CreateAssetMenu(fileName = "New Card", menuName = "CharacterCard")]
    public class CharCardScript : ScriptableObject
    {
        
        

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


        public AllJobs special1; 
        public int input_special1;

        public AllJobs special2;
        public int input_special2;

        public int find_which(AllJobs WhichJob)
        {
            if (WhichJob == AllJobs.HardHack)
            {
                return input_hardware;
            }
            else if (WhichJob == AllJobs.Crypt)
            {
                return input_cryptanalysis;
            }
            else if (WhichJob == AllJobs.NetNinja)
            {
                return input_network;
            }
            else if (WhichJob == AllJobs.SocialEng)
            {
                return input_socal;
            }
            else if (WhichJob == AllJobs.Kitchen)
            {
                return input_kitchen;
            }
            else if (WhichJob == AllJobs.SoftWiz)
            {
                return input_software;
            }
            if (WhichJob == special1)
            {
                return input_special1;
            }
            else if (WhichJob == special2)
            {
                return input_special2;
            }
            else
            {
                return input_kitchen;
            }
        }
    }
}
