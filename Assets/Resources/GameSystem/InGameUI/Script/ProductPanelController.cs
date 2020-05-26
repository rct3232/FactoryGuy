using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    public GameObject SearchPanel;
    public GameObject FunctionPanel;
    public GameObject BasicInfoPanel;
    public GameObject StatInfoPanel;
    public GameObject RequirementProductInfoPanel;
    public GameObject RequirementProcessorInfoPanel;
    public GameObject ListPanel;
    public GameObject CategoryListPanel;
    public GameObject CategoryCarrier;
    public GameObject ItemListPanel;
    public GameObject ItemCarrier;
    string CurrentCategory = "";
    string CurrentItem = "";
    string PlayerCompanyName;
    GoodsRecipe CallGoodsRecipe;
    TechValue CallTechValue;
    GameObject ImageObject ,NameTextObject ,TypeTextObject ,CompanyTextObject ,CostTextObject ,MaterialPointTextObject ,TechPointTextObject ,LookPointTextObject ,PerfectionPointTextObject
    , TotalPointTextObject ,PackagedValueImage ,RequireProcessorPanel ,RequireProcessorImage ,RequireProcessorNameText ,RequireProcessorTypeText;
    // Start is called before the first frame update
    void Awake()
    {
        CallGoodsRecipe = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        CallTechValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetTechValue().GetComponent<TechValue>();
        PlayerCompanyName = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().PlayerCompanyName;

        ImageObject = BasicInfoPanel.transform.GetChild(1).GetChild(0).gameObject;
        NameTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
        TypeTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        CompanyTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(2).GetChild(1).gameObject;
        CostTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(3).GetChild(1).gameObject;
        MaterialPointTextObject = StatInfoPanel.transform.GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject;
        TechPointTextObject = StatInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(1).gameObject;
        LookPointTextObject = StatInfoPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(1).gameObject;
        PerfectionPointTextObject = StatInfoPanel.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).gameObject;
        TotalPointTextObject = StatInfoPanel.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(1).gameObject;
        PackagedValueImage = StatInfoPanel.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(0).gameObject;
        RequireProcessorPanel = RequirementProcessorInfoPanel.transform.GetChild(1).gameObject;
        RequireProcessorImage = RequirementProcessorInfoPanel.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        RequireProcessorNameText = RequirementProcessorInfoPanel.transform.GetChild(1).GetChild(2).GetChild(0).gameObject;
        RequireProcessorTypeText = RequirementProcessorInfoPanel.transform.GetChild(1).GetChild(2).GetChild(1).gameObject;
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

        for(int i = 0; i < StatInfoPanel.transform.childCount; i++)
        {
            StatInfoPanel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize * 2f);
            if(i % (StatInfoPanel.transform.childCount - 1) == 0)
            {
                StatInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
            }
            else
            {
                StatInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.72f, 0);
                for(int j = 0; j < StatInfoPanel.transform.GetChild(i).childCount; j++)
                {
                    StatInfoPanel.transform.GetChild(i).GetChild(j).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
                    StatInfoPanel.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionTitlePanelSize);
                    StatInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionValuePanelSize);
                    StatInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionValuePanelSize, 0);
                    StatInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.72f - FunctionValuePanelSize, 0);
                }
            }
        }

        for(int i = 0; i < RequirementProductInfoPanel.transform.childCount; i++)
        {
            RequirementProductInfoPanel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize * 2f);
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

        for(int i = 0; i < RequirementProcessorInfoPanel.transform.childCount; i++)
        {
            RequirementProcessorInfoPanel.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize * 2f);
            if(i % (RequirementProcessorInfoPanel.transform.childCount - 1) == 0)
            {
                RequirementProcessorInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
            }
            else
            {
                RequirementProcessorInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 11.2f, 0);
                RequirementProcessorInfoPanel.transform.GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionPanelOneLineSize * 2f, 0);
                RequirementProcessorInfoPanel.transform.GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
                RequirementProcessorInfoPanel.transform.GetChild(i).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 9.3f, 0);
                RequirementProcessorInfoPanel.transform.GetChild(i).GetChild(2).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
                RequirementProcessorInfoPanel.transform.GetChild(i).GetChild(2).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, FunctionPanelOneLineSize);
            }
        }

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

    public void Initializing()
    {
        ClearInfoPanel();
        DisplayCategoryList();
        DisplayItemList("All");
    }

    void DisplayCategoryList()
    {
        List<string> CategoryList = new List<string>();

        foreach(var Recipe in CallTechValue.AvailableRecipe)
        {
            bool isDuplicate = false;
            foreach(var Item in CategoryList)
            {
                if(Recipe.Recipe.GoodsObject.name == Item)
                {
                    isDuplicate = true;
                    break;
                }
            }

            if(!isDuplicate)
            {
                CategoryList.Add(Recipe.Recipe.GoodsObject.name);
            }
        }

        if(CategoryCarrier.transform.childCount > 1)
        {
            for(int i = 1; i < CategoryCarrier.transform.childCount; i++)
            {
                Destroy(CategoryCarrier.transform.GetChild(i).gameObject);
            }    
        }

        for(int i = 0; i < CategoryList.Count; i++)
        {
            GameObject newItem = GameObject.Instantiate(CategoryCarrier.transform.GetChild(0).gameObject, CategoryCarrier.transform);

            newItem.transform.GetChild(0).gameObject.GetComponent<Text>().text = " " + CategoryList[i];
        }
    }

    void DisplayItemList(string Category)
    {
        ClearInfoPanel();

        List<TechValue.RecipeInfo> ItemList = new List<TechValue.RecipeInfo>();
        int RowLimit = 0;

        for(int i = ItemCarrier.transform.childCount - 1; i > 0; i--)
        {
            Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }

        if(Category == "All")
        {
            foreach(var Item in CallTechValue.AvailableRecipe)
            {
                ItemList.Add(Item);
            }
        }
        else
        {
            foreach(var Item in CallTechValue.AvailableRecipe)
            {
                if(Item.Recipe.GoodsObject.name == Category)
                {
                    ItemList.Add(Item);
                }
            }
        }

        RowLimit = Mathf.CeilToInt((float)ItemList.Count / 3f);

        if(ItemList.Count == 0)
        {
            for(int i = 0; i < 3; i++)
            {
                ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
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

                int LeftItem = ItemList.Count - (i * 3);
                if(LeftItem < 3)
                {
                    for(int j = 2; j >= LeftItem; j--)
                    {
                        ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.SetActive(false);
                    }
                }

                for(int j = 0; j < 3; j++)
                {
                    if(i * 3 + j >= ItemList.Count)
                    {
                        break;
                    }
                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite 
                        = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + ItemList[i * 3 + j].Recipe.GoodsObject.name);
                    ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = ItemList[i * 3 + j].Recipe.OutputName;
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
            ClearInfoPanel();
            DisplayItemList(SelectedCategoryName);
        }
    }

    public void ItemSelect(GameObject SelectedItem)
    {
        string SelectedItemName = SelectedItem.transform.GetChild(1).gameObject.GetComponent<Text>().text;

        if(CurrentItem != SelectedItemName)
        {
            ClearInfoPanel();
            DisplayInfo(SelectedItemName);
        }
    }

    void DisplayInfo(string Name)
    {
        TechValue.RecipeInfo TargetItemRecipe = CallTechValue.GetRecipe(Name);

        ImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetItemRecipe.Recipe.GoodsObject.name);
        ImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
        NameTextObject.GetComponent<Text>().text = TargetItemRecipe.Recipe.OutputName;
        TypeTextObject.GetComponent<Text>().text = TargetItemRecipe.Recipe.GoodsObject.name;
        CompanyTextObject.GetComponent<Text>().text = TargetItemRecipe.Owner;
        CostTextObject.GetComponent<Text>().text = "TEST";

        MaterialPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.MaterialPoint * 10) * 0.1).ToString();
        TechPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.TechPoint * 10) * 0.1).ToString();
        LookPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.LookPoint * 10) * 0.1).ToString();
        PerfectionPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.PerfectionPoint * 10) * 0.1).ToString();
        TotalPointTextObject.GetComponent<Text>().text = "x " + (Mathf.RoundToInt(TargetItemRecipe.Recipe.Attractiveness.TotalPoint * 10) * 0.1).ToString();
        if(TargetItemRecipe.Recipe.Attractiveness.isPackaged) PackagedValueImage.GetComponent<Image>().color = new Color(0,1f,0,1f);
        else PackagedValueImage.GetComponent<Image>().color = new Color(1f,0,0,1f);
        
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

        if(TargetItemRecipe.Recipe.RequiredProcessor == null)
        {
            RequireProcessorPanel.SetActive(false);
        }
        else
        {
            string ProcessorType = TargetItemRecipe.Recipe.RequiredProcessor.Split('?')[0];
            string ProcessorName = TargetItemRecipe.Recipe.RequiredProcessor.Split('?')[1];

            RequireProcessorPanel.SetActive(true);
            RequireProcessorImage.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
            RequireProcessorNameText.GetComponent<Text>().text = ProcessorName;
            RequireProcessorTypeText.GetComponent<Text>().text = ProcessorType;
        }
    }

    public void RequirementProductSelect(GameObject Target)
    {
        string SelectedItemName = Target.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text;

        DisplayInfo(SelectedItemName);
    }

    void ClearInfoPanel()
    {
        ImageObject.GetComponent<Image>().sprite = null;
        ImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,0);
        NameTextObject.GetComponent<Text>().text = "";
        TypeTextObject.GetComponent<Text>().text = "";
        CompanyTextObject.GetComponent<Text>().text = "";
        CostTextObject.GetComponent<Text>().text = "";
        MaterialPointTextObject.GetComponent<Text>().text = "";
        TechPointTextObject.GetComponent<Text>().text = "";
        LookPointTextObject.GetComponent<Text>().text = "";
        PerfectionPointTextObject.GetComponent<Text>().text = "";
        TotalPointTextObject.GetComponent<Text>().text = "";
        PackagedValueImage.GetComponent<Image>().color = new Color(1f,1f,1f,1f);

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

        RequireProcessorPanel.SetActive(true);
        RequireProcessorImage.GetComponent<Image>().color = new Color(1f,1f,1f,0);
        RequireProcessorNameText.GetComponent<Text>().text = "";
        RequireProcessorTypeText.GetComponent<Text>().text = "";
    }

    public void ClosePanel()
    {
        ClearInfoPanel();

        for(int i = ItemCarrier.transform.childCount - 1; i > 0; i--)
        {
            Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < 3; i++)
        {
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        if(CategoryCarrier.transform.childCount > 1)
        {
            for(int i = 1; i < CategoryCarrier.transform.childCount; i++)
            {
                Destroy(CategoryCarrier.transform.GetChild(i).gameObject);
            }    
        }

        CurrentCategory = "";
        CurrentItem = "";
    }
}
