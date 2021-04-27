using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Music {
    /// <summary>
    /// This is the class that holds and control the values of the master, music and sound effect of the entire game
    /// </summary>
    public static class GlobalMusicContorler 
    {
        /// <summary>
        /// The default volume for the music
        /// </summary>
        public const float defaultMusicVolume = 0;
        /// <summary>
        /// The default volume for the master
        /// </summary>
        public const float defaultMasterVolume = 0;
        /// <summary>
        /// The default volume for the sound effect
        /// </summary>
        public const float defaultSoundEffect = 0;
        /// <summary>
        /// This is a constant string for the MusicKey for the prefs
        /// </summary>
        private const string MusicKey = "MusicKey";
        /// <summary>
        /// This is a constant string for the SoundKey for the prefs
        /// </summary>
        private const string SoundKey = "SoundKey";
        /// <summary>
        /// This is a constant string for the MasterKey for the prefs
        /// </summary>
        private const string MasterKey = "MasterKey";
        /// <summary>
        /// Get set current Music volume
        /// </summary>
        public static float MusicVolume { get; set; }
        /// <summary>
        /// Get set current Sound Effect volume
        /// </summary>
        public static float SoundEffect { get; set; }
        /// <summary>
        /// Get set current Master volume
        /// </summary>
        public static float MasterVolume { get; set; }
        /// <summary>
        /// This is used to setup the volume and getting prefs
        /// </summary>
        public static void duringStart()
        {
            if (!PlayerPrefs.HasKey(MusicKey))
            {
                MusicVolume = defaultMusicVolume;
            }
            else
            {
                MusicVolume = PlayerPrefs.GetFloat(MusicKey);
            }
            if (!PlayerPrefs.HasKey(SoundKey))
            {
                SoundEffect = defaultMusicVolume;
            }
            else
            {
                SoundEffect = PlayerPrefs.GetFloat(SoundKey);
            }
            if (!PlayerPrefs.HasKey(MasterKey))
            {
                MasterVolume = defaultMasterVolume;
            }
            else
            {
                MasterVolume = PlayerPrefs.GetFloat(MasterKey);
            }
            Debug.Log("Music has start");
            setPrefeb();
        }
        /// <summary>
        /// This is to set all the volumes to default
        /// </summary>
        public static void setDefaultVolumes()
        {
            MusicVolume = defaultMusicVolume;
            SoundEffect = defaultSoundEffect;
            MasterVolume = defaultMasterVolume;
            setPrefeb();
        }
        /// <summary>
        /// This is used to set the volume and prefs of the Music volume
        /// </summary>
        /// <param name="volume"> Set music volume sound level</param>
        public static void setMusicVolume(float volume) {
            MusicVolume = volume;
            PlayerPrefs.SetFloat(MusicKey, MusicVolume);
        }
        /// <summary>
        /// This is used to set the volume and prefs of the SoundEffect volume
        /// </summary>
        /// <param name="volume">Set SoundEffect volume sound level</param>
        public static void setSoundEffectVolume(float volume) {
            SoundEffect = volume;
            PlayerPrefs.SetFloat(SoundKey, SoundEffect);
        }
        /// <summary>
        /// This is used to set the volume and prefs of the Master volume
        /// </summary>
        /// <param name="volume">Set Master volume sound level</param>
        public static void setMasterVolume(float volume)
        {
            MasterVolume = volume;
            PlayerPrefs.SetFloat(MasterKey, MasterVolume);
        }
        /// <summary>
        /// This is used to set all the prefs of the all the sound volume
        /// </summary>
        private static void setPrefeb()
        {
            PlayerPrefs.SetFloat(SoundKey, SoundEffect);
            PlayerPrefs.SetFloat(MusicKey, MusicVolume);
            PlayerPrefs.SetFloat(MasterKey, MasterVolume);
        }
    }

}
