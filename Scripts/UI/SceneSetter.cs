using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetter : MonoBehaviour
{
    [SerializeField] private GameObject _audio;
    private bool _test=false;
    private void Awake()
    {
        if (!_test)
        {
            _audio.SetActive(false);
        }
        else
        {
            _audio.SetActive(true);
        }
        
    }
    public void SceneLoaded()
    {
        _audio.SetActive(true);
    }
    public void SceneUnload()
    {
        _audio.SetActive(false);
    }
}
