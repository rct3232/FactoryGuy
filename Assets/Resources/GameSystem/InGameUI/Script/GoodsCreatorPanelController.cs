using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodsCreatorPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    [SerializeField]GameObject SearchPanel;
    [SerializeField]GameObject FunctionPanel;
    [SerializeField]GameObject ImageHolder;
    [SerializeField]GameObject InfoPanel;
    [SerializeField]GameObject FunctionButtonPanel;
    [SerializeField]GameObject ListPanel;
    [SerializeField]GameObject CategoryListPanel;
    [SerializeField]GameObject CategoryCarrier;
    [SerializeField]GameObject ItemListPanel;
    [SerializeField]GameObject ItemCarrier;
    public string CurrentCategory = "";
    public string CurrentItem = "";
    public GameObject TargetObject;
    string PlayerCompanyName;
    GoodsValue CallGoodsValue;
    GoodsRecipe CallGoodsRecipe;
    public GoodsCreater CallTargetGoodsCreator;
    GameObject ImageObject, RemainQuantityTextObject, NameTextObject, TypeTextObject, CompanyTextObject, MaterialPointTextObject, TechPointTextObject, LookPointTextObject, TotalPointTextObject, MainFunctionButton;
    
    void Awake()
    {
        CallGoodsValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        CallGoodsRecipe = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();

        ImageObject = ImageHolder.transform.GetChild(0).gameObject;
        RemainQuantityTextObject = ImageHolder.transform.GetChild(0).GetChild(0).gameObject;
        NameTextObject = InfoPanel.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        TypeTextObject = InfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;
        CompanyTextObject = InfoPanel.transform.GetChild(1).GetChild(2).GetChild(1).gameObject;
        MaterialPointTextObject = InfoPanel.transform.GetChild(2).GetChild(0).GetChild(1).GetChild(1).gameObject;
        TechPointTextObject = InfoPanel.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(1).gameObject;
        LookPointTextObject = InfoPanel.transform.GetChild(2).GetChild(2).GetChild(1).GetChild(1).gameObject;
        TotalPointTextObject = InfoPanel.transform.GetChild(2).GetChild(3).GetChild(1).GetChild(1).gameObject;
        MainFunctionButton = FunctionButtonPanel.transform.GetChild(0).gameObject;
        
    }

    public void Scaling()
    {
        SearchPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8, CallPanelController.CurrentUIsize);
        FunctionPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, CallPanelController.CurrentUIsize * 5);
        Vector2 FunctionPanelSize = FunctionPanel.GetComponent<RectTransform>().sizeDelta;
        ListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, Screen.height - CallPanelController.CurrentUIsize - FunctionPanelSize.y);
        Vector2 ListPanelSize = ListPanel.GetComponent<RectTransform>().sizeDelta;

        SearchPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
        SearchPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7, CallPanelController.CurrentUIsize);

        float FunctionPanelOneLineSize = CallPanelController.CurrentUIsize * 3;        
        ImageHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionPanelOneLineSize, FunctionPanelOneLineSize);
        ImageHolder.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding, 0);
        ImageHolder.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
        InfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(FunctionPanelSize.x - FunctionPanelOneLineSize - (CallPanelController.CurrentEdgePadding * 2), FunctionPanelOneLineSize);
        InfoPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CallPanelController.CurrentEdgePadding + FunctionPanelOneLineSize, - CallPanelController.CurrentEdgePadding);
        FunctionButtonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.9f);
        FunctionButtonPanel.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(- CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);

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

        InfoPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, FunctionPanelOneLineSize);
        InfoPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 5.4f, FunctionPanelOneLineSize);
        InfoPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 2.2f, FunctionPanelOneLineSize);
        
        FunctionButtonPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, CallPanelController.CurrentUIsize * 0.9f);

        for(int i = 1; i < 3; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                InfoPanel.transform.GetChild(i).GetChild(j).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.75f);
                InfoPanel.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
                InfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.45f);
                if(i == 2)
                {
                    InfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 0.45f, 0);
                    InfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((CallPanelController.CurrentUIsize * 2.2f) - (CallPanelController.CurrentUIsize * 0.45f), 0);
                }
            }
        }

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
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
            ItemCarrier.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemOneLineSize - CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
        }       

        CallPanelController.FontScaling(gameObject); 
    }

    void GetTargetObject()
    {
        TargetObject = CallPanelController.CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().TargetObject;

        CallTargetGoodsCreator = TargetObject.GetComponent<GoodsCreater>();
    }

    public void Initializing()
    {
        GetTargetObject();

        ClearInfoPanel();
        DisplayInfo(CallTargetGoodsCreator.TargetGoodsName);

        DisplayCategoryList();
        DisplayItemList("All");

        PlayerCompanyName = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().PlayerCompanyName;
    }

    void DisplayCategoryList()
    {
        List<string> CategoryList = new List<string>();

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

        if(CallTargetGoodsCreator.TargetGoodsName != "None")
        {
            if(CallGoodsValue.GetStoredGoods(CallTargetGoodsCreator.TargetGoodsName).Count == 0)
            {
                GoodsRecipe.Recipe TargetRecipe = CallGoodsRecipe.GetRecipe(CallTargetGoodsCreator.TargetGoodsName);
                bool isDuplicate = false;
                foreach(var Item in CategoryList)
                {
                    if(Item == TargetRecipe.GoodsObject.name)
                    {
                        isDuplicate = true;

                        break;
                    }
                }
                
                if(!isDuplicate) CategoryList.Add(TargetRecipe.GoodsObject.name);
            }
        }

        int PresetCategoryCount = 1;
        int RowLimit = CategoryList.Count + PresetCategoryCount;

        if(RowLimit > CategoryCarrier.transform.childCount)
        {
            for(int i = CategoryCarrier.transform.childCount; i < RowLimit; i++)
            {
                GameObject.Instantiate(CategoryCarrier.transform.GetChild(0).gameObject, CategoryCarrier.transform);
            }
        }
        else if(RowLimit < CategoryCarrier.transform.childCount)
        {
            for(int i = CategoryCarrier.transform.childCount - 1; i >= RowLimit; i--)
            {
                Destroy(CategoryCarrier.transform.GetChild(i).gameObject);
            }
        }

        for(int i = PresetCategoryCount; i < RowLimit; i++)
        {
            CategoryCarrier.transform.GetChild(i).gameObject.SetActive(false);
        }

        for(int i = PresetCategoryCount; i < RowLimit; i++)
        {
            CategoryCarrier.transform.GetChild(i).gameObject.SetActive(true);
            CategoryCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = " " + CategoryList[i - PresetCategoryCount];
        }
    }

    void DisplayItemList(string Category)
    {
        List<string> StoredList = CallGoodsValue.GetAllGoodsName(true);
        List<string> ItemList = new List<string>();
        int RowLimit = 0;

        for(int i = ItemCarrier.transform.childCount - 1; i > 0; i--)
        {
            Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }

        ItemList.Add("None");

        if(Category != "All")
        {
            foreach(var Item in StoredList)
            {
                if(CallGoodsRecipe.GetRecipe(Item).GoodsObject.name == Category)
                {
                    ItemList.Add(Item);
                }
            }

            if(CallTargetGoodsCreator.TargetGoodsName != "None")
            {
                if(CallGoodsRecipe.GetRecipe(CallTargetGoodsCreator.TargetGoodsName).GoodsObject.name == Category)
                {
                    if(CallGoodsValue.GetStoredGoods(CallTargetGoodsCreator.TargetGoodsName).Count == 0)
                    {
                        ItemList.Add(CallTargetGoodsCreator.TargetGoodsName);
                    }
                }
            }
        }
        else
        {
            ItemList.AddRange(StoredList);

            if(CallTargetGoodsCreator.TargetGoodsName != "None")
            {
                if(CallGoodsValue.GetStoredGoods(CallTargetGoodsCreator.TargetGoodsName).Count == 0)
                {
                    ItemList.Add(CallTargetGoodsCreator.TargetGoodsName);
                }
            }
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
                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
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
                else ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + CallGoodsRecipe.GetRecipe(ItemList[i * 3 + j]).GoodsObject.name);
                
                ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = ItemList[i * 3 + j];
                if(CallTargetGoodsCreator.TargetGoodsName == ItemList[i * 3 + j]) ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
        }

        CurrentCategory = Category;

        UpdateRemainQuantityText();
    }

    void DisplayInfo(string Name)
    {
        if(Name == "None")
        {
            NameTextObject.GetComponent<Text>().text = Name;
        }
        else
        {
            GoodsRecipe.Recipe TargetItemRecipe = CallGoodsRecipe.GetRecipe(Name);

            ImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetItemRecipe.GoodsObject.name);
            ImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
            NameTextObject.GetComponent<Text>().text = TargetItemRecipe.OutputName;
            TypeTextObject.GetComponent<Text>().text = TargetItemRecipe.GoodsObject.name;

            MaterialPointTextObject.GetComponent<Text>().text = " x " + (Mathf.RoundToInt(TargetItemRecipe.Attractiveness.MaterialPoint * 10) * 0.1).ToString();
            TechPointTextObject.GetComponent<Text>().text = " x " + (Mathf.RoundToInt(TargetItemRecipe.Attractiveness.TechPoint * 10) * 0.1).ToString();
            LookPointTextObject.GetComponent<Text>().text = " x " + (Mathf.RoundToInt(TargetItemRecipe.Attractiveness.LookPoint * 10) * 0.1).ToString();
            TotalPointTextObject.GetComponent<Text>().text = " x " + (Mathf.RoundToInt(TargetItemRecipe.Attractiveness.TotalPoint * 10) * 0.1).ToString();

            if(Name == CallTargetGoodsCreator.TargetGoodsName) MainFunctionButton.GetComponent<Button>().interactable = false;
            if(Name != CallTargetGoodsCreator.TargetGoodsName) MainFunctionButton.GetComponent<Button>().interactable = true;
        }

        CurrentItem = Name;

        UpdateRemainQuantityText();
    }

    public void MainFunctionButtonSelect()
    {
        CallTargetGoodsCreator.SetTargetGoods(CurrentItem);

        CallPanelController.CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().DisplayInfo();

        UpdateList(true);
    }

    public void ClearInfoPanel()
    {
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

        CurrentItem = "";
    }

    public void CategorySelect(GameObject SelectedItem)
    {
        string SelectedCategoryName = SelectedItem.transform.GetChild(0).gameObject.GetComponent<Text>().text.Substring(1);
        
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

    public void UpdateRemainQuantityText()
    {
        if(CurrentItem != "" && CurrentItem != "None") RemainQuantityTextObject.GetComponent<Text>().text = CallGoodsValue.GetStoredGoods(CurrentItem).Count.ToString() + " ";

        for(int i = 0; i < ItemCarrier.transform.childCount; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                Text TargetRemainText = ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
                string TargetName = ItemCarrier.transform.GetChild(i).GetChild(j).GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text;
                if(TargetName == "" || TargetName == "None") TargetRemainText.text = "";
                else TargetRemainText.text = CallGoodsValue.GetStoredGoods(TargetName).Count.ToString() + " ";
            }
        }
    }

    public void UpdateList(bool UpdateItemList)
    {
        if(UpdateItemList) DisplayItemList(CurrentCategory);
        DisplayCategoryList();
    }

    public void ClosePanel()
    {
        
    }
}
