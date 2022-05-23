using UnityEngine;
using TMPro;

public class MechanicWantEatUI : MonoBehaviour
{
    [SerializeField] private MechanicSpawner _mechanicSpawner;
    [SerializeField] private ProgresBarDrawer _progresBarDrawer;
    [SerializeField] private int _lastCount;
    private int _currentCount;
    private float _time;
    [SerializeField] private GameObject _needFoodIcon;
    [SerializeField] private bool _worked;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _redCirlcal;


    private void Update()
    {
        _worked = _mechanicSpawner.Worked;
        if (_mechanicSpawner.ResourceNeedInventory is {InventoryInfo: {ResourcesInfo: { }}})
        {
            if (_mechanicSpawner.ResourceNeedInventory.CurrentSpace > 0)
            {
                if (_mechanicSpawner.ResourceNeedInventory.CurrentSpace ==
                    _mechanicSpawner.ResourceNeedInventory.MaxCount)
                {
                    if(_needFoodIcon!=null)
                        _needFoodIcon.SetActive(true);
                }
                else
                {
                    if(_needFoodIcon!=null) 
                        _needFoodIcon.SetActive(false);
                }

                _text.text = (_mechanicSpawner.ResourceNeedInventory.MaxCount -
                              _mechanicSpawner.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count)
                    .ToString();
            }
            else
            {
                if(_needFoodIcon!=null) 
                    _needFoodIcon.SetActive(false);
            }

            if (_mechanicSpawner.CurrentLevel > 0)
            {

                _progresBarDrawer.gameObject.SetActive(true);
            }
            else
            {
                _progresBarDrawer.gameObject.SetActive(false);
            }

            _progresBarDrawer.MaximumValue = _mechanicSpawner.ResourceNeedInventory.MaxCount;
            _currentCount = _mechanicSpawner.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count;
            _progresBarDrawer.MinimumValue = 0;
            if (_lastCount != _currentCount)
            {
                _lastCount = _currentCount;
                _time = 0;
            }
        }
        else
        {
            _progresBarDrawer.gameObject.SetActive(false);
            if(_needFoodIcon!=null) 
                _needFoodIcon.SetActive(false);
        }

        if (_mechanicSpawner.CurrentLevel <= 0)
        {
            if(_needFoodIcon!=null)
                _needFoodIcon.SetActive(false);
        }

        /*if(_miniPump.ResourceNeedInventory != null)
        {
            _time += Time.unscaledDeltaTime / _miniPump.InventorySpeed / _miniPump.ResourceNeedInventory.MaxCount;
        }

        _progresBarDrawer.CurrentValue = Mathf.Lerp(_currentCount, 0, _time);*/
        if (_mechanicSpawner.ResourceNeedInventory != null)
        {
            if (_mechanicSpawner.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count == 2)
            {
                if(_redCirlcal!=null)
                    _redCirlcal.SetActive(false);
                _progresBarDrawer.CurrentValue = 3;
            }
            else if (_mechanicSpawner.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count == 1)
            {
                if(_redCirlcal!=null)
                    _redCirlcal.SetActive(false);
                _progresBarDrawer.CurrentValue = 1.138f;
            }
            else if (_mechanicSpawner.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count == 0)
            {
                if(_redCirlcal!=null) 
                    _redCirlcal.SetActive(true);
                _progresBarDrawer.CurrentValue = 0f;
            }
        }
    }
}
