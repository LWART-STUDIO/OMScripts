using System;
using System.Collections;
using UnityEngine;

public class ColectTriggerZone : MonoBehaviour
{
    [SerializeField] private BuildingBase _building;
    [SerializeField] private BuildingCreateInventoryVisulizer _buildingInventoryVisulizer;
    private ResourceInfo _lastResourceInfo;

    [SerializeField] private AudioSource _audioSource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            playerInventory.ResourceInPlayer += PlaySound;
            playerInventory.WantToWorkWithInventory = true;

            if (_building.CreateInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                _lastResourceInfo =
                    _building.CreateInventory.InventoryInfo.ResourcesInfo[
                        _building.CreateInventory.InventoryInfo.ResourcesInfo.Count - 1];
                playerInventory.AddResorceToInventory(_lastResourceInfo, _buildingInventoryVisulizer, _building.CreateInventory, null);
                StartCoroutine(TryFindAnotherResource(playerInventory));
            }
            else
            {
                StartCoroutine(Work(playerInventory));
            }



        }
        else if(other.TryGetComponent<FoodWorkerAI>(out FoodWorkerAI aIMain))
        {
            
            if (aIMain.NameOfWork == _building.Name)
            {
                if (_building.CreateInventory.InventoryInfo.ResourcesInfo.Count > 0)
                {
                    
                    aIMain.AddResource(_building.CreateInventory.InventoryInfo.ResourcesInfo[_building.CreateInventory.InventoryInfo.ResourcesInfo.Count - 1], _buildingInventoryVisulizer, _building.CreateInventory, null);
                    
                }
                else
                {
                    StartCoroutine(WorkerWork(aIMain));
                }
            }
        }
        else if(other.TryGetComponent<WaterWorkerAI>(out WaterWorkerAI workerAI))
        {
            
            if (workerAI.NameOfWork == _building.Name)
            {
                if (_building.CreateInventory.InventoryInfo.ResourcesInfo.Count > 0)
                {
                    
                    workerAI.AddResource(_building.CreateInventory.InventoryInfo.ResourcesInfo[_building.CreateInventory.InventoryInfo.ResourcesInfo.Count - 1], _buildingInventoryVisulizer, _building.CreateInventory, null);
                    
                }
                else
                {
                    StartCoroutine(WorkerWork(workerAI));
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            if (!playerInventory.WantToWorkWithInventory)
            {
                playerInventory.ResourceInPlayer += PlaySound;
                playerInventory.WantToWorkWithInventory = true;

                if (_building.CreateInventory.InventoryInfo.ResourcesInfo.Count > 0)
                {
                    _lastResourceInfo =
                        _building.CreateInventory.InventoryInfo.ResourcesInfo[
                            _building.CreateInventory.InventoryInfo.ResourcesInfo.Count - 1];
                    playerInventory.AddResorceToInventory(_lastResourceInfo, _buildingInventoryVisulizer,
                        _building.CreateInventory, null);
                    StartCoroutine(TryFindAnotherResource(playerInventory));
                }
                else
                {
                    StartCoroutine(Work(playerInventory));
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            playerInventory.ResourceInPlayer -= PlaySound;
            playerInventory.WantToWorkWithInventory = false;

        }
    }
    private IEnumerator Work(PlayerInventory playerInventory)
    {
        while (playerInventory.WantToWorkWithInventory)
        {
            if (_building.CreateInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                playerInventory.AddResorceToInventory(_building.CreateInventory.InventoryInfo.ResourcesInfo[_building.CreateInventory.InventoryInfo.ResourcesInfo.Count - 1], _buildingInventoryVisulizer, _building.CreateInventory, null);
                StopCoroutine(Work(playerInventory));
            }
            yield return null;
        }

    }
    private IEnumerator WorkerWork(AIMain aIMain)
    {
        while (aIMain.Inventory.InventoryInfo.ResourcesInfo.Count<=0)
        {
            if (_building.CreateInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                aIMain.AddResource(_building.CreateInventory.InventoryInfo.ResourcesInfo[_building.CreateInventory.InventoryInfo.ResourcesInfo.Count - 1], _buildingInventoryVisulizer, _building.CreateInventory, null);
                StopCoroutine(WorkerWork(aIMain));
            }
            yield return null;
        }

    }

    private IEnumerator TryFindAnotherResource(PlayerInventory playerInventory)
    {
        while (playerInventory.WantToWorkWithInventory)
        {
            if (_building.CreateInventory.InventoryInfo.ResourcesInfo.Count > 0)
            {
                if (_building.CreateInventory.InventoryInfo.ResourcesInfo[
                        _building.CreateInventory.InventoryInfo.ResourcesInfo.Count - 1] != _lastResourceInfo)
                {
                    _lastResourceInfo = _building.CreateInventory.InventoryInfo.ResourcesInfo[
                        _building.CreateInventory.InventoryInfo.ResourcesInfo.Count - 1];
                    playerInventory.AddResorceToInventory(_lastResourceInfo, _buildingInventoryVisulizer,
                        _building.CreateInventory, null);
                }
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    public void PlaySound()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
        
    }
}

