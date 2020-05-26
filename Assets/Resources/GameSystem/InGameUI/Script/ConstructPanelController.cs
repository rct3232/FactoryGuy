using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    public GameObject SearchPanel;
    public GameObject CategoryPanel;
    public GameObject ListPanel;
    public GameObject ItemCarrier;
    public GameObject BulldozeImageObject;
    string CurrentCategory = "";
    InGameValue CallValue;
    TechValue CallTechValue;
    ObjInstantiater CallObjInstantiater;
    
    // Start is called before the first frame update
    void Awake()
    {
        CallValue = GameObject.Find("BaseSystem").GetComponent<InGameValue>();
        CallTechValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetTechValue().GetComponent<TechValue>();
        CallObjInstantiater = GameObject.Find("ObjectInstaller").GetComponent<ObjInstantiater>();
    }

    void Start()
    {

    }

    public void Scaling()
    {
        SearchPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8, CallPanelController.CurrentUIsize);
        CategoryPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 4, CallPanelController.CurrentUIsize);
        ListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 4, Screen.height - (CallPanelController.CurrentUIsize * 2));

        SearchPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
        SearchPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7, CallPanelController.CurrentUIsize);
        for(int i = 0; i < CategoryPanel.transform.childCount; i++)
        {
            CategoryPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
        }
        ListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding,0);
        ListPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(CallPanelController.CurrentEdgePadding,0);
        ListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(- CallPanelController.CurrentEdgePadding,0);
        ListPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);

        float ItemSize = (CallPanelController.CurrentUIsize * 4) - (CallPanelController.CurrentEdgePadding * 2);
        ItemCarrier.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemSize, ItemSize);
        ItemCarrier.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemSize, ItemSize - (CallPanelController.CurrentUIsize / 2));
        ItemCarrier.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemSize, CallPanelController.CurrentUIsize / 2);

        CallPanelController.FontScaling(gameObject);
    }

    public void Initializing()
    {
        
    }

    public void CategorySelect(string Type)
    {
        if(CallValue.ModeBit[1]) BulldozerButtonSelect();
        
        List<string> ObjectList = new List<string>();

        switch(Type)
        {
            case "Logitics" : 
                if(CurrentCategory == "Logitics")
                {
                    break;
                }
                foreach(var Object in CallTechValue.FacilityList)
                {
                    if(Object.Type == "Storage" || Object.Type == "Belt" || Object.Type == "Divider")
                    {
                        ObjectList.Add(Object.Name);
                    }
                }
                DisplayList(ObjectList, Type);
                break;
            case "Processor" : 
                if(CurrentCategory == "Processor")
                {
                    break;
                }
                foreach(var Object in CallTechValue.FacilityList)
                {
                    if(Object.Type == "Processor" || Object.Type == "Destroyer" || Object.Type == "QualityChecker")
                    {
                        ObjectList.Add(Object.Name);
                    }
                }
                DisplayList(ObjectList, Type);
                break;
            case "Facility" : 
                if(CurrentCategory == "Facility")
                {
                    break;
                }
                foreach(var Object in CallTechValue.FacilityList)
                {
                    if(Object.Type == "Labatory" || Object.Type == "EnergyStorage" || Object.Type == "EnergySupplier" || Object.Type == "DayRoom")
                    {
                        ObjectList.Add(Object.Name);
                    }
                }
                DisplayList(ObjectList, Type);
                break;
        }
    }

    public void ObjectSelect(GameObject SelectedItem)
    {
        if(CallValue.ModeBit[1]) BulldozerButtonSelect();

        CallObjInstantiater.InstantiateNewObject(SelectedItem.transform.GetChild(1).gameObject.GetComponent<Text>().text);
    }

    public void SearchSelect(string Detail)
    {

    }

    void GetSearchResult(string Detail)
    {

    }

    void DisplayList(List<string> ObjectList, string Type)
    {
        for(int i = ItemCarrier.transform.childCount - 1; i > 0; i--)
        {
            Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }

        if(ObjectList.Count == 0)
        {
            ItemCarrier.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }

        CurrentCategory = Type;

        for(int i = 0; i < ObjectList.Count; i++)
        {
            GameObject NewItem;
            if(i == 0)
            {
                NewItem = ItemCarrier.transform.GetChild(0).gameObject;
                NewItem.SetActive(true);
            }
            else
            {
                NewItem = GameObject.Instantiate(ItemCarrier.transform.GetChild(0).gameObject, ItemCarrier.transform);
            }
            Sprite ItemImage = Resources.Load<Sprite>("GameSystem/InstallableObject/Sprite/"+ObjectList[i]);
            if(ItemImage != null)
            {
                NewItem.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ItemImage;
            }
            else
            {
                NewItem.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ItemImage; // Put NoImagePreview
            }
            NewItem.transform.GetChild(1).gameObject.GetComponent<Text>().text = ObjectList[i];
        }
    }

    public void BulldozerButtonSelect()
    {
        CallValue.ModeSelector(1);

        if(CallValue.ModeBit[1]) BulldozeImageObject.SetActive(true);
        else BulldozeImageObject.SetActive(false);
    }

    public void ClosePanel()
    {
        CurrentCategory = "";

        for(int i = 1; i < ItemCarrier.transform.childCount; i++)
        {
            Destroy(ItemCarrier.transform.GetChild(i).gameObject);
        }
        ItemCarrier.transform.GetChild(0).gameObject.SetActive(false);

        if(CallValue.ModeBit[1]) BulldozerButtonSelect();
    }
}
