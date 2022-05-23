using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class BreakManager : MonoBehaviour
{
    [SerializeReference] private Shop _shop;
    [SerializeField] private List<BuildingBase> _buildings;
    [SerializeField] private int _lvlOfShopToStartBreaking;
    [SerializeField] private float _timeOutBetwenBreaks;
    [SerializeField] private float _breakeSpeed;
    private bool _work;
    [SerializeField] private BuildingBase _lastBreak;
    [SerializeField] private CameraControllManager _cameraControllManager;
    private bool _newLevel;
    [SerializeField] private PlasticCreator _plasticCreator;

    private void Update()
    {
        if (!_work)
        {
            if (_shop.CurrentLevel >= _lvlOfShopToStartBreaking)
            {
                if (SaveManager.instance.NewLevel)
                {
                    _newLevel = true;
                }
                StartCoroutine(StartBrake());
            }
        }
    }

    private IEnumerator StartBrake()
    {
        _work = true;
        yield return new WaitForSecondsRealtime(3f);
        
            StartCoroutine(BreakBuilding());
    }

    private IEnumerator BreakBuilding()
    {
        while (true)
        {
            if (_plasticCreator.CurrentLevel > 0)
            {
                BuildingBase buildingToBreak = _buildings[Random.Range(0, _buildings.Count)];
                if (buildingToBreak.CurrentLevel < 1)
                {
                    buildingToBreak = _buildings[0];
                }
                buildingToBreak.BreakSpeed = _breakeSpeed;
                _lastBreak = buildingToBreak;
                if (_newLevel)
                {
                    _cameraControllManager.SwitchCamera(buildingToBreak.Name);
                }
                yield return new WaitForSecondsRealtime(_timeOutBetwenBreaks);
                buildingToBreak.BreakSpeed = 1;
            }
            else
            {
                yield return new WaitForSecondsRealtime(_timeOutBetwenBreaks);
            }
            
        }
    }
}