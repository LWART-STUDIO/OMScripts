[System.Serializable]
public class InventoryBase : IInventory
{
    public int MaxCount;
    public int CurrentSpace;
    public InventoryInfo InventoryInfo;
    public void Add(ResourceInfo resourceInfo)
    {
        InventoryInfo.ResourcesInfo.Add(resourceInfo);
    }
    public void Remove(ResourceInfo resourceInfo)
    {
        InventoryInfo.ResourcesInfo.Remove(InventoryInfo.ResourcesInfo.FindLast(x=>x.ID== resourceInfo.ID));
    }

    public void RemoveLast()
    {
        InventoryInfo.ResourcesInfo.Remove(InventoryInfo.ResourcesInfo[InventoryInfo.ResourcesInfo.Count-1]);
    }
   
}
