using System.Collections.Generic;
using UnityEngine;

public class BackPackVisulizer : MonoBehaviour
{
    public List<Transform> ResourcePoints;
    public List<Transform> ResourcecsInInventory;
    [SerializeField] private AllResousesSO _allResousesSO;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        ResourcePoints.AddRange(GetComponentsInChildren<Transform>());
        ResourcePoints.RemoveAt(0);
    }
    public void CreateResource(string id)
    {
        GameObject resource = Instantiate(_allResousesSO.VisualisationInfo.Find(x => x.Name.ID == id).gameObject, _spawnPoint.position, _spawnPoint.rotation);
        ResourceMover resourceMover = resource.GetComponent<ResourceMover>();
        resourceMover.MoveResource(ResourcePoints[ResourcecsInInventory.Count], gameObject.transform);
        resourceMover.ID = id;
        ResourcecsInInventory.Add(resource.transform);
    }
    public GameObject GetResource(string id,Transform pointToMove, Transform parentObject,bool remove=false)
    {
        GameObject resource = ResourcecsInInventory.FindLast(x=>x.GetComponent<ResourceMover>().ID==id).gameObject;
        ResourceMover resourceMover = resource.GetComponent<ResourceMover>();
        resourceMover.MoveResource(pointToMove, parentObject,remove);
        ResourcecsInInventory.Remove(resource.transform);
        for(int i=0;i< ResourcecsInInventory.Count; i++)
        {
            ResourcecsInInventory[i].position = ResourcePoints[i].position;
        }
        return resource;
    }
}

