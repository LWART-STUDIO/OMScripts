using UnityEngine;

public class BackPackResizer : MonoBehaviour
{
    [SerializeField] private PlayerInventory _inventory;
    
    [SerializeField] private GameObject _backPackLong;
    [SerializeField] private GameObject _backPackShot;
    
    private void Update()
    {
        if (_inventory.Inventory.InventoryInfo.ResourcesInfo.Count > 0)
        {
            _backPackLong.SetActive(true);
            _backPackShot.SetActive(true);
            _backPackLong.transform.localPosition =new Vector3(0.0224f, 0f, 0f);
            _backPackLong.transform.localScale = new Vector3(1.176106f, 1,1);
            _backPackShot.transform.localPosition = new Vector3(-0.076f, 0, 0);
            if (_inventory.Inventory.InventoryInfo.ResourcesInfo.Count > 10)
            {
                _backPackLong.transform.localPosition = new Vector3(0.1521f, 0f, 0f);
                _backPackLong.transform.localScale = new Vector3(2.200509f, 1, 1);
                _backPackShot.transform.localPosition = new Vector3(-0.503f, 0, 0);
                if (_inventory.Inventory.InventoryInfo.ResourcesInfo.Count > 20)
                {
                    _backPackLong.transform.localPosition = new Vector3(0.272f, 0f, 0f);
                    _backPackLong.transform.localScale = new Vector3(3.148257f, 1, 1);
                    _backPackShot.transform.localPosition = new Vector3(-0.902f, 0, 0);
                }
            }

        }
        else
        {
            _backPackLong.SetActive(false);
            _backPackShot.SetActive(false);
        }
    }
}
