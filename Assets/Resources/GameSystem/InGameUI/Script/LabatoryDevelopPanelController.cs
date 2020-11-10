using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabatoryDevelopPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    public GameObject SearchPanel;
    public GameObject FunctionPanel;
    public GameObject InputPanel;
    public GameObject InputNamePanel;
    public GameObject ResultArrowHolder;
    public GameObject ResultItemInfoPanel;
    public GameObject StatInfoPanel;
    public GameObject ProgressInfoPanel;
    public GameObject ConfirmPanel;
    public GameObject ListPanel;
    public GameObject CategoryListPanel;
    public GameObject CategoryCarrier;
    public GameObject ItemListPanel;
    public GameObject ItemCarrier;
    string CurrentFisrtItem;
    string CurrentSecondItem;
    string CurrentProcessor;
    string CurrentCategory;
    string CurrentSelector;
    string CurrentMode;
    public GameObject TargetObject;
    string PlayerCompanyName;
    LabatoryAct CallTargetLabatoryAct;
    NotificationManager CallNotificationManager;
    GoodsRecipe CallGoodsRecipe;
    GoodsValue CallGoodsValue;
    TechRecipe CallTechRecipe;
    TechValue CallTechValue;
    TimeManager CallTimeManager;
    ObjInstantiater CallObjInstantiater;
    GameObject FirstItemImageObject, SecondItemImageObject, ProcessorImageObject, FirstItemNameTextObject, SecondItemNameTextObject, ProcessorNameTextObject, ResultItemImageObject,
    ResultItemTypeTextObject, ResultItemNameInputFieldObject, ExpectedCostTextObject, MaterialPointTextObject, TechPointTextObject, LookPointTextObject, TotalPointTextObject, PackagedImageObject,
    ProgressBarObject, ProgressPercentageTextObject, PassedTimeTextObject, RemainTimeTextObject, CompletedPointTextObject, GainingPointTextObject, RemainPointTextObject, MainFunctionButton;
    List<GameObject> OverViewPanels;
    List<GameObject> SelectModePanels;

    // Start is called before the first frame update
    void Awake()
    {
        CallNotificationManager = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();
        CallGoodsRecipe = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        CallGoodsValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        CallTechRecipe = GameObject.Find("BaseSystem").GetComponent<TechRecipe>();
        CallTechValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetTechValue().GetComponent<TechValue>();
        CallTimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CallObjInstantiater = GameObject.Find("ObjectInstaller").GetComponent<ObjInstantiater>();
        
        int PanelListCount;
        OverViewPanels = new List<GameObject>();
        OverViewPanels.Add(InputPanel);
        OverViewPanels.Add(ResultArrowHolder);
        OverViewPanels.Add(ResultItemInfoPanel);
        OverViewPanels.Add(StatInfoPanel);
        OverViewPanels.Add(ProgressInfoPanel);
        OverViewPanels.Add(ConfirmPanel);
        PanelListCount = OverViewPanels.Count;
        for(int i = 0; i < PanelListCount; i++) OverViewPanels.Add(FunctionPanel.transform.GetChild(OverViewPanels[i].transform.GetSiblingIndex() - 1).gameObject);

        SelectModePanels = new List<GameObject>();
        SelectModePanels.Add(InputPanel);
        SelectModePanels.Add(ResultArrowHolder);
        SelectModePanels.Add(ResultItemInfoPanel);
        SelectModePanels.Add(StatInfoPanel);
        PanelListCount = SelectModePanels.Count;
        for(int i = 0; i < PanelListCount; i++) SelectModePanels.Add(FunctionPanel.transform.GetChild(SelectModePanels[i].transform.GetSiblingIndex() - 1).gameObject);

        FirstItemImageObject = InputPanel.transform.GetChild(1).GetChild(0).gameObject;
        SecondItemImageObject = InputPanel.transform.GetChild(5).GetChild(0).gameObject;
        ProcessorImageObject = InputPanel.transform.GetChild(3).GetChild(0).gameObject;
        FirstItemNameTextObject = InputNamePanel.transform.GetChild(1).gameObject;
        SecondItemNameTextObject = InputNamePanel.transform.GetChild(5).gameObject;
        ProcessorNameTextObject = InputNamePanel.transform.GetChild(3).gameObject;
        ResultItemImageObject = ResultItemInfoPanel.transform.GetChild(1).GetChild(0).gameObject;
        ResultItemTypeTextObject = ResultItemInfoPanel.transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
        ResultItemNameInputFieldObject = ResultItemInfoPanel.transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        ExpectedCostTextObject = ResultItemInfoPanel.transform.GetChild(3).GetChild(3).GetChild(1).gameObject;
        MaterialPointTextObject = StatInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;
        TechPointTextObject = StatInfoPanel.transform.GetChild(2).GetChild(1).GetChild(1).gameObject;
        LookPointTextObject = StatInfoPanel.transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        TotalPointTextObject = StatInfoPanel.transform.GetChild(4).GetChild(1).GetChild(1).gameObject;
        PackagedImageObject = StatInfoPanel.transform.GetChild(6).GetChild(1).GetChild(1).gameObject;
        ProgressBarObject = ProgressInfoPanel.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        ProgressPercentageTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        PassedTimeTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        RemainTimeTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
        CompletedPointTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject;
        GainingPointTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject;
        RemainPointTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(2).gameObject;
        MainFunctionButton = ConfirmPanel.transform.GetChild(1).gameObject;
    }

    public void Scaling()
    {
        SearchPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8, CallPanelController.CurrentUIsize);
        FunctionPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, CallPanelController.CurrentUIsize * 11.8f);
        Vector2 FunctionPanelSize = FunctionPanel.GetComponent<RectTransform>().sizeDelta;
        ListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, Screen.height - (CallPanelController.CurrentEdgePadding * 2) - FunctionPanelSize.y);

        SearchPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
        SearchPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7, CallPanelController.CurrentUIsize);

        CategoryListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, 0);
        CategoryListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3, CallPanelController.CurrentEdgePadding);
        CategoryListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(20f, 0, 0);
        CategoryListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        CategoryListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);
        CategoryListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(- CallPanelController.CurrentEdgePadding, 0);
        CategoryListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, - CallPanelController.CurrentEdgePadding);
        ItemListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8.2f, 0);
        ItemListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.8f, CallPanelController.CurrentEdgePadding);
        ItemListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        ItemListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        ItemListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);
        ItemListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(- CallPanelController.CurrentEdgePadding, 0);
        ItemListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, - CallPanelController.CurrentEdgePadding);

        float FunctionPanelOneLineSize = CallPanelController.CurrentUIsize * 0.75f;
        float FunctionTitlePanelSize = CallPanelController.CurrentUIsize * 0.3f;
        float FunctionValuePanelSize = CallPanelController.CurrentUIsize * 0.45f;

        for(int i = 0; i < FunctionPanel.transform.childCount; i++)
        {
            if(i % 2 == 0)
            {
                FunctionPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
            }
        }

        InputPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 3f);
        for(int i = 0; i < InputPanel.transform.childCount; i++)
        {
            if(i % (InputPanel.transform.childCount - 1) == 0)
            {
                InputPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
                InputNamePanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
            }
            else
            {
                if(i % 2 == 1)
                {
                    InputPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
                    InputNamePanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
                }
                else
                {
                    InputPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, 0);
                    InputNamePanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, 0);
                }
            }
        }

        ResultArrowHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);

        ResultItemInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 3f);
        for(int i = 0; i < ResultItemInfoPanel.transform.childCount; i++)
        {
            if(i % 2 == 0)
            {
                ResultItemInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
            }
            else
            {
                if(i == 1)
                {
                    ResultItemInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
                }
                else
                {
                    ResultItemInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.8f, 0);
                    for(int j = 0; j < ResultItemInfoPanel.transform.GetChild(i).childCount; j++)
                    {
                        ResultItemInfoPanel.transform.GetChild(i).GetChild(j).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
                        ResultItemInfoPanel.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionTitlePanelSize);
                        ResultItemInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionValuePanelSize);
                    }
                }
            }
        }

        StatInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
        for(int i = 0; i < StatInfoPanel.transform.childCount; i++)
        {
            if(i == 0 || i == StatInfoPanel.transform.childCount - 1) 
            {
                StatInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
            }
            else
            {
                if(i < StatInfoPanel.transform.childCount - 2)
                {
                    StatInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 2f, 0);
                    StatInfoPanel.transform.GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionTitlePanelSize);
                    StatInfoPanel.transform.GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionValuePanelSize);
                    StatInfoPanel.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionValuePanelSize, 0);
                    StatInfoPanel.transform.GetChild(i).GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((CallPanelController.CurrentUIsize * 2f) - FunctionValuePanelSize, 0);
                }
                else
                {
                    StatInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 1.2f, 0);
                    StatInfoPanel.transform.GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionTitlePanelSize);
                    StatInfoPanel.transform.GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionValuePanelSize);
                    StatInfoPanel.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((CallPanelController.CurrentUIsize * 1.2f) - FunctionValuePanelSize, 0);
                    StatInfoPanel.transform.GetChild(i).GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionValuePanelSize, 0);
                }
            }
        }

        ProgressInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.9f);
        for(int i = 0; i < ProgressInfoPanel.transform.childCount; i++) if(i % 2 == 0) ProgressInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        ProgressInfoPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 11.2f, 0);
        ProgressInfoPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.6f);
        ProgressInfoPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);

        ConfirmPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.6f);
        ConfirmPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8.6f, 0);
        ConfirmPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
        ConfirmPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);

        float CategoryListPanelSize = CategoryListPanel.GetComponent<RectTransform>().sizeDelta.x;
        float ItemListPanelSize = ItemListPanel.GetComponent<RectTransform>().sizeDelta.x;

        for(int i = 0; i < CategoryCarrier.transform.childCount; i++)
        {
            CategoryCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta 
                = new Vector2(CategoryListPanelSize - (CallPanelController.CurrentEdgePadding * 2), CallPanelController.CurrentEdgePadding);
        }
        
        ItemCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemListPanelSize - CallPanelController.CurrentEdgePadding, CallPanelController.CurrentUIsize * 3f);
        float ItemOneLineSize = ItemCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.y;
        for(int i = 0; i < 3; i++)
        {
            ItemCarrier.transform.GetChild(0).GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemOneLineSize - CallPanelController.CurrentEdgePadding, ItemOneLineSize);
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemOneLineSize - CallPanelController.CurrentEdgePadding, ItemOneLineSize - CallPanelController.CurrentEdgePadding);
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemOneLineSize - CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
        }

        CallPanelController.FontScaling(gameObject); 
    }

    void GetTargetObject()
    {
        TargetObject = CallPanelController.CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().TargetObject;
        CallTargetLabatoryAct = TargetObject.GetComponent<LabatoryAct>();
    }

    public void Initializing()
    {
        GetTargetObject();

        CurrentCategory = "";
        CurrentFisrtItem = "None";
        CurrentSecondItem = "None";
        CurrentProcessor = "None";
        CurrentSelector = "";

        ClearItemListPanel();
        ClaerInputPanel();
        ClearResultInfoPanel();
        ClearProgressInfoPanel();

        ChangeViewMode("OverView");

        if(CallTargetLabatoryAct.CurrentDevelopingProduct != null)
        {
            CurrentFisrtItem = CallTargetLabatoryAct.CurrentDevelopingProduct.ObjectInfo.Input[0];
            CurrentSelector = "FirstItem";
            DisplayInputInfo();

            if(CallTargetLabatoryAct.CurrentDevelopingProduct.ObjectInfo.Input.Count > 1)
            {
                CurrentSecondItem = CallTargetLabatoryAct.CurrentDevelopingProduct.ObjectInfo.Input[1];
                CurrentSelector = "SecondItem";
                DisplayInputInfo();
            }

            CurrentProcessor = CallTargetLabatoryAct.CurrentDevelopingProduct.ObjectInfo.RequiredProcessor;
            CurrentSelector = "Processor";
            DisplayInputInfo();

            DisplayResultItem(CallTargetLabatoryAct.CurrentDevelopingProduct.ObjectInfo);

            ResultItemNameInputFieldObject.GetComponent<InputField>().interactable = false;
            ConfirmPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = "Cancel";
            ConfirmPanel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(1f,0.4f,0.4f,1f);
            ConfirmPanel.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;

            FirstItemImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            SecondItemImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            ProcessorImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;

            UpdateProgressInfo();
        }
    }

    void ChangeViewMode(string Mode)
    {
        if(CurrentMode != Mode)
        {
            for(int i = 0; i < FunctionPanel.transform.childCount - 1; i++)
            {
                FunctionPanel.transform.GetChild(i).gameObject.SetActive(false);
            }

            if(Mode == "OverView")
            {
                foreach(var Panel in OverViewPanels) Panel.SetActive(true);
                Canvas.ForceUpdateCanvases();
                CallPanelController.ContentSizeFitterReseter(FunctionPanel);
                ListPanel.SetActive(false);
            }
            else if(Mode == "Select")
            {
                foreach(var Panel in SelectModePanels) Panel.SetActive(true);
                Canvas.ForceUpdateCanvases();
                CallPanelController.ContentSizeFitterReseter(FunctionPanel);
                ListPanel.SetActive(true);
                ListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Screen.height - CallPanelController.CurrentUIsize - FunctionPanel.GetComponent<RectTransform>().rect.height);
            }
            CurrentMode = Mode;
        }
    }

    public void InputButtonSelect(string Type)
    {
        if(CurrentSelector != Type)
        {
            ChangeViewMode("Select");

            if(Type == "FirstItem" || Type == "SecondItem")
            {
                DisplayItemCategoryList();
                DisplayItemList("All");
                CurrentCategory = "All";
            }
            else if(Type == "Processor")
            {
                DisplayProcessorList();
            }

            CurrentSelector = Type;
        }
    }

    void DisplayItemCategoryList()
    {
        List<string> CategoryList = new List<string>();

        CategoryList.Add("All");

        foreach(var Item in CallGoodsValue.GetStoredGoods())
        {
            bool isDuplicated = false;
            foreach(var Category in CategoryList)
            {
                if(Category == Item.Type)
                {
                    isDuplicated = true;
                    break;
                }
            }

            if(!isDuplicated)
            {
                CategoryList.Add(Item.Type);
            }
        }

        if(CategoryCarrier.transform.childCount > CategoryList.Count)
        {
            for(int i = CategoryCarrier.transform.childCount - 1; i <= CategoryList.Count; i--)
            {
                Destroy(CategoryCarrier.transform.GetChild(i).gameObject);
            }    
        }
        else if(CategoryCarrier.transform.childCount < CategoryList.Count)
        {
            for(int i = CategoryCarrier.transform.childCount; i < CategoryList.Count; i++)
            {
                GameObject.Instantiate(CategoryCarrier.transform.GetChild(0).gameObject, CategoryCarrier.transform);
            }
        }

        for(int i = 0; i < CategoryList.Count; i++)
        {
            CategoryCarrier.transform.GetChild(i).gameObject.SetActive(true);

            CategoryCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = " " + CategoryList[i];
        }
    }

    void DisplayItemList(string Category)
    {
        List<string> StoredList = CallGoodsValue.GetAllGoodsName(true);
        List<string> ItemList = new List<string>();
        int RowLimit = 0;

        ItemList.Add("None");

        if(Category != "All")
        {
            foreach(var Item in StoredList)
            {
                GoodsRecipe.Recipe TargetItemRecipe = CallGoodsRecipe.GetRecipe(Item);
                if(TargetItemRecipe.Type == Category)
                {
                    ItemList.Add(Item);
                }
            }
        }
        else
        {
            ItemList.AddRange(StoredList);
        }

        RowLimit = Mathf.CeilToInt(((float)ItemList.Count) / 3f);

        if(RowLimit > ItemCarrier.transform.childCount)
        {
            for(int i = ItemCarrier.transform.childCount; i < RowLimit; i++)
            {
                GameObject.Instantiate(ItemCarrier.transform.GetChild(0).gameObject, ItemCarrier.transform);
            }
        }
        else if(RowLimit < ItemCarrier.transform.childCount)
        {
            for(int i = ItemCarrier.transform.childCount - 1; i >= RowLimit; i--)
            {
                Destroy(ItemCarrier.transform.GetChild(i).gameObject);
            }
        }
        
        for(int i = 0; i < RowLimit; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.SetActive(false);
            }
        }

        for(int i = 0; i < RowLimit; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if(i * 3 + j >= ItemList.Count)
                {
                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = null;
                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "";
                    
                    break;
                }

                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.SetActive(true);

                if(ItemList[i * 3 + j] == "None") ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/InsideEmptyCircle");
                else ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + CallGoodsRecipe.GetRecipe(ItemList[i * 3 + j]).Type);
                
                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = ItemList[i * 3 + j];
            }
        }

        CurrentCategory = Category;
    }

    void DisplayProcessorList()
    {
        List<string> ItemList = new List<string>();
        int RowLimit = 0;

        if(CategoryCarrier.transform.childCount > 1)
        {
            for(int i = 2; i < CategoryCarrier.transform.childCount; i++)
            {
                Destroy(CategoryCarrier.transform.GetChild(i).gameObject);
            }    
        }
        CategoryCarrier.transform.GetChild(0).gameObject.SetActive(false);

        for(int i = ItemCarrier.transform.childCount - 1; i > 0; i--)
        {
            Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }
        
        foreach(var Facility in CallTechValue.FacilityList)
        {
            if(Facility.Type != "Processor" && Facility.Type != "Assembler") continue;

            TechRecipe.ProcessorInfo TargetProcessorRecipe = CallTechRecipe.GetProcessorRecipe(Facility.Name);

            if(TargetProcessorRecipe == null)
            {
                Debug.Log("There is no " + Facility.Name + " in System");
                return;
            }

            foreach(var ActorName in TargetProcessorRecipe.ActorList)
            {
                if(!CallTechValue.GetActorPossible(ActorName)) continue;

                string TargetName = Facility.Name + "-" + ActorName;
                bool isDuplicate = false;
                foreach(var Item in ItemList)
                {
                    if(TargetName == Item)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if(!isDuplicate)
                {
                    ItemList.Add(TargetName);
                }
            }
        }

        RowLimit = Mathf.CeilToInt((float)ItemList.Count / 3f);

        if(ItemList.Count == 0)
        {
            for(int i = 1; i < 3; i++)
            {
                ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
            }

            ItemCarrier.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/InsideEmptyCircle");
            ItemCarrier.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "None";
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true);
            }

            for(int i = 0; i < RowLimit; i++)
            {
                if(i != 0)
                {
                    GameObject.Instantiate(ItemCarrier.transform.GetChild(0).gameObject, ItemCarrier.transform);
                }

                int LeftItem = ItemList.Count - (i * 3) + 1;
                if(LeftItem < 3)
                {
                    for(int j = 2; j >= LeftItem; j--)
                    {
                        ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.SetActive(false);
                    }
                }

                for(int j = 0; j < 3; j++)
                {
                    if(i ==0 && j == 0)
                    {
                        ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/InsideEmptyCircle");
                        ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "None";
                    }
                    else
                    {
                        if(i * 3 + j - 1 >= ItemList.Count)
                        {
                            break;
                        }
                        ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = null;
                        ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = ItemList[i * 3 + j - 1];
                    }
                }
            }
        }

        CurrentCategory = "";
    }

    public void CategorySelect(GameObject SelectedItem)
    {
        string SelectedCategoryName = SelectedItem.transform.GetChild(0).gameObject.GetComponent<Text>().text.Substring(1);
        
        if(CurrentCategory != SelectedCategoryName) DisplayItemList(SelectedCategoryName);
    }

    public void ItemSelect(GameObject SelectedItem)
    {
        string SelectedItemName = SelectedItem.transform.GetChild(1).gameObject.GetComponent<Text>().text;
        bool hasChanged = false;

        if(CurrentSelector == "FirstItem")
        {
            if(CurrentFisrtItem != SelectedItemName)
            {
                hasChanged = true;
                CurrentFisrtItem = SelectedItemName;
                DisplayInputInfo();
            }
        }
        else if(CurrentSelector == "SecondItem")
        {
            if(CurrentSecondItem != SelectedItemName)
            {
                hasChanged = true;
                CurrentSecondItem = SelectedItemName;
                DisplayInputInfo();
            }
        }
        else if(CurrentSelector == "Processor")
        {
            string TargetString = "";
            if(SelectedItemName != "None") TargetString += SelectedItemName.Split('-')[0] + "?" + SelectedItemName.Split('-')[1];
            else TargetString = "None";
            
            if(CurrentProcessor != TargetString)
            {
                hasChanged = true;
                CurrentProcessor = TargetString;
                DisplayInputInfo();
            }
        }

        if(hasChanged)
        {
            CallTargetLabatoryAct.resultObject.Input.Clear();
            if(CurrentFisrtItem != "None") CallTargetLabatoryAct.resultObject.Input.Add(CurrentFisrtItem);
            if(CurrentSecondItem != "None") CallTargetLabatoryAct.resultObject.Input.Add(CurrentSecondItem);
            CallTargetLabatoryAct.resultObject.RequiredProcessor = CurrentProcessor;

            if(CallTargetLabatoryAct.setResultObject()) DisplayResultItem(CallTargetLabatoryAct.resultObject);
            else ClearResultInfoPanel();
        }

        CurrentSelector = "";
        ChangeViewMode("OverView");
    }

    void DisplayInputInfo()
    {
        if(CurrentSelector == "FirstItem")
        {
            if(CurrentFisrtItem != "None")
            {
                GoodsRecipe.Recipe TargetRecipe = CallGoodsRecipe.GetRecipe(CurrentFisrtItem);

                FirstItemImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetRecipe.Type);
                FirstItemNameTextObject.GetComponent<Text>().text = CurrentFisrtItem;
                FirstItemImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);

            }
            else
            {
                FirstItemImageObject.GetComponent<Image>().sprite = null;
                FirstItemNameTextObject.GetComponent<Text>().text = "";
                FirstItemImageObject.GetComponent<Image>().color = new Color(0,0,0,0);
            }
        }
        else if(CurrentSelector == "SecondItem")
        {
            if(CurrentSecondItem != "None")
            {
                GoodsRecipe.Recipe TargetRecipe = CallGoodsRecipe.GetRecipe(CurrentSecondItem);

                SecondItemImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetRecipe.Type);
                SecondItemNameTextObject.GetComponent<Text>().text = CurrentSecondItem;
                SecondItemImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
            }
            else
            {
                SecondItemImageObject.GetComponent<Image>().sprite = null;
                SecondItemNameTextObject.GetComponent<Text>().text = "";
                SecondItemImageObject.GetComponent<Image>().color = new Color(0,0,0,0);
            }
        }
        else if(CurrentSelector == "Processor")
        {
            if(CurrentProcessor != "None")
            {
                ProcessorImageObject.GetComponent<Image>().sprite = null;
                ProcessorNameTextObject.GetComponent<Text>().text = CurrentProcessor.Split('?')[0] + "-" + CurrentProcessor.Split('?')[1];
                ProcessorImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
            }
            else
            {
                ProcessorImageObject.GetComponent<Image>().sprite = null;
                ProcessorNameTextObject.GetComponent<Text>().text = "";
                ProcessorImageObject.GetComponent<Image>().color = new Color(0,0,0,0);
            }
        }
    }

    void DisplayResultItem(LabatoryAct.ResultObject TargetInfo)
    {
        ResultItemImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetInfo.Type);
        ResultItemImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);

        ResultItemTypeTextObject.GetComponent<Text>().text = TargetInfo.Type;
        ResultItemNameInputFieldObject.GetComponent<InputField>().interactable = true;
        ResultItemNameInputFieldObject.GetComponent<InputField>().text = "";
        ExpectedCostTextObject.GetComponent<Text>().text = (Mathf.CeilToInt((float)TargetInfo.RequiredPoint / (TargetInfo.RequiredResearchPower / CallTargetLabatoryAct.ResearchPower)) * CallTargetLabatoryAct.Budget).ToString();

        MaterialPointTextObject.GetComponent<Text>().text = "x " + (Mathf.CeilToInt(TargetInfo.Attractiveness.MaterialPoint * 10f) * 0.1f).ToString();
        TechPointTextObject.GetComponent<Text>().text = "x " + (Mathf.CeilToInt(TargetInfo.Attractiveness.TechPoint * 10f) * 0.1f).ToString();
        LookPointTextObject.GetComponent<Text>().text = "x " + (Mathf.CeilToInt(TargetInfo.Attractiveness.LookPoint * 10f) * 0.1f).ToString();
        TotalPointTextObject.GetComponent<Text>().text = "x " + (Mathf.CeilToInt(TargetInfo.Attractiveness.TotalPoint * 10f) * 0.1f).ToString();
        if(TargetInfo.Attractiveness.isPackaged) PackagedImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/PossitiveMark");
        else PackagedImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/NegativeMark");
    }

    public void NameInputFieldValueChange()
    {
        if(ResultItemNameInputFieldObject.GetComponent<InputField>().text != "")
        {
            if(CallTargetLabatoryAct.CurrentDevelopingProduct == null && CallTargetLabatoryAct.resultObject.Type != "")
            {
                MainFunctionButton.gameObject.GetComponent<Button>().interactable = true;
                MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Start";
            }
        }
        else
        {
            MainFunctionButton.gameObject.GetComponent<Button>().interactable = false;
            MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Start";
        }
    }

    void ClearItemListPanel()
    {
        for(int i = ItemCarrier.transform.childCount - 1; i > 0; i--)
        {
            Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < 3; i++)
        {
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }

    void ClaerInputPanel()
    {
        FirstItemImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        SecondItemImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        ProcessorImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;

        FirstItemImageObject.GetComponent<Image>().color = new Color(0,0,0,0);
        FirstItemNameTextObject.GetComponent<Text>().text = "";
        CurrentFisrtItem = "None";

        SecondItemImageObject.GetComponent<Image>().color = new Color(0,0,0,0);
        SecondItemNameTextObject.GetComponent<Text>().text = "";
        CurrentSecondItem = "None";

        ProcessorImageObject.GetComponent<Image>().color = new Color(0,0,0,0);
        ProcessorNameTextObject.GetComponent<Text>().text = "";
        CurrentProcessor = "None";
    }

    void ClearResultInfoPanel()
    {
        ResultItemTypeTextObject.GetComponent<Text>().text = "";
        ResultItemNameInputFieldObject.GetComponent<InputField>().interactable = true;
        ResultItemNameInputFieldObject.GetComponent<InputField>().text = "";
        ExpectedCostTextObject.GetComponent<Text>().text = "TEST";

        MaterialPointTextObject.GetComponent<Text>().text = "";
        TechPointTextObject.GetComponent<Text>().text = "";
        LookPointTextObject.GetComponent<Text>().text = "";
        TotalPointTextObject.GetComponent<Text>().text = "";
        PackagedImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/QuestionMark");
    }

    void ClearProgressInfoPanel()
    {
        ProgressBarObject.GetComponent<Image>().fillAmount = 0f;
        ProgressPercentageTextObject.GetComponent<Text>().text = "";

        PassedTimeTextObject.GetComponent<Text>().text = "00:00";
        RemainTimeTextObject.GetComponent<Text>().text = " / 00:00";
        GainingPointTextObject.GetComponent<Text>().text = "0";
        GainingPointTextObject.GetComponent<Text>().text = "(+0)";
        RemainPointTextObject.GetComponent<Text>().text = " / 0";
    }

    public void ConfirmButtonSelect()
    {
        string Inputs = ResultItemNameInputFieldObject.GetComponent<InputField>().text;
        if(CallGoodsRecipe.GetRecipe(Inputs) != null)
        {
            CallNotificationManager.AddAlert("Goods name is already exist!", 1, "");
            return;
        }

        string ButtonText = MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        Color ButtonColor = MainFunctionButton.gameObject.GetComponent<Image>().color;

        if(CallTargetLabatoryAct.CurrentDevelopingProduct == null)
        {
            ResultItemNameInputFieldObject.GetComponent<InputField>().interactable = false;
            MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Cancel";
            MainFunctionButton.gameObject.GetComponent<Image>().color = new Color(1f,0.4f,0.4f,1f);

            FirstItemImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            SecondItemImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            ProcessorImageObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;

            CallTargetLabatoryAct.StartDeveloping(Inputs);
        }
        else
        {
            MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Start";
            MainFunctionButton.gameObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);

            ClaerInputPanel();
            ClearResultInfoPanel();
            ClearProgressInfoPanel();
            NameInputFieldValueChange();

            CallTargetLabatoryAct.EndDeveloping();
        }

        CallPanelController.CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().DisplayInfo();
    }

    public void UpdateProgressInfo()
    {
        LabatoryAct.DevelopingProduct TargetInfo = CallTargetLabatoryAct.CurrentDevelopingProduct;
        float CompletePercentage = Mathf.CeilToInt(TargetInfo.CompletedPoint / (float)TargetInfo.ObjectInfo.RequiredPoint * 100f) * 0.01f;
        float CurrentGainingPoint = CallTargetLabatoryAct.ResearchPower / TargetInfo.ObjectInfo.RequiredResearchPower;

        ProgressBarObject.GetComponent<Image>().fillAmount = CompletePercentage;
        if(CompletePercentage >= 1f)
        {
            MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Finish";
            MainFunctionButton.gameObject.GetComponent<Image>().color = new Color(0,1f,0,1f);
            ProgressPercentageTextObject.GetComponent<Text>().text = "+ " + (Mathf.CeilToInt(TargetInfo.ObjectInfo.Attractiveness.PerfectionPoint * 10f) * 0.1f).ToString() + " Points";
        }
        else
        {
            ProgressPercentageTextObject.GetComponent<Text>().text = (CompletePercentage * 100).ToString() + " %";
        }

        PassedTimeTextObject.GetComponent<Text>().text = CallTimeManager.GetPeriodString(CallTimeManager.TimeValue - TargetInfo.StartTime, "Short");
        RemainTimeTextObject.GetComponent<Text>().text = " / " + CallTimeManager.GetPeriodString((CallTimeManager.TimeValue - TargetInfo.StartTime) + (Mathf.CeilToInt(((float)TargetInfo.ObjectInfo.RequiredPoint - TargetInfo.CompletedPoint) / CurrentGainingPoint) * CallTimeManager.Hour), "Short");

        CompletedPointTextObject.GetComponent<Text>().text = (Mathf.CeilToInt(TargetInfo.CompletedPoint)).ToString();
        GainingPointTextObject.GetComponent<Text>().text = "(+"+(Mathf.CeilToInt(CurrentGainingPoint)).ToString() + ")";
        RemainPointTextObject.GetComponent<Text>().text = " / " + TargetInfo.ObjectInfo.RequiredPoint.ToString();
    }

    public void ClosePanel()
    {
        
    }
}
