using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music {
    public static class GlobalMusicContorler
    {
        // use const  for forever
        private const float defaultMusicVolume = 1;
        private const float defaultSoundEffect = 1;
        private const string MusicKey = "MusicKey";
        private const string SoundKey = "SoundKey";
        public static float MusicVolume { get; set; }
        public static float SoundEffect { get; set; }
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
            Debug.Log("Music has start");
            setPrefeb();
        }
        public static void setDefaultVolumes()
        {
            MusicVolume = defaultMusicVolume;
            SoundEffect = defaultSoundEffect;
            setPrefeb();
        }
        public static void setMusicVolume(float volume) {
            MusicVolume = volume;
            setPrefeb();
        }
        public static void setSoundEffectVolume(float volume) {
            SoundEffect = volume;
            setPrefeb();
        }
        private static void setPrefeb()
        {
            PlayerPrefs.SetFloat(SoundKey, SoundEffect);
            PlayerPrefs.SetFloat(MusicKey, MusicVolume);
        }
    }

}
