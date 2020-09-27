using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabatoryResearchPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    public GameObject TechTreePrefab;
    public GameObject FunctionPanel;
    public GameObject BasicInfoPanel;
    public GameObject ProgressInfoPanel;
    public GameObject MainFunctionPanel;
    public GameObject ConfirmPanel;
    public GameObject ListPanel;
    public GameObject TechTreeScrollPanel;
    public GameObject TechTreeScrollCarrier;
    GameObject TitleImageObject, NameTextObject, ResultObjectFirstRowPanel, ResultObjectSecondRowPanel, ResultValuePanel,
    ProgressBarImageObject, ProgressPercentageTextObject, PassedTimeTextObject, RemainTimeTextObject, CompletedPointTextObject, GainingPointTextObject, RemainPointTextObject,
    ConfirmButtonObject, ConfirmButtonTextObject;
    string CurrnetResearchName = "";
    public GameObject TargetObject;
    TimeManager CallTimeManager;
    TechValue CallTechValue;
    TechRecipe CallTechRecipe;
    LabatoryAct CallTargetLabatoryAct;
    float[] TechTreeSizeRatio = new float[2];

    // Start is called before the first frame update
    void Awake()
    {
        CallTimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CallTechValue = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetTechValue().GetComponent<TechValue>();
        CallTechRecipe = GameObject.Find("BaseSystem").GetComponent<TechRecipe>();

        TitleImageObject = BasicInfoPanel.transform.GetChild(1).GetChild(0).gameObject;
        NameTextObject = BasicInfoPanel.transform.GetChild(3).GetChild(0).GetChild(1).gameObject;
        ResultObjectFirstRowPanel = BasicInfoPanel.transform.GetChild(3).GetChild(1).GetChild(1).gameObject;
        ResultObjectSecondRowPanel = BasicInfoPanel.transform.GetChild(3).GetChild(2).GetChild(1).gameObject;
        ResultValuePanel = ResultObjectFirstRowPanel = BasicInfoPanel.transform.GetChild(3).GetChild(3).GetChild(1).gameObject;

        ProgressBarImageObject = ProgressInfoPanel.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        ProgressPercentageTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        PassedTimeTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject;
        RemainTimeTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
        CompletedPointTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(0).gameObject;
        GainingPointTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(1).gameObject;
        RemainPointTextObject = ProgressInfoPanel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetChild(2).gameObject;

        ConfirmButtonObject = ConfirmPanel.transform.GetChild(1).gameObject;
        ConfirmButtonTextObject = ConfirmButtonObject.transform.GetChild(0).gameObject;
    }

    public void Scaling()
    {
        FunctionPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, CallPanelController.CurrentUIsize * 6.1f);
        Vector2 FunctionPanelSize = FunctionPanel.GetComponent<RectTransform>().sizeDelta;
        ListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 12, Screen.height - CallPanelController.CurrentUIsize - FunctionPanelSize.y);
        Vector2 ListPanelSize = ListPanel.GetComponent<RectTransform>().sizeDelta;

        for(int i = 0; i < FunctionPanel.transform.childCount; i++)
        {
            if(i % 2 == 0)
            {
                FunctionPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
            }
        }

        BasicInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 3f);
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
                        BasicInfoPanel.transform.GetChild(i).GetChild(j).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.75f);
                        BasicInfoPanel.transform.GetChild(i).GetChild(j).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.3f);
                        BasicInfoPanel.transform.GetChild(i).GetChild(j).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.45f);
                    }
                }
            }
        }

        ProgressInfoPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.9f);
        for(int i = 0; i < ProgressInfoPanel.transform.childCount; i++) if(i % 2 == 0) ProgressInfoPanel.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        ProgressInfoPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 11.2f, 0);
        ProgressInfoPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 0.6f, CallPanelController.CurrentUIsize * 0.6f);
        ProgressInfoPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 0.3f, CallPanelController.CurrentUIsize * 0.3f);

        ConfirmPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.6f);
        ConfirmPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 8.6f, 0);
        ConfirmPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize * 3f, 0);
        ConfirmPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);

        TechTreeScrollPanel.GetComponent<RectTransform>().sizeDelta = ListPanelSize;
        TechTreeScrollPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(ListPanelSize.x - (CallPanelController.CurrentEdgePadding * 2), CallPanelController.CurrentEdgePadding);
        TechTreeScrollPanel.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(CallPanelController.CurrentEdgePadding, 0, 0);
        TechTreeScrollPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding, 0);
        TechTreeScrollPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
        TechTreeScrollPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, CallPanelController.CurrentEdgePadding);
        TechTreeScrollPanel.transform.GetChild(2).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);
        TechTreeScrollPanel.transform.GetChild(3).gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
        TechTreeScrollPanel.transform.GetChild(3).gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);

        for(int i = 0; i < TechTreeScrollCarrier.transform.childCount; i++)
        {
            if(i == 0 || i == TechTreeScrollCarrier.transform.childCount - 1) TechTreeScrollCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentEdgePadding);
        }
        if(TechTreeScrollCarrier.transform.childCount > 2) TechTreeSizing();    

        CallPanelController.FontScaling(gameObject);
    }

    void TechTreeSizing()
    {
        Vector2 TechTreeSize = TechTreeScrollCarrier.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta;
        TechTreeScrollCarrier.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = TechTreeSize * TopValue.TopValueSingleton.UIScale;
    }

    void TechTreeInitializing(bool OnlyStateRefresh)
    {
        GameObject TechTreeObject = null;
        if(!OnlyStateRefresh)
        {
            TechTreeObject = GameObject.Instantiate(TechTreePrefab, TechTreeScrollCarrier.transform);

            TechTreeSizeRatio[0] = TechTreeObject.GetComponent<RectTransform>().sizeDelta.x;
            TechTreeSizeRatio[1] = TechTreeObject.GetComponent<RectTransform>().sizeDelta.y;
            TechTreeObject.transform.SetSiblingIndex(1);

            TechTreeSizing();
        }
        else
        {
            TechTreeObject = TechTreeScrollCarrier.transform.GetChild(1).gameObject;
        }

        int TechIndex = 0;
        for(int i = 0; i < TechTreeObject.transform.childCount; i++)
        {
            if(i % 2 == 1) continue;

            for(int j = 0; j < TechTreeObject.transform.GetChild(i).childCount; j++)
            {
                if(j % 2 == 1) continue;

                GameObject Target = TechTreeObject.transform.GetChild(i).GetChild(j).gameObject;

                if(!Target.GetComponent<Button>().IsActive()) continue;

                if(!OnlyStateRefresh)
                {
                    // Target.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/TechTreeTitleImage_" + CurrnetResearchName);
                    Target.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);

                    // Add Result Object code here

                    Target.transform.GetChild(2).gameObject.GetComponent<Text>().text = CallTechRecipe.TechInfoList[TechIndex].Name;
                    Target.name = CallTechRecipe.TechInfoList[TechIndex].Name;

                    Target.GetComponent<Button>().onClick.AddListener(()=>TechTreeButtonSelect(Target));
                }

                Target.GetComponent<Button>().interactable = CallTechValue.GetTechPossible(CallTechRecipe.TechInfoList[TechIndex].Name);

                TechIndex++;
            }
        }
    }

    void GetTargetObject()
    {
        TargetObject = CallPanelController.CurrentFloatingPanel.GetComponent<ObjectInfoPanelController>().TargetObject;
        CallTargetLabatoryAct = TargetObject.GetComponent<LabatoryAct>();
    }

    public void Initializing()
    {
        TechTreeInitializing(false);
        GetTargetObject();

        if(CallTargetLabatoryAct.CurrentResearchingTech != null)
        {
            CurrnetResearchName = CallTargetLabatoryAct.CurrentResearchingTech;
        }

        DisplayResearchInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TechTreeButtonSelect(GameObject Target)
    {
        CurrnetResearchName = Target.transform.GetChild(2).gameObject.GetComponent<Text>().text;

        DisplayResearchInfo();
    }

    void DisplayResearchInfo()
    {
        ClearInfoPanel();

        if(CurrnetResearchName != "")
        {
            // TitleImageObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("GameSystem/InGameUI/Sprite/TechTreeTitleImage_" + CurrnetResearchName);
            TitleImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
            NameTextObject.GetComponent<Text>().text = CurrnetResearchName;
            ConfirmButtonObject.GetComponent<Button>().interactable = true;

            if(CurrnetResearchName == CallTargetLabatoryAct.CurrentResearchingTech)
            {
                // ProgressBarImageObject.GetComponent<Image>().fillAmount = 

                ConfirmButtonTextObject.GetComponent<Text>().text = "Cancel";
                ConfirmButtonObject.GetComponent<Image>().color = new Color(1f,0.4f,0.4f,1f);
            }
            else
            {
                if(CallTargetLabatoryAct.CurrentResearchingTech != null)
                {
                    ConfirmButtonTextObject.GetComponent<Text>().text = "Change";
                    ConfirmButtonObject.GetComponent<Image>().color = new Color(1f,0.5f,0.2f,1f);
                }
                else
                {
                    if(CallTechValue.GetTechPossible(CurrnetResearchName))
                    {
                        ConfirmButtonTextObject.GetComponent<Text>().text = "Start";
                    }
                    else
                    {
                        ConfirmButtonTextObject.GetComponent<Text>().text = "Completed";
                        ConfirmButtonObject.GetComponent<Button>().interactable = false;
                    }
                    ConfirmButtonObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                }
            }

            UpdateProgressInfo();
        }
    }

    public void ConfirmButtonSelect()
    {
        string ButtonText = ConfirmButtonTextObject.GetComponent<Text>().text;
        if(ButtonText == "Start")
        {
            CallTargetLabatoryAct.StartResearch(CurrnetResearchName);
        }
        else if(ButtonText == "Change")
        {
            CallTargetLabatoryAct.StopResearch();
            CallTargetLabatoryAct.StartResearch(CurrnetResearchName);
        }
        else if(ButtonText == "Cancel")
        {
            CallTargetLabatoryAct.StopResearch();
        }

        DisplayResearchInfo();
    }

    public void UpdateProgressInfo()
    {
        TechValue.ResearchState TargetResearchState = CallTechValue.GetResearchState(CurrnetResearchName);
        if(TargetResearchState == null)
        {
            ProgressPanelClear();
            return;
        }

        float CompletePercentage = Mathf.CeilToInt(TargetResearchState.GainedWorkLoad / (float)TargetResearchState.TargetState.Info.RequiredWorkLoad * 100f) * 0.01f;
        float CurrentGainingPoint = 0f;

        foreach(var Labatory in TargetResearchState.LabatoryList)
        {
            LabatoryAct TargetLabatoryAct = Labatory.GetComponent<LabatoryAct>();

            CurrentGainingPoint += TargetLabatoryAct.ResearchPower;
        }

        ProgressBarImageObject.GetComponent<Image>().fillAmount = CompletePercentage;
        if(CompletePercentage >= 1)
        {
            UpdateCompleteState();
        }
        else
        {
            ProgressPercentageTextObject.GetComponent<Text>().text = (CompletePercentage * 100).ToString() + " %";
        }

        PassedTimeTextObject.GetComponent<Text>().text = CallTimeManager.GetPeriodString(CallTimeManager.TimeValue - TargetResearchState.StartTime, "Short");
        RemainTimeTextObject.GetComponent<Text>().text = " / " +
            CallTimeManager.GetPeriodString((CallTimeManager.TimeValue - TargetResearchState.StartTime) + (Mathf.CeilToInt(((float)TargetResearchState.TargetState.Info.RequiredWorkLoad - TargetResearchState.GainedWorkLoad) / CurrentGainingPoint) * CallTimeManager.Hour), "Short");

        CompletedPointTextObject.GetComponent<Text>().text = (Mathf.CeilToInt(TargetResearchState.GainedWorkLoad)).ToString();
        GainingPointTextObject.GetComponent<Text>().text = "(+"+(Mathf.CeilToInt(CurrentGainingPoint)).ToString() + ")";
        RemainPointTextObject.GetComponent<Text>().text = " / " + TargetResearchState.TargetState.Info.RequiredWorkLoad.ToString();
    }

    void UpdateCompleteState()
    {
        ConfirmButtonTextObject.GetComponent<Text>().text = "Completed";
        ConfirmButtonObject.GetComponent<Button>().interactable = false;
        
        ConfirmButtonObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);

        TechTreeInitializing(true);
    }

    void ClearInfoPanel()
    {
        TitleImageObject.GetComponent<Image>().sprite = null;
        TitleImageObject.GetComponent<Image>().color = new Color(1f,1f,1f,0f);
        NameTextObject.GetComponent<Text>().text = "";
        ProgressBarImageObject.GetComponent<Image>().fillAmount = 0f;
        
        ProgressPanelClear();

        ConfirmButtonTextObject.GetComponent<Text>().text = "Start";
        ConfirmButtonObject.GetComponent<Button>().interactable = false;
    }

    void ProgressPanelClear()
    {
        ProgressPercentageTextObject.GetComponent<Text>().text = "";
        PassedTimeTextObject.GetComponent<Text>().text = "00:00";
        RemainTimeTextObject.GetComponent<Text>().text = " / 00:00";
        GainingPointTextObject.GetComponent<Text>().text = "0";
        GainingPointTextObject.GetComponent<Text>().text = "(+0)";
        RemainPointTextObject.GetComponent<Text>().text = " / 0";
    }

    public void ClosePanel()
    {
        ClearInfoPanel();
    }
}
