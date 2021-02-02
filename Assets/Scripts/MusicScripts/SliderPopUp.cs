using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Music {
    public class SliderPopUp : MonoBehaviour
    {
        [SerializeField] private GameObject PopUp = null,CrossOutMaster=null, CrossOutMusic = null, CrossOutSoundEffect = null;
        [SerializeField] private Text MasterAmount = null, MusicAmount = null, SoundEffectAmount = null;
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
        public void clickOnCloseMaster()
        {
            if (Master.value == -80)
            {
                Master.value = GlobalMusicContorler.defaultMasterVolume;
            }
            else
            {
                Master.value = -80;
            }
            slideMaster();
        }
        public void slideMusic()
        {
            GlobalMusicContorler.setMusicVolume(Music.value);
            setMixer();
        }
        public void clickOnCloseMusic()
        {
            if (Music.value == -80)
            {
                Music.value = GlobalMusicContorler.defaultMusicVolume;
            }
            else
            {
                Music.value = -80;
            }
            slideMusic();
        }
        public void slideSound()
        {
            GlobalMusicContorler.setSoundEffectVolume(SoundEffect.value);
            setMixer();
        }
        public void clickOnCloseSoundEffect()
        {
            if (SoundEffect.value == -80)
            {
                SoundEffect.value = GlobalMusicContorler.defaultSoundEffect;
            }
            else
            {
                SoundEffect.value = -80;
            }
            slideSound();
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
            detectAmountChange();
        }
        public void detectAmountChange()
        {
            MasterAmount.text= (calculatePercentage(GlobalMusicContorler.MasterVolume)+"%");
            MusicAmount.text = (calculatePercentage(GlobalMusicContorler.MusicVolume) + "%");
            SoundEffectAmount.text = (calculatePercentage(GlobalMusicContorler.SoundEffect) + "%");
            CrossOutMaster.SetActive(GlobalMusicContorler.MasterVolume == -80);
            CrossOutMusic.SetActive(GlobalMusicContorler.MusicVolume == -80);
            CrossOutSoundEffect.SetActive(GlobalMusicContorler.SoundEffect == -80);
        }
        public int calculatePercentage(double input)
        {
            // range is -80 to 20
            //so shift +80 then divide by 100
            return ((int)Math.Round(input + 80));
        }
    }
}
