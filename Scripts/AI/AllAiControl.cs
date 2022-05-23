using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAiControl : MonoBehaviour
{
    [EditorButton(nameof(GoToTanker), "GoToTanker", activityType: ButtonActivityType.OnPlayMode)] [SerializeField]
    private Shop _shop;

    [SerializeField] private List<MiniPumpBase> _miniPumps;
    [SerializeField] private List<GameObject> _pumpWorkers;
    [SerializeField] private List<GameObject> _foodWorker;
    [SerializeField] private List<GameObject> _waterWorkers;
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private FoodCreator _foodCreator;
    [SerializeField] private WaterPump _waterPump;
    [SerializeField] private Transform _tankerPoint;
    [SerializeField] private List<MechanicWorkerAI> _mechanicWorker;
    [SerializeField] private PlayerAIBrain _playerAIBrain;
    [SerializeField] private TankerAnimationControll _tankerAnimationControll;
    [SerializeField] private int _countOfPumpWorkers=0;
    private int _lastFoodCreatorLevel;
    private int _lastWaterPupmLevel;


    private void Start()
    {
        _tankerAnimationControll = FindObjectOfType<TankerAnimationControll>();
        for (int i = 0; i < _miniPumps.Count; i++)
        {
            if (_miniPumps[i].CurrentLevel > 0)
            {
                _countOfPumpWorkers++;
            }
        }
        StartCoroutine(PumpWorkerActivation());
        if (_upgradeManager.UpgradesData.Find(x => x.Name == "AILevel").LevelsBought > 0 &&
            _foodCreator.CurrentLevel > 0)
        {
            if (_lastFoodCreatorLevel < _foodCreator.CurrentLevel)
            {
                _lastFoodCreatorLevel = _foodCreator.CurrentLevel;
            }
            _foodWorker[0].SetActive(true);
            if (_upgradeManager.UpgradesData.Find(x => x.Name == "AILevel").LevelsBought > 1 &&
                _waterPump.CurrentLevel > 0)
            {
                if (_lastWaterPupmLevel < _waterPump.CurrentLevel)
                {
                    _lastWaterPupmLevel = _waterPump.CurrentLevel;
                }
                _waterWorkers[0].SetActive(true);
                if (_upgradeManager.UpgradesData.Find(x => x.Name == "AILevel").LevelsBought > 2)
                {
                    if (_lastFoodCreatorLevel < _foodCreator.CurrentLevel)
                    {
                        _lastFoodCreatorLevel = _foodCreator.CurrentLevel;
                    }
                    _foodWorker[1].SetActive(true);
                }
            }
        }
    }

    private void Update()
    {
        if (_upgradeManager.UpgradesData.Find(x => x.Name == "AILevel").LevelsBought > 0 &&
            _foodCreator.CurrentLevel > 0)
        {
            if (_lastFoodCreatorLevel < _foodCreator.CurrentLevel)
            {
                _tankerAnimationControll.OpenDorForSec(3);
                _lastFoodCreatorLevel = _foodCreator.CurrentLevel;
            }
            _foodWorker[0].SetActive(true);
            if (_upgradeManager.UpgradesData.Find(x => x.Name == "AILevel").LevelsBought > 1 &&
                _waterPump.CurrentLevel > 0)
            {
                if (_lastWaterPupmLevel < _waterPump.CurrentLevel)
                {
                    _tankerAnimationControll.OpenDorForSec(3);
                    _lastWaterPupmLevel = _waterPump.CurrentLevel;
                }
                _waterWorkers[0].SetActive(true);
                if (_upgradeManager.UpgradesData.Find(x => x.Name == "AILevel").LevelsBought > 2)
                {
                    if (_lastFoodCreatorLevel < _foodCreator.CurrentLevel)
                    {
                        _tankerAnimationControll.OpenDorForSec(3);
                        _lastFoodCreatorLevel = _foodCreator.CurrentLevel;
                    }
                    _foodWorker[1].SetActive(true);
                }
            }
        }

        
    }

    public void DisableBrains()
    {
        GetComponent<FoodAIBrain>().enabled = false;
        GetComponent<MechanicAIBrain>().enabled = false;
        GetComponent<WaterWorkerAIBrain>().enabled = false;
    }

    public void DisableAllAi()
    {
        StopAllCoroutines();
        foreach (var worker in _pumpWorkers)
        {
            worker.gameObject.SetActive(false);
        }

        foreach (var worker in _waterWorkers)
        {
            worker.gameObject.SetActive(false);
        }

        foreach (var worker in _foodWorker)
        {
            worker.gameObject.SetActive(false);
        }

        foreach (var worker in _mechanicWorker)
        {
            worker.gameObject.SetActive(false);
        }

        _playerAIBrain.gameObject.SetActive(false);
    }

    public void GoToTanker()
    {
        _playerAIBrain.EndLevel();
        _playerAIBrain.enabled = true;
        foreach (var worker in _pumpWorkers)
        {
            PumpWorkerAI ai = worker.GetComponent<PumpWorkerAI>();
            ai.Tasks.Clear();
            ai.SetTask("Tanker", _tankerPoint.position);
        }

        foreach (var worker in _waterWorkers)
        {
            WaterWorkerAI ai = worker.GetComponent<WaterWorkerAI>();
            ai.Tasks.Clear();
            ai.SetTask("Tanker", _tankerPoint.position);
            ai.GetComponent<AIMover>().SetSpeed(2);
        }

        foreach (var worker in _foodWorker)
        {
            FoodWorkerAI ai = worker.GetComponent<FoodWorkerAI>();
            ai.Tasks.Clear();
            ai.SetTask("Tanker", _tankerPoint.position);
            ai.GetComponent<AIMover>().SetSpeed(2);
        }

        foreach (var worker in _mechanicWorker)
        {
            MechanicWorkerAI ai = worker.GetComponent<MechanicWorkerAI>();
            ai.Tasks.Clear();
            ai.WorkDone = true;
            ai.Repeir = false;
            ai.SetTask("Tanker", _tankerPoint.position);
        }

        
        DisableBrains();
    }

    private IEnumerator PumpWorkerActivation()
    {
        

        while (true)
        {
            for (int i = 0; i < _miniPumps.Count; i++)
            {
                if (_miniPumps[i].CurrentLevel > 0)
                {
                    if (_countOfPumpWorkers < (i+1))
                    {
                        _countOfPumpWorkers++;
                        _tankerAnimationControll.OpenDorForSec(4f);
                        
                    }
                    
                    _pumpWorkers[i].SetActive(true);
                }
                else
                {
                    _pumpWorkers[i].SetActive(false);
                }


                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
    }
}