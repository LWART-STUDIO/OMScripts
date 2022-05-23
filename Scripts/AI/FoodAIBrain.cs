using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FoodAIBrain : MonoBehaviour
{

    [SerializeField] private List<FoodWorkerAI> _foodWorkers;
    [SerializeField] private List<GameObject> _foodPoints;
    [SerializeField] private List<MiniPumpBase> _miniPumps;
    [SerializeField] private MiniPumpBase _hungryMiniPump;
    [SerializeField] private List<GameObject> _restPoint;
    [SerializeField] private MechanicSpawner _mechanicSpawner;
    [SerializeField] private Transform _mechanicFeedPoint;

    private void Update()
    {
        CheckTasks();
    }

    private void CheckTasks()
    {
        for (int i = 0; i < _foodWorkers.Count; i++)
        {
            FoodWorkerAI foodWorker = _foodWorkers[i];
            if (foodWorker.Work == false && foodWorker.Inventory.InventoryInfo.ResourcesInfo.Count <= 0 && foodWorker.Tasks.Count == 0)
            {
                foodWorker.SetTask("FoodCreator", _foodPoints[i].transform.position);
            }

            if (foodWorker.Inventory.InventoryInfo.ResourcesInfo.Count > 0 && foodWorker.Work == false && foodWorker.Tasks.Count == 0)
            {
                foodWorker.RemoveTask("FoodCreator");
                if (_mechanicSpawner.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count <= 0&&_mechanicSpawner.CurrentLevel>0)
                {
                    foodWorker.SetTask(_mechanicSpawner.Name, _mechanicFeedPoint.position);
                }
                else
                {
                    MiniPumpBase miniPump = FoundMostHungryPump();
                    if (miniPump != null)
                    {
                        foodWorker.SetTask(miniPump.Name, miniPump.transform.position);
                    }
                    else
                    {
                        foodWorker.SetTask("Rest", _restPoint[i].transform.position);
                        foodWorker.Timer = 0;
                    }
                }
               
            }

            if (foodWorker.Tasks[0].Name == "Rest" && foodWorker.Timer > 3 && foodWorker.Inventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                
                foodWorker.RemoveTask("Rest");
                foodWorker.Timer = 0;
                if (_mechanicSpawner.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count <=0&&_mechanicSpawner.CurrentLevel>0)
                {
                    foodWorker.SetTask(_mechanicSpawner.Name, _mechanicFeedPoint.position);
                }
                else
                {
                    MiniPumpBase miniPump = FoundMostHungryPump();
                    if (miniPump != null)
                    {
                        foodWorker.SetTask(miniPump.Name, miniPump.transform.position);
                    }
                    else
                    {
                        foodWorker.SetTask("Rest", _restPoint[i].transform.position);
                    }
                }
                
            }

            if (foodWorker.Tasks[0].Name != "Rest" && foodWorker.Timer > 16&& foodWorker.Inventory.InventoryInfo.ResourcesInfo.Count>0)
            {
                foodWorker.RemoveTask(foodWorker.Tasks[0].Name);
                foodWorker.SetTask("Rest", _restPoint[i].transform.position);
                foodWorker.Timer = 0;
            }
        }
    }

    private MiniPumpBase FoundMostHungryPump()
    {
        for (int i = 0; i < _miniPumps.Count; i++)
        {
            MiniPumpBase miniPump = _miniPumps[i];
            if (_hungryMiniPump != null)
            {
                if (miniPump.ResourceNeedInventory.CurrentSpace > 0 && miniPump != _hungryMiniPump && miniPump.CurrentLevel > 0)
                {
                    _hungryMiniPump = miniPump;
                    if (miniPump.ResourceNeedInventory.CurrentSpace == miniPump.ResourceNeedInventory.MaxCount)
                    {
                        _hungryMiniPump = miniPump;
                        return miniPump;
                    }
                    return miniPump;
                }
            }
            else
            {
                if (miniPump.ResourceNeedInventory.CurrentSpace > 0 && miniPump && miniPump.CurrentLevel > 0)
                {
                    _hungryMiniPump = miniPump;
                    if (miniPump.ResourceNeedInventory.CurrentSpace == miniPump.ResourceNeedInventory.MaxCount)
                    {
                        _hungryMiniPump = miniPump;
                        return miniPump;
                    }
                    return miniPump;
                }
            }

        }
        return null;
    }

}
