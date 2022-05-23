using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdShower : MonoBehaviour
{
    private CameraControllManager _cameraControllManager;
    private void Update()
    {
        if (_cameraControllManager == null)
        {
            _cameraControllManager = FindObjectOfType<CameraControllManager>();
        }
        if (TimerAds.Finished)
        {
            if (_cameraControllManager != null && _cameraControllManager.CameraMove == false)
            {
                ShowInterstitial();
                TimerAds.Reset();
            }
            else if(_cameraControllManager == null)
            {
                ShowInterstitial();
                TimerAds.Reset();
            }
            
        }
    }
    public void ShowInterstitial()
    {
        AdsManager.ShowInterstitial((bool value) =>
        {
            //LoadNewScene();
        });
    }
}
