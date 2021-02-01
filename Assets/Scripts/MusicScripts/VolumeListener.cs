using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class VolumeListener : MonoBehaviour
    {
        [SerializeField] private bool Music = false, Sound = false;
        private AudioSource audioComponent = null;
        private void Awake()
        {
            audioComponent = GetComponent<AudioSource>();
        }
        void Update()
        {
            if (Music)
            {
                audioComponent.volume = GlobalMusicContorler.MusicVolume;
            }
            if (Sound)
            {
                audioComponent.volume = GlobalMusicContorler.SoundEffect;
            }
        }
    }
}

