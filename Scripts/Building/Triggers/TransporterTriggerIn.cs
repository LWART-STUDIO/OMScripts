using System.Collections;
using UnityEngine;

public class TransporterTriggerIn : MonoBehaviour
{
    [SerializeField] private BuildingCreateInventoryVisulizer _buildingCreateInventoryVisulizer;
    [SerializeField] private TransporterPoint _transporterPoint;
    [SerializeField] private float _timeToMove;
    [SerializeField] private OilPump _oilPump;
    [SerializeField] private ResourceInfo _resourceToWork;
    [SerializeField] private Transporter _transporter;

    private void Start()
    {
        StartCoroutine(MoveResource());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TransporterPoint transporterPoint))
        {
            _transporterPoint = transporterPoint;
        }
    }

    private IEnumerator MoveResource()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(_timeToMove);
            if (_transporterPoint!=null &&_transporterPoint.Empty&& _transporter.Work&&_buildingCreateInventoryVisulizer.ResourcecsInInventory.Count>0)
            {
                
                _transporterPoint.GetResource(_buildingCreateInventoryVisulizer
                    .ResourcecsInInventory[_buildingCreateInventoryVisulizer.ResourcecsInInventory.Count-1]
                    .GetComponent<ResourceMover>());
                _oilPump.CreateInventory.Remove(_resourceToWork);
                var transform1 = _transporterPoint.transform;
                _buildingCreateInventoryVisulizer.GetResource(transform1, transform1.parent,false,true);
            }
        }
        
    }
}