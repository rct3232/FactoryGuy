using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesValue : MonoBehaviour
{
    public class SalesInfo
    {
        public SalesInfo () {}
        public TechValue.RecipeInfo RecipeInfo;
        public string Seller;
        public int UploadDate;
        public float QualityEvaluation;
        public int Price;
        public int ItemCount;
        public int SoldCount;
        public List<int> SoldCountList;
        public List<ContractInfo> ContractList;
    }
    public class ContractInfo
    {
        public ContractInfo() {}
        public string CompanyName;
        public int Term;
        public int Quantity;
        public int ContractDate;
    }
    public List<SalesInfo> SalesItemArray = new List<SalesInfo>();
    InGameValue InGameValueCall;
    TimeManager TimeManagerCall;
    PanelController PanelControllerCall;
    CompanyManager CompanyManagerCall;
    NotificationManager NotificationManagerCall;

    // Start is called before the first frame update
    void Start()
    {
        InGameValueCall = GameObject.Find("BaseSystem").GetComponent<InGameValue>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        NotificationManagerCall = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnforceContract();

        if(TimeManagerCall.TimeValue % TimeManagerCall.Month < TimeManagerCall.PlaySpeed) UpdateSoldCountList();
    }

    public void UpdateItemCount(string CompanyName)
    {
        if(CompanyManagerCall.GetCompanyValue(CompanyName) == null)
        {
            Debug.Log("There is no " + CompanyName + " in CompanyManager");
            return;
        }
        foreach(var SalesItem in SalesItemArray)
        {
            if(SalesItem.Seller == CompanyName)
            {
                SalesItem.ItemCount = 0;
                List<GoodsValue.Goods> GoodsList = new List<GoodsValue.Goods>();
                GoodsList.AddRange(CompanyManagerCall.GetCompanyValue(CompanyName).GetGoodsValue().GetComponent<GoodsValue>().GetAllGoodsInfo(SalesItem.RecipeInfo.Recipe.OutputName, true));
                SalesItem.ItemCount = GoodsList.Count;
            }
        }

        if(PanelControllerCall.CurrentSidePanel != null)
        {
            if(PanelControllerCall.CurrentSidePanel.name == "ContractPanel")
            {
                ContractPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<ContractPanelController>();
                if(PanelComponent.CurrentCategory != "Storage")
                {
                    PanelComponent.UpdateRemainQuantityText();
                }
            }
        }
    }

    public void UpdateItemCount(string CompanyName, string Name)
    {
        if(CompanyManagerCall.GetCompanyValue(CompanyName) == null)
        {
            // Debug.Log("There is no " + CompanyName + " in CompanyManager");
            return;
        }

        List<GoodsValue.Goods> GoodsList = new List<GoodsValue.Goods>();
        GoodsList.AddRange(CompanyManagerCall.GetCompanyValue(CompanyName).GetGoodsValue().GetComponent<GoodsValue>().GetAllGoodsInfo(Name, true));

        foreach(var SalesItem in SalesItemArray)
        {
            if(SalesItem.RecipeInfo.Recipe.OutputName == Name && SalesItem.Seller == CompanyName)
            {
                SalesItem.ItemCount = GoodsList.Count;
            }
        }

        if(PanelControllerCall.CurrentSidePanel != null)
        {
            if(PanelControllerCall.CurrentSidePanel.name == "ContractPanel")
            {
                ContractPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<ContractPanelController>();
                if(PanelComponent.CurrentCategory != "Storage")
                {
                    PanelComponent.UpdateRemainQuantityText();
                }
            }
        }
    }

    void UpdateSoldCountList()
    {
        foreach(var Sales in SalesItemArray)
        {
            int PreviousSoldCount = 0;
            foreach(var Count in Sales.SoldCountList) PreviousSoldCount += Count;

            Sales.SoldCountList.Add(Sales.SoldCount - PreviousSoldCount);
        }
    }

    int getSalesIndex(string Name)
    {
        int result = -1;

        for(int i = 0; i < SalesItemArray.Count; i++)
        {
            if(SalesItemArray[i].RecipeInfo.Recipe.OutputName == Name)
            {
                result = i;
                break;
            }
        }

        return result;
    }

    public void AddSales(string Name, string Company, int Price)
    {
        TechValue.RecipeInfo RecipeInfo = CompanyManagerCall.GetCompanyValue(Company).GetTechValue().GetComponent<TechValue>().GetRecipe(Name);

        foreach(var SalesItem in SalesItemArray)
        {
            if(SalesItem.RecipeInfo.Recipe == RecipeInfo.Recipe)
            {
                if(SalesItem.Seller == Company)
                {
                    ModifySales(Name, Price);
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        SalesInfo newContent = new SalesInfo();

        newContent.RecipeInfo = RecipeInfo;
        newContent.Seller = Company;
        newContent.QualityEvaluation = 1f;
        newContent.Price = Price;
        newContent.UploadDate = TimeManagerCall.TimeValue;
        newContent.SoldCount = 0;
        newContent.SoldCountList = new List<int>();
        newContent.ContractList = new List<ContractInfo>();

        SalesItemArray.Add(newContent);

        UpdateItemCount(Company, Name);

        if(PanelControllerCall.CurrentSidePanel != null)
        {
            if(PanelControllerCall.CurrentSidePanel.name == "ContractPanel")
            {
                ContractPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<ContractPanelController>();
                if(PanelComponent.CurrentCategory == newContent.RecipeInfo.Recipe.GoodsObject.name) PanelComponent.UpdateList(true);
                else PanelComponent.UpdateList(false);
            }
        }
    }

    public void ModifySales(string Name, int Price)
    {
        int Index = getSalesIndex(Name);
        if(Index == -1)
        {
            return;
        }

        SalesItemArray[Index].Price = Price;
    }

    public void DeleteSales(string Name)
    {
        int Index = getSalesIndex(Name);
        if(Index == -1)
        {
            return;
        }

        if(PanelControllerCall.CurrentSidePanel != null)
        {
            if(PanelControllerCall.CurrentSidePanel.name == "ContractPanel")
            {
                ContractPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<ContractPanelController>();
                if(PanelComponent.CurrentCategory == SalesItemArray[Index].RecipeInfo.Recipe.GoodsObject.name) 
                {
                    PanelComponent.UpdateList(true);
                    if(PanelComponent.CurrentItem == SalesItemArray[Index].RecipeInfo.Recipe.OutputName) PanelComponent.ClearInfoPanel();
                }
                else PanelComponent.UpdateList(false);
            }
        }


        SalesItemArray.RemoveAt(Index);
    }

    public string getSalesItemType(string Name)
    {
        int Index = getSalesIndex(Name);

        if(Index != -1)
            return SalesItemArray[Index].RecipeInfo.Recipe.GoodsObject.name;
        else
            return null;
    }

    public SalesInfo GetSalesInfo(string Name)
    {
        int Index = getSalesIndex(Name);

        if(Index != -1)
            return SalesItemArray[Index];
        else
            return null;
    }

    public int AddContract(string Name, string CompanyName, int ContractTerm, int ContractQuantity)
    {
        int Index = getSalesIndex(Name);
        if(Index == -1)
        {
            Debug.Log("No Info of " + Name + ". Try from " + CompanyName);
            return -1;
        }

        if(SalesItemArray[Index].ItemCount < ContractQuantity)
        {
            return -2;
        }

        List<float> ResultList = BuyItem(Name, CompanyName, ContractQuantity);

        if(ResultList[0] <= 0)
        {
            switch(ResultList[0])
            {
                case -2f : // Contract cannot proceed with not enough stock
                    break;
                case -1f : // BuyItem will debug it
                    break;
                case 0f : return 0;
            }
        }
        else
        {
            int LackCount = 0;
            foreach(var value in ResultList)
            {
                if(value == 0)
                {
                    LackCount++;
                }
            }

            if(LackCount > 0)
            {
                NotificationManagerCall.AddAlert("Lack of cash. Only " + (ContractQuantity - LackCount).ToString() + " " + Name + " were delivered to you at this time", 1, "");
            }
        }

        ContractInfo newContent = new ContractInfo();
        newContent.CompanyName = CompanyName;
        newContent.Term = ContractTerm - 1;
        newContent.Quantity = ContractQuantity;
        newContent.ContractDate = TimeManagerCall.TimeValue + TimeManagerCall.Month;

        SalesItemArray[Index].ContractList.Add(newContent);

        if(SalesItemArray[Index].Seller == CompanyManagerCall.PlayerCompanyName)
        {
            NotificationManagerCall.AddNews("Info", CompanyName + " just conclude a contract of " + Name + " for " + ContractTerm + " month");
        }

        return 1;
    }

    public int ModifyContract(string Name, int ContractIndex, int ContractTerm, int ContractQuantity)
    {
        int Index = getSalesIndex(Name);
        if(Index == -1)
        {
            Debug.Log("There is no Contract you asked");
            return - 1;
        }
        if(ContractQuantity > SalesItemArray[Index].ContractList[ContractIndex].Quantity)
        {
            if(SalesItemArray[Index].ItemCount < ContractQuantity)
            {
                return -2;
            }
        }

        SalesItemArray[Index].ContractList[ContractIndex].Term = ContractTerm;
        SalesItemArray[Index].ContractList[ContractIndex].Quantity = ContractQuantity;

        return 1;
    }

    public int ModifyContract(string Name, string CompanyName, int ContractTerm, int ContractQuantity)
    {
        int Index = getSalesIndex(Name);
        int ContractIndex = GetContractIndex(SalesItemArray[Index], CompanyName);
        if(Index == -1)
        {
            Debug.Log("There is no Contract you asked");
            return - 1;
        }
        if(ContractQuantity > SalesItemArray[Index].ContractList[ContractIndex].Quantity)
        {
            if(SalesItemArray[Index].ItemCount < ContractQuantity)
            {
                return -2;
            }
        }

        SalesItemArray[Index].ContractList[ContractIndex].Term = ContractTerm;
        SalesItemArray[Index].ContractList[ContractIndex].Quantity = ContractQuantity;

        return 1;
    }

    public void DeleteContract(string Name, int ContractIndex)
    {
        int Index = getSalesIndex(Name);
        if(Index == -1)
        {
            return;
        }

        SalesItemArray[Index].ContractList.RemoveAt(ContractIndex);
    }

    public void DeleteContract(string Name, string CompnayName)
    {
        int Index = getSalesIndex(Name);
        int ContractIndex = GetContractIndex(SalesItemArray[Index], CompnayName);
        if(Index == -1)
        {
            return;
        }

        SalesItemArray[Index].ContractList.RemoveAt(ContractIndex);
    }

    public ContractInfo GetContractInfo(SalesInfo Info, string CompanyName)
    {
        ContractInfo result = null;

        foreach(var Contract in Info.ContractList)
        {
            if(Contract.CompanyName == CompanyName)
            {
                result = Contract;
            }
        }

        return result;
    }

    public int GetContractIndex(SalesInfo Info, string CompanyName)
    {
        int result = -1;

        for(int i = 0; i < Info.ContractList.Count; i++)
        {
            if(Info.ContractList[i].CompanyName == CompanyName)
            {
                result = i;
            }
        }

        return result;
    }

    public bool CheckContractExist(SalesInfo Info, string CompanyName)
    {
        bool result = false;

        foreach(var Contract in Info.ContractList)
        {
            if(Contract.CompanyName == CompanyName)
            {
                result = true;
            }
        }

        return result;
    }

    public List<float> BuyItem(string Name, string BuyerName, int Num)
    {
        List<float> result = new List<float>();
        int Index = getSalesIndex(Name);
        if(Index == -1)
        {
            Debug.Log("No Info of " + Name + ". Try from " + BuyerName);
            result.Add(-1f);
            return result;
        }

        SalesInfo TargetItem = SalesItemArray[Index];
        GoodsValue SellerGoodsValueCall = CompanyManagerCall.GetCompanyValue(TargetItem.Seller).GetGoodsValue().GetComponent<GoodsValue>();
        EconomyValue SellerEconomyCall = CompanyManagerCall.GetCompanyValue(TargetItem.Seller).GetEconomyValue().GetComponent<EconomyValue>();
        
        int Limit = 0;
        if(TargetItem.ItemCount >= Num)
        {
            Limit = Num;
        }
        else if(TargetItem.ItemCount > 0)
        {
            Limit = TargetItem.ItemCount;
        }
        else
        {
            // Debug.Log("Lack of stock " + TargetItem.ItemCount + " " + Num);
            result.Add(-2f);
            return result;
        }
        
        float TotalQualityValue = TargetItem.QualityEvaluation * TargetItem.SoldCount;
        for(int i = 0; i < Limit; i++)
        {
            int GoodsID = SellerGoodsValueCall.FindGoodsID(Name, false);
            if(GoodsID == -1)
            {
                Debug.Log("No Info of " + TargetItem.Seller + "'s " + Name + ". Try from " + BuyerName);
                result.Add(-1f);
                return result;
            }

            if(BuyerName != "Consumer")
            {
                GoodsValue BuyerGoodsValueCall = CompanyManagerCall.GetCompanyValue(BuyerName).GetGoodsValue().GetComponent<GoodsValue>();
                EconomyValue BuyerEconomyCall = CompanyManagerCall.GetCompanyValue(BuyerName).GetEconomyValue().GetComponent<EconomyValue>();

                if(BuyerEconomyCall.Balance < TargetItem.Price)
                {
                    result.Add(0f);
                    return result;
                }
                
                BuyerGoodsValueCall.AddGoodsArray(Name, SellerGoodsValueCall.CheckQuality(GoodsID));
                BuyerEconomyCall.AddHistory(TimeManagerCall.TimeValue, "Buy", Name, "Buy " + Name + " from " + TargetItem.Seller, -TargetItem.Price);
            }        

            result.Add(SellerGoodsValueCall.CheckQuality(GoodsID)); 
            SellerGoodsValueCall.DeleteGoodsArray(GoodsID);
            SellerEconomyCall.AddHistory(TimeManagerCall.TimeValue, "Sell", Name, TargetItem.Seller + " Sells " + Name + " to " + BuyerName, TargetItem.Price);
            
            if(TargetItem.Seller == CompanyManagerCall.PlayerCompanyName && TargetItem.SoldCount == 0)
            {
                NotificationManagerCall.AddNews("Award", "Your first shipment of " + Name + " products has been made!");
            }
            
            TargetItem.ItemCount--;
            TargetItem.SoldCount++;
        }

        foreach(var quality in result)
        {
            TotalQualityValue += quality;
        }
        TargetItem.QualityEvaluation = TotalQualityValue / TargetItem.SoldCount;

        if(PanelControllerCall.CurrentSidePanel != null)
        {
            if(PanelControllerCall.CurrentSidePanel.name == "ContractPanel")
            {
                ContractPanelController PanelComponent = PanelControllerCall.CurrentSidePanel.GetComponent<ContractPanelController>();
                
                PanelComponent.UpdateRemainQuantityText();

                if(PanelComponent.CurrentItem == TargetItem.RecipeInfo.Recipe.OutputName) 
                {
                    PanelComponent.UpdateSalesInfo();
                }
            }
        }

        return result;
    }

    void EnforceContract()
    {
        List<ContractInfo> FinishedContract = new List<ContractInfo>();
        foreach(var Sale in SalesItemArray)
        {
            foreach(var Contract in Sale.ContractList)
            {
                if(Contract.Term > 0)
                {
                    if(TimeManagerCall.TimeValue - Contract.ContractDate > TimeManagerCall.Hour)
                    {
                        List<float> ResultList = BuyItem(Sale.RecipeInfo.Recipe.OutputName, Contract.CompanyName, Contract.Quantity);
                        if(ResultList[0] <= 0)
                        {
                            switch(ResultList[0])
                            {
                                case -2f:
                                    if(Contract.CompanyName == CompanyManagerCall.PlayerCompanyName)
                                    {
                                        NotificationManagerCall.AddNews("Attention", Sale.RecipeInfo.Recipe.OutputName + " is out of stock. Seller cannot delivery to you at this time");
                                    }
                                    else if(Sale.Seller == CompanyManagerCall.PlayerCompanyName)
                                    {
                                        NotificationManagerCall.AddNews("Attention", "Your " + Sale.RecipeInfo.Recipe.OutputName + " is out of stock. You are losing reliability!");
                                    }
                                    else
                                    {
                                        // Write code about transfer info to AI script
                                    }
                                break;
                                case -1f : // BuyItem will debug it
                                break;
                                case 0f :
                                    if(Contract.CompanyName == CompanyManagerCall.PlayerCompanyName)
                                    {
                                        NotificationManagerCall.AddNews("Attention", Sale.RecipeInfo.Recipe.OutputName + " will not be delivered to you because you do not have enough money");
                                    }
                                    else
                                    {
                                        // Write code about transfer info to AI script
                                    }
                                break;
                            }
                        }

                        Contract.Term--;
                        Contract.ContractDate += TimeManagerCall.Hour;

                        if(Contract.Term == 0)
                        {
                            FinishedContract.Add(Contract);
                            if(Contract.CompanyName == CompanyManagerCall.PlayerCompanyName)
                            {
                                NotificationManagerCall.AddNews("Info", "Contract of " + Sale.RecipeInfo.Recipe.OutputName + " is just over.");
                            }
                        }
                    }
                }
            }

            foreach(var Contract in FinishedContract)
            {
                Sale.ContractList.Remove(Contract);
            }

            FinishedContract.Clear();
        }
    }

    public float CalculateMarketShare(string Type, string Company)
    {
        int WholeSalesCount = 0;
        int CompanySalesCount = 0;

        foreach(var Item in SalesItemArray)
        {
            if(Item.RecipeInfo.Recipe.GoodsObject.name == Type)
            {
                WholeSalesCount += Item.SoldCount;
                if(Item.Seller == Company)
                {
                    CompanySalesCount += Item.SoldCount;
                }
            }
        }

        if(WholeSalesCount > 0 && CompanySalesCount > 0)
            return (float)CompanySalesCount / (float)WholeSalesCount;
        else
            return 0;
    }

    public float CalculateMarketShare(string Name)
    {
        string Type = getSalesItemType(Name);
        int WholeSalesCount = 0;

        foreach(var Item in SalesItemArray)
        {
            if(Item.RecipeInfo.Recipe.GoodsObject.name == Type)
            {
                WholeSalesCount += Item.SoldCount;
            }
        }

        if(WholeSalesCount > 0 && SalesItemArray[getSalesIndex(Name)].SoldCount > 0)
            return (float)SalesItemArray[getSalesIndex(Name)].SoldCount / (float)WholeSalesCount;
        else
            return 0;
    }

    public int GetWholeSoldCount(string Type)
    {
        int result = 0;

        foreach(var Item in SalesItemArray)
        {
            if(Item.RecipeInfo.Recipe.GoodsObject.name == Type)
            {
                result += Item.SoldCount;
            }
        }

        return result;
    }

    public int GetWholeSoldCount(string Type, string Company)
    {
        int result = 0;

        foreach(var Item in SalesItemArray)
        {
            if(Item.RecipeInfo.Recipe.GoodsObject.name == Type && Item.Seller == Company)
            {
                result += Item.SoldCount;
            }
        }

        return result;
    }

    public float GetMarketPower(string Type, string Company)
    {
        return CalculateMarketShare(Type, Company) * GetWholeSoldCount(Type, Company);
    }

    public float GetMarketPower(string Company)
    {
        float result = 0f;
        List<string> TypeList = new List<string>();

        foreach(var Item in SalesItemArray)
        {
            bool isDuplicate = false;
            if(Item.Seller == Company)
            {
                foreach(var Type in TypeList)
                {
                    if(Type == Item.RecipeInfo.Recipe.GoodsObject.name)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
            }

            if(!isDuplicate)
            {
                TypeList.Add(Item.RecipeInfo.Recipe.GoodsObject.name);
            }
        }

        List<float> MarketPowerList = new List<float>();

        foreach(var Type in TypeList)
        {
            MarketPowerList.Add(GetMarketPower(Type, Company));
        }

        foreach(var value in MarketPowerList)
        {
            result += value;
        }

        if(MarketPowerList.Count > 0 && result > 0)
            return result / (float)MarketPowerList.Count;
        else
            return 0;
    }
}