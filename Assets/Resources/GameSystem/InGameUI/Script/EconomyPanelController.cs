using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    [SerializeField]GameObject SummaryPanel;
    [SerializeField]GameObject DetailScrollPanel;
    [SerializeField]GameObject DetailLineGraphPanel;
    [SerializeField]GameObject DetailReportPanel;
    GameObject IncomePieGraphPanel, IncomePieGraphCarrier, ExpensePieGraphPanel, ExpensePieGraphCarrier, SummaryReportPanel, SummaryReportCarrier, DetailLineGraphCategoryPanel
    , DetailLineGraphInnerPanel, DetailLineGraphCategoryDropdown, DetailLineGraphCarrier, DetailReportCategoryPanel, DetailReportInnerPanel, DetailReportPanelCategoryDropdown, DetailReportCarrier;
    CompanyManager CallCompanyManager;
    EconomyValue CallEconomyValue;
    TimeManager CallTimeManager;
    bool Updated = false;

    void Awake()
    {
        CallCompanyManager = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CallEconomyValue = CallCompanyManager.GetPlayerCompanyValue().GetEconomyValue().GetComponent<EconomyValue>();
        CallTimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();

        IncomePieGraphPanel = SummaryPanel.transform.GetChild(0).gameObject;
        IncomePieGraphCarrier = IncomePieGraphPanel.transform.GetChild(1).gameObject;
        ExpensePieGraphPanel = SummaryPanel.transform.GetChild(1).gameObject;
        ExpensePieGraphCarrier = ExpensePieGraphPanel.transform.GetChild(1).gameObject;
        SummaryReportPanel = SummaryPanel.transform.GetChild(2).gameObject;
        SummaryReportCarrier = SummaryReportPanel.transform.GetChild(0).gameObject;
        DetailLineGraphCategoryPanel = DetailLineGraphPanel.transform.GetChild(0).gameObject;
        DetailLineGraphInnerPanel = DetailLineGraphPanel.transform.GetChild(1).gameObject;
        DetailLineGraphCategoryDropdown = DetailLineGraphCategoryPanel.transform.GetChild(1).gameObject;
        DetailLineGraphCarrier = DetailLineGraphInnerPanel.transform.GetChild(0).gameObject;
        DetailReportCategoryPanel = DetailReportPanel.transform.GetChild(0).gameObject;
        DetailReportInnerPanel = DetailReportPanel.transform.GetChild(1).gameObject;
        DetailReportPanelCategoryDropdown = DetailReportCategoryPanel.transform.GetChild(1).gameObject;
        DetailReportCarrier = DetailReportInnerPanel.transform.GetChild(1).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Scaling()
    {
        SummaryPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 6f);
        Vector2 SummaryPanelSize = SummaryPanel.GetComponent<RectTransform>().sizeDelta;
        DetailScrollPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Screen.height - CallPanelController.CurrentUIsize - SummaryPanelSize.y);
        Vector2 DetailScrollPanelSize = DetailScrollPanel.GetComponent<RectTransform>().sizeDelta;

        IncomePieGraphPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 5.6f, 0);
        float PieGraphPanelSize = IncomePieGraphPanel.GetComponent<RectTransform>().sizeDelta.x - (CallPanelController.CurrentEdgePadding * 2f);
        IncomePieGraphPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PieGraphPanelSize, CallPanelController.CurrentEdgePadding);
        IncomePieGraphPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);
        IncomePieGraphCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(PieGraphPanelSize, PieGraphPanelSize);
        IncomePieGraphCarrier.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding * 2f);
        IncomePieGraphPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PieGraphPanelSize, PieGraphPanelSize);
        IncomePieGraphPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding * 2f);
        ExpensePieGraphPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(PieGraphPanelSize + CallPanelController.CurrentEdgePadding, 0);
        ExpensePieGraphPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PieGraphPanelSize, CallPanelController.CurrentEdgePadding);
        ExpensePieGraphPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - CallPanelController.CurrentEdgePadding);
        ExpensePieGraphCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(PieGraphPanelSize, PieGraphPanelSize);
        ExpensePieGraphCarrier.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - CallPanelController.CurrentEdgePadding * 2f);
        ExpensePieGraphPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PieGraphPanelSize, PieGraphPanelSize);
        ExpensePieGraphPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - CallPanelController.CurrentEdgePadding * 2f);
        SummaryReportPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 4.4f, 0);
        SummaryReportCarrier.GetComponent<RectTransform>().offsetMin = new Vector2(0, CallPanelController.CurrentEdgePadding);
        SummaryReportCarrier.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);
        SummaryReportCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 1.25f);
        SummaryReportCarrier.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 0.5f);
        for(int i = 2; i < 8; i++)
        {
            SummaryReportCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
        }
        SummaryReportCarrier.transform.GetChild(8).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 0.5f);
        for(int i = 9; i < 12; i++)
        {
            SummaryReportCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
        }
        SummaryReportCarrier.transform.GetChild(12).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 0.5f);
        SummaryReportCarrier.transform.GetChild(13).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 1.25f);
        for(int i = 0; i < SummaryReportCarrier.transform.childCount; i++)
        {
            if(i != 1 && i != 8 && i != 12 && i != 13)
            {
                SummaryReportCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.4f);
                SummaryReportCarrier.transform.GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.4f);
            }
        }
        SummaryReportCarrier.transform.GetChild(13).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.6f);
        SummaryReportCarrier.transform.GetChild(13).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.2f);
        SummaryReportCarrier.transform.GetChild(13).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.4f);

        float DetailPanelBasicSize = CallPanelController.CurrentUIsize * 5.6f;
        float DetailPanelCarrierBasicSize = CallPanelController.CurrentUIsize * 4.6f;
        float DetailPanelCategoryObjectSize = CallPanelController.CurrentUIsize - CallPanelController.CurrentEdgePadding;

        DetailScrollPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        DetailScrollPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, 0);
        DetailScrollPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(- CallPanelController.CurrentEdgePadding, 0);
        DetailScrollPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        DetailLineGraphPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, DetailPanelBasicSize);
        DetailLineGraphCategoryPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
        DetailLineGraphCategoryPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, DetailPanelCategoryObjectSize);
        DetailLineGraphCategoryPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentUIsize,  - (CallPanelController.CurrentEdgePadding * 0.5f));
        DetailLineGraphCategoryPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, DetailPanelCategoryObjectSize);
        DetailLineGraphCategoryPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,  - (CallPanelController.CurrentEdgePadding * 0.5f));
        DetailLineGraphInnerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, DetailPanelCarrierBasicSize);
        DetailLineGraphCarrier.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        DetailLineGraphCarrier.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        DetailLineGraphCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 14.2f, DetailPanelCarrierBasicSize - CallPanelController.CurrentEdgePadding);
        DetailLineGraphCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentUIsize, 0);
        DetailLineGraphCarrier.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 14.2f, CallPanelController.CurrentEdgePadding);
        DetailLineGraphCarrier.transform.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentUIsize, 0);
        DetailLineGraphCarrier.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize * 4.2f);
        DetailLineGraphCarrier.transform.GetChild(2).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        DetailReportCategoryPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
        DetailReportCategoryPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, DetailPanelCategoryObjectSize);
        DetailReportCategoryPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentUIsize,  - (CallPanelController.CurrentEdgePadding * 0.5f));
        DetailReportCategoryPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, DetailPanelCategoryObjectSize);
        DetailReportCategoryPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,  - (CallPanelController.CurrentEdgePadding * 0.5f));
        DetailReportInnerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, DetailPanelCarrierBasicSize + CallPanelController.CurrentEdgePadding);
        DetailReportInnerPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
        DetailReportInnerPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        DetailReportCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 15.2f, 0);
        DetailReportCarrier.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        
        for(int i = 0; i < DetailReportCarrier.transform.childCount; i++)
        {
            DetailReportCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
            DetailReportCarrier.transform.GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 4f, 0);
            DetailReportCarrier.transform.GetChild(i).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 2f, 0);
            DetailReportCarrier.transform.GetChild(i).GetChild(3).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding * 0.5f, 0);
            DetailReportCarrier.transform.GetChild(i).GetChild(4).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 4f, 0);
            DetailReportCarrier.transform.GetChild(i).GetChild(5).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 2f, 0);
            for(int j = 0; j < DetailReportCarrier.transform.GetChild(i).GetChild(j).childCount; j++)
            {
                if(i > 1) DetailReportCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
                else DetailReportCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding * 0.75f);
            }
        }

        CallPanelController.FontScaling(gameObject);
    }

    public void Initializing()
    {
        // StartCoroutine("DisplayDetailLineGraph", "Income and Expense");
        DisplayDetailLineGraph("Income and Expense");
        DiplaySummary();
        DisplayDetailCurrentReport();
        DisplayDetailReportCategory();

        if(DetailReportPanelCategoryDropdown.GetComponent<Dropdown>().options.Count == 0) ClearDetailReportPanel(false);
        else DisplayDetailPastReport(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Updated)
        {
            ResizingReportPanel();
        }
    }

    void DiplaySummary()
    {
        List<EconomyValue.History> HistoryList = new List<EconomyValue.History>();

        SummaryReportCarrier.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "TEST";
        string[] ExpenseCategoryList = new string[] {"Buy", "Install", "Upkeep", "Research", "Employee Pay", "Real Estate"};
        string[] IncomeCategoryList = new string[] {"Sell", "Subsidy", "Royalty"};

        List<int> ExpenseAmountList = new List<int>();
        List<int> IncomeAmountList = new List<int>();

        for(int i = 0; i < ExpenseCategoryList.Length; i++)
        {
            ExpenseAmountList.Add(CallEconomyValue.GetHistorySubByCategory(-1, ExpenseCategoryList[i]));

            SummaryReportCarrier.transform.GetChild(i + 2).GetChild(1).gameObject.GetComponent<Text>().text = ExpenseAmountList[i].ToString();
        }

        for(int i = 0; i < IncomeCategoryList.Length; i++)
        {
            IncomeAmountList.Add(CallEconomyValue.GetHistorySubByCategory(-1, IncomeCategoryList[i]));

            SummaryReportCarrier.transform.GetChild(i + 9).GetChild(1).gameObject.GetComponent<Text>().text = IncomeAmountList[i].ToString();
        }

        SummaryReportCarrier.transform.GetChild(13).GetChild(1).gameObject.GetComponent<Text>().text = CallEconomyValue.RealtimeIncome.ToString();
        if(CallEconomyValue.RealtimeIncome > 0) SummaryReportCarrier.transform.GetChild(13).GetChild(1).gameObject.GetComponent<Text>().color = new Color(0, 1f, 0, 1f);
        else if(CallEconomyValue.RealtimeIncome < 0) SummaryReportCarrier.transform.GetChild(13).GetChild(1).gameObject.GetComponent<Text>().color = new Color(1f, 0, 0, 1f);
        else SummaryReportCarrier.transform.GetChild(13).GetChild(1).gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
        SummaryReportCarrier.transform.GetChild(13).GetChild(2).gameObject.GetComponent<Text>().text = CallEconomyValue.Balance.ToString();

        List<string> PieGraphIndex = new List<string>();
        List<float> PieGraphData = new List<float>();
        foreach(var Category in IncomeCategoryList) PieGraphIndex.Add(Category);
        foreach(var Amount in IncomeAmountList) PieGraphData.Add(Amount);
        IncomePieGraphCarrier.GetComponent<GraphDrawer>().DrawPieGraph(PieGraphIndex, PieGraphData, null);
        
        PieGraphIndex.Clear();
        PieGraphData.Clear();
        foreach(var Category in ExpenseCategoryList) PieGraphIndex.Add(Category);
        foreach(var Amount in ExpenseAmountList) PieGraphData.Add(- Amount);
        ExpensePieGraphCarrier.GetComponent<GraphDrawer>().DrawPieGraph(PieGraphIndex, PieGraphData, null);
    }

    public void DetailLineGraphCategoryValueChange()
    {
        int Dropdownvalue = DetailLineGraphCategoryDropdown.GetComponent<Dropdown>().value;

        DetailLineGraphCarrier.GetComponent<GraphDrawer>().LineGraphClear();
        // StartCoroutine("DisplayDetailLineGraph", DetailLineGraphCategoryDropdown.GetComponent<Dropdown>().options[Dropdownvalue].text);
        DisplayDetailLineGraph(DetailLineGraphCategoryDropdown.GetComponent<Dropdown>().options[Dropdownvalue].text);
    }

    void DisplayDetailLineGraph(string Category)
    {
        float[] MinMaxValue = new float[2];
        MinMaxValue[0] = 0;
        MinMaxValue[1] = 0;
        float StandardValue;
        List<string> DateIndex = new List<string>();
        float YaxisIndexGap;
        List<float>[] DataList;
        Color[] GraphColor;
        
        int CeilMonth = Mathf.FloorToInt((CallTimeManager.TimeValue % CallTimeManager.Year) / CallTimeManager.Month);
        int FloorMonth = CeilMonth - 12;
        if(FloorMonth < 0) FloorMonth = 0;

        for(int i = FloorMonth; i < FloorMonth + 12; i++)
        {
            DateIndex.Add(CallTimeManager.ConvertMonthToString(i % 12 + 1));
        }

        if(Category == "Income and Expense")
        {
            DataList = new List<float>[3];
            for(int i = 0; i < 3; i++) DataList[i] = new List<float>();

            int ExpenseValue;
            int IncomeValue;
            int NetProfitValue;

            for(int i = FloorMonth; i < CeilMonth; i++)
            {
                ExpenseValue = CallEconomyValue.GetHistoryExpenseSub(i);
                if(MinMaxValue[0] > ExpenseValue) MinMaxValue[0] = ExpenseValue;
                IncomeValue = CallEconomyValue.GetHistoryIncomeSub(i);
                if(MinMaxValue[1] < IncomeValue) MinMaxValue[1] = IncomeValue;
                NetProfitValue = IncomeValue + ExpenseValue;
                if(MinMaxValue[0] > NetProfitValue) MinMaxValue[0] = NetProfitValue;
                if(MinMaxValue[1] < NetProfitValue) MinMaxValue[1] = NetProfitValue;

                DataList[0].Add(IncomeValue);
                DataList[1].Add(ExpenseValue);
                DataList[2].Add(NetProfitValue);
            }
            

            StandardValue = 0;
            YaxisIndexGap = float.NaN;

            GraphColor = new Color[3];
            GraphColor[0] = new Color(0,1f,0,1f);
            GraphColor[1] = new Color(1f,0,0,1f);
            GraphColor[2] = new Color(0,0,1f,1f);
        }
        else if(Category == "Balance")
        {
            GameObject CompanyObjectCarrier = GameObject.Find("CompanyManager");
            int CompanyCount = 0;
            for(int i = 0; i < CompanyObjectCarrier.transform.childCount; i++)
            {
                CompanyValue CallTargetCompanyValue = CompanyObjectCarrier.transform.GetChild(i).gameObject.GetComponent<CompanyValue>();
                if(!CallTargetCompanyValue.Unevaluable) CompanyCount++;
            }

            DataList = new List<float>[CompanyCount];
            int CurrentIndex = 0;
            

            for(int i = 0; i < CompanyObjectCarrier.transform.childCount; i++)
            {
                CompanyValue CallTargetCompanyValue = CompanyObjectCarrier.transform.GetChild(i).gameObject.GetComponent<CompanyValue>();
                EconomyValue CallTargetCompanyEconomyValue = CallTargetCompanyValue.GetEconomyValue().GetComponent<EconomyValue>();
                if(!CallTargetCompanyValue.Unevaluable)
                {
                    List<float> BalanceList = new List<float>();
                    int Balance;

                    for(int j = FloorMonth; j < CeilMonth; j++)
                    {
                        Balance = CallTargetCompanyEconomyValue.GetHistoryBalance(j);

                        if(MinMaxValue[0] > Balance) MinMaxValue[0] = Balance;
                        if(MinMaxValue[1] < Balance) MinMaxValue[1] = Balance;

                        BalanceList.Add(Balance);

                        if(CurrentIndex == 0 && j == FloorMonth)
                        {
                            MinMaxValue[0] = Balance;
                            MinMaxValue[1] = Balance;
                        }
                    }

                    DataList[CurrentIndex++] = BalanceList;
                }
            }

            StandardValue = float.NaN;
            YaxisIndexGap = float.NaN;
            GraphColor = null;
        }
        else if(Category == "Company Value")
        {
            GameObject CompanyObjectCarrier = GameObject.Find("CompanyManager");
            int CompanyCount = 0;
            for(int i = 0; i < CompanyObjectCarrier.transform.childCount; i++)
            {
                CompanyValue CallTargetCompanyValue = CompanyObjectCarrier.transform.GetChild(i).gameObject.GetComponent<CompanyValue>();
                if(!CallTargetCompanyValue.Unevaluable) CompanyCount++;
            }

            DataList = new List<float>[CompanyCount];
            int CurrentIndex = 0;
            for(int i = 0; i < CompanyObjectCarrier.transform.childCount; i++)
            {
                CompanyValue CallTargetCompanyValue = CompanyObjectCarrier.transform.GetChild(i).gameObject.GetComponent<CompanyValue>();
                if(!CallTargetCompanyValue.Unevaluable)
                {
                    List<float> CompanyValueList = new List<float>();
                    if(CallTargetCompanyValue.CompanyValueHistory.Count > 1)
                    {
                        for(int j = FloorMonth; j < CeilMonth; j++)
                        {
                            float Value = CallTargetCompanyValue.CompanyValueHistory[j];
                            CompanyValueList.Add(Value);

                            if(CurrentIndex == 0 && j == FloorMonth)
                            {
                                MinMaxValue[0] = Value;
                                MinMaxValue[1] = Value;
                            }
                        }
                    }
                    
                    DataList[CurrentIndex++] = CompanyValueList;
                }
            }

            StandardValue = float.NaN;
            YaxisIndexGap = float.NaN;
            GraphColor = null;
        }
        else if(Category == "Market Share")
        {
            SalesValue CallSalesValue = GameObject.Find("SalesManager").GetComponent<SalesValue>();
            int SalesCount = 0;
            foreach(var Sales in CallSalesValue.SalesItemArray)
            {
                if(Sales.RecipeInfo.Owner == CallCompanyManager.PlayerCompanyName) SalesCount++;
            }

            DataList = new List<float>[SalesCount];
            int CurrentIndex = 0;
            foreach(var Sales in CallSalesValue.SalesItemArray)
            {
                if(Sales.RecipeInfo.Owner == CallCompanyManager.PlayerCompanyName)
                {
                    List<float> MarketShareList = new List<float>();
                    if(Sales.SoldCountList.Count > 1)
                    {
                        for(int j = FloorMonth; j < CeilMonth; j++)
                        {
                            int MonthlyTotalSoldCount = 0;
                            float MarketSharePoint = 0f;
                            foreach(var Target in CallSalesValue.SalesItemArray)
                            {
                                if(Target.RecipeInfo.Recipe.GoodsObject.name == Sales.RecipeInfo.Recipe.GoodsObject.name)
                                {
                                    MonthlyTotalSoldCount += Target.SoldCountList[j];
                                }
                            }

                            if(MonthlyTotalSoldCount > 0)
                            {
                                MarketSharePoint = Mathf.FloorToInt(MonthlyTotalSoldCount / Sales.SoldCountList[j] * 1000f) * 0.1f;
                            }

                            MarketShareList.Add(MarketSharePoint);
                        }
                    }
                    DataList[CurrentIndex++] = MarketShareList;
                }
            }
            
            StandardValue = float.NaN;
            YaxisIndexGap = float.NaN;
            MinMaxValue[0] = 0f;
            MinMaxValue[1] = 100f;
            GraphColor = null;
        }
        else if(Category == "Sales")
        {
            SalesValue CallSalesValue = GameObject.Find("SalesManager").GetComponent<SalesValue>();
            int SalesCount = 0;
            foreach(var Sales in CallSalesValue.SalesItemArray)
            {
                if(Sales.RecipeInfo.Owner == CallCompanyManager.PlayerCompanyName) SalesCount++;
            }

            DataList = new List<float>[SalesCount];
            int CurrentIndex = 0;
            foreach(var Sales in CallSalesValue.SalesItemArray)
            {
                if(Sales.RecipeInfo.Owner == CallCompanyManager.PlayerCompanyName)
                {
                    List<float> SalesCountList = new List<float>();
                    if(CurrentIndex == 0)
                    {
                        MinMaxValue[0] = Sales.SoldCount;
                        MinMaxValue[1] = Sales.SoldCount;
                    }
                    if(Sales.SoldCountList.Count > 1)
                    {
                        for(int j = FloorMonth; j < CeilMonth; j++)
                        {
                            float Value = Sales.SoldCountList[j];
                            SalesCountList.Add(Value);

                            if(CurrentIndex == 0 && j == FloorMonth)
                            {
                                MinMaxValue[0] = Value;
                                MinMaxValue[1] = Value;
                            }
                        }
                    }
                    
                    DataList[CurrentIndex++] = SalesCountList;
                }
            }
            
            StandardValue = float.NaN;
            YaxisIndexGap = float.NaN;
            GraphColor = null;
        }
        else
        {
            DataList = new List<float>[1];
            DataList[0] = new List<float>();

            StandardValue = float.NaN;
            YaxisIndexGap = float.NaN;
            GraphColor = null;
        }

        DetailLineGraphCarrier.GetComponent<GraphDrawer>().DrawLineGraph(MinMaxValue, StandardValue, DateIndex, YaxisIndexGap, DataList, GraphColor);
    }

    void DisplayDetailReportCategory()
    {
        DetailReportPanelCategoryDropdown.GetComponent<Dropdown>().ClearOptions();

        List<string> MonthList = new List<string>();

        for(int i = 1; i < 5; i++)
        {
            int TargetTimeValue = CallTimeManager.TimeValue - (CallTimeManager.Month * i);
            if(TargetTimeValue >= 0) MonthList.Add(CallTimeManager.ConvertMonthToString(CallTimeManager.GetMonthInt(TargetTimeValue)));
        }

        DetailReportPanelCategoryDropdown.GetComponent<Dropdown>().AddOptions(MonthList);

        if(DetailReportPanelCategoryDropdown.GetComponent<Dropdown>().options.Count == 0) DetailReportPanelCategoryDropdown.GetComponent<Dropdown>().interactable = false;
        else DetailReportPanelCategoryDropdown.GetComponent<Dropdown>().interactable = true;
    }

    public void DetailReportCategoryValueChange()
    {
        int DropdownValue = DetailReportPanelCategoryDropdown.GetComponent<Dropdown>().value;

        DisplayDetailPastReport(DropdownValue);
    }

    void DisplayDetailPastReport(int PastMonth)
    {
        GameObject TargetDetailTextObject = null;
        GameObject TargetAmountTextObject = null;
        string TargetCategory = "";
        int TargetIndex = Mathf.FloorToInt(CallTimeManager.TimeValue / CallTimeManager.Month) - (PastMonth + 1);

        if(TargetIndex < 0)
        {
            ClearDetailReportPanel(false);
            return;
        }

        for(int i = 2; i < DetailReportCarrier.transform.childCount - 1; i++)
        {
            TargetDetailTextObject = DetailReportCarrier.transform.GetChild(i).GetChild(4).GetChild(0).gameObject;
            TargetAmountTextObject = DetailReportCarrier.transform.GetChild(i).GetChild(5).GetChild(0).gameObject;
            TargetCategory = DetailReportCarrier.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text.Substring(1);
        
            DisplayReport(TargetIndex, TargetCategory, TargetDetailTextObject, TargetAmountTextObject);
        }

        Updated = true;
    }

    public void DisplayDetailCurrentReport()
    {
        GameObject TargetDetailTextObject = null;
        GameObject TargetAmountTextObject = null;
        string TargetCategory = "";
        int TargetIndex = -1;

        for(int i = 2; i < DetailReportCarrier.transform.childCount - 1; i++)
        {
            TargetDetailTextObject = DetailReportCarrier.transform.GetChild(i).GetChild(1).GetChild(0).gameObject;
            TargetAmountTextObject = DetailReportCarrier.transform.GetChild(i).GetChild(2).GetChild(0).gameObject;
            TargetCategory = DetailReportCarrier.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text.Substring(1);
            
            DisplayReport(TargetIndex, TargetCategory, TargetDetailTextObject, TargetAmountTextObject);
        }

        Updated = true;
    }

    void DisplayReport(int MonthIndex, string Category, GameObject DetailTextObject, GameObject AmountTextObject)
    {
        List<EconomyValue.History> HistoryList = new List<EconomyValue.History>();
        HistoryList.AddRange(CallEconomyValue.GetHistoryByCategory(MonthIndex, Category));
        List<string> DetailList = new List<string>();
        // Debug.Log(Category + " " + HistoryList.Count);
        foreach(var History in HistoryList)
        {
            bool isDuplicated = false;
            foreach(var Detail in DetailList)
            {
                if(Detail == History.Detail)
                {
                    isDuplicated = true;
                    break;
                }
            }

            if(!isDuplicated) DetailList.Add(History.Detail);
        }

        Transform ParentTransform = DetailTextObject.transform.parent;

        if(DetailTextObject.transform.parent.childCount < DetailList.Count)
        {
            for(int i = ParentTransform.childCount; i < DetailList.Count; i++)
            {
                GameObject.Instantiate(DetailTextObject, DetailTextObject.transform.parent);
                GameObject.Instantiate(AmountTextObject, AmountTextObject.transform.parent);
            }

            for(int i = 0; i < ParentTransform.childCount; i++)
            {
                DetailTextObject.transform.parent.GetChild(i).gameObject.SetActive(true);
                AmountTextObject.transform.parent.GetChild(i).gameObject.SetActive(true);
            }
        }
        else if(ParentTransform.childCount > DetailList.Count)
        {
            for(int i = ParentTransform.childCount - 1; i >= DetailList.Count; i--)
            {
                if(i == 0) break;
                DetailTextObject.transform.parent.GetChild(i).gameObject.SetActive(false);
                AmountTextObject.transform.parent.GetChild(i).gameObject.SetActive(false);
            }
        }

        if(DetailList.Count == 0)
        {
            DetailTextObject.GetComponent<Text>().text = "";
            AmountTextObject.GetComponent<Text>().text = "0";
        }
        else
        {
            for(int i = 0; i < ParentTransform.childCount; i++)
            {
                if(!ParentTransform.GetChild(i).gameObject.activeSelf) break;

                GameObject TargetDetailText = DetailTextObject.transform.parent.GetChild(i).gameObject;
                GameObject TargetAmountlText = AmountTextObject.transform.parent.GetChild(i).gameObject;

                TargetDetailText.GetComponent<Text>().text = DetailList[i];
                int Amount = 0;
                foreach(var History in HistoryList)
                {
                    if(History.Detail == DetailList[i]) Amount += History.Amount;
                }
                TargetAmountlText.GetComponent<Text>().text = Amount.ToString();
            }
        }

        for(int i = 1; i < DetailTextObject.transform.parent.childCount; i++)
        {
            if(!ParentTransform.GetChild(i).gameObject.activeSelf)
            {
                Destroy(DetailTextObject.transform.parent.GetChild(i).gameObject);
                Destroy(AmountTextObject.transform.parent.GetChild(i).gameObject);
            }
        }

        CallPanelController.ContentSizeFitterReseter(DetailTextObject.transform.parent.parent.gameObject);
    }

    void ClearDetailReportPanel(bool Current)
    {
        int PanelValue = 0;

        if(Current) PanelValue = 1;
        else PanelValue = 4;

        for(int i = 2; i < DetailReportCarrier.transform.childCount - 1; i++)
        {
            GameObject TargetDetailTextParentObject = DetailReportCarrier.transform.GetChild(i).GetChild(PanelValue).gameObject;
            GameObject TargetAmountTextParentObject = DetailReportCarrier.transform.GetChild(i).GetChild(PanelValue + 1).gameObject;

            for(int j = TargetDetailTextParentObject.transform.childCount - 1; j >= 1; j--)
            {
                Destroy(TargetDetailTextParentObject.transform.GetChild(j).gameObject);
                Destroy(TargetAmountTextParentObject.transform.GetChild(j).gameObject);
            }

            TargetDetailTextParentObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            TargetAmountTextParentObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        }

        Updated = true;
    }

    void ResizingReportPanel()
    {
        Canvas.ForceUpdateCanvases();

        float DefaultReportCarrierSize = (CallPanelController.CurrentUIsize * 5f) - CallPanelController.CurrentEdgePadding;
        float DefaultReportPanelSize = CallPanelController.CurrentUIsize * 6f;
        float ReportPanelDifferenceSize = 0;

        if(DetailReportCarrier.GetComponent<RectTransform>().rect.height > DefaultReportCarrierSize)
        {
            ReportPanelDifferenceSize = DetailReportCarrier.GetComponent<RectTransform>().rect.height - DefaultReportCarrierSize;
        }

        DetailReportInnerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 15.6f, DefaultReportCarrierSize + CallPanelController.CurrentEdgePadding + ReportPanelDifferenceSize);
        
        CallPanelController.ContentSizeFitterReseter(DetailReportPanel);
        CallPanelController.ContentSizeFitterReseter(DetailReportPanel.transform.parent.gameObject);
        CallPanelController.ContentSizeFitterReseter(DetailReportCarrier);
    }

    void ClearPanel()
    {
        IncomePieGraphCarrier.GetComponent<GraphDrawer>().PieGraphClear();
        ExpensePieGraphCarrier.GetComponent<GraphDrawer>().PieGraphClear();
        DetailLineGraphCarrier.GetComponent<GraphDrawer>().LineGraphClear();

        ClearDetailReportPanel(true);
        ClearDetailReportPanel(false);
    }

    public void ClosePanel()
    {
        ClearPanel();
    }
}
