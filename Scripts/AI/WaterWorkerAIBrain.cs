using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WaterWorkerAIBrain : MonoBehaviour
{

    [SerializeField] private List<WaterWorkerAI> _waterWorker;
    [SerializeField] private List<GameObject> _foodPoints;
    [FormerlySerializedAs("_foodCreater")] [SerializeField] private FoodCreator _foodCreator;
    [SerializeField] private GameObject _restPoint;

    private void Update()
    {
        CheckTasks();
    }

    private void CheckTasks()
    {
        for (int i = 0; i < _waterWorker.Count; i++)
        {
            WaterWorkerAI waterWorkerAI = _waterWorker[i];
            if (waterWorkerAI.Work == false && waterWorkerAI.Inventory.InventoryInfo.ResourcesInfo.Count <= 0 && waterWorkerAI.Tasks.Count == 0)
            {
                waterWorkerAI.SetTask("WaterPump", _foodPoints[Random.Range(0, _foodPoints.Count)].transform.position);
            }

            if (waterWorkerAI.Inventory.InventoryInfo.ResourcesInfo.Count > 0 && waterWorkerAI.Work == false && waterWorkerAI.Tasks.Count == 0)
            {

                if (_foodCreator != null)
                {
                    waterWorkerAI.SetTask(_foodCreator.Name, _foodCreator.transform.position);
                }
                else
                {
                    waterWorkerAI.SetTask("Rest", _restPoint.transform.position);
                    waterWorkerAI.Timer = 0;
                }
            }

            if (waterWorkerAI.Tasks[0].Name == "Rest" && waterWorkerAI.Timer > 10 && waterWorkerAI.Inventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                waterWorkerAI.RemoveTask("Rest");
 
                if (_foodCreator != null)
                {
                    waterWorkerAI.SetTask(_foodCreator.Name, _foodCreator.transform.position);
                }
                else
                {
                    waterWorkerAI.SetTask("Rest", _restPoint.transform.position);
                }
            }
        }
    }

}
