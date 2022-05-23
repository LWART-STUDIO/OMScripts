using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }

    public SaveInfo PlayerSaveInfo;
    public SaveInfo TankerSaveInfo;
    public SaveInfo OilPumpSaveInfo;
    public SaveInfo PlasticCreatorSaveInfo;
    public SaveInfo FoodCreatorSaveInfo;
    public SaveInfo WaterPumpSaveInfo;
    public SaveInfo BurSaveInfo;
    public SaveInfo ShopSaveInfo;
    public SaveInfo StartResourcecsSaveInfo;
    public SaveInfo TransporterSveInfo;
    public SaveInfo MechanicSaveInfo;
    public int ShopCounter;

    public List<SaveInfo> MiniPumpSaveInfo = new List<SaveInfo>(12)
        {null, null, null, null, null, null, null, null, null, null, null, null};

    public List<UpgradeManagerData> UpgradeData = new List<UpgradeManagerData>() { };
    public int CurrentLvl = -1;
    public string GameVersion;
    [SerializeField] private UpgradeDataSO _upgrdesSo;
    [SerializeField] private LevelInformator _levelInformator;
    [SerializeField] private bool _newLevel;
    public int NextLevel;
    public bool NewLevel => _newLevel;
    public bool MuteMusic;
    public bool MuteSounds;
    public int LevelCounter;

    private void Awake()
    {

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);
        _levelInformator = FindObjectOfType<LevelInformator>();
        Load();

    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter binaryFormater = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData_Storage data = (PlayerData_Storage) binaryFormater.Deserialize(file);
            if (_levelInformator != null)
            {
                if (data.CurrentLvl != _levelInformator.LevelNumber)
                {
                    _newLevel = true;
                    PlayerSaveInfo = data.PlayerSaveInfo;
                    PlayerSaveInfo.CreateInventory.InventoryInfo.ResourcesInfo = new List<ResourceInfo>();

                    TankerSaveInfo = data.TankerSaveInfo;
                    TankerSaveInfo.UpgradeInventory = null;

                    OilPumpSaveInfo = data.OilPumpSaveInfo;
                    OilPumpSaveInfo.UpgradeLevel = 0;
                    //OilPumpSaveInfo.NeedResource.ID = null;
                    OilPumpSaveInfo.CreateInventory.MaxCount = 0;


                    PlasticCreatorSaveInfo = data.PlasticCreatorSaveInfo;
                    //PlasticCreatorSaveInfo.NeedResource.ID = null;
                    PlasticCreatorSaveInfo.UpgradeLevel = 0;
                    PlasticCreatorSaveInfo.CreateInventory.MaxCount = 0;

                    FoodCreatorSaveInfo = data.FoodCreatorSaveInfo;
                    //FoodCreatorSaveInfo.NeedResource.ID = null;
                    FoodCreatorSaveInfo.UpgradeLevel = 0;
                    FoodCreatorSaveInfo.CreateInventory.MaxCount = 0;

                    WaterPumpSaveInfo = data.WaterPumpSaveInfo;
                   // WaterPumpSaveInfo.NeedResource.ID = null;
                    WaterPumpSaveInfo.UpgradeLevel = 0;
                    WaterPumpSaveInfo.CreateInventory.MaxCount = 0;


                    BurSaveInfo = data.BurSaveInfo;
                    BurSaveInfo.NeedResource.ID = null;
                    BurSaveInfo.UpgradeLevel = 0;
                    BurSaveInfo.CreateInventory.MaxCount = 0;


                    MiniPumpSaveInfo = data.MiniPumpSaveInfo;
                    foreach (var miniPump in MiniPumpSaveInfo)
                    {
                        miniPump.ResourceNeedInventory.InventoryInfo.ResourcesInfo.Clear();
                        miniPump.UpgradeLevel = 0;
                    }

                    UpgradeData = data.UpgradeData;

                    ShopSaveInfo = data.ShopSaveInfo;

                    TransporterSveInfo = data.TransporterSveInfo;
                    TransporterSveInfo.NeedResource.ID = null;
                    TransporterSveInfo.UpgradeLevel = 0;
                    TransporterSveInfo.CreateInventory.MaxCount = 0;

                    ShopCounter = data.ShopCounter;

                    StartResourcecsSaveInfo = data.StartResourcecsSaveInfo;
                    StartResourcecsSaveInfo.CreateInventory.InventoryInfo.ResourcesInfo = null;


                    MechanicSaveInfo = data.MechanicSaveInfo;
                    //MechanicSaveInfo.NeedResource.ID = null;
                    MechanicSaveInfo.UpgradeLevel = 0;
                    MechanicSaveInfo.CreateInventory.MaxCount = 0;

                    CurrentLvl = data.CurrentLvl;
                    GameVersion = data.GameVersion;
                    NextLevel = data.NextLevel;
                    LevelCounter = data.LevelCounter;
                }
                else
                {
                    _newLevel = false;
                    PlayerSaveInfo = data.PlayerSaveInfo;
                    TankerSaveInfo = data.TankerSaveInfo;
                    OilPumpSaveInfo = data.OilPumpSaveInfo;
                    PlasticCreatorSaveInfo = data.PlasticCreatorSaveInfo;
                    FoodCreatorSaveInfo = data.FoodCreatorSaveInfo;
                    WaterPumpSaveInfo = data.WaterPumpSaveInfo;
                    BurSaveInfo = data.BurSaveInfo;
                    MiniPumpSaveInfo = data.MiniPumpSaveInfo;
                    UpgradeData = data.UpgradeData;
                    ShopSaveInfo = data.ShopSaveInfo;
                    TransporterSveInfo = data.TransporterSveInfo;
                    ShopCounter = data.ShopCounter;
                    StartResourcecsSaveInfo = data.StartResourcecsSaveInfo;
                    CurrentLvl = data.CurrentLvl;
                    GameVersion = data.GameVersion;
                    MechanicSaveInfo = data.MechanicSaveInfo;
                    LevelCounter = data.LevelCounter;

                }

            }
            else
            {
                PlayerSaveInfo = data.PlayerSaveInfo;
                TankerSaveInfo = data.TankerSaveInfo;
                OilPumpSaveInfo = data.OilPumpSaveInfo;
                PlasticCreatorSaveInfo = data.PlasticCreatorSaveInfo;
                FoodCreatorSaveInfo = data.FoodCreatorSaveInfo;
                WaterPumpSaveInfo = data.WaterPumpSaveInfo;
                BurSaveInfo = data.BurSaveInfo;
                MiniPumpSaveInfo = data.MiniPumpSaveInfo;
                UpgradeData = data.UpgradeData;
                ShopSaveInfo = data.ShopSaveInfo;
                ShopCounter = data.ShopCounter;
                StartResourcecsSaveInfo = data.StartResourcecsSaveInfo;
                TransporterSveInfo = data.TransporterSveInfo;
                CurrentLvl = data.CurrentLvl;
                GameVersion = data.GameVersion;
                MechanicSaveInfo = data.MechanicSaveInfo;
                NextLevel = data.NextLevel;
                MuteMusic = data.MuteMusic;
                MuteSounds = data.MuteSounds;
                LevelCounter = data.LevelCounter;
            }

            file.Close();

        }
        else
        {
            MiniPumpSaveInfo = new List<SaveInfo>(12)
            {
                new SaveInfo(), new SaveInfo(), new SaveInfo(), new SaveInfo(), new SaveInfo(), new SaveInfo(),
                new SaveInfo(), new SaveInfo(), new SaveInfo(), new SaveInfo(), new SaveInfo(), new SaveInfo()
            };
            NextLevel = 1;
            UpgradeData = new List<UpgradeManagerData>(_upgrdesSo.UpgradeDatas.Count);
            StartResourcecsSaveInfo.CreateInventory.InventoryInfo.ResourcesInfo = null;
            TankerSaveInfo.UpgradeInventory = null;
            MuteMusic = true;
            MuteSounds = false;
            LevelCounter = 1;
        }
    }

    public void Save()
    {
        BinaryFormatter binaryFormater = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData_Storage data = new PlayerData_Storage();
        data.PlayerSaveInfo = PlayerSaveInfo;
        data.TankerSaveInfo = TankerSaveInfo;
        data.OilPumpSaveInfo = OilPumpSaveInfo;
        data.PlasticCreatorSaveInfo = PlasticCreatorSaveInfo;
        data.FoodCreatorSaveInfo = FoodCreatorSaveInfo;
        data.WaterPumpSaveInfo = WaterPumpSaveInfo;
        data.BurSaveInfo = BurSaveInfo;
        data.MiniPumpSaveInfo = MiniPumpSaveInfo;
        data.UpgradeData = UpgradeData;
        data.ShopSaveInfo = ShopSaveInfo;
        data.TransporterSveInfo = TransporterSveInfo;
        data.ShopCounter = ShopCounter;
        data.StartResourcecsSaveInfo = StartResourcecsSaveInfo;
        data.CurrentLvl = CurrentLvl;
        data.GameVersion = GameVersion;
        data.MechanicSaveInfo = MechanicSaveInfo;
        data.NextLevel = NextLevel;
        data.MuteMusic = MuteMusic;
        data.MuteSounds = MuteSounds;
        data.LevelCounter = LevelCounter;
        binaryFormater.Serialize(file, data);
        file.Close();
    }

    //����� �������� ������ � ��������� ����������.
    public void DellAllSave()
    {
        BinaryFormatter binaryFormater = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData_Storage data = new PlayerData_Storage();

        binaryFormater.Serialize(file, data);
        file.Close();
        Load();
    }

    public void DoCheckNewLebel()
    {
        if (_levelInformator == null)
        {
            _levelInformator = FindObjectOfType<LevelInformator>();
        }

        if (_levelInformator != null)
        {
            if (CurrentLvl != _levelInformator.LevelNumber)
                {
                    Debug.Log("new Level");
                    _newLevel = true;
                    NewLevelStart();
                }
        }
        }

    public void NewLevelStart()
    {
        PlayerSaveInfo.CreateInventory.InventoryInfo.ResourcesInfo = new List<ResourceInfo>();


        TankerSaveInfo?.UpgradeInventory?.InventoryInfo?.ResourcesInfo.Clear();


        OilPumpSaveInfo.UpgradeLevel = 0;
        //OilPumpSaveInfo.NeedResource.ID = null;
        OilPumpSaveInfo.CreateInventory.MaxCount = 0;



        //PlasticCreatorSaveInfo.NeedResource.ID = null;
        PlasticCreatorSaveInfo.UpgradeLevel = 0;
        PlasticCreatorSaveInfo.CreateInventory.MaxCount = 0;


        //FoodCreatorSaveInfo.NeedResource.ID = null;
        FoodCreatorSaveInfo.UpgradeLevel = 0;
        FoodCreatorSaveInfo.CreateInventory.MaxCount = 0;


        WaterPumpSaveInfo.NeedResource.ID = null;
        WaterPumpSaveInfo.UpgradeLevel = 0;
        WaterPumpSaveInfo.CreateInventory.MaxCount = 0;



        BurSaveInfo.NeedResource.ID = null;
        BurSaveInfo.UpgradeLevel = 0;
        BurSaveInfo.CreateInventory.MaxCount = 0;

                    
        foreach (var miniPump in MiniPumpSaveInfo)
        {
            miniPump.ResourceNeedInventory?.InventoryInfo?.ResourcesInfo.Clear();
            miniPump.UpgradeLevel = 0;
        }


        TransporterSveInfo.NeedResource.ID = null;
        TransporterSveInfo.UpgradeLevel = 0;
        TransporterSveInfo.CreateInventory.MaxCount = 0;


                    
        StartResourcecsSaveInfo.CreateInventory.InventoryInfo.ResourcesInfo = null;



        //MechanicSaveInfo.NeedResource.ID = null;
        MechanicSaveInfo.UpgradeLevel = 0;
        MechanicSaveInfo.CreateInventory.MaxCount = 0;


        CurrentLvl = _levelInformator.LevelNumber;
    }
    }



//���� ������(������ ����������� ������ �� SaveManager)
[Serializable]
class PlayerData_Storage
{
    public SaveInfo PlayerSaveInfo;
    public SaveInfo TankerSaveInfo;
    public SaveInfo OilPumpSaveInfo;
    public SaveInfo PlasticCreatorSaveInfo;
    public SaveInfo FoodCreatorSaveInfo;
    public SaveInfo WaterPumpSaveInfo;
    public SaveInfo BurSaveInfo;
    public SaveInfo ShopSaveInfo;
    public SaveInfo StartResourcecsSaveInfo;
    public SaveInfo TransporterSveInfo;
    public int ShopCounter;
    public List<SaveInfo> MiniPumpSaveInfo;
    public SaveInfo MechanicSaveInfo;
    public List<UpgradeManagerData> UpgradeData;
    public int CurrentLvl;
    public string GameVersion;
    public int NextLevel;
    public bool MuteMusic;
    public bool MuteSounds;
    public int LevelCounter;
}
