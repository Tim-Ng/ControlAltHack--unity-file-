using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Music {
    /// <summary>
    /// This script controls UI and function of the pop up setting for the music and sound effects.
    /// </summary>
    public class SliderPopUp : MonoBehaviour
    {
        /// <summary>
        /// This is the game object that holds all the elements for the music setting popup
        /// </summary>
        [Header("Game Objects")]
        [SerializeField] private GameObject PopUp = null;
        /// <summary>
        /// The game object involve in the music setting popup
        /// </summary>
        [SerializeField] private GameObject CrossOutMaster = null, CrossOutMusic = null, CrossOutSoundEffect = null;
        /// <summary>
        /// The text that states the percentage of the each controlable sounds volumes
        /// </summary>
        [Header("Text")]
        [SerializeField] private Text MasterAmount = null;
        /// <summary>
        /// The text that states the percentage of the each controlable sounds volumes
        /// </summary>
        [SerializeField] private Text MusicAmount = null, SoundEffectAmount = null;
        /// <summary>
        /// This is the slider element for the Master volume
        /// </summary>
        [Header("Sliders")]
        [SerializeField] private Slider Master = null;
        /// <summary>
        /// This is the slider element for the Music volume
        /// </summary>
        [SerializeField] private Slider Music = null;
        /// <summary>
        /// This is the slider element for the SoundEffect volume
        /// </summary>
        [SerializeField] private Slider SoundEffect = null;
        /// <summary>
        /// This is the element of the AudioMixer
        /// </summary>
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer = null;
        /// <summary>
        /// This is the function to change the popup from hidden to shown and shown to hidden
        /// </summary>
        public void OnClickButton()
        {
            PopUp.SetActive(!PopUp.activeSelf);
        }
        /// <summary>
        /// This function will run when this script is rendered
        /// </summary>
        private void Awake()
        {
            startSlider();
        }
        /// <summary>
        /// This function is used to setup the sliders to the current set volume
        /// </summary>
        public void startSlider()
        {
            Master.value = GlobalMusicContorler.MasterVolume;
            Music.value = GlobalMusicContorler.MusicVolume;
            SoundEffect.value = GlobalMusicContorler.SoundEffect;
            setMixer();
        }
        /// <summary>
        /// This funtion is called when it detects a change in value of the master volume sider 
        /// </summary>
        public void slideMaster()
        {
            GlobalMusicContorler.setMasterVolume(Master.value);
            setMixer();
        }
        /// <summary>
        /// This function is called to mute the master volume [if it is not muted] or set it to the default volume [if it is muted]
        /// </summary>
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
        /// <summary>
        /// This funtion is called when it detects a change in value of the music volume sider 
        /// </summary>
        public void slideMusic()
        {
            GlobalMusicContorler.setMusicVolume(Music.value);
            setMixer();
        }
        /// <summary>
        /// This function is called to mute the music volume [if it is not muted] or set it to the default volume [if it is muted]
        /// </summary>
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
        /// <summary>
        /// This funtion is called when it detects a change in value of the sound effect volume sider 
        /// </summary>
        public void slideSound()
        {
            GlobalMusicContorler.setSoundEffectVolume(SoundEffect.value);
            setMixer();
        }
        /// <summary>
        /// This function is called to mute the sound effect volume [if it is not muted] or set it to the default volume [if it is muted]
        /// </summary>
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
        /// <summary>
        /// This function is called to set all the volumes to their default value 
        /// </summary>
        public void clickOnSetDefault()
        {
            GlobalMusicContorler.setDefaultVolumes();
            Master.value = GlobalMusicContorler.MasterVolume;
            Music.value = GlobalMusicContorler.MusicVolume;
            SoundEffect.value = GlobalMusicContorler.SoundEffect;
            setMixer();
        }
        /// <summary>
        /// This is to set the Mixer to the set volume
        /// </summary>
        public void setMixer()
        {
            audioMixer.SetFloat("Master", GlobalMusicContorler.MasterVolume);
            audioMixer.SetFloat("Music", GlobalMusicContorler.MusicVolume);
            audioMixer.SetFloat("SoundEffect", GlobalMusicContorler.SoundEffect);
            detectAmountChange();
        }
        /// <summary>
        /// This is used to display the persentage of each volume
        /// </summary>
        public void detectAmountChange()
        {
            MasterAmount.text= (calculatePercentage(GlobalMusicContorler.MasterVolume)+"%");
            MusicAmount.text = (calculatePercentage(GlobalMusicContorler.MusicVolume) + "%");
            SoundEffectAmount.text = (calculatePercentage(GlobalMusicContorler.SoundEffect) + "%");
            CrossOutMaster.SetActive(GlobalMusicContorler.MasterVolume == -80);
            CrossOutMusic.SetActive(GlobalMusicContorler.MusicVolume == -80);
            CrossOutSoundEffect.SetActive(GlobalMusicContorler.SoundEffect == -80);
        }
        /// <summary>
        /// This is used to calculate the persentage to the set volume
        /// </summary>
        /// <param name="input"> the amount of current value to change to percentage</param>
        /// <returns>The rounded value of the percentage of the input</returns>
        public int calculatePercentage(double input)
        {
            // range is -80 to 20
            //so shift +80 then divide by 100
            return ((int)Math.Round(input + 80));
        }
    }
}
