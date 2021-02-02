using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Music {
    public static class GlobalMusicContorler 
    {
        public const float defaultMusicVolume = 0;
        public const float defaultMasterVolume = 0;
        public const float defaultSoundEffect = 0;
        private const string MusicKey = "MusicKey";
        private const string SoundKey = "SoundKey";
        private const string MasterKey = "MasterKey";
        public static float MusicVolume { get; set; }
        public static float SoundEffect { get; set; }
        public static float MasterVolume { get; set; }
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
        public static void setDefaultVolumes()
        {
            MusicVolume = defaultMusicVolume;
            SoundEffect = defaultSoundEffect;
            MasterVolume = defaultMasterVolume;
            setPrefeb();
        }
        public static void setMusicVolume(float volume) {
            MusicVolume = volume;
            PlayerPrefs.SetFloat(MusicKey, MusicVolume);
        }
        public static void setSoundEffectVolume(float volume) {
            SoundEffect = volume;
            PlayerPrefs.SetFloat(SoundKey, SoundEffect);
        }
        public static void setMasterVolume(float volume)
        {
            MasterVolume = volume;
            PlayerPrefs.SetFloat(MasterKey, MasterVolume);
        }
        private static void setPrefeb()
        {
            PlayerPrefs.SetFloat(SoundKey, SoundEffect);
            PlayerPrefs.SetFloat(MusicKey, MusicVolume);
            PlayerPrefs.SetFloat(MasterKey, MasterVolume);
        }
    }

}
