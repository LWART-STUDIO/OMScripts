using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _sounds;
    [SerializeField] private List<AudioSource> _music;

    private void Update()
    {
        foreach (AudioSource sound in _sounds)
        {
            sound.mute = SaveManager.instance.MuteSounds;
        }

        foreach (AudioSource music in _music)
        {
            music.mute = SaveManager.instance.MuteMusic;
        }
    }

    public void SwitchMusic()
    {
        SaveManager.instance.MuteMusic = !SaveManager.instance.MuteMusic;
    }

    public void SwitchSounds()
    {
        SaveManager.instance.MuteSounds = !SaveManager.instance.MuteSounds;
    }
}
