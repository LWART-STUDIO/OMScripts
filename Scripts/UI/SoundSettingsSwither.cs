using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettingsSwither : MonoBehaviour
{
    [SerializeField] private List<GameObject> _soundSwitcher;
    [SerializeField] private List<GameObject> _musicSwitcher;

    private void Update()
    {
        if (SaveManager.instance.MuteSounds)
        {
            _soundSwitcher[0].SetActive(false);
            _soundSwitcher[1].SetActive(true);
        }
        else
        {
            _soundSwitcher[0].SetActive(true);
            _soundSwitcher[1].SetActive(false);
        }
        if (SaveManager.instance.MuteMusic)
        {
            _musicSwitcher[0].SetActive(false);
            _musicSwitcher[1].SetActive(true);
        }
        else
        {
            _musicSwitcher[0].SetActive(true);
            _musicSwitcher[1].SetActive(false);
        }
    }
}
