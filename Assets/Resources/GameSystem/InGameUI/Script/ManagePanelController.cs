using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePanelController : MonoBehaviour
{
    // public PanelController CallPanelController;
    // public bool isInitialized = false;
    
    // void Start()
    // {
        
    // }

    // public void Initializing()
    // {
    //     DisplayDetailLineGraph("Income and Expense");
    // }

    // void Update()
    // {
        
    // }

    // public void DetailLineGraphCategoryValueChange()
    // {
    //     int Dropdownvalue = DetailLineGraphCategoryDropdown.GetComponent<Dropdown>().value;

    //     DetailLineGraphCarrier.GetComponent<GraphDrawer>().LineGraphClear();
    //     // StartCoroutine("DisplayDetailLineGraph", DetailLineGraphCategoryDropdown.GetComponent<Dropdown>().options[Dropdownvalue].text);
    //     DisplayDetailLineGraph(DetailLineGraphCategoryDropdown.GetComponent<Dropdown>().options[Dropdownvalue].text);
    // }

    // void DisplayDetailLineGraph(string Category)
    // {
    //     float[] MinMaxValue = new float[2];
    //     MinMaxValue[0] = 0;
    //     MinMaxValue[1] = 0;
    //     float StandardValue;
    //     List<string> DateIndex = new List<string>();
    //     float YaxisIndexGap;
    //     List<float>[] DataList;
    //     Color[] GraphColor;
        
    //     int CeilMonth = Mathf.FloorToInt((CallTimeManager.TimeValue % CallTimeManager.Year) / CallTimeManager.Month);
    //     int FloorMonth = CeilMonth - 12;
    //     if(FloorMonth < 0) FloorMonth = 0;

    //     for(int i = FloorMonth; i < FloorMonth + 12; i++)
    //     {
    //         DateIndex.Add(CallTimeManager.ConvertMonthToString(i % 12 + 1));
    //     }

    //     if(Category == "Income and Expense")
    //     {
    //         DataList = new List<float>[3];
    //         for(int i = 0; i < 3; i++) DataList[i] = new List<float>();

    //         int ExpenseValue;
    //         int IncomeValue;
    //         int NetProfitValue;

    //         for(int i = FloorMonth; i < CeilMonth; i++)
    //         {
    //             ExpenseValue = CallEconomyValue.GetHistoryExpenseSub(i);
    //             if(MinMaxValue[0] > ExpenseValue) MinMaxValue[0] = ExpenseValue;
    //             IncomeValue = CallEconomyValue.GetHistoryIncomeSub(i);
    //             if(MinMaxValue[1] < IncomeValue) MinMaxValue[1] = IncomeValue;
    //             NetProfitValue = IncomeValue + ExpenseValue;
    //             if(MinMaxValue[0] > NetProfitValue) MinMaxValue[0] = NetProfitValue;
    //             if(MinMaxValue[1] < NetProfitValue) MinMaxValue[1] = NetProfitValue;

    //             DataList[0].Add(IncomeValue);
    //             DataList[1].Add(ExpenseValue);
    //             DataList[2].Add(NetProfitValue);
    //         }
            

    //         StandardValue = 0;
    //         YaxisIndexGap = float.NaN;

    //         GraphColor = new Color[3];
    //         GraphColor[0] = new Color(0,1f,0,1f);
    //         GraphColor[1] = new Color(1f,0,0,1f);
    //         GraphColor[2] = new Color(0,0,1f,1f);
    //     }
    //     else if(Category == "Balance")
    //     {
    //         GameObject CompanyObjectCarrier = GameObject.Find("CompanyManager");
    //         int CompanyCount = 0;
    //         for(int i = 0; i < CompanyObjectCarrier.transform.childCount; i++)
    //         {
    //             CompanyValue CallTargetCompanyValue = CompanyObjectCarrier.transform.GetChild(i).gameObject.GetComponent<CompanyValue>();
    //             if(!CallTargetCompanyValue.Unevaluable) CompanyCount++;
    //         }

    //         DataList = new List<float>[CompanyCount];
    //         int CurrentIndex = 0;
            

    //         for(int i = 0; i < CompanyObjectCarrier.transform.childCount; i++)
    //         {
    //             CompanyValue CallTargetCompanyValue = CompanyObjectCarrier.transform.GetChild(i).gameObject.GetComponent<CompanyValue>();
    //             EconomyValue CallTargetCompanyEconomyValue = CallTargetCompanyValue.GetEconomyValue().GetComponent<EconomyValue>();
    //             if(!CallTargetCompanyValue.Unevaluable)
    //             {
    //                 List<float> BalanceList = new List<float>();
    //                 int Balance;

    //                 for(int j = FloorMonth; j < CeilMonth; j++)
    //                 {
    //                     Balance = CallTargetCompanyEconomyValue.GetHistoryBalance(j);

    //                     if(MinMaxValue[0] > Balance) MinMaxValue[0] = Balance;
    //                     if(MinMaxValue[1] < Balance) MinMaxValue[1] = Balance;

    //                     BalanceList.Add(Balance);

    //                     if(CurrentIndex == 0 && j == FloorMonth)
    //                     {
    //                         MinMaxValue[0] = Balance;
    //                         MinMaxValue[1] = Balance;
    //                     }
    //                 }

    //                 DataList[CurrentIndex++] = BalanceList;
    //             }
    //         }

    //         StandardValue = float.NaN;
    //         YaxisIndexGap = float.NaN;
    //         GraphColor = null;
    //     }
    //     else if(Category == "Company Value")
    //     {
    //         GameObject CompanyObjectCarrier = GameObject.Find("CompanyManager");
    //         int CompanyCount = 0;
    //         for(int i = 0; i < CompanyObjectCarrier.transform.childCount; i++)
    //         {
    //             CompanyValue CallTargetCompanyValue = CompanyObjectCarrier.transform.GetChild(i).gameObject.GetComponent<CompanyValue>();
    //             if(!CallTargetCompanyValue.Unevaluable) CompanyCount++;
    //         }

    //         DataList = new List<float>[CompanyCount];
    //         int CurrentIndex = 0;
    //         for(int i = 0; i < CompanyObjectCarrier.transform.childCount; i++)
    //         {
    //             CompanyValue CallTargetCompanyValue = CompanyObjectCarrier.transform.GetChild(i).gameObject.GetComponent<CompanyValue>();
    //             if(!CallTargetCompanyValue.Unevaluable)
    //             {
    //                 List<float> CompanyValueList = new List<float>();
    //                 if(CallTargetCompanyValue.CompanyValueHistory.Count > 1)
    //                 {
    //                     for(int j = FloorMonth; j < CeilMonth; j++)
    //                     {
    //                         float Value = CallTargetCompanyValue.CompanyValueHistory[j];
    //                         CompanyValueList.Add(Value);

    //                         if(CurrentIndex == 0 && j == FloorMonth)
    //                         {
    //                             MinMaxValue[0] = Value;
    //                             MinMaxValue[1] = Value;
    //                         }
    //                     }
    //                 }
                    
    //                 DataList[CurrentIndex++] = CompanyValueList;
    //             }
    //         }

    //         StandardValue = float.NaN;
    //         YaxisIndexGap = float.NaN;
    //         GraphColor = null;
    //     }
    //     else if(Category == "Market Share")
    //     {
    //         SalesValue CallSalesValue = GameObject.Find("SalesManager").GetComponent<SalesValue>();
    //         int SalesCount = 0;
    //         foreach(var Sales in CallSalesValue.SalesItemArray)
    //         {
    //             if(Sales.RecipeInfo.Owner == CallCompanyManager.PlayerCompanyName) SalesCount++;
    //         }

    //         DataList = new List<float>[SalesCount];
    //         int CurrentIndex = 0;
    //         foreach(var Sales in CallSalesValue.SalesItemArray)
    //         {
    //             if(Sales.RecipeInfo.Owner == CallCompanyManager.PlayerCompanyName)
    //             {
    //                 List<float> MarketShareList = new List<float>();
    //                 if(Sales.SoldCountList.Count > 1)
    //                 {
    //                     for(int j = FloorMonth; j < CeilMonth; j++)
    //                     {
    //                         int MonthlyTotalSoldCount = 0;
    //                         float MarketSharePoint = 0f;
    //                         foreach(var Target in CallSalesValue.SalesItemArray)
    //                         {
    //                             if(Target.RecipeInfo.Recipe.Type == Sales.RecipeInfo.Recipe.Type)
    //                             {
    //                                 MonthlyTotalSoldCount += Target.SoldCountList[j];
    //                             }
    //                         }

    //                         if(MonthlyTotalSoldCount > 0)
    //                         {
    //                             MarketSharePoint = Mathf.FloorToInt(MonthlyTotalSoldCount / Sales.SoldCountList[j] * 1000f) * 0.1f;
    //                         }

    //                         MarketShareList.Add(MarketSharePoint);
    //                     }
    //                 }
    //                 DataList[CurrentIndex++] = MarketShareList;
    //             }
    //         }
            
    //         StandardValue = float.NaN;
    //         YaxisIndexGap = float.NaN;
    //         MinMaxValue[0] = 0f;
    //         MinMaxValue[1] = 100f;
    //         GraphColor = null;
    //     }
    //     else if(Category == "Sales")
    //     {
    //         SalesValue CallSalesValue = GameObject.Find("SalesManager").GetComponent<SalesValue>();
    //         int SalesCount = 0;
    //         foreach(var Sales in CallSalesValue.SalesItemArray)
    //         {
    //             if(Sales.RecipeInfo.Owner == CallCompanyManager.PlayerCompanyName) SalesCount++;
    //         }

    //         DataList = new List<float>[SalesCount];
    //         int CurrentIndex = 0;
    //         foreach(var Sales in CallSalesValue.SalesItemArray)
    //         {
    //             if(Sales.RecipeInfo.Owner == CallCompanyManager.PlayerCompanyName)
    //             {
    //                 List<float> SalesCountList = new List<float>();
    //                 if(CurrentIndex == 0)
    //                 {
    //                     MinMaxValue[0] = Sales.SoldCount;
    //                     MinMaxValue[1] = Sales.SoldCount;
    //                 }
    //                 if(Sales.SoldCountList.Count > 1)
    //                 {
    //                     for(int j = FloorMonth; j < CeilMonth; j++)
    //                     {
    //                         float Value = Sales.SoldCountList[j];
    //                         SalesCountList.Add(Value);

    //                         if(CurrentIndex == 0 && j == FloorMonth)
    //                         {
    //                             MinMaxValue[0] = Value;
    //                             MinMaxValue[1] = Value;
    //                         }
    //                     }
    //                 }
                    
    //                 DataList[CurrentIndex++] = SalesCountList;
    //             }
    //         }
            
    //         StandardValue = float.NaN;
    //         YaxisIndexGap = float.NaN;
    //         GraphColor = null;
    //     }
    //     else
    //     {
    //         DataList = new List<float>[1];
    //         DataList[0] = new List<float>();

    //         StandardValue = float.NaN;
    //         YaxisIndexGap = float.NaN;
    //         GraphColor = null;
    //     }

    //     DetailLineGraphCarrier.GetComponent<GraphDrawer>().DrawLineGraph(MinMaxValue, StandardValue, DateIndex, YaxisIndexGap, DataList, GraphColor);
    // }
}
