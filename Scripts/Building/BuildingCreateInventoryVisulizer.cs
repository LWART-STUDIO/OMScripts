using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreateInventoryVisulizer : MonoBehaviour
{
    [SerializeField] private GameObject _resourcePointsParent;
    [SerializeField] private List<Transform> _resourcePoints;
    [SerializeField] private BuildingBase _building;
    [SerializeField] private AllResousesSO _allResousesSO;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<Transform> _resourcecsInInventory;
    public List<Transform> ResourcecsInInventory => _resourcecsInInventory;
    [SerializeField] private Transform _inventoryParent;
    [SerializeField] private bool _start;

    private void Start()
    {
        _resourcecsInInventory = new List<Transform>();
        StartCoroutine(LateStart());
        for (int i = 0; i < _resourcePointsParent.transform.childCount; i++)
        {
            _resourcePoints.Add(_resourcePointsParent.transform.GetChild(i));
        }
        _start = true;
        
    }

    private void Update()
    {
        if (_start)
        {
            if (_resourcePoints.Count == 0&&_resourcePointsParent!=null)
            {
                
                for (int i = 0; i < _resourcePointsParent.transform.childCount; i++)
                {
                    _resourcePoints.Add(_resourcePointsParent.transform.GetChild(i));
                }
            }
        }
    }

    public void CreateResource(string id)
    {
        GameObject resource = Instantiate(_allResousesSO.VisualisationInfo.Find(x => x.Name.ID == id).gameObject, _spawnPoint.position, _spawnPoint.rotation);
        ResourceMover resourceMover = resource.GetComponent<ResourceMover>();
        if (_resourcecsInInventory.Count>0)
        {
            resourceMover.MoveResource(_resourcePoints[_resourcecsInInventory.Count], _inventoryParent);
        }
        else
        {
            resourceMover.MoveResource(_resourcePoints[0], _inventoryParent);
        }
        resourceMover.ID = id;
        _resourcecsInInventory.Add(resource.transform);
    }
    public GameObject GetResource(Transform pointToMove, Transform parentObject, bool remove = false,bool useDG=false)
    {
        GameObject resource = _resourcecsInInventory[_resourcecsInInventory.Count - 1].gameObject;
        ResourceMover resourceMover = resource.GetComponent<ResourceMover>();
        resourceMover.MoveResource(pointToMove, parentObject, remove,useDG);
        _resourcecsInInventory.Remove(resource.transform);
        return resource;
    }
    private IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        for (int i = 0; i < _resourcePointsParent.transform.childCount; i++)
        {
            _resourcePoints.Add(_resourcePointsParent.transform.GetChild(i));
        }
        for (int i = 0; i < _building.CreateInventory.InventoryInfo.ResourcesInfo.Count; i++)
        {

            GameObject resource = Instantiate(_allResousesSO.VisualisationInfo.Find(x => x.Name.ID == _building.CreateInventory.InventoryInfo.ResourcesInfo[i].ID).gameObject, _spawnPoint.position, _spawnPoint.rotation);
            ResourceMover resourceMover = resource.GetComponent<ResourceMover>();
            resourceMover.MoveResource(_resourcePoints[i], _inventoryParent);
            resourceMover.ID = _building.CreateInventory.InventoryInfo.ResourcesInfo[i].ID;
            _resourcecsInInventory.Add(resource.transform);
        }
    }
}
