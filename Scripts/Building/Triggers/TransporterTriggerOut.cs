using System.Collections;
using UnityEngine;

public class TransporterTriggerOut : MonoBehaviour
{
    [SerializeField] private BuildingInventoryVisulizer _buildingInventoryVisulizer;
    [SerializeField] private TransporterPoint _transporterPoint;
    [SerializeField] private PlasticCreator _plasticCreator;
    [SerializeField] private ResourceInfo _resourceToWork;
    [SerializeField] private Animator _trasporterAnimator;
    [SerializeField] private Transporter _transporter;
    private bool _wantToGive = false;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TransporterPoint transporterPoint))
        {
            _transporterPoint = transporterPoint;
            if (_transporterPoint.Empty == false&&_plasticCreator.ResourceNeedInventory.CurrentSpace>0)
            {
                
                GameObject res= _transporterPoint.GiveResource(_buildingInventoryVisulizer.ResourcePoints[_buildingInventoryVisulizer.ResourcecsInInventory.Count],
                    _buildingInventoryVisulizer.InventoryParent.transform);
                _buildingInventoryVisulizer.ResourcecsInInventory.Add(res.transform);
                _plasticCreator.ResourceNeedInventory.Add(_resourceToWork);
            }
            else if(_transporterPoint.Empty == false && _plasticCreator.ResourceNeedInventory.CurrentSpace==0)
            {
                _trasporterAnimator.SetBool("Stop",true);
                _trasporterAnimator.speed = 0;
                _wantToGive = true;
                StartCoroutine(WantToGive());

            }
            
        }
    }

    private IEnumerator WantToGive()
    {
        while (_wantToGive)
        {
            yield return new WaitForSeconds(1f);
            if (_plasticCreator.ResourceNeedInventory.CurrentSpace > 0&& _transporter.Work)
            {
                GameObject res= _transporterPoint.GiveResource(_buildingInventoryVisulizer.ResourcePoints[_buildingInventoryVisulizer.ResourcecsInInventory.Count],
                    _buildingInventoryVisulizer.InventoryParent.transform);
                _buildingInventoryVisulizer.ResourcecsInInventory.Add(res.transform);
                _plasticCreator.ResourceNeedInventory.Add(_resourceToWork);
                _wantToGive = false;
                _trasporterAnimator.SetBool("Stop",false);
                _trasporterAnimator.speed = 1;

            }
        }

        
    }
}
