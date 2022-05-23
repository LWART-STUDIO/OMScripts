using UnityEngine;

public class TriggerOffer : MonoBehaviour
{
    [SerializeField] private bool _upgradeTrigger;
    [SerializeField] private bool _getTrigger;
    [SerializeField] private bool _setTrigger;
    [SerializeField] private GameObject _upgradeTriggerObject=null;
    [SerializeField] private GameObject _getTriggerObject=null;
    [SerializeField] protected GameObject _setTriggerObject=null;
    private bool _upgradeWasBeActive;
    private bool _getWasBeActive;
    private bool _setWasBeActive;
    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {
            if (_upgradeTrigger)
            {
                if (_getTriggerObject != null && _getTriggerObject.activeSelf)
                {
                    _getWasBeActive = true;
                    _getTriggerObject.SetActive(false);
                }
                else
                {
                    _getWasBeActive = false;
                }
                if (_setTriggerObject != null && _setTriggerObject.activeSelf)
                {
                    _setWasBeActive = true;
                    _setTriggerObject.SetActive(false);
                }
                else
                {
                    _setWasBeActive = false;
                }
            }
            else if (_getTrigger)
            {
                if (_upgradeTriggerObject != null && _upgradeTriggerObject.activeSelf)
                {
                    _upgradeWasBeActive = true;
                    _upgradeTriggerObject.SetActive(false);
                }
                else
                {
                    _upgradeWasBeActive = false;
                }
                if (_setTriggerObject != null && _setTriggerObject.activeSelf)
                {
                    _setWasBeActive = true;
                    _setTriggerObject.SetActive(false);
                }
                else
                {
                    _setWasBeActive = false;
                }
            }
            else if (_setTrigger)
            {
                if (_getTriggerObject != null && _getTriggerObject.activeSelf)
                {
                    _getWasBeActive = true;
                    _getTriggerObject.SetActive(false);
                }
                else
                {
                    _getWasBeActive = false;
                }
                if (_upgradeTriggerObject != null && _upgradeTriggerObject.activeSelf)
                {
                    _upgradeWasBeActive = true;
                    _upgradeTriggerObject.SetActive(false);
                }
                else
                {
                    _upgradeWasBeActive = false;
                }
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))
        {

            if (_upgradeTrigger)
            {
                if (_setWasBeActive)
                {
                    _setTriggerObject.SetActive(true);
                }
                if (_getWasBeActive)
                {
                    _getTriggerObject.SetActive(true);
                }
            }
            else if (_getTrigger)
            {
                if (_setWasBeActive)
                {
                    _setTriggerObject.SetActive(true);
                }
                if (_upgradeWasBeActive)
                {
                    _upgradeTriggerObject.SetActive(true);
                }
            }
            else if (_setTrigger)
            {
                if (_upgradeWasBeActive)
                {
                    _upgradeTriggerObject.SetActive(true);
                }
                if (_getWasBeActive)
                {
                    _getTriggerObject.SetActive(true);
                }
            }

        }
    }

}
