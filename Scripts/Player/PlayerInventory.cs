using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool WantToWorkWithInventory;
    [SerializeField] private InventoryBase _inventory;
    public InventoryBase Inventory => _inventory;
    [SerializeField] private UpgradeDataSO _upgradeData;
    [SerializeField] private string _name;
    [SerializeField] private UpgradeData _upgradeInfo;
    [SerializeField] private int _currentLevel;
    public int CurrentLevel => _currentLevel;
    [SerializeField] private float _inventorySpeed;
    private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    [SerializeField] private BackPackVisulizer _backPackVisualizer;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private int _lastBuildingLevel = -1;
    [SerializeField] private AudioSource _resourceOutSound;
    [SerializeField] private AudioSource _resourceInSond;

    public delegate void ResourceIn();

    public event ResourceIn ResourceInPlayer;

    public delegate void ResourceOut();

    public event ResourceOut ResourceOutOfPlayer;


    private void Start()
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
        _upgradeInfo = _upgradeData.UpgradeDatas.Find(x => x.Name == _name);
        Synchronize();
    }

    private void Synchronize()
    {
        _currentLevel = SaveManager.instance.PlayerSaveInfo.UpgradeLevel;
        _inventory.MaxCount = _upgradeInfo.UpgaradesGrades[_currentLevel].MaxInputStorage;
        _inventory.CurrentSpace = _inventory.MaxCount - _inventory.InventoryInfo.ResourcesInfo.Count;
        _inventorySpeed = _upgradeInfo.UpgaradesGrades[_currentLevel].CollectTime;
        _moveSpeed = _upgradeInfo.UpgaradesGrades[_currentLevel].CreationSpeed;
        if (SaveManager.instance.PlayerSaveInfo.CreateInventory.InventoryInfo.ResourcesInfo.Count > 0)
        {
            _inventory = SaveManager.instance.PlayerSaveInfo.CreateInventory;
        }

        for (int i = 0; i < _inventory.InventoryInfo.ResourcesInfo.Count; i++)
        {
            if (_backPackVisualizer.ResourcecsInInventory.Count < _inventory.InventoryInfo.ResourcesInfo.Count)
            {
                _backPackVisualizer.CreateResource(_inventory.InventoryInfo.ResourcesInfo[i].ID);
            }
        }
    }

    private void Save()
    {
        SaveManager.instance.PlayerSaveInfo.UpgradeLevel = _currentLevel;
        SaveManager.instance.PlayerSaveInfo.CreateInventory = _inventory;
    }

    private void Update()
    {
        _inventory.CurrentSpace = _inventory.MaxCount - _inventory.InventoryInfo.ResourcesInfo.Count;
        if (_upgradeManager.UpgradesData.Find(x => x.Name == _name).LevelsBought > _currentLevel)
        {
            _currentLevel = _upgradeManager.UpgradesData.Find(x => x.Name == _name).LevelsBought;
            Save();
            Synchronize();
        }

        if (!WantToWorkWithInventory)
        {
            _lastBuildingLevel = -1;
            StopAllCoroutines();
        }
    }

    public void GenerateResource(ResourceInfo resourceInfo)
    {
        if (_inventory.CurrentSpace > 0)
        {
            _inventory.Add(resourceInfo);
            _backPackVisualizer.CreateResource(resourceInfo.ID);
        }
    }

    public void Clear()
    {
        _inventory.InventoryInfo.ResourcesInfo.Clear();
        _inventory.CurrentSpace = _inventory.MaxCount;
        foreach (var visual in _backPackVisualizer.ResourcecsInInventory)
        {
            Destroy(visual.gameObject);
        }

        _backPackVisualizer.ResourcecsInInventory.Clear();
    }

    private void RestuckResources()
    {
        for (int i = 0; i < _backPackVisualizer.ResourcecsInInventory.Count; i++)
        {
            _backPackVisualizer.ResourcecsInInventory[i].GetComponent<ResourceMover>()
                .MoveResource(_backPackVisualizer.ResourcePoints[i], _backPackVisualizer.transform, false, true);
        }
    }

    public void AddResorceToInventory(ResourceInfo resourceInfo,
        BuildingCreateInventoryVisulizer buildingInventoryVisulizer = null, InventoryBase inventoryToRemove = null,
        InventoryBase inventoryToAdd = null)
    {
        StartCoroutine(AddResource(resourceInfo, buildingInventoryVisulizer, inventoryToRemove, inventoryToAdd));
    }

    public void RemoveResorceFromInventory(ResourceInfo resourceInfo, InventoryBase inventoryToRemove = null,
        InventoryBase inventoryToAdd = null, BuildingInventoryVisulizer visulizer = null)
    {
        StartCoroutine(RemoveResource(resourceInfo, inventoryToRemove, inventoryToAdd, visulizer));
    }

    public void RemoveResorceFromInventoryToUpgrade(List<UpgradeInfo> upgradeInfo, BuildingBase buildingBase = null,
        InventoryBase inventoryToRemove = null, InventoryBase inventoryToAdd = null, Transform point = null,
        bool isTanker = false)
    {
        if (buildingBase != null)
        {
            _lastBuildingLevel = buildingBase.CurrentLevel;
            StartCoroutine(RemoveResourceToUpgrde(upgradeInfo, buildingBase, inventoryToRemove, inventoryToAdd, point,
                isTanker));
        }
    }

    public void RemoveResorceFromInventoryToTrash(Transform point = null)
    {
        StartCoroutine(RemoveResourceToTrash(point));
    }

    private IEnumerator AddResource(ResourceInfo resourceInfo,
        BuildingCreateInventoryVisulizer buildingInventoryVisulizer = null, InventoryBase inventoryToRemove = null,
        InventoryBase inventoryToAdd = null)
    {
        while (WantToWorkWithInventory)
        {
            if (_inventory.MaxCount - _inventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                if (resourceInfo.ID != null)
                {
                    if (inventoryToRemove != null &&
                        inventoryToRemove.InventoryInfo.ResourcesInfo.Exists(x => x.ID == resourceInfo.ID))
                    {
                        if (buildingInventoryVisulizer != null)
                        {
                            if (buildingInventoryVisulizer.ResourcecsInInventory.Count > 0)
                            {
                                GameObject resource = buildingInventoryVisulizer.GetResource(
                                    _backPackVisualizer.ResourcePoints[_inventory.InventoryInfo.ResourcesInfo.Count],
                                    _backPackVisualizer.transform);
                                _backPackVisualizer.ResourcecsInInventory.Add(resource.transform);
                                if (inventoryToRemove.InventoryInfo.ResourcesInfo.Exists(x =>
                                        x.ID == resourceInfo.ID))
                                {
                                    inventoryToRemove.Remove(resourceInfo);
                                }

                                if (inventoryToAdd != null)
                                {
                                    inventoryToAdd.Add(resourceInfo);
                                }

                                ResourceInPlayer?.Invoke();
                                _inventory.Add(resourceInfo);
                            }
                        }
                        else
                        {
                            if (inventoryToRemove.InventoryInfo.ResourcesInfo.Exists(x => x.ID == resourceInfo.ID))
                            {
                                inventoryToRemove.Remove(resourceInfo);
                            }

                            if (inventoryToAdd != null)
                            {
                                inventoryToAdd.Add(resourceInfo);
                            }

                            _inventory.Add(resourceInfo);
                        }
                    }
                }
            }


            RestuckResources();
            Save();
            yield return new WaitForSecondsRealtime(_inventorySpeed);
        }
    }

    private IEnumerator RemoveResource(ResourceInfo resourceInfo, InventoryBase inventoryToRemove = null,
        InventoryBase inventoryToAdd = null, BuildingInventoryVisulizer visulizer = null)
    {
        while (WantToWorkWithInventory)
        {
            if (_inventory.CurrentSpace < _inventory.MaxCount)
            {
                if (_inventory.InventoryInfo.ResourcesInfo.Exists(x => x.ID == resourceInfo.ID))
                {
                    if (inventoryToRemove != null)
                    {
                        if (inventoryToRemove.InventoryInfo.ResourcesInfo.Exists(x => x.ID == resourceInfo.ID))
                        {
                            inventoryToRemove.Remove(resourceInfo);
                        }
                    }

                    if (inventoryToAdd != null)
                    {
                        if (inventoryToAdd.CurrentSpace > 0)
                        {
                            if (visulizer != null)
                            {
                                if (_backPackVisualizer.ResourcecsInInventory.Exists((x =>
                                        x.GetComponent<ResourceMover>().ID == resourceInfo.ID)))
                                {
                                    GameObject res = _backPackVisualizer.GetResource(resourceInfo.ID,
                                        visulizer.ResourcePoints[visulizer.ResourcecsInInventory.Count],
                                        visulizer.InventoryParent.transform);

                                    visulizer.ResourcecsInInventory.Add(res.transform);
                                }
                            }

                            inventoryToAdd.Add(resourceInfo);
                            ResourceOutOfPlayer?.Invoke();
                            _inventory.Remove(resourceInfo);
                        }
                    }
                    else
                    {
                        if (visulizer != null)
                        {
                            if (_backPackVisualizer.ResourcecsInInventory.Exists((x =>
                                    x.GetComponent<ResourceMover>().ID == resourceInfo.ID)))
                            {
                                GameObject res = _backPackVisualizer.GetResource(resourceInfo.ID,
                                    visulizer.ResourcePoints[visulizer.ResourcecsInInventory.Count],
                                    visulizer.InventoryParent.transform);
                                visulizer.ResourcecsInInventory.Add(res.transform);
                            }
                        }

                        ResourceOutOfPlayer?.Invoke();
                        _inventory.Remove(resourceInfo);
                    }
                }
            }

            RestuckResources();
            Save();
            yield return new WaitForSecondsRealtime(_inventorySpeed);
        }
    }

    private IEnumerator RemoveResourceToUpgrde(List<UpgradeInfo> upgradeInfo, BuildingBase buildingBase = null,
        InventoryBase inventoryToRemove = null, InventoryBase inventoryToAdd = null, Transform point = null,
        bool isTanker = false)
    {
        while (WantToWorkWithInventory)
        {
            if (buildingBase != null && _lastBuildingLevel == buildingBase.CurrentLevel)
            {
                if (buildingBase.WantToUpgrade)
                {
                    if (_inventory.CurrentSpace < _inventory.MaxCount)
                    {
                        if (!isTanker)
                        {
                            for (int i = 0; i < upgradeInfo.Count; i++)
                            {
                                if (buildingBase != null)
                                {
                                    if (!buildingBase.UpgradesCountCheck[i])
                                    {
                                        if (_inventory.InventoryInfo.ResourcesInfo.Exists(x =>
                                                x.ID == upgradeInfo[i].ResourceInfo.ID))
                                        {
                                            ResourceOutOfPlayer?.Invoke();
                                            if (inventoryToRemove != null)
                                            {
                                                inventoryToRemove.Remove(upgradeInfo[i].ResourceInfo);
                                            }

                                            if (inventoryToAdd != null)
                                            {
                                                inventoryToAdd.Add(upgradeInfo[i].ResourceInfo);
                                                if (point != null)
                                                {
                                                    _backPackVisualizer.GetResource(upgradeInfo[i].ResourceInfo.ID,
                                                        point,
                                                        point, true);
                                                }

                                                _inventory.Remove(upgradeInfo[i].ResourceInfo);
                                            }
                                            else
                                            {
                                                if (point != null)
                                                {
                                                    _backPackVisualizer.GetResource(upgradeInfo[i].ResourceInfo.ID,
                                                        point,
                                                        point, true);
                                                }

                                                _inventory.Remove(upgradeInfo[i].ResourceInfo);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < _inventory.InventoryInfo.ResourcesInfo.Count; i++)
                            {
                                if (buildingBase != null)
                                {
                                    ResourceOutOfPlayer?.Invoke();
                                    if (inventoryToRemove != null)
                                    {
                                        inventoryToRemove.Remove(_inventory.InventoryInfo.ResourcesInfo[i]);
                                    }

                                    if (inventoryToAdd != null)
                                    {
                                        inventoryToAdd.Add(_inventory.InventoryInfo.ResourcesInfo[i]);
                                        if (point != null)
                                        {
                                            _backPackVisualizer.GetResource(
                                                _inventory.InventoryInfo.ResourcesInfo[i].ID,
                                                point,
                                                point, true);
                                        }

                                        _inventory.Remove(_inventory.InventoryInfo.ResourcesInfo[i]);
                                        yield return new WaitForSecondsRealtime(_inventorySpeed);
                                    }
                                    else
                                    {
                                        if (point != null)
                                        {
                                            _backPackVisualizer.GetResource(
                                                _inventory.InventoryInfo.ResourcesInfo[i].ID,
                                                point,
                                                point, true);
                                        }

                                        _inventory.Remove(_inventory.InventoryInfo.ResourcesInfo[i]);
                                        yield return new WaitForSecondsRealtime(_inventorySpeed);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            RestuckResources();
            Save();
            yield return new WaitForSecondsRealtime(_inventorySpeed);
        }
    }

    private IEnumerator RemoveResourceToTrash(Transform point = null)
    {
        while (WantToWorkWithInventory)
        {
            if (_inventory.CurrentSpace < _inventory.MaxCount)
            {
                for (int i = 0; i < _inventory.InventoryInfo.ResourcesInfo.Count; i++)
                {
                    if (point != null)
                    {
                        _backPackVisualizer.GetResource(_inventory.InventoryInfo.ResourcesInfo[i].ID, point, point,
                            true);
                    }

                    ResourceOutOfPlayer?.Invoke();
                    _inventory.Remove(_inventory.InventoryInfo.ResourcesInfo[i]);
                    yield return new WaitForSecondsRealtime(_inventorySpeed);
                    RestuckResources();
                    Save();
                }
            }
            else
            {
                yield return new WaitForSecondsRealtime(_inventorySpeed);
            }
        }
    }
}