using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public class ShipMover:MonoBehaviour
{
    [SerializeField] private AIMover _aiMover;
    [SerializeField] private List<Transform> _pointsToMove;
    [SerializeField] private int _levelNumber;
    [SerializeField] private CameraControllRoadMap _cameraControll;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private bool _startScene=false;

    private void Start()
    {
        if (!_startScene)
        {
            if (TimerAds.Finished)
            {
                AdsManager.ShowInterstitial((bool value) =>
                {
                    //LoadNewScene();
                });
            }
        }
        
        if (SaveManager.instance.NextLevel > 0)
        { 
            _levelNumber = SaveManager.instance.NextLevel;
        }
        else
        {
            _levelNumber = 1;
        }

        if (!_startScene)
        {
            transform.position = _pointsToMove[_levelNumber-1].position;
            _aiMover.SetPoint(_pointsToMove[_levelNumber].position);
        }
        else
        {
            transform.rotation = _pointsToMove[_levelNumber].rotation;
            transform.position = _pointsToMove[_levelNumber].position;
            _aiMover.SetPoint(_pointsToMove[_levelNumber].position);
        }
        
    }

    private void Update()
    {
        if (_aiMover.IsMoving)
        {
            //_cameraControll.enabled = false;
            _playButton.SetActive(false);
        }
        else
        {
           // _cameraControll.enabled = true;
            _playButton.SetActive(true);
        }
    }
}
