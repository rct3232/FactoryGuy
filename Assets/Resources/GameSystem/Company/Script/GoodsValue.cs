using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsValue : MonoBehaviour
{
    public class Goods
    {
        public Goods() {}
        public int ID;
        public string Name;
        public string Type;
        public float Quality;
        public bool inMap;
        public bool isMoving;
        public GameObject GoodsObject;
    }
    public class StorageInfo
    {
        public StorageInfo() {}
        public int StorageType;
        public int Capacity;
        public GameObject Object;
    }
    List<Goods> GoodsArray = new List<Goods>();
    List<StorageInfo> StorageList = new List<StorageInfo>();
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    GoodsRecipe RecipeCall;
    TimeManager TimeManagerCall;
    PanelController PanelControllerCall;
    Goods newGoods;
    int CurGoodsID;
    public float OrganizeEfficiency;
    public int TotalCapacity;
    public int TEMP;

    // Start is called before the first frame update
    void Start()
    {
        CompanyManagerCall = transform.parent.parent.gameObject.GetComponent<CompanyManager>();
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        RecipeCall = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();

        // newGoods = new Goods();
        // newGoods.ID = -1;
        // newGoods.Name = "None";
        // newGoods.inMap = false;
        // newGoods.isMoving = false;
        // newGoods.GoodsObject = null;
        // GoodsArray.Add(newGoods);

        CurGoodsID = 0;
        TotalCapacity = 0;

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFactoryInfo("Warehouse", GetStoredGoods().Count, TotalCapacity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(TimeManagerCall.TimeValue % TimeManagerCall.Hour < TimeManagerCall.PlaySpeed)
        {
            // ManageStorage();
        }
        TEMP = GoodsArray.Count;
    }

    void ManageStorage()
    {
        List<Goods> StorageGoods = GetStoredGoods();
        List<Goods> RemoveList = new List<Goods>();

        if(TotalCapacity < StorageGoods.Count)
        {
            foreach(var GoodsInfo in StorageGoods)
            {
                if(Random.Range(0, TotalCapacity + 1) < (StorageGoods.Count - RemoveList.Count) - TotalCapacity)
                {
                    int TargetIndext = FindGoodsIndex(GoodsInfo.ID);

                    if((StorageGoods.Count - RemoveList.Count) - TotalCapacity > TotalCapacity)
                    {
                        GoodsArray[TargetIndext].Quality = 0f;
                    }
                    else
                    {
                        GoodsArray[TargetIndext].Quality -= Random.Range((float)(((StorageGoods.Count - RemoveList.Count) - TotalCapacity) / TotalCapacity) * 0.1f, 1f);
                    }
                }

                if(GoodsInfo.Quality <= 0)
                {
                    RemoveList.Add(GoodsInfo);
                }
            }
        }

        foreach(var GoodsInfo in RemoveList)
        {
            DeleteGoodsArray(GoodsInfo.ID);
        }
    }

    public void AddStorage(GameObject Object)
    {
        StorageInfo newStorage = new StorageInfo();
        WarehouseObjectAct WarehouseObjectActCall = Object.GetComponent<WarehouseObjectAct>();
        InstallableObjectAct ObjectActCall = Object.GetComponent<InstallableObjectAct>();

        newStorage.StorageType = WarehouseObjectActCall.StorageType;
        newStorage.Capacity = WarehouseObjectActCall.Capacity;
        newStorage.Object = Object;

        StorageList.Add(newStorage);

        TotalCapacity += newStorage.Capacity;
        
        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFactoryInfo("Warehouse", GetStoredGoods().Count, TotalCapacity);
    }

    public void DeleteStorage(GameObject Object)
    {
        StorageInfo Target = null;
        foreach(var Info in StorageList)
        {
            if(Info.Object == Object)
                Target = Info;
        }

        if(Target != null)
            StorageList.Remove(Target);
        else
            Debug.Log("There is no " + Object.name + " in Storage List");

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFactoryInfo("Warehouse", GetStoredGoods().Count, TotalCapacity);
    }

    public int FindGoodsIndex(int ID)
    {
        if(ID == -1)
            return -1;

        for (int i = 0; i < GoodsArray.Count; i++)
        {
            if(GoodsArray[i].ID == ID)
            {
                return i;
            }
        }
        return -1;
    }

    public int FindGoodsID(string Name)
    {
        foreach (var tmp in GoodsArray)
        {
            if (tmp.Name == Name)
            {
                return tmp.ID;
            }
        }

        return -1;
    }

    public int FindGoodsID(string Name, float Quality)
    {
        foreach (var tmp in GoodsArray)
        {
            if (tmp.Name == Name && tmp.Quality >= Quality)
            {
                return tmp.ID;
            }
        }

        return -1;
    }

    public int FindGoodsID(string Name, float Quality, bool inMapState)
    {
        foreach (var tmp in GoodsArray)
        {
            if (tmp.Name == Name && tmp.Quality >= Quality && tmp.inMap == inMapState)
            {
                return tmp.ID;
            }
        }

        return -1;
    }

    public int FindGoodsID(string Name, bool inMapState)
    {
        foreach (var tmp in GoodsArray)
        {
            if (tmp.Name == Name && tmp.inMap == inMapState)
            {
                return tmp.ID;
            }
        }

        return -1;
    }

    public int FindGoodsID(GameObject Object)
    {
        foreach(var tmp in GoodsArray)
        {
            if(tmp.GoodsObject == Object)
            {
                return tmp.ID;
            }
        }

        return -1;
    }

    public string FindGoodsName(int ID)
    {
        int index = FindGoodsIndex(ID);
        if(index != -1)
        {
            return GoodsArray[index].Name;
        }

        return "None";
    }

    public string FindGoodsName(GameObject Object)
    {
        int index = FindGoodsIndex(FindGoodsID(Object));
        if(index != -1)
        {
            return GoodsArray[index].Name;
        }

        return "None";
    }

    public string FindGoodsType(int ID)
    {
        int index = FindGoodsIndex(ID);
        if(index != -1)
        {
            return GoodsArray[index].Type;
        }

        return "None";
    }

    public string FindGoodsType(GameObject Object)
    {
        int index = FindGoodsIndex(FindGoodsID(Object));
        if(index != -1)
        {
            return GoodsArray[index].Type;
        }

        return "None";
    }

    public GameObject FindGoodsObject(int ID)
    {
        int index = FindGoodsIndex(ID);
        if(index != -1)
        {
            return GoodsArray[index].GoodsObject;
        }

        return null;
    }

    // public void ChangeGoodsInfo(int ID, string Name, float Quality, GameObject Object)
    // {
    //     int index = FindGoodsIndex(ID);
    //     if(index != -1)
    //     {
    //         GoodsArray[index].Name = Name;
    //         GoodsArray[index].Type = RecipeCall.getObjectType(GoodsArray[index].Name);
    //         GoodsArray[index].Quality = Quality;
    //         GoodsArray[index].GoodsObject = Object;
    //         return;
    //     }

    //     Debug.Log("There is no ID like " + ID);
    //     return;
    // }

    public void ChangeGoodsInfo(int ID, string Name, float Quality)
    {
        int index = FindGoodsIndex(ID);
        if(index != -1)
        {
            GoodsArray[index].Name = Name;
            GoodsArray[index].Type = RecipeCall.getObjectType(GoodsArray[index].Name);
            GoodsArray[index].Quality = Quality;
            return;
        }

        Debug.Log("There is no ID like " + ID);
        return;
    }

    // public void ChangeGoodsInfo(int ID, string Name, GameObject Object)
    // {
    //     int index = FindGoodsIndex(ID);
    //     if(index != -1)
    //     {
    //         GoodsArray[index].Name = Name;
    //         GoodsArray[index].Type = RecipeCall.getObjectType(GoodsArray[index].Name);
    //         GoodsArray[index].GoodsObject = Object;
    //         return;
    //     }

    //     Debug.Log("There is no ID like " + ID);
    //     return;
    // }

    public void ChangeGoodsInfo(int ID, string Name)
    {
        int index = FindGoodsIndex(ID);
        if(index != -1)
        {
            GoodsArray[index].Name = Name;
            GoodsArray[index].Type = RecipeCall.getObjectType(GoodsArray[index].Name);
            return;
        }

        Debug.Log("There is no ID like " + ID);
        return;
    }

    public void AddGoodsArray(string Name, float Quality)
    {
        newGoods = new Goods();
        CurGoodsID++;
        newGoods.ID = CurGoodsID;
        newGoods.Name = Name;
        newGoods.Type = RecipeCall.getObjectType(newGoods.Name);
        newGoods.Quality = Quality;
        newGoods.isMoving = false;
        newGoods.inMap = false;
        newGoods.GoodsObject = null;

        GoodsArray.Add(newGoods);

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName)
        {
            StoredAmountChangePanelUpdate(newGoods, false);
        }
    }

    public void DeleteGoodsArray(int ID)
    {
        int index = FindGoodsIndex(ID);

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName)
        {
            StoredAmountChangePanelUpdate(GoodsArray[index], true);
        }
        
        GoodsArray.RemoveAt(index);
    }

    public void DeleteGoodsArray(string Name)
    {
        DeleteGoodsArray(FindGoodsID(Name));
    }

    public void DeleteGoodsArray(GameObject Object)
    {
        DeleteGoodsArray(FindGoodsID(Object));
    }

    public void ChangeInMapState(int ID, bool State, GameObject Object)
    {
        int index = -1;
        for (int i = 0; i < GoodsArray.Count; i++)
        {
            if(GoodsArray[i].ID == ID)
            {
                index = i;
                GoodsArray[index].inMap = State;
                GoodsArray[index].GoodsObject = Object;
                break;
            }
        }

        if(index == -1) return;

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName)
        {
            StoredAmountChangePanelUpdate(GoodsArray[index], State);
        }
    }

    public void ChangeMovingState(int ID, bool State)
    {
        for (int i = 0; i < GoodsArray.Count; i++)
        {
            if(GoodsArray[i].ID == ID)
            {
                // Debug.Log(ID + "'s Moving State is Changed to " + State);
                GoodsArray[i].isMoving = State;
                return;
            }
        }
    }

    public void ChangeMovingState(GameObject Object, bool State)
    {
        ChangeMovingState(FindGoodsID(Object), State);
    }

    public int CheckMovingState(int ID)
    {
        foreach(var tmp in GoodsArray)
        {
            if(tmp.ID == ID)
            {
                if (tmp.isMoving)
                    return 1;
                else
                    return 0;
            }
        }

        return -1;
    }

    public int CheckMovingState(GameObject Object)
    {
        return CheckMovingState(FindGoodsID(Object));
    }

    public void ChangeQuality(int ID, float Quality)
    {
        for (int i = 0; i < GoodsArray.Count; i++)
        {
            if (GoodsArray[i].ID == ID)
            {
                GoodsArray[i].Quality = Quality;
                return;
            }
        }

        Debug.Log("There is no ID like " + ID);
        return;
    }

    public void ChangeQuality(GameObject Object, float Quality)
    {
        ChangeQuality(FindGoodsID(Object), Quality);
    }

    public float CheckQuality(int ID)
    {
        int index = FindGoodsIndex(ID);
        if(index != -1)
        {
            return GoodsArray[index].Quality;
        }

        Debug.Log("There is no ID like " + ID);
        return -1f;
    }

    public float CheckQuality(GameObject Object)
    {
        return CheckQuality(FindGoodsID(Object));
    }

    public int GetGoodsCount(string Name, bool OnlyStorage)
    {
        int result = 0;

        foreach(var Goods in GoodsArray)
        {
            if(Goods.Name == Name)
            {
                if(OnlyStorage)
                {
                    if(Goods.inMap)
                    {
                        continue;
                    }
                }

                result++;
            }
        }

        return result;
    }

    public List<Goods> GetAllGoodsInfo(string Name, bool OnlyStorage)
    {
        List<Goods> result = new List<Goods>();

        foreach(var Goods in GoodsArray)
        {
            if(Goods.Name == Name)
            {
                if(OnlyStorage)
                {
                    if(Goods.inMap)
                    {
                        continue;
                    }
                }

                result.Add(Goods);
            }
        }

        return result;
    }

    public List<string> GetAllGoodsName(bool OnlyStorage)
    {
        List<string> TempList = new List<string>();
        TempList.Add("None");

        foreach(var Goods in GoodsArray)
        {
            if(Goods.Name == "None")
                continue;

            if(OnlyStorage)
            {
                if(Goods.inMap)
                    continue;
            }
            
            bool isDuplicate = false;
            foreach(var Target in TempList)
            {
                if(Target == Goods.Name)
                {
                    isDuplicate = true;
                    break;
                }
            }

            // Debug.Log(isDuplicate + Goods.Name);
            if(!isDuplicate)
            {
                TempList.Add(Goods.Name);
            }
        }

        TempList.RemoveAt(0);
        return TempList;
    }

    public float[] GetGoodsQualitySpectrum(string Name, bool OnlyStorage)
    {
        float[] result = new float[2];

        result[0] = 1f;
        result[1] = 0f;

        foreach(var Goods in GoodsArray)
        {
            if(Goods.Name == Name)
            {
                if(OnlyStorage)
                {
                    if(Goods.inMap)
                        continue;
                }

                if(Goods.Quality < result[0])
                {
                    result[0] = Goods.Quality;
                }
                if(Goods.Quality > result[1])
                {
                    result[1] = Goods.Quality;
                }
            }
        }

        return result;
    }

    public List<Goods> GetStoredGoods()
    {
        List<Goods> result = new List<Goods>();

        foreach(var GoodsInfo in GoodsArray)
        {
            if(!GoodsInfo.inMap)
            {
                result.Add(GoodsInfo);
            }
        }

        return result;
    }

    public List<Goods> GetStoredGoods(string Name)
    {
        List<Goods> result = new List<Goods>();

        foreach(var GoodsInfo in GoodsArray)
        {
            if(GoodsInfo.Name == Name)
            {
                if(!GoodsInfo.inMap)
                {
                    result.Add(GoodsInfo);
                }
            }
        }

        return result;
    }

    void StoredAmountChangePanelUpdate(Goods Info, bool Sign)
    {
        PanelControllerCall.UpdateFactoryInfo("Warehouse", GetStoredGoods().Count, TotalCapacity);

        if(PanelControllerCall.CurrentSidePanel != null)
        {
            if(PanelControllerCall.CurrentSidePanel.name == "ContractPanel")
            {
                ContractPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<ContractPanelController>();
                if(PanelComponent.CurrentCategory == "Storage")
                {
                    PanelComponent.UpdateRemainQuantityText();
                    
                    if(PanelComponent.CurrentItem == Info.Name)
                    {
                        if(GetStoredGoods(Info.Name).Count == 0 && Sign)
                        {
                            PanelComponent.UpdateList(true);

                            if(PanelComponent.TargetItemSalesInfo == null) PanelComponent.ClearInfoPanel();
                        }
                    }
                    else
                    {
                        if(GetStoredGoods(Info.Name).Count == 0 && Sign) PanelComponent.UpdateList(true);
                        if(GetStoredGoods(Info.Name).Count == 1 && !Sign) PanelComponent.UpdateList(true);
                    }
                }
            }
            if(PanelControllerCall.CurrentSidePanel.name == "GoodsCreatorPanel")
            {
                GoodsCreatorPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<GoodsCreatorPanelController>();
                if(PanelComponent.CurrentCategory == Info.Type || PanelComponent.CurrentCategory == "All")
                {
                    PanelComponent.UpdateRemainQuantityText();
                    
                    if(PanelComponent.CurrentItem == Info.Name)
                    {
                        if(GetStoredGoods(Info.Name).Count == 0 && Sign)
                        {
                            PanelComponent.UpdateList(true);

                            if(PanelComponent.CallTargetGoodsCreator.TargetGoodsName != Info.Name) PanelComponent.ClearInfoPanel();
                        }
                    }
                    else
                    {
                        if(GetStoredGoods(Info.Name).Count == 0 && Sign) PanelComponent.UpdateList(true);
                        if(GetStoredGoods(Info.Name).Count == 1 && !Sign) PanelComponent.UpdateList(true);
                    }
                }
                else
                {
                    if(GetStoredGoods(Info.Name).Count == 0 && Sign) PanelComponent.UpdateList(false);
                    if(GetStoredGoods(Info.Name).Count == 1 && !Sign) PanelComponent.UpdateList(false);
                }
            }
        }
    }
}
