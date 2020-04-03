using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    [SerializeField]GameObject SearchPanel;
    [SerializeField]GameObject FunctionPanel;
    [SerializeField]GameObject ImageHolder;
    [SerializeField]GameObject UpperInfoPanel;
    [SerializeField]GameObject LowerInfoPanel;
    [SerializeField]GameObject FunctionButtonPanel;
    [SerializeField]GameObject ListPanel;
    [SerializeField]GameObject CategoryListPanel;
    [SerializeField]GameObject CategoryCarrier;
    [SerializeField]GameObject ItemListPanel;
    [SerializeField]GameObject ItemCarrier;
    public string CurrentCategory = "";
    public string CurrentItem = "";
    string PlayerCompanyName;
    ClickChecker CallClickChecker;
    NotificationManager CallNotificationManager;
    SalesValue CallSalesValue;
    EconomyValue CallEconomyValue;
    GoodsValue CallGoodsValue;
    GoodsRecipe CallGoodsRecipe;
    TechValue CallTechValue;
    public SalesValue.SalesInfo TargetItemSalesInfo;
    GameObject ImageObject, RemainQuantityTextObject, NameTextObject, TypeTextObject, CompanyTextObject, MaterialPointTextObject, TechPointTextObject, LookPointTextObject, TotalPointTextObject, ExpectedQualityTextObject
    , MarketShareTextObject, SoldAmountTextObject, CSPointTextObject, ContractListTitleTextObject, ContractListDropdownObject, TermTitleTextObject, TermInputObject, PriceInputObject
    , QuantityTitleTextObject , QuantityInputObject , SubFunctionButton, MainFunctionButton;

    // Start is called before the first frame update
    void Awake()
    {
        CallClickChecker = GameObject.Find("BaseSystem").GetComponent<ClickChecker>();
        CallNotificationManager = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();
        CallSalesValue = GameObject.Find("SalesManager").GetComponent<SalesValue>();
        CallEconomyValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetEconomyValue().GetComponent<EconomyValue>();
        CallGoodsValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        CallGoodsRecipe = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        CallTechValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetTechValue().GetComponent<TechValue>();

        ImageObject = ImageHolder.transform.GetChild(0).gameObject;
        RemainQuantityTextObject = ImageHolder.transform.GetChild(0).GetChild(0).gameObject;
        NameTextObject = UpperInfoPanel.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        TypeTextObject = UpperInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;
        CompanyTextObject = UpperInfoPanel.transform.GetChild(1).GetChild(2).GetChild(1).gameObject;
        MaterialPointTextObject = UpperInfoPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(1).gameObject;
        TechPointTextObject = UpperInfoPanel.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).gameObject;
        LookPointTextObject = UpperInfoPanel.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(1).gameObject;
        TotalPointTextObject = UpperInfoPanel.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(1).gameObject;
        ExpectedQualityTextObject = LowerInfoPanel.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        MarketShareTextObject = LowerInfoPanel.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        SoldAmountTextObject = LowerInfoPanel.transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
        CSPointTextObject = LowerInfoPanel.transform.GetChild(0).GetChild(3).GetChild(1).gameObject;
        ContractListTitleTextObject = LowerInfoPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).gameObject;
        ContractListDropdownObject = LowerInfoPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).gameObject;
        TermTitleTextObject = LowerInfoPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(0).gameObject;
        TermInputObject = LowerInfoPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(1).gameObject;
        PriceInputObject = LowerInfoPanel.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).gameObject;
        QuantityTitleTextObject = LowerInfoPanel.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).gameObject;
        QuantityInputObject = LowerInfoPanel.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).gameObject;
        SubFunctionButton = FunctionButtonPanel.transform.GetChild(0).gameObject;
        MainFunctionButton = FunctionButtonPanel.transform.GetChild(1).gameObject;
    }

    void Start()
    {

    }

    void Update()
    {
        if(CurrentCategory != "Storage")
        {
            if(CurrentItem != "")
            {
                if(CallClickChecker.ui == MainFunctionButton)
                {
                    if(MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text == "Buy")
                    {
                        if(MainFunctionButton.GetComponent<Button>().interactable)
                        {
                            int InputValue = int.Parse(QuantityInputObject.GetComponent<InputField>().text);
                            int LackCount = InputValue;
                            string ReasonString = "";
                            
                            if(InputValue * TargetItemSalesInfo.Price > CallEconomyValue.Balance)
                            {
                                LackCount = Mathf.FloorToInt(CallEconomyValue.Balance / TargetItemSalesInfo.Price);
                                ReasonString = "cash";
                            }
                            if(TargetItemSalesInfo.ItemCount < InputValue)
                            {
                                if(TargetItemSalesInfo.ItemCount < LackCount)
                                {
                                    LackCount = TargetItemSalesInfo.ItemCount;
                                    ReasonString = "stock";
                                }
                            }

                            if(LackCount != InputValue)
                            {
                                CallNotificationManager.SetNote("Lack of " + ReasonString + ". Only " + LackCount + " items will deliver to you", new Color(1f,0.2f,0.2f));
                            }
                        }
                    }
                } 
            }
        }
    }

    public void Scaling()
    {
        SearchPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8, CallPanelController.CurrentUIsize);
        FunctionPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, CallPanelController.CurrentUIsize * 8);
        Vector2 FunctionPanelSize = FunctionPanel.GetComponent<RectTransform>().sizeDelta;
        ListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, Screen.height - CallPanelController.CurrentUIsize - FunctionPanelSize.y);
        Vector2 ListPanelSize = ListPanel.GetComponent<RectTransform>().sizeDelta;

        SearchPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
        SearchPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7, CallPanelController.CurrentUIsize);

        float FunctionPanelOneLineSize = CallPanelController.CurrentUIsize * 3;        
        ImageHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionPanelOneLineSize, FunctionPanelOneLineSize);
        ImageHolder.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding, 0);
        ImageHolder.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
        UpperInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionPanelSize.x - FunctionPanelOneLineSize - (CallPanelController.CurrentEdgePadding * 2), FunctionPanelOneLineSize);
        UpperInfoPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CallPanelController.CurrentEdgePadding + FunctionPanelOneLineSize, - CallPanelController.CurrentEdgePadding);
        LowerInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionPanelSize.x - (CallPanelController.CurrentEdgePadding * 2), FunctionPanelOneLineSize);
        LowerInfoPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CallPanelController.CurrentEdgePadding, - ((CallPanelController.CurrentEdgePadding * 1.25f) + FunctionPanelOneLineSize));
        FunctionButtonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.9f);
        FunctionButtonPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(- CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);

        CategoryListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, ListPanelSize.y);
        CategoryListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3, CallPanelController.CurrentEdgePadding);
        CategoryListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CallPanelController.CurrentEdgePadding, 0, 0);
        CategoryListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        CategoryListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);
        CategoryListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(- CallPanelController.CurrentEdgePadding, 0);
        CategoryListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, - CallPanelController.CurrentEdgePadding);
        ItemListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8.2f, ListPanelSize.y);
        ItemListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.8f, CallPanelController.CurrentEdgePadding);
        ItemListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        ItemListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
        ItemListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);
        ItemListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(- CallPanelController.CurrentEdgePadding, 0);
        ItemListPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, - CallPanelController.CurrentEdgePadding);

        float CategoryListPanelSize = CategoryListPanel.GetComponent<RectTransform>().sizeDelta.x;
        float ItemListPanelSize = ItemListPanel.GetComponent<RectTransform>().sizeDelta.x;

        UpperInfoPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, FunctionPanelOneLineSize);
        UpperInfoPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 5.4f, FunctionPanelOneLineSize);
        UpperInfoPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 2.2f, FunctionPanelOneLineSize);
        LowerInfoPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionPanelOneLineSize, FunctionPanelOneLineSize);
        LowerInfoPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, FunctionPanelOneLineSize);
        LowerInfoPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.8f, FunctionPanelOneLineSize);
        FunctionButtonPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, CallPanelController.CurrentUIsize * 0.9f);
        FunctionButtonPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, CallPanelController.CurrentUIsize * 0.9f);

        for(int i = 1; i < 3; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                UpperInfoPanel.transform.GetChild(i).GetChild(j).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.75f);
                UpperInfoPanel.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
                UpperInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.45f);
                if(i == 2)
                {
                    UpperInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 0.45f, 0);
                    UpperInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((CallPanelController.CurrentUIsize * 2.2f) - (CallPanelController.CurrentUIsize * 0.45f), 0);
                }
            }
        }

        for(int i = 0; i < 4; i++)
        {
            LowerInfoPanel.transform.GetChild(0).GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.75f);
            LowerInfoPanel.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
            LowerInfoPanel.transform.GetChild(0).GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.45f);
        }

        for(int i = 0; i < 2; i++)
        {
            LowerInfoPanel.transform.GetChild(2).GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, 0);
            for(int j = 0; j < 2; j++)
            {
                LowerInfoPanel.transform.GetChild(2).GetChild(i).GetChild(j).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
                LowerInfoPanel.transform.GetChild(2).GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
                LowerInfoPanel.transform.GetChild(2).GetChild(i).GetChild(j).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.7f);
            }
        }

        for(int i = 0; i < CategoryCarrier.transform.childCount; i++)
        {
            CategoryCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CategoryListPanelSize - (CallPanelController.CurrentEdgePadding * 2), CallPanelController.CurrentEdgePadding);
        }
        
        ItemCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemListPanelSize - CallPanelController.CurrentEdgePadding, CallPanelController.CurrentUIsize * 3f);
        float ItemOneLineSize = ItemCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta.y;
        for(int i = 0; i < 3; i++)
        {
            ItemCarrier.transform.GetChild(0).GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemOneLineSize - CallPanelController.CurrentEdgePadding, ItemOneLineSize);
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemOneLineSize - CallPanelController.CurrentEdgePadding, ItemOneLineSize - CallPanelController.CurrentEdgePadding);
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemOneLineSize - CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
        }

        CallPanelController.FontScaling(gameObject);
    }

    public void Initializing()
    {
        DisplayCategoryList();
        DisplayItemList("All");

        ClearInfoPanel();

        PlayerCompanyName = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().PlayerCompanyName;
    }

    void DisplayCategoryList()
    {
        List<string> CategoryList = new List<string>();

        CategoryList.Add("Storage");
        CategoryList.Add("All");

        foreach(var SalesItem in CallSalesValue.SalesItemArray)
        {
            bool isDuplicated = false;
            foreach(var Category in CategoryList)
            {
                if(Category == SalesItem.RecipeInfo.Recipe.GoodsObject.name)
                {
                    isDuplicated = true;
                    break;
                }
            }

            if(!isDuplicated)
            {
                CategoryList.Add(SalesItem.RecipeInfo.Recipe.GoodsObject.name);
            }
        }

        if( CategoryList.Count > CategoryCarrier.transform.childCount)
        {
            for(int i = CategoryCarrier.transform.childCount; i <  CategoryList.Count; i++)
            {
                GameObject.Instantiate(CategoryCarrier.transform.GetChild(1).gameObject, CategoryCarrier.transform);
            }
        }
        else if( CategoryList.Count < CategoryCarrier.transform.childCount)
        {
            for(int i = CategoryCarrier.transform.childCount - 1; i >=  CategoryList.Count; i--)
            {
                Destroy(CategoryCarrier.transform.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < CategoryCarrier.transform.childCount; i++)
        {
            CategoryCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = " " + CategoryList[i];
        }
    }

    void DisplayItemList(string Category)
    {
        List<SalesValue.SalesInfo> ItemList = new List<SalesValue.SalesInfo>();
        int RowLimit = 0;
        
        if(Category == "All")
        {
            foreach(var Item in CallSalesValue.SalesItemArray)
            {
                ItemList.Add(Item);
            }
        }
        else if(Category == "Commercial Goods")
        {

        }
        else if(Category == "Raw Material")
        {

        }
        else
        {
            foreach(var Item in CallSalesValue.SalesItemArray)
            {
                if(Item.RecipeInfo.Recipe.GoodsObject.name == Category)
                {
                    ItemList.Add(Item);
                }
            }
        }

        RowLimit = Mathf.CeilToInt((float)ItemList.Count / 3f);

        if(RowLimit > ItemCarrier.transform.childCount)
        {
            for(int i = ItemCarrier.transform.childCount; i < RowLimit; i++)
            {
                GameObject.Instantiate(ItemCarrier.transform.GetChild(0).gameObject, ItemCarrier.transform);
            }
        }
        else if(RowLimit < ItemCarrier.transform.childCount)
        {
            if(RowLimit ==  0) RowLimit = 1;
            for(int i = ItemCarrier.transform.childCount - 1; i >= RowLimit; i--)
            {
                ItemCarrier.transform.GetChild(i).gameObject.SetActive(false);
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

                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + ItemList[i * 3 + j].RecipeInfo.Recipe.GoodsObject.name);
                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = ItemList[i * 3 + j].RecipeInfo.Recipe.OutputName;
            }
        }

        for(int i = 1; i < ItemCarrier.transform.childCount; i++)
        {
            if(!ItemCarrier.transform.GetChild(i).gameObject.activeSelf) Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }

        CurrentCategory = Category;
    }

    void DisplayStorageList()
    {
        List<string> ItemList = CallGoodsValue.GetAllGoodsName(true);

        foreach(var Sales in CallSalesValue.SalesItemArray)
        {
            if(Sales.Seller == PlayerCompanyName)
            {
                bool isDuplicated = false;
                foreach(var Item in ItemList)
                {
                    if(Item == Sales.RecipeInfo.Recipe.OutputName)
                    {
                        isDuplicated = true;

                        break;
                    }
                }

                if(!isDuplicated) ItemList.Add(Sales.RecipeInfo.Recipe.OutputName);
            }
        }
        
        int RowLimit = Mathf.CeilToInt((float)ItemList.Count / 3f);

        if(RowLimit > ItemCarrier.transform.childCount)
        {
            for(int i = ItemCarrier.transform.childCount; i < RowLimit; i++)
            {
                GameObject.Instantiate(ItemCarrier.transform.GetChild(0).gameObject, ItemCarrier.transform);
            }
        }
        else if(RowLimit < ItemCarrier.transform.childCount)
        {
            if(RowLimit ==  0) RowLimit = 1;
            for(int i = ItemCarrier.transform.childCount - 1; i >= RowLimit; i--)
            {
                ItemCarrier.transform.GetChild(i).gameObject.SetActive(false);
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

                string ItemType = CallGoodsRecipe.GetRecipe(ItemList[i * 3 + j]).GoodsObject.name;
                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + ItemType);
                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = ItemList[i * 3 + j];
            }
        }

        for(int i = 1; i < ItemCarrier.transform.childCount; i++)
        {
            if(!ItemCarrier.transform.GetChild(i).gameObject.activeSelf) Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }

        CurrentCategory = "Storage";
    }

    void DisplayInfo(string Name)
    {
        GoodsRecipe.Recipe TargetItemRecipe = CallGoodsRecipe.GetRecipe(Name);
        TargetItemSalesInfo = CallSalesValue.GetSalesInfo(Name);

        ImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetItemRecipe.GoodsObject.name);
        ImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
        NameTextObject.GetComponent<Text>().text = TargetItemRecipe.OutputName;
        TypeTextObject.GetComponent<Text>().text = TargetItemRecipe.GoodsObject.name;

        MaterialPointTextObject.GetComponent<Text>().text = " x " + (Mathf.RoundToInt(TargetItemRecipe.Attractiveness.MaterialPoint * 10) * 0.1).ToString();
        TechPointTextObject.GetComponent<Text>().text = " x " + (Mathf.RoundToInt(TargetItemRecipe.Attractiveness.TechPoint * 10) * 0.1).ToString();
        LookPointTextObject.GetComponent<Text>().text = " x " + (Mathf.RoundToInt(TargetItemRecipe.Attractiveness.LookPoint * 10) * 0.1).ToString();
        TotalPointTextObject.GetComponent<Text>().text = " x " + (Mathf.RoundToInt(TargetItemRecipe.Attractiveness.TotalPoint * 10) * 0.1).ToString();

        if(TargetItemSalesInfo != null)
        {
            CompanyTextObject.GetComponent<Text>().text = TargetItemSalesInfo.Seller;

            ExpectedQualityTextObject.GetComponent<Text>().text = (Mathf.CeilToInt(TargetItemSalesInfo.QualityEvaluation * 10) * 0.1f).ToString();
            MarketShareTextObject.GetComponent<Text>().text = (Mathf.RoundToInt(CallSalesValue.CalculateMarketShare(TargetItemRecipe.OutputName) * 10f) * 0.1f).ToString() + " %";
            SoldAmountTextObject.GetComponent<Text>().text = TargetItemSalesInfo.SoldCount.ToString();
            CSPointTextObject.GetComponent<Text>().text = "TEST";
        }
        else
        {
            CompanyTextObject.GetComponent<Text>().text = "";

            ExpectedQualityTextObject.GetComponent<Text>().text = "-";
            MarketShareTextObject.GetComponent<Text>().text = "-";
            SoldAmountTextObject.GetComponent<Text>().text = "-";
            CSPointTextObject.GetComponent<Text>().text = "-";
        }

        CurrentItem = Name;

        if(CurrentCategory == "Storage")
        {
            DisplaySalesControlPanel("Mine");
        }
        else
        {
            if(TargetItemSalesInfo.Seller == PlayerCompanyName) DisplaySalesControlPanel("Mine");
            else DisplaySalesControlPanel("Others");
        }

        UpdateRemainQuantityText();
    }

    void DisplaySalesControlPanel(string Type)
    {
        if(Type == "Mine")
        {
            ContractListTitleTextObject.GetComponent<Text>().text = "Contract List";
            ContractListDropdownObject.GetComponent<Dropdown>().interactable = false;
            List<string> ContractOptions = new List<string>();
            ContractOptions.Add("Common");
            ContractListDropdownObject.GetComponent<Dropdown>().ClearOptions();
            ContractListDropdownObject.GetComponent<Dropdown>().AddOptions(ContractOptions);
            ContractListDropdownObject.GetComponent<Dropdown>().value = 0;
            TermTitleTextObject.SetActive(false);
            TermInputObject.SetActive(false);
            QuantityTitleTextObject.SetActive(false);
            QuantityInputObject.SetActive(false);

            
            if(CallTechValue.GetRecipe(CurrentItem) == null)
            {
                PriceInputObject.GetComponent<InputField>().interactable = false;
                MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Enroll";
                MainFunctionButton.GetComponent<Button>().interactable = false;
                SubFunctionButton.gameObject.SetActive(false);
            }
            else
            {
                if(TargetItemSalesInfo != null)
                {
                    if(TargetItemSalesInfo.Seller == PlayerCompanyName)
                    {
                        PriceInputObject.GetComponent<InputField>().interactable = true;
                        PriceInputObject.GetComponent<InputField>().text = TargetItemSalesInfo.Price.ToString();

                        MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Modify";
                        MainFunctionButton.GetComponent<Button>().interactable = false;
                        SubFunctionButton.gameObject.SetActive(true);
                        SubFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Delete";
                        if(TargetItemSalesInfo.ContractList.Count > 0)
                        {
                            ContractListDropdownObject.GetComponent<Dropdown>().interactable = true;
                            foreach(var Contract in TargetItemSalesInfo.ContractList)
                            {
                                ContractOptions.Add(Contract.CompanyName);
                            }
                        }
                    }
                    else
                    {
                        PriceInputObject.GetComponent<InputField>().interactable = false;
                        PriceInputObject.GetComponent<InputField>().text = "0";

                        MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Enroll";
                        MainFunctionButton.GetComponent<Button>().interactable = false;
                        SubFunctionButton.gameObject.SetActive(false);
                    }
                }
                else
                {
                    PriceInputObject.GetComponent<InputField>().interactable = true;
                    PriceInputObject.GetComponent<InputField>().text = "0";

                    MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Enroll";
                    MainFunctionButton.GetComponent<Button>().interactable = false;
                    SubFunctionButton.gameObject.SetActive(false);
                }
            }
        }
        
        if(Type == "Others")
        {
            ContractListTitleTextObject.GetComponent<Text>().text = "Mode";
            ContractListDropdownObject.GetComponent<Dropdown>().interactable = true;
            List<string> ContractOptions = new List<string>();
            ContractOptions.Add("Buy");
            ContractOptions.Add("Contract");
            ContractListDropdownObject.GetComponent<Dropdown>().ClearOptions();
            ContractListDropdownObject.GetComponent<Dropdown>().AddOptions(ContractOptions);
            
            QuantityTitleTextObject.SetActive(true);
            QuantityInputObject.SetActive(true);
            QuantityInputObject.GetComponent<InputField>().interactable = true;
            PriceInputObject.GetComponent<InputField>().interactable = false;
            PriceInputObject.GetComponent<InputField>().text = TargetItemSalesInfo.Price.ToString();

            if(CallSalesValue.CheckContractExist(TargetItemSalesInfo, PlayerCompanyName))
            {
                SalesValue.ContractInfo TargetContractInfo = CallSalesValue.GetContractInfo(TargetItemSalesInfo, PlayerCompanyName);
                ContractListDropdownObject.GetComponent<Dropdown>().value = 1;
                QuantityInputObject.GetComponent<InputField>().text = TargetContractInfo.Quantity.ToString();
                TermTitleTextObject.SetActive(true);
                TermInputObject.SetActive(true);
                TermInputObject.GetComponent<InputField>().interactable = true;
                TermInputObject.GetComponent<InputField>().text = TargetContractInfo.Term.ToString();

                MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Modify";
                MainFunctionButton.GetComponent<Button>().interactable = false;
                SubFunctionButton.gameObject.SetActive(true);
                SubFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Cancel";
            }
            else
            {
                ContractListDropdownObject.GetComponent<Dropdown>().value = 0;
                QuantityInputObject.GetComponent<InputField>().text = "";
                TermTitleTextObject.SetActive(false);
                TermInputObject.SetActive(false);

                MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Buy";
                MainFunctionButton.GetComponent<Button>().interactable = false;
                SubFunctionButton.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateRemainQuantityText()
    {
        if(CurrentItem != "")
        {
            if(CurrentCategory == "Storage") RemainQuantityTextObject.GetComponent<Text>().text = CallGoodsValue.GetStoredGoods(CurrentItem).Count.ToString();
            else RemainQuantityTextObject.GetComponent<Text>().text = CallSalesValue.GetSalesInfo(CurrentItem).ItemCount.ToString();
        }

        for(int i = 0; i < ItemCarrier.transform.childCount; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                Text TargetRemainText = ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
                string TargetName = ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text;
                if(TargetName == "") TargetRemainText.text = "";
                else
                {
                    if(CurrentCategory == "Storage") TargetRemainText.text = CallGoodsValue.GetStoredGoods(TargetName).Count.ToString() + " ";
                    else TargetRemainText.text = CallSalesValue.GetSalesInfo(TargetName).ItemCount.ToString() + " ";
                }
            }
        }
    }

    public void UpdateSalesInfo()
    {
        UpdateRemainQuantityText();

        ExpectedQualityTextObject.GetComponent<Text>().text = (Mathf.CeilToInt(TargetItemSalesInfo.QualityEvaluation * 10) * 0.1f).ToString();
        MarketShareTextObject.GetComponent<Text>().text = (Mathf.RoundToInt(CallSalesValue.CalculateMarketShare(CurrentItem) * 10f) * 0.1f).ToString() + " %";
        SoldAmountTextObject.GetComponent<Text>().text = TargetItemSalesInfo.SoldCount.ToString();
        CSPointTextObject.GetComponent<Text>().text = "TEST";
    }

    public void UpdateList(bool UpdateItemList)
    {
        if(UpdateItemList)
        {
            if(CurrentCategory == "Storage") DisplayStorageList();
            else DisplayItemList(CurrentCategory);
        }
        DisplayCategoryList();
    }

    public void ClearInfoPanel()
    {
        TargetItemSalesInfo = null;

        ImageObject.GetComponent<Image>().sprite = null;
        ImageObject.GetComponent<Image>().color = new Color(0,0,0,0);
        NameTextObject.GetComponent<Text>().text = "";
        TypeTextObject.GetComponent<Text>().text = "";
        CompanyTextObject.GetComponent<Text>().text = "";

        RemainQuantityTextObject.GetComponent<Text>().text = "";

        MaterialPointTextObject.GetComponent<Text>().text = " x ";
        TechPointTextObject.GetComponent<Text>().text = " x ";
        LookPointTextObject.GetComponent<Text>().text = " x ";
        TotalPointTextObject.GetComponent<Text>().text = " x ";

        ExpectedQualityTextObject.GetComponent<Text>().text = "";
        MarketShareTextObject.GetComponent<Text>().text = "";
        SoldAmountTextObject.GetComponent<Text>().text = "";
        CSPointTextObject.GetComponent<Text>().text = "";

        ContractListDropdownObject.GetComponent<Dropdown>().interactable = false;
        ContractListDropdownObject.GetComponent<Dropdown>().ClearOptions();
        TermInputObject.GetComponent<InputField>().interactable = false;
        TermInputObject.GetComponent<InputField>().text = "";
        PriceInputObject.GetComponent<InputField>().interactable = false;
        PriceInputObject.GetComponent<InputField>().text = "";
        QuantityInputObject.GetComponent<InputField>().interactable = false;
        QuantityInputObject.GetComponent<InputField>().text = "";

        MainFunctionButton.GetComponent<Button>().interactable = false;

        CurrentItem = "";
    }

    public void DropdownValueChange()
    {
        GoodsRecipe.Recipe TargetItemRecipe = CallGoodsRecipe.GetRecipe(CurrentItem);

        int DropdownValue = ContractListDropdownObject.GetComponent<Dropdown>().value;

        if(CurrentCategory == "Storage")
        {
            if(DropdownValue == 0)
            {
                TermTitleTextObject.SetActive(false);
                TermInputObject.SetActive(false);
                QuantityTitleTextObject.SetActive(false);
                QuantityInputObject.SetActive(false);

                if(TargetItemSalesInfo != null)
                {
                    PriceInputObject.GetComponent<InputField>().text = TargetItemSalesInfo.Price.ToString();
                    PriceInputObject.GetComponent<InputField>().interactable = true;

                    MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Modify";
                    SubFunctionButton.SetActive(true);
                    SubFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Delete";
                }
                else
                {
                    PriceInputObject.GetComponent<InputField>().text = "";

                    MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Enroll";
                    SubFunctionButton.SetActive(false);
                }
            }
            else
            {
                TermTitleTextObject.SetActive(true);
                TermInputObject.SetActive(true);
                TermInputObject.GetComponent<InputField>().text = TargetItemSalesInfo.ContractList[DropdownValue - 1].Term.ToString();
                PriceInputObject.GetComponent<InputField>().interactable = false;
                PriceInputObject.GetComponent<InputField>().text = TargetItemSalesInfo.Price.ToString();
                QuantityTitleTextObject.SetActive(true);
                QuantityInputObject.SetActive(true);
                QuantityInputObject.GetComponent<InputField>().text = TargetItemSalesInfo.ContractList[DropdownValue - 1].Quantity.ToString();

                MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Modify";
                SubFunctionButton.SetActive(true);
                SubFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Cancel";
            }
        }
        else
        {
            if(DropdownValue == 0)
            {
                TermTitleTextObject.SetActive(false);
                TermInputObject.SetActive(false);
                QuantityTitleTextObject.SetActive(true);
                QuantityInputObject.SetActive(true);
                QuantityInputObject.GetComponent<InputField>().text = "";

                MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Buy";
                SubFunctionButton.SetActive(false);
            }
            else
            {
                TermTitleTextObject.SetActive(true);
                TermInputObject.SetActive(true);
                QuantityTitleTextObject.SetActive(true);
                QuantityInputObject.SetActive(true);

                if(CallSalesValue.CheckContractExist(TargetItemSalesInfo, PlayerCompanyName))
                {
                    SalesValue.ContractInfo TargetContractInfo = CallSalesValue.GetContractInfo(TargetItemSalesInfo, PlayerCompanyName);

                    TermInputObject.GetComponent<InputField>().text = TargetContractInfo.Term.ToString();
                    QuantityInputObject.GetComponent<InputField>().text = TargetContractInfo.Quantity.ToString();

                    MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Modify";
                    SubFunctionButton.SetActive(true);
                    SubFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Cancel";
                }
                else
                {
                    TermInputObject.GetComponent<InputField>().interactable = true;
                    TermInputObject.GetComponent<InputField>().text = "0";
                    QuantityInputObject.GetComponent<InputField>().text = "0";

                    MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Contract";
                    SubFunctionButton.SetActive(false);
                }
            }
        }
    }

    public void MainFunctionButtonSelect()
    {
        int DropdownValue = ContractListDropdownObject.GetComponent<Dropdown>().value;
        string MainFunctionButtonText = MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text;

        if(CurrentCategory == "Storage")
        {
            if(DropdownValue == 0)
            {
                if(MainFunctionButtonText == "Enroll")
                {
                    CallSalesValue.AddSales(CurrentItem, PlayerCompanyName, int.Parse(PriceInputObject.GetComponent<InputField>().text));
                    TargetItemSalesInfo = CallSalesValue.GetSalesInfo(CurrentItem);

                    DisplayInfo(CurrentItem);
                }
                else if(MainFunctionButtonText == "Modify")
                {
                    CallSalesValue.ModifySales(CurrentItem, int.Parse(PriceInputObject.GetComponent<InputField>().text));
                }
            }
            else
            {
                CallSalesValue.ModifyContract(CurrentItem, DropdownValue - 1, int.Parse(TermInputObject.GetComponent<InputField>().text), int.Parse(QuantityInputObject.GetComponent<InputField>().text));
            }
        }
        else
        {
            if(DropdownValue == 0)
            {
                List<float> ResultList = new List<float>();
                int InputValue = int.Parse(QuantityInputObject.GetComponent<InputField>().text);

                ResultList.AddRange(CallSalesValue.BuyItem(CurrentItem, PlayerCompanyName, InputValue));

                if(ResultList[0] <= 0)
                {
                    switch(ResultList[0])
                    {
                        case -2f :
                            CallNotificationManager.AddAlert("Out of Stock", 1, "");
                            break;
                        case -1f :
                            break;
                        case 0 :
                            CallNotificationManager.AddAlert("Lack of Cash", 1, "");
                            break;
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
                        CallNotificationManager.AddAlert("Lack of Cash.\nOnly " + LackCount + " items delivered to you", 1, "");
                    }
                    else
                    {
                        if(ResultList.Count < InputValue)
                        {
                            CallNotificationManager.AddAlert("Out of stock.\nOnly " + ResultList.Count.ToString() + " items delivered to you", 1, "");
                        }
                    }

                    QuantityInputObject.GetComponent<InputField>().text = "";
                }
            }
            else
            {
                if(MainFunctionButtonText == "Contract")
                {
                    CallSalesValue.AddContract(CurrentItem, PlayerCompanyName, int.Parse(TermInputObject.GetComponent<InputField>().text), int.Parse(QuantityInputObject.GetComponent<InputField>().text));
                }
                else if(MainFunctionButtonText == "Modify")
                {
                    CallSalesValue.ModifyContract(CurrentItem, PlayerCompanyName, int.Parse(TermInputObject.GetComponent<InputField>().text), int.Parse(QuantityInputObject.GetComponent<InputField>().text));
                }
            }
        }

        UpdateList(false);
    }

    public void SubFunctionButtonSelect()
    {
        int DropdownValue = ContractListDropdownObject.GetComponent<Dropdown>().value;
        string SubFunctionButtonText = SubFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text;

        if(CurrentCategory == "Storage")
        {
            if(DropdownValue == 0)
            {
                CallSalesValue.DeleteSales(CurrentItem);
            }
            else
            {
                CallSalesValue.DeleteContract(CurrentItem, DropdownValue - 1);
            }
        }
        else
        {
            CallSalesValue.DeleteContract(CurrentItem, PlayerCompanyName);
        }

        UpdateList(true);
    }

    public void InputFieldValueChange()
    {
        int DropdownValue = ContractListDropdownObject.GetComponent<Dropdown>().value;
        string TermInputFieldText = TermInputObject.GetComponent<InputField>().text;
        string PriceInputFieldText = PriceInputObject.GetComponent<InputField>().text;
        string QuantityInputFieldText = QuantityInputObject.GetComponent<InputField>().text;

        if(CurrentCategory == "Storage")
        {
            if(DropdownValue == 0)
            {
                if(PriceInputFieldText != "")
                {
                    if(int.Parse(PriceInputFieldText) > 0)
                    {
                        MainFunctionButton.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        MainFunctionButton.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    MainFunctionButton.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                if(TermInputFieldText != "" && QuantityInputFieldText != "")
                {
                    if(int.Parse(TermInputFieldText) > 0 && int.Parse(QuantityInputFieldText) > 0)
                    {
                        MainFunctionButton.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        MainFunctionButton.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    MainFunctionButton.GetComponent<Button>().interactable = false;
                }
            }
        }
        else
        {
            if(DropdownValue == 0)
            {
                if(QuantityInputFieldText != "")
                {
                    if(int.Parse(QuantityInputFieldText) > 0)
                    {
                        MainFunctionButton.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        MainFunctionButton.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    MainFunctionButton.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                if(TermInputFieldText != "" && QuantityInputFieldText != "")
                {
                    if(int.Parse(TermInputFieldText) > 0 && int.Parse(QuantityInputFieldText) > 0)
                    {
                        MainFunctionButton.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        MainFunctionButton.GetComponent<Button>().interactable = false;
                    }
                }
                else
                {
                    MainFunctionButton.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void CategorySelect(GameObject SelectedItem)
    {
        string SelectedCategoryName = SelectedItem.transform.GetChild(0).gameObject.GetComponent<Text>().text.Substring(1);
        
        if(CurrentCategory != SelectedCategoryName)
        {
            ClearInfoPanel();
            if(SelectedCategoryName == "Storage")
            {
                DisplayStorageList();
            }
            else
            {
                DisplayItemList(SelectedCategoryName);
            }
        }
    }

    public void ItemSelect(GameObject SelectedItem)
    {
        string SelectedItemName = SelectedItem.transform.GetChild(1).gameObject.GetComponent<Text>().text;

        if(CurrentItem != SelectedItemName)
        {
            DisplayInfo(SelectedItemName);
        }
    }

    public void ClosePanel()
    {
        ClearInfoPanel();
    }
}
