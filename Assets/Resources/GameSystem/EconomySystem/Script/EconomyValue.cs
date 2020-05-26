using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyValue : MonoBehaviour
{
    public class History
    {
        public History() {}
        public int Date;
        public string Detail;
        public string DebugDetail;
        public int Amount;
        public int ResultBalance;
    }
    public class PersistHistory
    {
        public PersistHistory() {}
        public int NextEnforceDate;
        public int EnforceTerm;
        public string Category;
        public string Detail;
        public string DebugDetail;
        public int Amount;
    }
    public class MonthlyHistory
    {
        public MonthlyHistory(int Time)
        {
            TimeValue = Time;

            SellList = new List<History>();
            MilestoneList = new List<History>();
            LoanList = new List<History>();

            BuyList = new List<History>();
            InstallList = new List<History>();
            UpkeepList = new List<History>();
            ResearchList = new List<History>();
            EmployeePayList = new List<History>();
            RealEstateList = new List<History>();

            SellSub = 0;
            MilestoneSub = 0;
            LoanSub = 0;

            BuySub = 0;
            InstallSub = 0;
            UpkeepSub = 0;
            ResearchSub = 0;
            EmployeePaySub = 0;
            RealEstateSub = 0;

            Balance = 0;
        }
        public int TimeValue;
        public List<History> SellList;
        public List<History> MilestoneList;
        public List<History> LoanList;

        public List<History> BuyList;
        public List<History> InstallList;
        public List<History> UpkeepList;
        public List<History> ResearchList;
        public List<History> EmployeePayList;
        public List<History> RealEstateList;

        public int SellSub;
        public int MilestoneSub;
        public int LoanSub;
        
        public int BuySub;
        public int InstallSub;
        public int UpkeepSub;
        public int ResearchSub;
        public int EmployeePaySub;
        public int RealEstateSub;

        public int Balance;
    }
    List<PersistHistory> PersistHistoryArray;
    List<MonthlyHistory> HistoryList = new List<MonthlyHistory>();
    public int Balance;
    public int RealtimeIncome;
    InGameValue ValueCall;
    TimeManager TimeManagerCall;
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    PanelController PanelControllerCall;
    int CurrentHistoryIndex;

    // Start is called before the first frame update
    void Start()
    {
        ValueCall = GameObject.Find("BaseSystem").GetComponent<InGameValue>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CompanyManagerCall = transform.parent.parent.gameObject.GetComponent<CompanyManager>();
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();
        RealtimeIncome = 0;
        CurrentHistoryIndex = 0;

        PersistHistoryArray = new List<PersistHistory>();
        
        AddNewMonthlyHistroy();

        CalculateRealtimeIncome();
        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFinanceInfo(RealtimeIncome, Balance);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(TimeManagerCall.TimeValue % TimeManagerCall.Month < TimeManagerCall.PlaySpeed)
        {
            AddNewMonthlyHistroy();
            CalculateRealtimeIncome();
        }
        for(int i = 0; i < PersistHistoryArray.Count; i++)
        {
            if(PersistHistoryArray[i].NextEnforceDate <= TimeManagerCall.TimeValue)
            {
                EnforcePersistHistory(i);
            }
        }
    }

    void AddNewMonthlyHistroy()
    {
        int MonthDifference = (Mathf.FloorToInt(TimeManagerCall.TimeValue / TimeManagerCall.Month) + 1) - HistoryList.Count;
        for(int i = 0; i < MonthDifference; i++)
        {
            MonthlyHistory newHistory = new MonthlyHistory(HistoryList.Count * TimeManagerCall.Month);
            HistoryList.Add(newHistory);
        }

        CurrentHistoryIndex = HistoryList.Count - 1;

        HistoryList[CurrentHistoryIndex].Balance = Balance;
    }

    public List<History> GetHistory(int Month)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        List<History> ResultList = new List<History>();

        if(Month <= CurrentHistoryIndex)
        {
            ResultList.AddRange(HistoryList[Month].SellList);
            ResultList.AddRange(HistoryList[Month].MilestoneList);
            ResultList.AddRange(HistoryList[Month].LoanList);

            ResultList.AddRange(HistoryList[Month].BuyList);
            ResultList.AddRange(HistoryList[Month].InstallList);
            ResultList.AddRange(HistoryList[Month].UpkeepList);
            ResultList.AddRange(HistoryList[Month].ResearchList);
            ResultList.AddRange(HistoryList[Month].EmployeePayList);
            ResultList.AddRange(HistoryList[Month].RealEstateList);
        }

        return ResultList;
    }

    public List<History> GetIncome(int Month)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        List<History> ResultList = new List<History>();

        if(Month <= CurrentHistoryIndex)
        {
            ResultList.AddRange(HistoryList[Month].SellList);
            ResultList.AddRange(HistoryList[Month].MilestoneList);
            ResultList.AddRange(HistoryList[Month].LoanList);
        }

        return ResultList;
    }

    public List<History> GetExpense(int Month)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        List<History> ResultList = new List<History>();

        if(Month <= CurrentHistoryIndex)
        {
            ResultList.AddRange(HistoryList[Month].BuyList);
            ResultList.AddRange(HistoryList[Month].InstallList);
            ResultList.AddRange(HistoryList[Month].UpkeepList);
            ResultList.AddRange(HistoryList[Month].ResearchList);
            ResultList.AddRange(HistoryList[Month].EmployeePayList);
            ResultList.AddRange(HistoryList[Month].RealEstateList);
        }

        return ResultList;
    }

    public List<History> GetHistoryByCategory(int Month, string Category)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        if(Month <= CurrentHistoryIndex)
        {
            switch(Category)
            {
                case "Sell" : return HistoryList[Month].SellList;
                case "Milestone" : return HistoryList[Month].MilestoneList;
                case "Loan" : return HistoryList[Month].LoanList;
                case "Buy" : return HistoryList[Month].BuyList;
                case "Install" : return HistoryList[Month].InstallList;
                case "Upkeep" : return HistoryList[Month].UpkeepList;
                case "Research" : return HistoryList[Month].ResearchList;
                case "Employee Pay" : return HistoryList[Month].EmployeePayList;
                case "Real Estate" : return HistoryList[Month].RealEstateList;
            }
        }

        return new List<History>();
    }

    public int GetHistorySub(int Month)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        int Result = 0;

        if(Month <= CurrentHistoryIndex)
        {
            Result += GetHistorySubByCategory(Month, "Sell");
            Result += GetHistorySubByCategory(Month, "Milestone");
            Result += GetHistorySubByCategory(Month, "Loan");

            Result += GetHistorySubByCategory(Month, "Buy");
            Result += GetHistorySubByCategory(Month, "Install");
            Result += GetHistorySubByCategory(Month, "Upkeep");
            Result += GetHistorySubByCategory(Month, "Research");
            Result += GetHistorySubByCategory(Month, "Employee Pay");
            Result += GetHistorySubByCategory(Month, "Real Estate");
        }

        return Result;
    }

    public int GetHistoryIncomeSub(int Month)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        int Result = 0;

        if(Month <= CurrentHistoryIndex)
        {
            Result += GetHistorySubByCategory(Month, "Sell");
            Result += GetHistorySubByCategory(Month, "Milestone");
            Result += GetHistorySubByCategory(Month, "Loan");
        }
        
        return Result;
    }

    public int GetHistoryExpenseSub(int Month)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        int Result = 0;

        if(Month <= CurrentHistoryIndex)
        {
            Result += GetHistorySubByCategory(Month, "Buy");
            Result += GetHistorySubByCategory(Month, "Install");
            Result += GetHistorySubByCategory(Month, "Upkeep");
            Result += GetHistorySubByCategory(Month, "Research");
            Result += GetHistorySubByCategory(Month, "Employee Pay");
            Result += GetHistorySubByCategory(Month, "Real Estate");
        }
        
        return Result;
    }

    public int GetHistorySubByCategory(int Month, string Category)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        if(Month <= CurrentHistoryIndex)
        {
            switch(Category)
            {
                case "Sell" : return HistoryList[Month].SellSub;
                case "Milestone" : return HistoryList[Month].MilestoneSub;
                case "Loan" : return HistoryList[Month].LoanSub;
                case "Buy" : return HistoryList[Month].BuySub;
                case "Install" : return HistoryList[Month].InstallSub;
                case "Upkeep" : return HistoryList[Month].UpkeepSub;
                case "Research" : return HistoryList[Month].ResearchSub;
                case "Employee Pay" : return HistoryList[Month].EmployeePaySub;
                case "Real Estate" : return HistoryList[Month].RealEstateSub;
            }
        }

        return 0;
    }

    public int GetHistoryBalance(int Month)
    {
        if(Month == -1) Month = CurrentHistoryIndex;

        if(Month <= CurrentHistoryIndex)
        {
            return HistoryList[Month].Balance;
        }

        return 0;
    }

    public void AddHistory(int Date, string Category, string Detail, string DebugDetail, int Amount)
    {
        AddNewMonthlyHistroy();

        History NewHistory = new History();

        NewHistory.Date = Date;
        NewHistory.Detail = Detail;
        NewHistory.DebugDetail = DebugDetail;
        NewHistory.Amount = Amount;
        NewHistory.ResultBalance = Balance;
        
        switch(Category)
        {
            case "Sell" : HistoryList[CurrentHistoryIndex].SellList.Add(NewHistory); HistoryList[CurrentHistoryIndex].SellSub += Amount; break;
            case "Milestone" : HistoryList[CurrentHistoryIndex].MilestoneList.Add(NewHistory); HistoryList[CurrentHistoryIndex].MilestoneSub += Amount; break;
            case "Loan" : HistoryList[CurrentHistoryIndex].LoanList.Add(NewHistory); HistoryList[CurrentHistoryIndex].LoanSub += Amount; break;
            case "Buy" : HistoryList[CurrentHistoryIndex].BuyList.Add(NewHistory); HistoryList[CurrentHistoryIndex].BuySub += Amount; break;
            case "Install" : HistoryList[CurrentHistoryIndex].InstallList.Add(NewHistory); HistoryList[CurrentHistoryIndex].InstallSub += Amount; break;
            case "Upkeep" : HistoryList[CurrentHistoryIndex].UpkeepList.Add(NewHistory); HistoryList[CurrentHistoryIndex].UpkeepSub += Amount; break;
            case "Research" : HistoryList[CurrentHistoryIndex].ResearchList.Add(NewHistory); HistoryList[CurrentHistoryIndex].ResearchSub += Amount; break;
            case "Employee Pay" : HistoryList[CurrentHistoryIndex].EmployeePayList.Add(NewHistory); HistoryList[CurrentHistoryIndex].EmployeePaySub += Amount; break;
            case "Real Estate" : HistoryList[CurrentHistoryIndex].RealEstateList.Add(NewHistory); HistoryList[CurrentHistoryIndex].RealEstateSub += Amount; break;
        }

        HistoryList[CurrentHistoryIndex].Balance += Amount;
        Balance += Amount;

        CalculateRealtimeIncome();
    }

    void EnforcePersistHistory(int Index)
    {
        Balance += PersistHistoryArray[Index].Amount;
        PersistHistoryArray[Index].NextEnforceDate += PersistHistoryArray[Index].EnforceTerm;

        AddHistory(TimeManagerCall.TimeValue, PersistHistoryArray[Index].Category, PersistHistoryArray[Index].Detail, PersistHistoryArray[Index].DebugDetail, PersistHistoryArray[Index].Amount);
    }

    public void AddPersistHistory(int FirstEnforceDate, int Term, string Category, string Detail, string DebugDetail, int Amount)
    {
        PersistHistory NewPersistHistory = new PersistHistory();

        NewPersistHistory.NextEnforceDate = FirstEnforceDate;
        NewPersistHistory.EnforceTerm = Term;
        NewPersistHistory.Category = Category;
        NewPersistHistory.Detail = Detail;
        NewPersistHistory.DebugDetail = DebugDetail;
        NewPersistHistory.Amount = Amount;

        PersistHistoryArray.Add(NewPersistHistory);

        CalculateRealtimeIncome();
    }

    int FindHistoryIndex(string DebugDetail)
    {
        int result = -1;

        for(int i = 0; i < PersistHistoryArray.Count; i++)
        {
            if(PersistHistoryArray[i].DebugDetail == DebugDetail)
            {
                result = i;
                break;
            }
        }

        return result;
    }

    public void ModifyPersistHistory(string DebugDetailForSearch, int Date, int Term, string Category, string Detail, string DebugDetail, int Amount)
    {
        int Index = FindHistoryIndex(DebugDetailForSearch);

        if(Index != -1)
        {
            if(Date != -1)
            {
                PersistHistoryArray[Index].NextEnforceDate = Date;
            }
            PersistHistoryArray[Index].EnforceTerm = Term;
            PersistHistoryArray[Index].Category = Category;
            PersistHistoryArray[Index].Detail = Detail;
            PersistHistoryArray[Index].DebugDetail = DebugDetail;
            PersistHistoryArray[Index].Amount = Amount;

            CalculateRealtimeIncome();
        }
        else
        {
            Debug.Log("Wrong Index in PersistHistoryArray at ModifyPersistHistory()");
        }
    }

    public void ModifyPersistHistory(string DebugDetailForSearch, int Amount)
    {
        int Index = FindHistoryIndex(DebugDetailForSearch);

        if(Index != -1)
        {
            PersistHistoryArray[Index].Amount = Amount;

            CalculateRealtimeIncome();
        }
        else
        {
            Debug.Log("Wrong Index in PersistHistoryArray at ModifyPersistHistory()");
        }
    }

    public void DeletePersistHistory(string DebugDetailForSearch)
    {
        int Index = FindHistoryIndex(DebugDetailForSearch);

        if(Index != -1)
        {
            PersistHistoryArray.RemoveAt(Index);

            CalculateRealtimeIncome();
        }
        else
        {
            Debug.Log("Wrong Index in PersistHistoryArray at DeletePersistHistory()");
        }
    }

    void CalculateRealtimeIncome()
    {
        int result = 0;
        int ThisMonth = TimeManagerCall.TimeValue - (TimeManagerCall.TimeValue % TimeManagerCall.Month);

        result += GetHistorySub(CurrentHistoryIndex);
        
        foreach(var tmp in PersistHistoryArray)
        {
            result = result + tmp.Amount;
        }

        RealtimeIncome = result;

        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName) PanelControllerCall.UpdateFinanceInfo(RealtimeIncome, Balance);
    }
}
