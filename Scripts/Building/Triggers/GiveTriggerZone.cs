using System.Collections;
using UnityEngine;

public class GiveTriggerZone : MonoBehaviour
{
    [SerializeField] private BuildingBase _building;
    [SerializeField] private BuildingInventoryVisulizer _visulizer;
    [SerializeField] private AudioSource _audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            playerInventory.ResourceOutOfPlayer += PlaySound;
            playerInventory.WantToWorkWithInventory = true;
            if (playerInventory.Inventory.InventoryInfo.ResourcesInfo.Exists(x => x.ID == _building.NeedResource.ID))
            {
                playerInventory.RemoveResorceFromInventory(_building.NeedResource, null, _building.ResourceNeedInventory, _visulizer);
            }
            else
            {
                StartCoroutine(Work(playerInventory));
            }
            
        }
        else if (other.TryGetComponent<FoodWorkerAI>(out FoodWorkerAI aIMain))
        {
            if (aIMain.NameOfWork == _building.Name)
            {
                if (aIMain.Inventory.InventoryInfo.ResourcesInfo.Exists(x => x.ID == _building.NeedResource.ID))
                {
                    aIMain.RemoveResource(_building.NeedResource, null, _building.ResourceNeedInventory, _visulizer);
                }
                else
                {
                    StartCoroutine(WorkerWork(aIMain));
                }
            }
        }
        else if (other.TryGetComponent<WaterWorkerAI>(out WaterWorkerAI waterWorkerAI))
        {
            
            if (waterWorkerAI.NameOfWork == _building.Name)
            {
                
                if (waterWorkerAI.Inventory.InventoryInfo.ResourcesInfo.Exists(x => x.ID == _building.NeedResource.ID))
                {
                    
                    waterWorkerAI.RemoveResource(_building.NeedResource, null, _building.ResourceNeedInventory, _visulizer);
                    
                }
                else
                {
                    StartCoroutine(WorkerWork(waterWorkerAI));
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
                playerInventory.ResourceOutOfPlayer += PlaySound;
                playerInventory.WantToWorkWithInventory = true;
                if (playerInventory.Inventory.InventoryInfo.ResourcesInfo.Exists(x => x.ID == _building.NeedResource.ID))
                {
                    playerInventory.RemoveResorceFromInventory(_building.NeedResource, null,
                        _building.ResourceNeedInventory, _visulizer);
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
            playerInventory.ResourceOutOfPlayer -= PlaySound;
            playerInventory.WantToWorkWithInventory = false;

        }
    }
    private IEnumerator Work(PlayerInventory playerInventory)
    {
        while (playerInventory.WantToWorkWithInventory)
        {
            if (playerInventory.Inventory.InventoryInfo.ResourcesInfo.Exists(x => x.ID == _building.NeedResource.ID))
            {
                playerInventory.RemoveResorceFromInventory(_building.NeedResource, null, _building.ResourceNeedInventory, _visulizer);
                StopCoroutine(Work(playerInventory));
            }
            yield return null;
        }

    }
    private IEnumerator WorkerWork(AIMain aIMain)
    {
        
        while (aIMain.Inventory.InventoryInfo.ResourcesInfo.Count>0)
        {
            
            if (aIMain.Inventory.InventoryInfo.ResourcesInfo.Exists(x => x.ID == _building.NeedResource.ID))
            {
                aIMain.RemoveResource(_building.NeedResource, null, _building.ResourceNeedInventory, _visulizer);
                StopCoroutine(WorkerWork(aIMain));
            }
            yield return null;
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
