using System.Collections;
using UnityEngine;

public class MechanicTriggerZone : MonoBehaviour
{
    [SerializeField] private float _repairTime;
    [SerializeField] private BuildingBase _buildingBase;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MechanicWorkerAI mechanicWorkerAI))
        {
            if (mechanicWorkerAI.Tasks[0].Name == _buildingBase.Name)
            {
                StartCoroutine(StartWork(mechanicWorkerAI));
            }
            
        }
    }

    private IEnumerator StartWork(MechanicWorkerAI mechanicWorkerAI)
    {
        mechanicWorkerAI.Repeir = true;
        yield return new WaitForSecondsRealtime(_repairTime);
        _buildingBase.Repear(); 
        //mechanicWorkerAI.Repeir = false;
        mechanicWorkerAI.WorkDone = true;
    }
}
