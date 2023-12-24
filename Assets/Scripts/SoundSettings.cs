using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Slider soundFXSlider;
    [SerializeField] private AudioMixer soundfxMixer;
    // Start is called before the first frame update
    public void SetMusicVolume(){
        float volume = musicSlider.value;
        musicMixer.SetFloat("music", MathF.Log10(volume)*20);
    }

    public void SetSoundVolume(){
        float volume = soundFXSlider.value;
        soundfxMixer.SetFloat("soundfx", MathF.Log10(volume)*20);
    }
}   
