using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  DG;
using DG.Tweening;

public class AirDrop : MonoBehaviour
{
    [EditorButton(nameof(SpawnDron), "SpawnDron", activityType: ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(Open), "OpenLootBox", activityType: ButtonActivityType.OnPlayMode)]
    [SerializeField] private GameObject _lootBoxInDron;
    [SerializeField] private GameObject _mainLootBox;
    [SerializeField] private GameObject _dron;
    [SerializeField] private Animator _dronAnimator;
    [SerializeField] private Animator _mainLootBoxAnimator;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private CameraControllManager _cameraControllManager;
    [SerializeField] private SkinnedMeshRenderer[] _mainLootBoxMeshRenderers;
    private Tween _moveToDropTween;
    private Tween _moveToSpawnTween;
    [SerializeField] private LootBoxResources _lootBoxResources;
    [SerializeField] private AirDropTrigger _airDropTrigger;
    [SerializeField] private List<float> _timeToStay;
    [Space(6)] [SerializeField] private WaterPump _waterPump;
    [SerializeField] private FoodCreator _foodCreator;
    [SerializeField] private OilPump _oilPump;
    [SerializeField] private PlasticCreator _plasticCreator;
    [SerializeField] private bool _opend=false;
    [SerializeField] private bool _stay=false;
    [SerializeField] private bool _wait = false;
    [SerializeField] private int _currentIndex;
    [SerializeField] private GameObject _resourceCanvas;
    [SerializeField] private GameObject _setTrigger;
    [SerializeField] private GameObject _buttonParticals;
    [SerializeField] private GameObject _openParticals;
    [SerializeField] private BoxCollider _collider;
    public int CurrentIndex => _currentIndex;

    private void Start()
    {
        _collider = _mainLootBox.GetComponent<BoxCollider>();
        _mainLootBoxMeshRenderers = _mainLootBox.GetComponentsInChildren<SkinnedMeshRenderer>();
        _collider.enabled = false;
    }

    private void Update()
    {
        if (_plasticCreator.CurrentLevel > 2)
        {
            _currentIndex = 3;
        }
        else if (_plasticCreator.CurrentLevel > 1)
        {
            _currentIndex = 2;
        }
        else if (_oilPump.CurrentLevel > 2)
        {
            _currentIndex = 1;
        }
        else
        {
            _currentIndex = 0;
        }
        if (!_opend && !_stay&&!_wait&&_foodCreator.CurrentLevel>0)
        {
            _stay = true;
            StartCoroutine(SpawnDronDelay());
        }

        if (!_wait && !_stay && _opend&&_lootBoxResources.CreateInventory.InventoryInfo.ResourcesInfo.Count<=0)
        {
            StartCoroutine(WaitToSpawnNewDron());
        }
    }

    public void DeactivateMainLootBox()
    {
        _collider.enabled = false;
        foreach (var skinnedMeshRenderer in _mainLootBoxMeshRenderers)
        {
            skinnedMeshRenderer.enabled = false;
        }
        _openParticals.SetActive(false);
        _airDropTrigger.gameObject.SetActive(false);
        _buttonParticals.SetActive(false);
    }

    public void ActivateMainLootBox()
    {
        _collider.enabled = true;
        foreach (var skinnedMeshRenderer in _mainLootBoxMeshRenderers)
        {
            skinnedMeshRenderer.enabled = true;
        }
        _buttonParticals.SetActive(true);
        _resourceCanvas.SetActive(true);
        _airDropTrigger.gameObject.SetActive(true);
    }

    public void Open()
    {
        _setTrigger.SetActive(true);
        _resourceCanvas.SetActive(false);
        _airDropTrigger.gameObject.SetActive(false);
        StartCoroutine(OpenLootBox(_currentIndex,_timeToStay[_currentIndex]));
        
    }
    public void SpawnDron()
    {
        _wait = false;
        _stay = true;
        DeactivateMainLootBox();
        _lootBoxInDron.SetActive(true);
        _dron.SetActive(true);
        _dronAnimator.Play("Fly whith box");
        StartCoroutine(MoveToDrop());
    }

    public void DropBox()
    {
        _dronAnimator.Play("Fly dropping");
        _lootBoxInDron.SetActive(false);
        ActivateMainLootBox();
    }

    private IEnumerator OpenLootBox(int index,float time)
    {
        _mainLootBoxAnimator.Play("LootBox Open");
        yield return new WaitForSecondsRealtime(2f);
        _openParticals.SetActive(true);
        StartCoroutine(WaitOpen(index,time));
    }

    private IEnumerator WaitOpen(int index,float time)
    {
        _lootBoxResources.SpawnResources(index);
        yield return new WaitForSecondsRealtime(time);
        DeactivateMainLootBox();
        _mainLootBoxAnimator.Play("Idle");
        _stay = false;
        _opend = true;
    }
    private IEnumerator MoveToDrop()
    {
        _cameraControllManager.SwitchToDronCamera();
        _moveToDropTween = _dron.transform.DOLocalMove(Vector3.zero, 2f);
        yield return new WaitForSecondsRealtime(2f);
        DropBox();
       StartCoroutine(MoveToSpawn());
    }

    private IEnumerator MoveToSpawn()
    {
        yield return new WaitForSecondsRealtime(1f);
        _mainLootBoxAnimator.Play("LootBox Idle");
        _moveToSpawnTween= _dron.transform.DOLocalMove(_spawnPoint.position, 2f);
        StartCoroutine(DisableDron());
    }

    private IEnumerator DisableDron()
    {
        yield return _moveToSpawnTween.WaitForKill();
        _dron.SetActive(false);
    }

    private IEnumerator WaitToSpawnNewDron()
    {
        _wait = true;
        yield return new WaitForSecondsRealtime(150f);
        SpawnDron();
    }

    private IEnumerator SpawnDronDelay()
    {
        yield return new WaitForSeconds(5f);
        SpawnDron();
    }
}
