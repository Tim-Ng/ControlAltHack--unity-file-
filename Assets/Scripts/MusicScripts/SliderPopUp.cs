using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Music {
    public class SliderPopUp : MonoBehaviour
    {
        [SerializeField] private GameObject PopUp = null;
        [SerializeField] private Slider Master = null, Music = null, SoundEffect = null;
        [SerializeField] private AudioMixer audioMixer = null;
        public void OnClickButton()
        {
            PopUp.SetActive(!PopUp.activeSelf);
        }
        void Awake()
        {
            GlobalMusicContorler.duringStart();
            Master.value = GlobalMusicContorler.MasterVolume;
            Music.value = GlobalMusicContorler.MusicVolume;
            SoundEffect.value = GlobalMusicContorler.SoundEffect;
            setMixer();
        }
        public void slideMaster()
        {
            GlobalMusicContorler.setMasterVolume(Master.value);
            setMixer();
        }
        public void slideMusic()
        {
            GlobalMusicContorler.setMusicVolume(Music.value);
            setMixer();
        }
        public void slideSound()
        {
            GlobalMusicContorler.setSoundEffectVolume(SoundEffect.value);
            setMixer();
        }
        public void clickOnSetDefault()
        {
            GlobalMusicContorler.setDefaultVolumes();
            Master.value = GlobalMusicContorler.MasterVolume;
            Music.value = GlobalMusicContorler.MusicVolume;
            SoundEffect.value = GlobalMusicContorler.SoundEffect;
            setMixer();
        }
        public void setMixer()
        {
            audioMixer.SetFloat("Master", GlobalMusicContorler.MasterVolume);
            audioMixer.SetFloat("Music", GlobalMusicContorler.MusicVolume);
            audioMixer.SetFloat("SoundEffect", GlobalMusicContorler.SoundEffect);
        }
    }
}
