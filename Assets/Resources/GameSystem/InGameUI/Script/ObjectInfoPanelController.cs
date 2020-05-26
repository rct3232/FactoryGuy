using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    FloatingPanelController CallFloatingPanelController;
    public GameObject MainPanel;
    public GameObject InfoPanelCarrier;
    public GameObject AdditionalInfoPanel;
    public GameObject FunctionPanel;
    public GameObject ButtonCarrier;
    public GameObject TargetObject;
    TimeManager CallTimeManager;
    public Vector2 PanelPosition;
    public TechRecipe.FacilityInfo TargetInfo;

    void Awake()
    {
        CallFloatingPanelController = transform.parent.gameObject.GetComponent<FloatingPanelController>();
        CallTimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
    }

    void Update()
    {
        if(CallTimeManager.TimeValue % CallTimeManager.Hour < CallTimeManager.PlaySpeed)
        {
            DisplayInfo();
        }
    }
    
    public void Scaling()
    {
        MainPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 3.8f);
        FunctionPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 1.2f);

        InfoPanelCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.2f, 0);
        InfoPanelCarrier.GetComponent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentEdgePadding, 0);

        for(int i = 0; i < InfoPanelCarrier.transform.childCount; i++)
        {
            if(i == 0 || i == InfoPanelCarrier.transform.childCount - 1)
            {
                InfoPanelCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
            }
            else
            {
                InfoPanelCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.75f);
                InfoPanelCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
                InfoPanelCarrier.transform.GetChild(i).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.45f);
            }
        }
        InfoPanelCarrier.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding * 1.5f, CallPanelController.CurrentEdgePadding * 1.5f);
        InfoPanelCarrier.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - CallPanelController.CurrentEdgePadding);

        AdditionalInfoPanel.GetComponentInParent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3.8f, CallPanelController.CurrentUIsize * 3.8f);
        AdditionalInfoPanel.GetComponentInParent<RectTransform>().anchoredPosition = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        AdditionalInfoPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, CallPanelController.CurrentUIsize * 3f);
        AdditionalInfoPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - CallPanelController.CurrentEdgePadding * 0.5f);
        AdditionalInfoPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, CallPanelController.CurrentUIsize * 0.4f);
        AdditionalInfoPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - (CallPanelController.CurrentEdgePadding * 0.5f + CallPanelController.CurrentUIsize * 3f));

        ButtonCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 7.2f, CallPanelController.CurrentUIsize * 0.8f);

        CallPanelController.FontScaling(gameObject);
    }

    void SetPanelInitialPosition()
    {
        PanelPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        CallFloatingPanelController.Initializing(PanelPosition, true);
    }

    public void Initializing(GameObject SelectedObject)
    {
        TargetObject = SelectedObject;

        TargetInfo = TargetObject.GetComponent<InstallableObjectAct>().Info;
        
        ClearInfoPanel();
        DisplayInfo();

        CallFloatingPanelController.CallPanelController = CallPanelController;
        SetPanelInitialPosition();
    }

    public void DisplayInfo()
    {
        InstallableObjectAct TargetBasicInfo = TargetObject.GetComponent<InstallableObjectAct>();
        FacilityValue.FacilityInfo TargetValue = TargetBasicInfo.Value;
        CompanyValue CallCompnayValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
        GoodsRecipe CallGoodsRecipe = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();

        InfoPanelCarrier.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = TargetInfo.Type;
        InfoPanelCarrier.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Text>().text = TargetInfo.Name;
        InfoPanelCarrier.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>().text = "Consume";
        InfoPanelCarrier.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Text>().text = "Electric : " + TargetValue.SuppliedElectricity.ToString() + " / Labor : " + TargetValue.SuppliedLabor.ToString();
        InfoPanelCarrier.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = "Performance";

        switch(TargetInfo.Type)
        {
            case "Door" :
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = (TargetBasicInfo.WorkSpeed).ToString() + " item/sec";

                if(TargetObject.GetComponent<DoorAct>().TargetGoodsName != "None")
                {
                    AdditionalInfoPanel.SetActive(true);
                    AdditionalInfoPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + CallGoodsRecipe.GetRecipe(TargetObject.GetComponent<DoorAct>().TargetGoodsName).GoodsObject.name);
                    AdditionalInfoPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = TargetObject.GetComponent<DoorAct>().TargetGoodsName;
                }

                if(TargetObject.GetComponent<DoorAct>().DoorMode == "Ejector")
                {
                    FunctionPanel.SetActive(true);
                    ButtonCarrier.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Select Item";
                }
                break;
            case "Processor" :
                InfoPanelCarrier.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Text>().text += " " + TargetObject.GetComponent<ProcessorAct>().ProcessorActorName;
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = (TargetObject.GetComponent<ProcessorAct>().WorkTime + TargetBasicInfo.WorkSpeed).ToString() + " item/sec";
                InfoPanelCarrier.transform.GetChild(4).GetChild(0).gameObject.GetComponent<Text>().text = "Current Item";
                if(TargetObject.GetComponent<ProcessorAct>().TargetGoodsRecipe != null)
                {
                    AdditionalInfoPanel.SetActive(true);
                    AdditionalInfoPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetObject.GetComponent<ProcessorAct>().TargetGoodsRecipe.GoodsObject.name);
                    AdditionalInfoPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = TargetObject.GetComponent<ProcessorAct>().TargetGoodsRecipe.OutputName;
                }

                FunctionPanel.SetActive(true);
                ButtonCarrier.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Select Item";
                break;
            case "Belt" :
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = (TargetObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<BeltAct>().BeltSpeed + TargetBasicInfo.WorkSpeed).ToString() + " item/sec";
                break;
            case "Distributor" :
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = (TargetBasicInfo.WorkSpeed).ToString() + " item/sec";
                break;
            case "QualityControlUnit" :
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = (TargetBasicInfo.WorkSpeed).ToString() + " item/sec";

                FunctionPanel.SetActive(true);
                ButtonCarrier.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Set Standard";
                break;
            case "Destroyer" :
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = (TargetBasicInfo.WorkSpeed).ToString() + " item/sec";
                break;
            case "Labatory" :
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = (TargetBasicInfo.WorkSpeed).ToString() + " point/sec";

                FunctionPanel.SetActive(true);
                if(TargetObject.GetComponent<LabatoryAct>().CurrentResearchingTech == null && TargetObject.GetComponent<LabatoryAct>().CurrentDevelopingProduct == null)
                {
                    ButtonCarrier.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    ButtonCarrier.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Research Technology";
                    ButtonCarrier.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Develop Product";
                }
                else
                {
                    if(TargetObject.GetComponent<LabatoryAct>().CurrentResearchingTech != null)
                    {
                        AdditionalInfoPanel.SetActive(true);
                        AdditionalInfoPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Researching";

                        ButtonCarrier.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Research Technology";
                    }
                    else if(TargetObject.GetComponent<LabatoryAct>().CurrentDevelopingProduct != null)
                    {
                        AdditionalInfoPanel.SetActive(true);
                        AdditionalInfoPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/Goods/Sprite/" + TargetObject.GetComponent<LabatoryAct>().CurrentDevelopingProduct.ObjectInfo.Type);
                        AdditionalInfoPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = TargetObject.GetComponent<LabatoryAct>().CurrentDevelopingProduct.Name;

                        ButtonCarrier.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "Develop Product";
                    }
                }
                break;
            case "DayRoom" :
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = "x " + (TargetObject.GetComponent<DayRoomAct>().CurrentPerformance + TargetBasicInfo.WorkSpeed).ToString();
                break;
            case "EnergyStorage" :
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = TargetObject.GetComponent<EnergyStorageAct>().CurrentChargingAmount.ToString() + " point/tic";
                break;
            case "EnergySupplier" :
                ElectricityValue ElectricityValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetElectricityValue().GetComponent<ElectricityValue>();
                InfoPanelCarrier.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Text>().text = "Usage : " + ElectricityValueCall.TotalUsage.ToString() + " / Capacity : " + ElectricityValueCall.ElectricityInputValue.ToString();
                break;
        }
    }

    public void ButtonSelect(GameObject SelectedObject)
    {
        string ButtonText = SelectedObject.transform.GetChild(0).gameObject.GetComponent<Text>().text;

        switch(TargetInfo.Type)
        {
            case "Door" :
                CallPanelController.DisplaySidePanel("GoodsCreatorPanel");
                break;
            case "Processor" :
                CallPanelController.DisplaySidePanel("ProcessorPanel");
                break;
            case "QualityControlUnit" :
                break;
            case "Labatory" :
                if(ButtonText == "Research Technology") CallPanelController.DisplaySidePanel("LabatoryResearchPanel");
                else if(ButtonText == "Develop Product") CallPanelController.DisplaySidePanel("LabatoryDevelopPanel");
                break;
        }
    }

    void ClearInfoPanel()
    {
        FunctionPanel.SetActive(false);
        ButtonCarrier.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        
        for(int i = 1; i < InfoPanelCarrier.transform.childCount - 1; i++)
        {
            InfoPanelCarrier.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = "";
            InfoPanelCarrier.transform.GetChild(i).GetChild(1).gameObject.GetComponent<Text>().text = "";
        }

        AdditionalInfoPanel.SetActive(false);
    }

    public void SelectCloseButton()
    {
        CallPanelController.CloseCurrentFloatingPanel();
    }

    public void ClosePanel()
    {
        CallFloatingPanelController.ClosePanel();

        ClearInfoPanel();
    }
}
