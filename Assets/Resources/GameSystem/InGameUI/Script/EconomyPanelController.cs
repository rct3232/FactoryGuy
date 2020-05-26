using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    public GameObject SummaryPanel;
    public GameObject DetailScrollPanel;
    public GameObject DetailReportPanel;
    public GameObject LoanPanel;
    GameObject IncomePieGraphPanel, IncomePieGraphCarrier, ExpensePieGraphPanel, ExpensePieGraphCarrier, SummaryReportPanel, SummaryReportCarrier
    , DetailReportCategoryPanel, DetailReportInnerPanel, DetailReportPanelCategoryDropdown, DetailReportCarrier, BankListPanel, LoanFunctionPanel;
    CompanyValue CallCompanyValue;
    EconomyValue CallEconomyValue;
    BankValue CallBankValue;
    TimeManager CallTimeManager;
    NotificationManager CallNotificationManager;
    string CurrentBank = "";
    bool Updated = false;

    void Awake()
    {
        CallCompanyValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
        CallEconomyValue = CallCompanyValue.GetEconomyValue().GetComponent<EconomyValue>();
        CallBankValue = GameObject.Find("BaseSystem").GetComponent<BankValue>();
        CallTimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CallNotificationManager = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();

        IncomePieGraphPanel = SummaryPanel.transform.GetChild(0).gameObject;
        IncomePieGraphCarrier = IncomePieGraphPanel.transform.GetChild(1).gameObject;
        ExpensePieGraphPanel = SummaryPanel.transform.GetChild(1).gameObject;
        ExpensePieGraphCarrier = ExpensePieGraphPanel.transform.GetChild(1).gameObject;
        SummaryReportPanel = SummaryPanel.transform.GetChild(2).gameObject;
        SummaryReportCarrier = SummaryReportPanel.transform.GetChild(0).gameObject;
        DetailReportCategoryPanel = DetailReportPanel.transform.GetChild(0).gameObject;
        DetailReportInnerPanel = DetailReportPanel.transform.GetChild(1).gameObject;
        DetailReportPanelCategoryDropdown = DetailReportCategoryPanel.transform.GetChild(1).gameObject;
        DetailReportCarrier = DetailReportInnerPanel.transform.GetChild(0).gameObject;
        BankListPanel = LoanPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        LoanFunctionPanel = LoanPanel.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;
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

        DetailReportCategoryPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
        DetailReportCategoryPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, DetailPanelCategoryObjectSize);
        DetailReportCategoryPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentUIsize,  - (CallPanelController.CurrentEdgePadding * 0.5f));
        DetailReportCategoryPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 4f, DetailPanelCategoryObjectSize);
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
        DiplaySummary();
        DisplayDetailCurrentReport();
        DisplayDetailReportCategory();
        DisplayBankList();

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

    public void DiplaySummary()
    {
        ClearSummaryPanel();

        List<EconomyValue.History> HistoryList = new List<EconomyValue.History>();

        SummaryReportCarrier.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "TEST";
        string[] ExpenseCategoryList = new string[] {"Buy", "Install", "Upkeep", "Research", "Employee Pay", "Real Estate"};
        string[] IncomeCategoryList = new string[] {"Sell", "Milestone", "Loan"};

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

    void ClearSummaryPanel()
    {
        IncomePieGraphCarrier.GetComponent<GraphDrawer>().PieGraphClear();
        ExpensePieGraphCarrier.GetComponent<GraphDrawer>().PieGraphClear();

        for(int i = 0; i < SummaryReportCarrier.transform.childCount; i++)
        {
            if(i == 1 || i == 8 || i == 12) continue;

            SummaryReportCarrier.transform.GetChild(i).GetChild(1).gameObject.GetComponent<Text>().text = "0";
        }
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

    void DisplayBankList()
    {
        List<string> BankList = CallBankValue.GetBankList();

        for(int i = BankList.Count - 1; i >= 0; i--)
        {
            if(CallBankValue.EvaluateCostomer(BankList[i], CallCompanyValue) == null) BankList.Remove(BankList[i]);
        }
        
        int Limit = BankList.Count;

        if(Limit > BankListPanel.transform.childCount)
        {
            for(int i = BankListPanel.transform.childCount; i < BankList.Count; i++)
            {
                GameObject.Instantiate(BankListPanel.transform.GetChild(0).gameObject, BankListPanel.transform).name = "BankButton";
            }
        }
        else if(Limit < BankListPanel.transform.childCount)
        {
            if(Limit == 0) Limit = 1;
            for(int i = BankListPanel.transform.childCount - 1; i >= Limit; i--)
            {
                Destroy(BankListPanel.transform.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < Limit; i++)
        {
            GameObject NewBankButton = BankListPanel.transform.GetChild(i).gameObject;
            NewBankButton.SetActive(true);

            if(BankList.Count > 0)
            {
                float[] Evaluated = CallBankValue.EvaluateCostomer(BankList[i], CallCompanyValue);
                if (Evaluated == null) continue;
                if(CallBankValue.GetBankLevel(BankList[i]) == 1) NewBankButton.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/BankLevel1Icon");
                else if(CallBankValue.GetBankLevel(BankList[i]) == 2) NewBankButton.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/BankLevel2Icon");
                else NewBankButton.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/BankLevel3Icon");

                NewBankButton.transform.GetChild(2).gameObject.GetComponent<Text>().text = BankList[i];
                string MaxLoanValueString = "";
                if(Evaluated[1] > 1000000f) MaxLoanValueString = Mathf.FloorToInt(Evaluated[1] * 0.000001f).ToString() + "M";
                else if (Evaluated[1] > 1000f) MaxLoanValueString = Mathf.FloorToInt(Evaluated[1] * 0.001f).ToString() + "K";
                else MaxLoanValueString = Evaluated[1].ToString();
                NewBankButton.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Max: $" + MaxLoanValueString + " / " + (Mathf.Ceil(Evaluated[0] * 100f) * 0.01f).ToString() + "%";

                if(Evaluated[1] > 0) NewBankButton.GetComponent<Button>().interactable = true;
                else NewBankButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                NewBankButton.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/ImojiBad");
                NewBankButton.transform.GetChild(2).gameObject.GetComponent<Text>().text = "There is no bank";
                NewBankButton.transform.GetChild(3).gameObject.GetComponent<Text>().text = "How about upgrade yourself";
                NewBankButton.GetComponent<Button>().interactable = false;
            }
        }

        if(BankListPanel.transform.childCount == 0) ClearBankList();
    }

    public void UpdateBankList()
    {
        DisplayBankList();

        if(CurrentBank != "")
        {
            List<string> CurrentBankList = new List<string>();

            for(int i = 0; i < BankListPanel.transform.childCount; i++)
            {
                CurrentBankList.Add(BankListPanel.transform.GetChild(i).GetChild(2).gameObject.GetComponent<Text>().text);
            }

            if(!CurrentBankList.Contains(CurrentBank))
            {
                ClearDealFunction();
            }
        }
    }

    public void SelectBank(GameObject TargetObject)
    {
        string TargetBankName = TargetObject.transform.GetChild(2).gameObject.GetComponent<Text>().text;
        if(TargetBankName != CurrentBank)
        {
            CurrentBank = TargetBankName;
            TargetObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            ActivateDealFunction();
        }
    }

    public void BalanceValueCheck()
    {
        if(CurrentBank != "")
        {
            int InputValue = int.Parse(LoanFunctionPanel.transform.GetChild(1).gameObject.GetComponent<InputField>().text);
            if(InputValue < 0)
            {
                LoanFunctionPanel.transform.GetChild(1).gameObject.GetComponent<InputField>().text = "0";
            }
            else
            {
                int MaxLoanValue = Mathf.FloorToInt(CallBankValue.EvaluateCostomer(CurrentBank, CallCompanyValue)[1]);
                if(MaxLoanValue < InputValue)
                {
                    CallNotificationManager.AddAlert("You cannot loan over $" + MaxLoanValue, 1, "");
                }
            }
        }
        else ClearDealFunction();
    }

    public void SelectDealFunction()
    {
        if(CurrentBank != "")
        {
            int Balance = int.Parse(LoanFunctionPanel.transform.GetChild(1).gameObject.GetComponent<InputField>().text);
            if(Balance <= 0)
            {
                CallNotificationManager.AddAlert("You cannot loan under $0", 1, "");
                return;
            }

            CallBankValue.StartNewDeal(CallCompanyValue, CurrentBank, Balance);
            DisplayBankList();
            ClearDealFunction();
        }
        else ClearDealFunction();
    }

    void ActivateDealFunction()
    {
        LoanFunctionPanel.transform.GetChild(1).gameObject.GetComponent<InputField>().interactable = true;
        LoanFunctionPanel.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
    }

    void ClearDealFunction()
    {
        CurrentBank = "";

        for(int i = 0; i < BankListPanel.transform.childCount; i++)
        {
            BankListPanel.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
        }

        LoanFunctionPanel.transform.GetChild(1).gameObject.GetComponent<InputField>().interactable = false;
        LoanFunctionPanel.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
    }

    void ClearBankList()
    {
        for(int i = 0; i < BankListPanel.transform.childCount; i++)
        {
            BankListPanel.transform.GetChild(i).gameObject.SetActive(false);

            BankListPanel.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.SetActive(false);
            BankListPanel.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = null;
            BankListPanel.transform.GetChild(i).GetChild(2).gameObject.GetComponent<Text>().text = "";
            BankListPanel.transform.GetChild(i).GetChild(3).gameObject.GetComponent<Text>().text = "";
            BankListPanel.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
        }
    }

    void ClearPanel()
    {
        ClearSummaryPanel();
        ClearDetailReportPanel(true);
        ClearDetailReportPanel(false);
        ClearBankList();
        ClearDealFunction();
    }

    public void ClosePanel()
    {
        ClearPanel();
    }
}
