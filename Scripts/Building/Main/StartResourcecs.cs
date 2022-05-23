public class StartResourcecs : BuildingBase
{
    private void Start()
    {
        if (SaveManager.instance.StartResourcecsSaveInfo.CreateInventory.InventoryInfo.ResourcesInfo != null)
        {
            CreateInventory = SaveManager.instance.StartResourcecsSaveInfo.CreateInventory;
        }
        else if(SaveManager.instance.LevelCounter>7)
        {
            InventoryInfo countInfo = UpgradeData.UpgradeDatas.Find(x => x.Name == "StartResourcecs")
                .UpgaradesGrades[0].ResourcesToCreate;
            CreateInventory.MaxCount = countInfo.ResourcesInfo.Count;
                
            CreateInventory.InventoryInfo.ResourcesInfo.Clear();
            for (int i = 0; i < countInfo.ResourcesInfo.Count;i++)
            {
                CreateInventory.Add(countInfo.ResourcesInfo[i]);
            }
            
        }
    }
    private void Update()
    {
        SaveManager.instance.StartResourcecsSaveInfo.CreateInventory = CreateInventory;
        InventorySpaceCheck();
        if (CreateInventory.InventoryInfo.ResourcesInfo.Count <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
