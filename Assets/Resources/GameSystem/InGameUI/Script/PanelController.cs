using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject BottomPanel;
    public GameObject MenuButton;
    public GameObject RightPanel;
    public GameObject LeftPanel;
    public GameObject LandManageButton;
    public GameObject BolldozeImageObject;
    public GameObject SidePanelCarrier;
    public GameObject FloatingPanelCarrier;
    public GameObject NewsPanel;
    public GameObject AlertPopUpPanel;
    public GameObject NoteToolTipPanel;
    public GameObject CurrentSidePanel = null;
    public GameObject CurrentFloatingPanel = null;
    List<GameObject> SidePanelList = new List<GameObject>();
    List<GameObject> FloatingPanelList = new List<GameObject>();
    public float UIscale;
    public float CurrentUIsize;
    public float CurrentEdgePadding;
    public List<int> FontSizeList;
    string PlayerCompanyName;
    ClickChecker ClickCheckerCall;
    InGameValue ValueCall;
    TimeManager TimeManagerCall;
    CompanyValue CompanyValueCall;
    NewsPanelController NewsPanelControllerCall;

    void Awake()
    {
        FontSizeList = new List<int>();
        
        FontSizeList.Add(13);
        FontSizeList.Add(14);
        FontSizeList.Add(19);
        FontSizeList.Add(9);
    }
    void Start()
    {
        ClickCheckerCall = GameObject.Find("BaseSystem").GetComponent<ClickChecker>();
        ValueCall = GameObject.Find("BaseSystem").GetComponent<InGameValue>();
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CompanyValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
        NewsPanelControllerCall = NewsPanel.GetComponent<NewsPanelController>();

        PlayerCompanyName = CompanyValueCall.CompanyName;

        SidePanelManager();
        FloatingPanelManager();

        NewsPanelControllerCall.CallPanelController = this;

        AlertPopUpPanel.GetComponent<AlertPopUpPanelController>().CallPanelController = this;

        NoteToolTipPanel.GetComponent<NoteToolTipPanelController>().CallPanelController = this;
        NoteToolTipPanel.GetComponent<NoteToolTipPanelController>().CallNotificationManager = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();

        SetUIScale();
        Scaling();

        NewsPanelControllerCall.UpdateTimeText();
    }

    public void SetUIScale()
    {
        UIscale = TopValue.TopValueSingleton.UIScale;
    }

    void Scaling()
    {
        CurrentUIsize = 50 * UIscale;
        CurrentEdgePadding = CurrentUIsize / 5 * 2;

        BottomPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(BottomPanel.GetComponent<RectTransform>().sizeDelta.x, CurrentUIsize);

        MenuButton.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize, 0);
        RightPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize);
        LeftPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize);

        MenuButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        RightPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(- CurrentUIsize, 0, 0);
        LeftPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CurrentEdgePadding, 0, 0);

        FontScaling(MenuButton);
        FontScaling(RightPanel);
        FontScaling(LeftPanel);

        for(int i = 0; i < MenuButton.transform.childCount; i++)
        {
            MenuButton.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize, CurrentUIsize);
        }
        for(int i = 0; i < RightPanel.transform.childCount; i++)
        {
            RightPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize, CurrentUIsize);
        }

        LeftPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 3f, 0);
        LeftPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize * 0.5f);
        LeftPanel.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize * 0.5f);

        LeftPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 6.2f, 0);
        LeftPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize * 0.5f);
        LeftPanel.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 3f, 0);
        LeftPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 0.5f, 0);
        LeftPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 2f, 0);
        LeftPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 0.5f, 0);
        LeftPanel.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentEdgePadding * 0.5f, 0);
        LeftPanel.transform.GetChild(1).GetChild(0).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 3f, 0);
        LeftPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 0.5f, CurrentUIsize * 0.5f);
        LeftPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize * 0.5f);
        LeftPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize * 0.5f);
        LeftPanel.transform.GetChild(1).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 3f, 0);
        LeftPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 0.5f, CurrentUIsize * 0.5f);
        LeftPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize * 0.5f);
        LeftPanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentEdgePadding * 0.5f, 0);
        LeftPanel.transform.GetChild(1).GetChild(1).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 3f, 0);
        LeftPanel.transform.GetChild(1).GetChild(1).GetChild(2).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 0.5f, CurrentUIsize * 0.5f);
        LeftPanel.transform.GetChild(1).GetChild(1).GetChild(2).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize * 0.5f);

        LandManageButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(CurrentEdgePadding, - CurrentEdgePadding);
        LandManageButton.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize, CurrentUIsize);

        BolldozeImageObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, CurrentEdgePadding);

        RectTransform NewsPanelRect = NewsPanel.GetComponent<RectTransform>();
        RectTransform PaddingPanelRect = NewsPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        GameObject NewsScrollPanel = NewsPanel.transform.GetChild(1).gameObject;
        RectTransform NewsScrollPanelRect = NewsScrollPanel.GetComponent<RectTransform>();

        NewsPanelRect.sizeDelta = new Vector2(CurrentUIsize * 10, 0);
        PaddingPanelRect.sizeDelta = new Vector2(0, CurrentEdgePadding);
        NewsScrollPanelRect.sizeDelta = new Vector2(CurrentUIsize * 10, CurrentUIsize);

        NewsScrollPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CurrentUIsize);
        NewsScrollPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentEdgePadding, 0);
        
        NewsPanelControllerCall.Scaling();
        AlertPopUpPanel.GetComponent<AlertPopUpPanelController>().Scaling();
        NoteToolTipPanel.GetComponent<NoteToolTipPanelController>().Scaling();
    }

    public void FontScaling(GameObject TargetObject)
    {
        for(int i = 0; i < TargetObject.transform.childCount; i++)
        {
            if(TargetObject.transform.GetChild(i).gameObject.GetComponent<TextInfomation>() != null)
            {
                TargetObject.transform.GetChild(i).gameObject.GetComponent<Text>().fontSize = GetFontSize(TargetObject.transform.GetChild(i).gameObject.GetComponent<TextInfomation>().TextSizeIndex);
            }

            FontScaling(TargetObject.transform.GetChild(i).gameObject);
        }
    }

    public int GetFontSize(int FontSizeIndex)
    {
        return Mathf.CeilToInt(FontSizeList[FontSizeIndex] * UIscale);
    }

    void SidePanelManager()
    {
        for(int i = 0; i < SidePanelCarrier.transform.childCount; i++)
        {
            SidePanelList.Add(SidePanelCarrier.transform.GetChild(i).gameObject);
            
            List<GameObject> ChildList = new List<GameObject>();
            GetAllChildren(SidePanelList[i].transform, ChildList);
            foreach(var Object in ChildList) Object.tag = "SidePanel";
        }
    }

    public void DisplaySidePanel(string Type)
    {        
        ValueCall.ModeSelector(0);

        if(CurrentSidePanel != null)
        {
            if(CurrentSidePanel.name == Type)
            {
                CloseCurrentSidePanel();
                return;
            }
            else
            {
                CloseCurrentSidePanel();
            }
        }
        
        if(NewsPanelControllerCall.isExpanded) NewsPanelControllerCall.NewsExpandSelect();
        
        switch(Type)
        {
            case "ConstructPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 4, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[0];
                CurrentSidePanel.GetComponent<ConstructPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<ConstructPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<ConstructPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<ConstructPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<ConstructPanelController>().Initializing();
                break;
            case "ContractPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 12, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[1];
                CurrentSidePanel.GetComponent<ContractPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<ContractPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<ContractPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<ContractPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<ContractPanelController>().Initializing();
                break;
            case "ProcessingPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 12, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[2];
                CurrentSidePanel.GetComponent<ProcessingPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<ProcessingPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<ProcessingPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<ProcessingPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<ProcessingPanelController>().Initializing();
                break;
            case "ProductPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 12, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[3];
                CurrentSidePanel.GetComponent<ProductPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<ProductPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<ProductPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<ProductPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<ProductPanelController>().Initializing();
                break;
            case "WorkerPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 8, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[4];
                CurrentSidePanel.GetComponent<WorkerPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<WorkerPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<WorkerPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<WorkerPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<WorkerPanelController>().Initializing();
                break;
            case "ManagePanel" :
                break;
            case "EconomyPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 16, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[6];
                CurrentSidePanel.GetComponent<EconomyPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<EconomyPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<EconomyPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<EconomyPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<EconomyPanelController>().Initializing();
                break;
            case "GoodsCreatorPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 12, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[7];
                CurrentSidePanel.GetComponent<GoodsCreatorPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<GoodsCreatorPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<GoodsCreatorPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<GoodsCreatorPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<GoodsCreatorPanelController>().Initializing();
                break;
            case "ProcessorPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 12, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[8];
                CurrentSidePanel.GetComponent<ProcessorPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<ProcessorPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<ProcessorPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<ProcessorPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<ProcessorPanelController>().Initializing();
                break;
            case "LabatoryResearchPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 12, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[9];
                CurrentSidePanel.GetComponent<LabatoryResearchPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<LabatoryResearchPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<LabatoryResearchPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<LabatoryResearchPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<LabatoryResearchPanelController>().Initializing();
                break;
            case "LabatoryDevelopPanel" :
                SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 12, Screen.height - CurrentUIsize);
                CurrentSidePanel = SidePanelList[10];
                CurrentSidePanel.GetComponent<LabatoryDevelopPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentSidePanel.SetActive(true);
                if(!CurrentSidePanel.GetComponent<LabatoryDevelopPanelController>().isInitialized) 
                {
                    CurrentSidePanel.GetComponent<LabatoryDevelopPanelController>().Scaling();
                    CurrentSidePanel.GetComponent<LabatoryDevelopPanelController>().isInitialized = true;
                }
                CurrentSidePanel.GetComponent<LabatoryDevelopPanelController>().Initializing();
                break;
        }
    }

    public void CloseCurrentSidePanel()
    {
        if(CurrentSidePanel == null) return;
    
        switch(CurrentSidePanel.name)
        {
            case "ConstructPanel" :
                CurrentSidePanel.GetComponent<ConstructPanelController>().ClosePanel();
                break;
            case "ContractPanel" :
                CurrentSidePanel.GetComponent<ContractPanelController>().ClosePanel();
                break;
            case "ProcessingPanel" :
                CurrentSidePanel.GetComponent<ProcessingPanelController>().ClosePanel();
                break;
            case "ProductPanel" :
                CurrentSidePanel.GetComponent<ProductPanelController>().ClosePanel();
                break;
            case "WorkerPanel" :
                CurrentSidePanel.GetComponent<WorkerPanelController>().ClosePanel();
                break;
            case "ManagePanel" :
                break;
            case "EconomyPanel" :
                CurrentSidePanel.GetComponent<EconomyPanelController>().ClosePanel();
                break;
            case "GoodsCreatorPanel" :
                CurrentSidePanel.GetComponent<GoodsCreatorPanelController>().ClosePanel();
                break;
            case "ProcessorPanel" :
                CurrentSidePanel.GetComponent<ProcessorPanelController>().ClosePanel();
                break;
            case "LabatoryResearchPanel" :
                CurrentSidePanel.GetComponent<LabatoryResearchPanelController>().ClosePanel();
                break;
            case "LabatoryDevelopPanel" :
                CurrentSidePanel.GetComponent<LabatoryDevelopPanelController>().ClosePanel();
                break;
        }

        CurrentSidePanel.SetActive(false);
        CurrentSidePanel = null;

        SidePanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Screen.height - CurrentUIsize);
    }

    void FloatingPanelManager()
    {
        for(int i = 0; i < FloatingPanelCarrier.transform.childCount; i++)
        {
            FloatingPanelList.Add(FloatingPanelCarrier.transform.GetChild(i).gameObject);
            
            List<GameObject> ChildList = new List<GameObject>();
            GetAllChildren(FloatingPanelList[i].transform, ChildList);
            foreach(var Object in ChildList) Object.tag = "FloatingPanel";
        }
    }

    public void DisplayFloatingPanel(string Type, GameObject TargetObject)
    {
        if(!ValueCall.ModeBit[0]) return;

        if(CurrentFloatingPanel != null)  CloseCurrentFloatingPanel();

        switch(Type)
        {
            case "ObjectInfoPanel" :
                FloatingPanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CurrentUIsize * 8f, CurrentUIsize * 5f);
                CurrentFloatingPanel = FloatingPanelList[0];
                CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().CallPanelController = gameObject.GetComponent<PanelController>();
                CurrentFloatingPanel.SetActive(true);
                if(!CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().isInitialized) 
                {
                    CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().Scaling();
                    CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().isInitialized = true;
                }
                CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().Initializing(TargetObject);
                break;
        }
    }

    public void CloseCurrentFloatingPanel()
    {
        if(CurrentFloatingPanel == null) return;
    
        switch(CurrentFloatingPanel.name)
        {
            case "ObjectInfoPanel" :
                if(CurrentSidePanel != null) if(CurrentSidePanel.name == "GoodsCreatorPanel" || CurrentSidePanel.name == "ProcessorPanel" ||
                                                CurrentSidePanel.name == "LabatoryResearchPanel" || CurrentSidePanel.name == "LabatoryDevelopPanel") CloseCurrentSidePanel();
                CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().ClosePanel();
                break;
        }

        CurrentFloatingPanel.SetActive(false);
        CurrentFloatingPanel = null;

        FloatingPanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
    }

    public void LandManageButtonSelect()
    {
        ValueCall.ModeSelector(2);
        CloseCurrentSidePanel();
    }

    public void UpdateFinanceInfo(int Income, int Balance)
    {
        Text RealTimeIncomeText = LeftPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        Text BalanceTextText = LeftPanel.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();

        RealTimeIncomeText.text = Income.ToString();
        BalanceTextText.text = "$ " + Balance.ToString();

        if(Income > 0) RealTimeIncomeText.color = new Color(0, 1f, 0, 1f);
        else if(Income < 0) RealTimeIncomeText.color = new Color(1f, 0, 0, 1f);
        else RealTimeIncomeText.color = new Color(1f, 1f, 1f, 1f);

        if(CurrentSidePanel != null)
        {
            if(CurrentSidePanel.name == "EconomyPanel")
            {
                CurrentSidePanel.GetComponent<EconomyPanelController>().DisplayDetailCurrentReport();
                CurrentSidePanel.GetComponent<EconomyPanelController>().DiplaySummary();
            }
        }
    }

    public void UpdateFactoryInfo(string Category, float Value, float StandardValue)
    {
        Text CompanyValueText = LeftPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
        Image CompanyValueStateImage = LeftPanel.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<Image>();
        Text ElectricityCurrentValueText = LeftPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        Text ElectricityStandardValueText = LeftPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(1).GetChild(1).gameObject.GetComponent<Text>();
        Text WarehouseCurrentValueText = LeftPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        Text WarehouseStandardValueText = LeftPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<Text>();
        Text EmployeeCurrentValueText = LeftPanel.transform.GetChild(1).GetChild(1).GetChild(2).GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
        Text EmployeeStandardValueText = LeftPanel.transform.GetChild(1).GetChild(1).GetChild(2).GetChild(1).GetChild(1).gameObject.GetComponent<Text>();
        HorizontalLayoutGroup WarehouseValueCarrierLayoutGroup = LeftPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<HorizontalLayoutGroup>();
        HorizontalLayoutGroup EmployeeValueCarrierLayoutGroup = LeftPanel.transform.GetChild(1).GetChild(1).GetChild(2).GetChild(1).gameObject.GetComponent<HorizontalLayoutGroup>();

        if(Category == "CompanyValue")
        {
            int PreviousValue = Mathf.CeilToInt(StandardValue);
            int CurrentValue = Mathf.CeilToInt(Value);
            if(PreviousValue == CurrentValue)
            {
                CompanyValueText.text = CurrentValue.ToString();
                CompanyValueStateImage.sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/FixedValue");
            }
            else
            {
                CompanyValueText.text = CurrentValue.ToString();
                if(PreviousValue < CurrentValue)
                {
                    CompanyValueStateImage.sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/UpArrow");
                }
                else
                {
                    CompanyValueStateImage.sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/DownArrow");
                }
            }
        }
        else if(Category == "Electricity")
        {
            ElectricityCurrentValueText.text = (Mathf.CeilToInt(Value * 10) * 0.1f).ToString() + " ";
            ElectricityStandardValueText.text = "/ " + (Mathf.CeilToInt(StandardValue * 10) * 0.1f).ToString();

            if(Value < StandardValue) ElectricityCurrentValueText.color = new Color(0, 1f, 0, 1f);
            else if(Value > StandardValue) ElectricityCurrentValueText.color = new Color(1f, 0, 0, 1f);
            else ElectricityCurrentValueText.color = new Color(1f, 1f, 1f, 1f);
        }
        else if(Category == "Warehouse")
        {
            WarehouseCurrentValueText.text = Value.ToString() + " ";
            WarehouseStandardValueText.text = "/ " + StandardValue.ToString();

            if(Value > StandardValue) WarehouseCurrentValueText.color = new Color(1f, 0, 0, 1f);
            else if(Value < StandardValue) WarehouseCurrentValueText.color = new Color(0, 1f, 0, 1f);
            else WarehouseCurrentValueText.color = new Color(1f, 1f, 1f, 1f);
        }
        else if(Category == "Employee")
        {
            EmployeeCurrentValueText.text = Value.ToString() + " ";
            EmployeeStandardValueText.text = "/ " + StandardValue.ToString();

            if(Value > StandardValue) EmployeeCurrentValueText.color = new Color(0, 1f, 0, 1f);
            else if(Value < StandardValue) EmployeeCurrentValueText.color = new Color(1f, 0, 0, 1f);
            else EmployeeCurrentValueText.color = new Color(1f, 1f, 1f, 1f);
        }

        Canvas.ForceUpdateCanvases();

        ContentSizeFitterReseter(LeftPanel.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(1).gameObject);
        ContentSizeFitterReseter(LeftPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject);
        ContentSizeFitterReseter(LeftPanel.transform.GetChild(1).GetChild(1).GetChild(2).GetChild(1).gameObject);
    }

    public void ContentSizeFitterReseter(GameObject TargetObject)
    {
        TargetObject.GetComponent<ContentSizeFitter>().enabled = false;
        TargetObject.GetComponent<ContentSizeFitter>().enabled = true;
    }

    public void GetAllChildren(Transform TargetParent, List<GameObject> ChildList)
    {
        ChildList.Add(TargetParent.gameObject);
        for(int i = 0; i < TargetParent.childCount; i++)
        {
            GetAllChildren(TargetParent.transform.GetChild(i), ChildList);
        }
    }
}
