using System.Collections.Generic;
using UnityEngine;

public class UpgradeTriggerZone : MonoBehaviour
{
    [SerializeField] private BuildingBase _building;
    private List<UpgradeInfo> _upgradeInfo;
    [SerializeField] private Transform _getPoint;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _isTanker;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            playerInventory.ResourceOutOfPlayer += PlaySound;
            UpgradeData upgradeData =_building.UpgradeData.UpgradeDatas.Find(x => x.Name ==_building.Name);
            _upgradeInfo = upgradeData.UpgaradesGrades[_building.CurrentLevel].UpgradeResourses;
            playerInventory.WantToWorkWithInventory = true;
            playerInventory.RemoveResorceFromInventoryToUpgrade(_upgradeInfo,_building,null, _building.UpgradeInventory,_getPoint,_isTanker);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            if (!playerInventory.WantToWorkWithInventory)
            {
                playerInventory.ResourceOutOfPlayer += PlaySound;
                UpgradeData upgradeData =_building.UpgradeData.UpgradeDatas.Find(x => x.Name ==_building.Name);
                _upgradeInfo = upgradeData.UpgaradesGrades[_building.CurrentLevel].UpgradeResourses;
                playerInventory.WantToWorkWithInventory = true;
                playerInventory.RemoveResorceFromInventoryToUpgrade(_upgradeInfo,_building,null, _building.UpgradeInventory,_getPoint,_isTanker);
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
    public void PlaySound()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }
}
