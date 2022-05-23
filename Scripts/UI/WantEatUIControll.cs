using UnityEngine;
using TMPro;

public class WantEatUIControll : MonoBehaviour
{
    [SerializeField] private MiniPumpBase _miniPump;
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
        _worked = _miniPump.Worked;
        if (_miniPump.ResourceNeedInventory is {InventoryInfo: {ResourcesInfo: { }}})
        {
            if (_miniPump.ResourceNeedInventory.CurrentSpace>0)
            { 
                if(_miniPump.ResourceNeedInventory.CurrentSpace==_miniPump.ResourceNeedInventory.MaxCount)
                {
                    _needFoodIcon.SetActive(true);
                }
                else
                {
                    _needFoodIcon.SetActive(false);
                }
                _text.text = (_miniPump.ResourceNeedInventory.MaxCount - _miniPump.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count)
                    .ToString();
            }
            else
            {
                _needFoodIcon.SetActive(false);
            }

            if (_miniPump.CurrentLevel > 0)
            {
                
                _progresBarDrawer.gameObject.SetActive(true);
            }
            else
            {
                _progresBarDrawer.gameObject.SetActive(false);
            }
            _progresBarDrawer.MaximumValue = _miniPump.ResourceNeedInventory.MaxCount;
            _currentCount = _miniPump.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Count;
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
            _needFoodIcon.SetActive(false);
        }

        if (_miniPump.CurrentLevel <= 0)
        {
            _needFoodIcon.SetActive(false);
        }

        /*if(_miniPump.ResourceNeedInventory != null)
        {
            _time += Time.unscaledDeltaTime / _miniPump.InventorySpeed / _miniPump.ResourceNeedInventory.MaxCount;
        }

        _progresBarDrawer.CurrentValue = Mathf.Lerp(_currentCount, 0, _time);*/
        if (_miniPump.ResourceNeedInventory != null&&_miniPump.ResourceNeedInventory.InventoryInfo!=null&&_miniPump.ResourceNeedInventory.InventoryInfo.ResourcesInfo!=null)
        {
            if (_miniPump.ResourceNeedInventory?.InventoryInfo?.ResourcesInfo.Count == 2)
            {
                _redCirlcal.SetActive(false);
                _progresBarDrawer.CurrentValue = 3;
            }
            else if (_miniPump.ResourceNeedInventory?.InventoryInfo?.ResourcesInfo.Count == 1)
            {
                _redCirlcal.SetActive(false);
                _progresBarDrawer.CurrentValue = 1.138f;
            }
            else if (_miniPump.ResourceNeedInventory?.InventoryInfo?.ResourcesInfo.Count == 0)
            {
                _redCirlcal.SetActive(true);
                _progresBarDrawer.CurrentValue = 0f;
            }
        }
    }
}