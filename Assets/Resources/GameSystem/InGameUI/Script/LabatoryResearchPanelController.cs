using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabatoryResearchPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    public GameObject TechTreePrefab;
    [SerializeField]GameObject FunctionPanel;
    [SerializeField]GameObject BasicInfoPanel;
    [SerializeField]GameObject ProgressInfoPanel;
    [SerializeField]GameObject MainFunctionPanel;
    [SerializeField]GameObject ConfirmPanel;
    [SerializeField]GameObject ListPanel;
    [SerializeField]GameObject TechTreeScrollPanel;
    [SerializeField]GameObject TechTreeScrollCarrier;
    string CurrentResearchType = "";
    string CurrnetResearchName = "";
    GameObject TargetObject;
    LabatoryAct CallLabatoryAct;
    TechValue CallTechValue;
    TechRecipe CallTechRecipe;
    float[] TechTreeSizeRatio = new float[2];

    // Start is called before the first frame update
    void Awake()
    {
        
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

    void GetTargetObject()
    {

    }

    public void Initializing()
    {
        GameObject TechTreeObject = GameObject.Instantiate(TechTreePrefab, TechTreeScrollCarrier.transform);

        TechTreeSizeRatio[0] = TechTreeObject.GetComponent<RectTransform>().sizeDelta.x;
        TechTreeSizeRatio[1] = TechTreeObject.GetComponent<RectTransform>().sizeDelta.y;
        TechTreeObject.transform.SetSiblingIndex(1);

        TechTreeSizing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayResearchInfo()
    {

    }

    void RefreshProgressInfo()
    {

    }

    void ClearInfoPanel()
    {

    }

    public void MainFunctionButtonSelect(string SelectedType)
    {

    }

    public void ConfirmButtonSelect()
    {

    }

    public void ClosePanel()
    {
        ClearInfoPanel();
    }
}
