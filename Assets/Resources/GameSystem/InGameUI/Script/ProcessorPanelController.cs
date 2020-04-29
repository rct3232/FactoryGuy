using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessorPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    [SerializeField]GameObject SearchPanel;
    [SerializeField]GameObject FunctionPanel;
    [SerializeField]GameObject BasicInfoPanel;
    [SerializeField]GameObject StatInfoPanel;
    [SerializeField]GameObject RequirementProductInfoPanel;
    [SerializeField]GameObject MainFunctionButtonPanel;
    [SerializeField]GameObject ListPanel;
    [SerializeField]GameObject CategoryListPanel;
    [SerializeField]GameObject CategoryCarrier;
    [SerializeField]GameObject ItemListPanel;
    [SerializeField]GameObject ItemCarrier;
    string CurrentCategory = "";
    string CurrentItem = "";
    public GameObject TargetObject;
    string PlayerCompanyName;
    GoodsRecipe CallGoodsRecipe;
    TechRecipe CallTechRecipe;
    TechValue CallTechValue;
    ProcessorAct CallTargetProcessorAct;
    InstallableObjectAct CallInstallableObjectAct;
    GameObject ImageObject ,NameTextObject ,TypeTextObject ,CompanyTextObject ,ExpectQualityTextObject ,MaterialPointTextObject ,TechPointTextObject ,LookPointTextObject ,PerfectionPointTextObject
    , TotalPointTextObject ,PackagedValueImage ,ChangeCostPanel ,MainFunctionButton;

    void Awake()
    {
        CallGoodsRecipe = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        CallTechRecipe = GameObject.Find("BaseSystem").GetComponent<TechRecipe>();
        CallTechValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetTechValue().GetComponent<TechValue>();
        PlayerCompanyName = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().PlayerCompanyName;

        ImageObject = BasicInfoPanel.transform.GetChild(1).GetChild(0).gameObject;
        NameTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
        TypeTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        CompanyTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(2).GetChild(1).gameObject;
        ExpectQualityTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(3).GetChild(1).gameObject;
        MaterialPointTextObject = StatInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;
        TechPointTextObject = StatInfoPanel.transform.GetChild(2).GetChild(1).GetChild(1).gameObject;
        LookPointTextObject = StatInfoPanel.transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        TotalPointTextObject = StatInfoPanel.transform.GetChild(4).GetChild(1).GetChild(1).gameObject;
        PerfectionPointTextObject = StatInfoPanel.transform.GetChild(5).GetChild(1).GetChild(1).gameObject;
        PackagedValueImage = StatInfoPanel.transform.GetChild(6).GetChild(1).GetChild(1).gameObject;
        ChangeCostPanel = MainFunctionButtonPanel.transform.GetChild(1).gameObject;
        MainFunctionButton = MainFunctionButtonPanel.transform.GetChild(3).gameObject;
    }

    public void Scaling()
    {
        SearchPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, CallPanelController.CurrentUIsize);
        FunctionPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, CallPanelController.CurrentUIsize * 9.4f);
        Vector2 FunctionPanelSize = FunctionPanel.GetComponent<RectTransform>().sizeDelta;
        ListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, Screen.height - CallPanelController.CurrentUIsize - FunctionPanelSize.y);
        Vector2 ListPanelSize = ListPanel.GetComponent<RectTransform>().sizeDelta;

        SearchPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
        SearchPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7, CallPanelController.CurrentUIsize);

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
        
        BasicInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize * 4f);
        for(int i = 0; i < BasicInfoPanel.transform.childCount; i++)
        {
            if(i % 2 == 0)
            {
                BasicInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
            }
            else
            {
                if(i == 1)
                {
                    BasicInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
                }
                else
                {
                    BasicInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.8f, 0);
                    for(int j = 0; j < BasicInfoPanel.transform.GetChild(i).childCount; j++)
                    {
                        BasicInfoPanel.transform.GetChild(i).GetChild(j).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
                        BasicInfoPanel.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionTitlePanelSize);
                        BasicInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionValuePanelSize);
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

        RequirementProductInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize * 2f);
        for(int i = 0; i < RequirementProductInfoPanel.transform.childCount; i++)
        {
            
            if(i % (RequirementProductInfoPanel.transform.childCount - 1) == 0)
            {
                RequirementProductInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
            }
            else
            {
                RequirementProductInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 5.6f, 0);
                RequirementProductInfoPanel.transform.GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionPanelOneLineSize * 2f, 0);
                RequirementProductInfoPanel.transform.GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
                RequirementProductInfoPanel.transform.GetChild(i).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.7f, 0);
                RequirementProductInfoPanel.transform.GetChild(i).GetChild(2).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
                RequirementProductInfoPanel.transform.GetChild(i).GetChild(2).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
            }
        }

        MainFunctionButtonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
        for(int i = 0; i < MainFunctionButtonPanel.transform.childCount; i++)
        {
            if(i % 2 == 0) MainFunctionButtonPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        }
        MainFunctionButtonPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.8f, 0);
        MainFunctionButtonPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionTitlePanelSize);
        MainFunctionButtonPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionValuePanelSize);
        MainFunctionButtonPanel.transform.GetChild(3).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);

        CategoryListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, ListPanelSize.y);
        CategoryListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3, CallPanelController.CurrentEdgePadding);
        CategoryListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(20f, 0, 0);
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
        CallInstallableObjectAct = TargetObject.GetComponent<InstallableObjectAct>();
        CallTargetProcessorAct = TargetObject.GetComponent<ProcessorAct>();
    }

    public void Initializing()
    {
        GetTargetObject();

        ClearInfoPanel();
        CurrentCategory = "";
        CurrentItem = "";
        
        string TargetGoodsCurrentItem = "None";
        if(CallTargetProcessorAct.TargetGoodsRecipe != null)
        {
            TargetGoodsCurrentItem = CallTargetProcessorAct.TargetGoodsRecipe.OutputName;
            CurrentCategory = CallTargetProcessorAct.ProcessorActorName;
        }

        DisplayInfo(TargetGoodsCurrentItem);

        DisplayCategoryList();
        if(CallTargetProcessorAct.ProcessorActorName != "") DisplayItemList(CallTargetProcessorAct.ProcessorActorName);
        else ClearItemList();
    }

    void DisplayCategoryList()
    {
        List<string> CategoryList = new List<string>();
        TechRecipe.ProcessorInfo TargetProcessorInfo = CallTechRecipe.GetProcessorRecipe(CallInstallableObjectAct.Info.Name);

        foreach(var ActorName in TargetProcessorInfo.ActorList)
        {
            if(!CallTechValue.GetActorPossible(ActorName)) continue;

            bool isDuplicate = false;
            foreach(var Item in CategoryList)
            {
                if(ActorName == Item)
                {
                    isDuplicate = true;
                    break;
                }
            }

            if(!isDuplicate)
            {
                CategoryList.Add(ActorName);
            }
        }

        int RowLimit = CategoryList.Count;

        if(RowLimit > CategoryCarrier.transform.childCount)
        {
            for(int i = CategoryCarrier.transform.childCount; i < RowLimit; i++)
            {
                GameObject.Instantiate(CategoryCarrier.transform.GetChild(0).gameObject, CategoryCarrier.transform);
            }
        }
        else if(RowLimit < CategoryCarrier.transform.childCount)
        {
            if(RowLimit == 0) RowLimit = 1;
            for(int i = CategoryCarrier.transform.childCount - 1; i >= RowLimit; i--)
            {
                Destroy(CategoryCarrier.transform.GetChild(i).gameObject);
            }
        }

        for(int i = 0; i < RowLimit; i++)
        {
            CategoryCarrier.transform.GetChild(i).gameObject.SetActive(false);
        }

        for(int i = 0; i < CategoryList.Count; i++)
        {
            CategoryCarrier.transform.GetChild(i).gameObject.SetActive(true);
            CategoryCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = " " + CategoryList[i];
        }
    }

    void DisplayItemList(string Category)
    {
        TechRecipe.ProcessorInfo TargetProcessorInfo = CallTechRecipe.GetProcessorRecipe(CallInstallableObjectAct.Info.Name);
        List<TechValue.RecipeInfo> ItemList = new List<TechValue.RecipeInfo>();
        int RowLimit = 0;

        foreach(var Item in CallTechValue.AvailableRecipe)
        {
            if(Item.Recipe.RequiredProcessor != null)
            {
                string ProcessorType = Item.Recipe.RequiredProcessor.Split('?')[0];
                string ProcessorName = Item.Recipe.RequiredProcessor.Split('?')[1];

                if(ProcessorType == TargetProcessorInfo.Type && ProcessorName == Category)
                {
                    ItemList.Add(Item);
                }
            }
        }

        RowLimit = Mathf.CeilToInt(((float)ItemList.Count + 1) / 3f);

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
                if(i == 0 && j == 0)
                {
                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.SetActive(true);

                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/InsideEmptyCircle");
                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "None";
                }
                else
                {
                    if(i * 3 + j - 1 >= ItemList.Count) break;

                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.SetActive(true);

                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + ItemList[i * 3 + j - 1].Recipe.GoodsObject.name);
                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = ItemList[i * 3 + j - 1].Recipe.OutputName;
                }
            }
        }

        CurrentCategory = Category;
    }

    public void CategorySelect(GameObject SelectedItem)
    {
        string SelectedCategoryName = SelectedItem.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        SelectedCategoryName = SelectedCategoryName.Substring(1);

        if(CurrentCategory != SelectedCategoryName)
        {
            CurrentCategory = SelectedCategoryName;
            DisplayItemList(SelectedCategoryName);
        }
    }

    public void ItemSelect(GameObject SelectedItem)
    {
        string SelectedItemName = SelectedItem.transform.GetChild(1).gameObject.GetComponent<Text>().text;

        if(CurrentItem != SelectedItemName)
        {
            CurrentItem = SelectedItemName;
            DisplayInfo(SelectedItemName);
        }
    }

    void DisplayInfo(string Name)
    {
        if(Name == "None")
        {
            NameTextObject.GetComponent<Text>().text = "None";

            ChangeCostPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "$ 0";
            string ButtonName = "";
            if(CallTargetProcessorAct.TargetGoodsRecipe != null)
            {
                ButtonName = "Remove Process";
                MainFunctionButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                ButtonName = "Change";
                MainFunctionButton.GetComponent<Button>().interactable = false;
            }

            MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = ButtonName;
        }
        else
        {
            TechValue.RecipeInfo TargetItemRecipe = CallTechValue.GetRecipe(Name);
            TechRecipe.ProcessActorInfo TargetActorInfo = CallTechRecipe.GetProcessActorInfo(CurrentCategory);

            ImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetItemRecipe.Recipe.GoodsObject.name);
            ImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
            NameTextObject.GetComponent<Text>().text = TargetItemRecipe.Recipe.OutputName;
            TypeTextObject.GetComponent<Text>().text = TargetItemRecipe.Recipe.GoodsObject.name;
            CompanyTextObject.GetComponent<Text>().text = TargetItemRecipe.Owner;
            ExpectQualityTextObject.GetComponent<Text>().text = "TEST";

            MaterialPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.MaterialPoint * 10) * 0.1).ToString();
            TechPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.TechPoint * 10) * 0.1).ToString();
            LookPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.LookPoint * 10) * 0.1).ToString();
            PerfectionPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.PerfectionPoint * 10) * 0.1).ToString();
            TotalPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.TotalPoint * 10) * 0.1).ToString();
            if(TargetItemRecipe.Recipe.Attractiveness.isPackaged) PackagedValueImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/PossitiveMark");
            else PackagedValueImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/NegativeMark");
            
            for(int i = 1; i < RequirementProductInfoPanel.transform.childCount - 1; i++)
            {
                for(int j = 0; j < RequirementProductInfoPanel.transform.GetChild(i + 1).childCount; j++)
                {
                    RequirementProductInfoPanel.transform.GetChild(i + 1).GetChild(j).gameObject.SetActive(false);
                }
            }

            if(TargetItemRecipe.Recipe.InputName != null)
            {
                for(int i = 0; i < TargetItemRecipe.Recipe.InputName.Length; i++)
                {
                    for(int j = 0; j < RequirementProductInfoPanel.transform.GetChild(i + 1).childCount; j++)
                    {
                        RequirementProductInfoPanel.transform.GetChild(i + 1).GetChild(j).gameObject.SetActive(true);    
                    }
                    GoodsRecipe.Recipe InputItem = CallGoodsRecipe.GetRecipe(TargetItemRecipe.Recipe.InputName[i]);
                    RequirementProductInfoPanel.transform.GetChild(i + 1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                    RequirementProductInfoPanel.transform.GetChild(i + 1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + InputItem.GoodsObject.name);
                    RequirementProductInfoPanel.transform.GetChild(i + 1).GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = InputItem.OutputName;
                    RequirementProductInfoPanel.transform.GetChild(i + 1).GetChild(2).GetChild(1).gameObject.GetComponent<Text>().text = InputItem.GoodsObject.name;
                    if(TargetItemRecipe.Owner == PlayerCompanyName) RequirementProductInfoPanel.transform.GetChild(i + 1).gameObject.GetComponent<Button>().interactable = true;
                    else RequirementProductInfoPanel.transform.GetChild(i + 1).gameObject.GetComponent<Button>().interactable = false;
                }
            }

            string ButtonName = "";
            if(CallTargetProcessorAct.ProcessorActorName == CurrentCategory)
            {
                if(CallTargetProcessorAct.TargetGoodsRecipe != null)
                {
                    ButtonName = "Change Goods";
                    if(CallTargetProcessorAct.TargetGoodsRecipe.OutputName == CurrentItem) MainFunctionButton.GetComponent<Button>().interactable = false;
                    else MainFunctionButton.GetComponent<Button>().interactable = true;

                    ChangeCostPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "$ 0";
                }
            }
            else
            {
                MainFunctionButton.GetComponent<Button>().interactable = true;

                if(CallTargetProcessorAct.TargetGoodsRecipe != null)
                {
                    if(CallTargetProcessorAct.TargetGoodsRecipe.OutputName == CurrentItem) ButtonName = "Change Processor";
                    else ButtonName = "Change Processor & Goods";
                }
                else ButtonName = "Change Processor & Goods";

                ChangeCostPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "$ " + TargetActorInfo.Cost.ToString();
            }
            MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = ButtonName;
        }
    }

    public void MainFunctionButtonSelect()
    {
        if(MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text == "Change Goods")
        {
            TargetObject.GetComponent<ProcessorAct>().SetTargetGoods(CurrentItem);
        }
        else if(MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text == "Change Processor")
        {
            TargetObject.GetComponent<ProcessorAct>().ChangeProcessorActor(CurrentCategory);
        }
        else if(MainFunctionButton.transform.GetChild(0).gameObject.GetComponent<Text>().text == "Change Processor & Goods")
        {
            TargetObject.GetComponent<ProcessorAct>().ChangeProcessorActor(CurrentCategory);
            TargetObject.GetComponent<ProcessorAct>().SetTargetGoods(CurrentItem);
        }

        CallPanelController.CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().DisplayInfo();
    }

    void ClearInfoPanel()
    {
        ImageObject.GetComponent<Image>().sprite = null;
        ImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,0);
        NameTextObject.GetComponent<Text>().text = "";
        TypeTextObject.GetComponent<Text>().text = "";
        CompanyTextObject.GetComponent<Text>().text = "";
        ExpectQualityTextObject.GetComponent<Text>().text = "";
        MaterialPointTextObject.GetComponent<Text>().text = "";
        TechPointTextObject.GetComponent<Text>().text = "";
        LookPointTextObject.GetComponent<Text>().text = "";
        PerfectionPointTextObject.GetComponent<Text>().text = "";
        TotalPointTextObject.GetComponent<Text>().text = "";
        PackagedValueImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/QuestionMark");

        for(int i = 1; i < RequirementProductInfoPanel.transform.childCount - 1; i++)
        {
            for(int j = 0; j < RequirementProductInfoPanel.transform.GetChild(i + 1).childCount; j++)
            {
                RequirementProductInfoPanel.transform.GetChild(i + 1).GetChild(j).gameObject.SetActive(true);
            }
        }

        for(int i = 1; i < RequirementProductInfoPanel.transform.childCount - 1; i++)
        {
            RequirementProductInfoPanel.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f,1f,1f,0);
            RequirementProductInfoPanel.transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = null;
            RequirementProductInfoPanel.transform.GetChild(i).GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "";
            RequirementProductInfoPanel.transform.GetChild(i).GetChild(2).GetChild(1).gameObject.GetComponent<Text>().text = "";
            RequirementProductInfoPanel.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
        }

        ChangeCostPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "$ 0";
    }

    void ClearItemList()
    {
        for(int i = 0; i < ItemCarrier.transform.childCount; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void ClosePanel()
    {
        ClearInfoPanel();
    }
}
