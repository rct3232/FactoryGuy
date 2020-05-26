using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankValue : MonoBehaviour
{
    public class BankInfo
    {
        public BankInfo() {}
        public string Name;
        public float InterestRate;
        public float InterestReductionRate;
        public float InterestFloor;
        public float CreditStandard;
        public int MaximumLoanValue;
        public int MinimumLoanValue;
        public List<DealInfo> DealList;
    }
    public List<BankInfo> BankList;
    public class DealInfo
    {
        public DealInfo() {}
        public CompanyValue CostomerValue;
        public BankInfo ServiceBank;
        public int LoanValue;
        public float InterestRate;
        public int StartDate;
    }
    TimeManager TimeManagerCall;
    CompanyManager CompanyManagerCall;
    
    void Start()
    {
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();

        BankList = new List<BankInfo>();

        ADDTESTSET();
    }

    void FixedUpdate()
    {
        
    }

    BankInfo GetBankInfo(string BankName)
    {
        BankInfo Result = null;

        foreach(var Bank in BankList)
        {
            if (Bank.Name == BankName)
            {
                Result = Bank;
                break;
            }
        }

        return Result;
    }

    public List<string> GetBankList()
    {
        List<string> Result = new List<string>();

        foreach(var Bank in BankList)
        {
            Result.Add(Bank.Name);
        }

        return Result;
    }

    public int GetBankLevel(string BankName)
    {
        int Result = 0;

        BankInfo TargetBankInfo = GetBankInfo(BankName);
        if(TargetBankInfo.InterestRate > 0.3f) Result = 3;
        else if(TargetBankInfo.InterestRate > 0.1f) Result = 2;
        else Result = 1;

        return Result;
    }

    public float[] EvaluateCostomer(string BankName, CompanyValue CostomerValue)
    {
        float[] Result = new float[2]; // 0: Interest Rate 1: Maximum Loan Amount

        BankInfo TargetBankInfo = GetBankInfo(BankName);
        int ExistLoanValue = 0;

        if (CostomerValue.TotalValue < TargetBankInfo.CreditStandard)
        {
            Result = null;
            return Result;
        }

        foreach(var Deal in TargetBankInfo.DealList)
        {
            if (Deal.CostomerValue == CostomerValue)
            {
                ExistLoanValue = Deal.LoanValue;
                break;
            }
        }

        float InterestReductionRatio = Mathf.Min(1f, CostomerValue.TotalValue * TargetBankInfo.InterestReductionRate);

        Result[0] = Mathf.Max(TargetBankInfo.InterestFloor, TargetBankInfo.InterestRate * (1 - InterestReductionRatio));
        Result[1] = Mathf.Max(0, Mathf.Max(TargetBankInfo.MinimumLoanValue, TargetBankInfo.MaximumLoanValue * InterestReductionRatio) - ExistLoanValue);

        return Result;
    }

    public DealInfo StartNewDeal(CompanyValue TargetCompanyValue, string BankName, int LoanValue)
    {
        DealInfo NewDeal = new DealInfo();

        NewDeal.CostomerValue = TargetCompanyValue;
        NewDeal.ServiceBank = GetBankInfo(BankName);
        NewDeal.LoanValue = LoanValue;
        NewDeal.InterestRate = EvaluateCostomer(BankName, NewDeal.CostomerValue)[0];
        NewDeal.StartDate = TimeManagerCall.TimeValue;

        NewDeal.ServiceBank.DealList.Add(NewDeal);
        NewDeal.CostomerValue.GetEconomyValue().GetComponent<EconomyValue>().AddPersistHistory(TimeManagerCall.GetNextMonth(0) + (NewDeal.StartDate % TimeManagerCall.Month), TimeManagerCall.Month, "Upkeep", "Loan Interest", BankName + " Loan Interest", - Mathf.CeilToInt(NewDeal.InterestRate * NewDeal.LoanValue));
        NewDeal.CostomerValue.GetEconomyValue().GetComponent<EconomyValue>().AddHistory(TimeManagerCall.TimeValue, "Loan", "Loan Money", BankName + " Loan Money", LoanValue);

        return NewDeal;
    }

    public void RepayLoan(DealInfo TargetLoan, int Balance)
    {
        TargetLoan.LoanValue = Balance;

        string TargetEconomyHistory = TargetLoan.ServiceBank.Name + " Loan Interest";
        TargetLoan.CostomerValue.GetEconomyValue().GetComponent<EconomyValue>().ModifyPersistHistory(TargetEconomyHistory, -1, TimeManagerCall.Month, "Loan", "Loan Interest", TargetEconomyHistory, - Mathf.CeilToInt(TargetLoan.InterestRate * TargetLoan.LoanValue));
    }

    void EndDeal(DealInfo TargetLoan)
    {
        TargetLoan.CostomerValue.GetEconomyValue().GetComponent<EconomyValue>().DeletePersistHistory(TargetLoan.ServiceBank.Name + " Loan Interest");

        TargetLoan.ServiceBank.DealList.Remove(TargetLoan);
    }

    void ADDTESTSET()
    {
        BankInfo NewInfo = new BankInfo();

        NewInfo.Name = "TEST1";
        NewInfo.InterestRate = 0.35f;
        NewInfo.InterestReductionRate = 0.01f;
        NewInfo.InterestFloor = 0.2f;
        NewInfo.MinimumLoanValue = 10000;
        NewInfo.MaximumLoanValue = 1000000;
        NewInfo.CreditStandard = 50;
        NewInfo.DealList = new List<DealInfo>();
        BankList.Add(NewInfo);

        NewInfo = new BankInfo();
        NewInfo.Name = "TEST2";
        NewInfo.InterestRate = 0.31f;
        NewInfo.InterestReductionRate = 0.007f;
        NewInfo.InterestFloor = 0.25f;
        NewInfo.MinimumLoanValue = 1000;
        NewInfo.MaximumLoanValue = 2000000;
        NewInfo.CreditStandard = 100;
        NewInfo.DealList = new List<DealInfo>();
        BankList.Add(NewInfo);

        NewInfo = new BankInfo();
        NewInfo.Name = "TEST3";
        NewInfo.InterestRate = 0.25f;
        NewInfo.InterestReductionRate = 0.015f;
        NewInfo.InterestFloor = 0.2f;
        NewInfo.MinimumLoanValue = 3000;
        NewInfo.MaximumLoanValue = 10000000;
        NewInfo.CreditStandard = 200;
        NewInfo.DealList = new List<DealInfo>();
        BankList.Add(NewInfo);

        NewInfo = new BankInfo();
        NewInfo.Name = "TEST4";
        NewInfo.InterestRate = 0.05f;
        NewInfo.InterestReductionRate = 0.005f;
        NewInfo.InterestFloor = 0.035f;
        NewInfo.MinimumLoanValue = 40000;
        NewInfo.MaximumLoanValue = 100000000;
        NewInfo.CreditStandard = 400;
        NewInfo.DealList = new List<DealInfo>();
        BankList.Add(NewInfo);
    }
}
