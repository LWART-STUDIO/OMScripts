using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(2)]
public class BuildingInventoryVisulizer : MonoBehaviour
{
    [SerializeField] private GameObject _resourcePointsParent;
    [SerializeField] private List<Transform> _resourcePoints;
    public List<Transform> ResourcePoints => _resourcePoints;
    [SerializeField] private BuildingBase _building;
    [SerializeField] private AllResousesSO _allResousesSO;
    [SerializeField] private Transform _spawnPoint;
    public List<Transform> ResourcecsInInventory;
    public Transform InventoryParent;
   [SerializeField] private bool _start;

    private void Start()
    {
        StartCoroutine(LateStart());
        
        for (int i = 0; i < _resourcePointsParent.transform.childCount; i++)
        {
            _resourcePoints.Add(_resourcePointsParent.transform.GetChild(i));
        }

        if (_building.ResourceNeedInventory is {InventoryInfo: {ResourcesInfo: { }}})
        {
            for (int i = 0; i < _building.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count; i++)
            {
                GameObject resource = Instantiate(_allResousesSO.VisualisationInfo.Find(x => x.Name.ID == _building.ResourceNeedInventory.InventoryInfo.ResourcesInfo[i].ID).gameObject, _spawnPoint.position, _spawnPoint.rotation);
                ResourceMover resourceMover = resource.GetComponent<ResourceMover>();
                resourceMover.MoveResource(_resourcePoints[i], InventoryParent);
                resourceMover.ID = _building.ResourceNeedInventory.InventoryInfo.ResourcesInfo[i].ID;
                ResourcecsInInventory.Add(resource.transform);
                _start = true;
            }
        }
        
    }
    public GameObject GetResource(Transform pointToMove, Transform parentObject, bool remove = false)
    {
        GameObject resource = ResourcecsInInventory[ResourcecsInInventory.Count - 1].gameObject;
        ResourceMover resourceMover = resource.GetComponent<ResourceMover>();
        resourceMover.MoveResource(pointToMove, parentObject, remove);
        ResourcecsInInventory.Remove(resource.transform);
        return resource;
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
    private IEnumerator LateStart()
    {
        yield return new WaitForSecondsRealtime(0.2f);
       
    }
}
