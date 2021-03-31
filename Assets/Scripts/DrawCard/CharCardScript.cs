using UnityEngine;

namespace DrawCards
{
    /// <summary>
    /// The enum to the Skills/Jobs
    /// </summary>
     public enum  AllJobs
    {
        /// <summary> For empty </summary>
        Null,
        /// <summary> For Hardware Hacking</summary>
        HardHack,
        /// <summary> For Cryptanalysis</summary>
        Crypt,
        /// <summary> For Network Ninja</summary>
        NetNinja,
        /// <summary> For Social Engineering</summary>
        SocialEng,
        /// <summary> For Kitchen</summary>
        Kitchen,
        /// <summary> For Software wizard</summary>
        SoftWiz,
        /// <summary> For Barista</summary>
        Barista,
        /// <summary> For Connections</summary>
        Connnections,
        /// <summary> For WebProcurement</summary>
        WebProcurement,
        /// <summary> For Forensics</summary>
        Forensics,
        /// <summary> For Lock Picking</summary>
        LockPicking,
        /// <summary> For SearchFU</summary>
        SearchFU
    }
    public static class GetStringOfTask
    {
        /// <summary>The static string For Hardware Hacking</summary>
        public static string HardHackSTR = "Hardware Hacking";
        /// <summary>The static string For Cryptanalysis</summary>
        public static string CryptSTR = "Cryptanalysis";
        /// <summary>The static string For Network Ninja</summary>
        public static string NetNinjaSTR = "Network Ninja";
        /// <summary>The static string For Social Engineering</summary>
        public static string SocialEngSTR = "Social Engineering";
        /// <summary>The static string For Kitchen Sink</summary>
        public static string KitchenSTR = "Kitchen Sink";
        /// <summary>The static string For Software Wizardry</summary>
        public static string SoftWizSTR = "Software Wizardry";
        /// <summary>The static string For Barista</summary>
        public static string BaristaSTR = "Barista";
        /// <summary>The static string For Connnections</summary>
        public static string ConnnectionsSTR = "Connnections";
        /// <summary>The static string For WebProcurement</summary>
        public static string WebProcurementSTR = "WebProcurement";
        /// <summary>The static string For Forensics</summary>
        public static string ForensicsSTR = "Forensics";
        /// <summary>The static string For LockPicking</summary>
        public static string LockPickingSTR = "LockPicking";
        /// <summary>The static string For SearchFU</summary>
        public static string SearchFUSTR = "SearchFU";
        /// <summary>The static string For Null ERROR</summary>
        public static string nullySTR = "Null ERROR";
        /// <summary>
        /// To get the name of the Job with the enum code
        /// </summary>
        /// <param name="WhichJob">The enum of the job to find</param>
        /// <returns>The string of the enum job</returns>
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
        /// <summary>
        /// To get the enam job of the Job with the name of the job
        /// </summary>
        /// <param name="WhichJob">The string of the job to find</param>
        /// <returns>enum of the job</returns>
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
    /// <summary>
    /// This class to hold the data of a charater
    /// </summary>
    [CreateAssetMenu(fileName = "New Card", menuName = "CharacterCard")]
    public class CharCardScript : ScriptableObject
    {
        /// <summary>
        /// The character code of the character 
        /// </summary>
        [Header("Character code")]
        public int character_code;
        /// <summary>
        /// The name of the character 
        /// </summary>
        [Header("Character name")]
        public string character_card_name;

        /// <summary>
        /// The sprite of the back of the character card
        /// </summary>
        [Header("Back image of the character card")]
        public Sprite artwork_back;
        /// <summary>
        /// The sprite of the front of the character card
        /// </summary>
        [Header("Front image of the character card")]
        public Sprite artwork_front_info;
        /// <summary>
        /// The sprite of the character Avertar
        /// </summary>
        [Header("Image Avertar of Character")]
        public Sprite image_Avertar;
        [Header("Skill Values")]
        /// <summary>
        /// Amount of Hardware
        /// </summary>
        public int input_hardware;
        /// <summary>
        /// Amount of cryptanalysis
        /// </summary>
        public int input_cryptanalysis;
        /// <summary>
        /// Amount of network ninja 
        /// </summary>
        public int input_network;
        /// <summary>
        /// Amount of social netwoking
        /// </summary>
        public int input_social;
        /// <summary>
        /// Amount of kitchen
        /// </summary>
        public int input_kitchen;
        /// <summary>
        /// Amount of software engineering
        /// </summary>
        public int input_software;

        /// <summary>
        /// The advantages and disadvantage of the character 
        /// </summary>
        [Multiline(5)]
        public string ad_disadvantages;

        /// <summary>
        /// The Enum code of the special1 skill of the character
        /// </summary>
        public AllJobs special1;
        /// <summary>
        /// The Amount of the special1 skill of the character
        /// </summary>
        public int input_special1;

        /// <summary>
        /// The Enum code of the special2 skill of the character
        /// </summary>
        public AllJobs special2;
        /// <summary>
        /// The Amount of the special2 skill of the character
        /// </summary>
        public int input_special2;

        /// <summary>
        /// This is to return the amount of the skill of with an input of the enum job
        /// </summary>
        /// <remarks>
        /// If this character does not have the skill the amount of the kitchen skill is used
        /// </remarks>
        /// <param name="WhichJob">Which skill amount to get</param>
        /// <returns>the skill amount of the enum job of this character</returns>
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
                return input_social;
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
