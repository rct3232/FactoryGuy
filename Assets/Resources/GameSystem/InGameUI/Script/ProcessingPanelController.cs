using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessingPanelController : MonoBehaviour
{
    public PanelController CallPanelController;
    public bool isInitialized = false;
    public GameObject GraphScrollPanel;
    public GameObject GraphCarrier;
    public GameObject ElectricityGraphCarrier;
    public GameObject LaborForceGraphCarrier;
    public GameObject ProcessGraphCarrier;
    public GameObject ProcessGraphIndexCarrier;
    GameObject ElectricityGraphPanelCarrier, LaborForceGraphPanelCarrier, ProcessGraphPanelCarrier, ProcessGraphIndexPanel;
    TimeManager CallTimeManager;
    CompanyManager CallCompanyManager;
    CompanyValue CallCompanyValue;
    ElectricityValue CallElectricityValue;
    EmployeeValue CallEmployeeValue;
    GoodsValue CallGoodsValue;
    FacilityValue CallFacilityValue;
    int xValueMaximum;
    List<float>[] ElectricityStateList;
    List<float>[] LaborForceStateList;
    class ProcessingState
    {
        public ProcessingState(string title, Color color)
        {
            Title = title;
            GraphColor = color;

            Value = new List<float>();
            Activated = true;
        }
        public string Title;
        public Color GraphColor;
        public List<float> Value;
        public bool Activated;
    }
    List<ProcessingState> ProcessingStateList;
    class UpdateValue
    {
        public UpdateValue()
        {
            ResetValue();
        }
        public bool NeedUpdate;
        public bool CompleteUpdate;
        public void ResetValue()
        {
            NeedUpdate = false;
            CompleteUpdate = false;
        }
    }
    UpdateValue UpdateState = new UpdateValue();

    void Awake()
    {
        CallTimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CallCompanyManager = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CallCompanyValue = CallCompanyManager.GetPlayerCompanyValue();
        CallElectricityValue = CallCompanyValue.GetElectricityValue().GetComponent<ElectricityValue>();
        CallEmployeeValue = CallCompanyValue.GetEmployeeValue().GetComponent<EmployeeValue>();
        CallGoodsValue = CallCompanyValue.GetGoodsValue().GetComponent<GoodsValue>();
        CallFacilityValue = CallCompanyValue.GetFacilityValue().GetComponent<FacilityValue>();

        ElectricityGraphPanelCarrier = GraphCarrier.transform.GetChild(0).gameObject;
        LaborForceGraphPanelCarrier = GraphCarrier.transform.GetChild(2).gameObject;
        ProcessGraphPanelCarrier = GraphCarrier.transform.GetChild(4).gameObject;
        ProcessGraphIndexPanel = ProcessGraphIndexCarrier.transform.parent.gameObject;
    }

    void Start()
    {
        
    }

    public void Scaling()
    {
        float GraphPanelWidth = CallPanelController.CurrentUIsize * 11.2f;
        float GraphPanelHeight = CallPanelController.CurrentUIsize * 4f;
        float YaxisWidth = CallPanelController.CurrentUIsize * 0.5f;
        float XaxisHeight = CallPanelController.CurrentEdgePadding;

        GraphScrollPanel.GetComponent<RectTransform>().offsetMin = new Vector2(CallPanelController.CurrentEdgePadding, CallPanelController.CurrentEdgePadding);
        GraphScrollPanel.GetComponent<RectTransform>().offsetMax = new Vector2(- CallPanelController.CurrentEdgePadding, - CallPanelController.CurrentEdgePadding);

        GraphScrollPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(CallPanelController.CurrentEdgePadding, 0, 0);
        GraphScrollPanel.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentEdgePadding, 0);

        ProcessGraphCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(GraphPanelWidth, GraphPanelHeight + CallPanelController.CurrentUIsize + CallPanelController.CurrentUIsize * 0.5f);

        for(int i = 0; i < Mathf.CeilToInt(GraphCarrier.transform.childCount / 2f); i++)
        {
            int GraphPanelCarrierIndex = i * 2;

            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GraphPanelWidth, GraphPanelHeight + (CallPanelController.CurrentUIsize * 1.5f));

            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, GraphPanelHeight);

            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(CallPanelController.CurrentUIsize, 0);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GraphPanelWidth - CallPanelController.CurrentUIsize, CallPanelController.CurrentUIsize);

            
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GraphPanelWidth, GraphPanelHeight);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(YaxisWidth, 0, 0);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GraphPanelWidth - YaxisWidth, GraphPanelHeight - XaxisHeight);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(YaxisWidth, - GraphPanelHeight);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GraphPanelWidth - YaxisWidth, XaxisHeight);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(1).GetChild(0).GetChild(2).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(1).GetChild(0).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(YaxisWidth, GraphPanelHeight - XaxisHeight);
            
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(2).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.5f);
            GraphCarrier.transform.GetChild(GraphPanelCarrierIndex).GetChild(2).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.5f);
        }

        float xDistance = (GraphPanelWidth - YaxisWidth) / (CallPanelController.CurrentUIsize * 0.5f);
        xValueMaximum = Mathf.CeilToInt((GraphPanelWidth - YaxisWidth) / xDistance);

        CallPanelController.FontScaling(gameObject);
    }

    void Update()
    {
        if(CallTimeManager.TimeValue % CallTimeManager.Hour < CallTimeManager.PlaySpeed)
        {
            ClearAllGraph();

            GetElectricityStateValue();
            GetLaborForceStateValue();
            GetProcessingStateValue();

            DrawGraph();
        }
        
        if(UpdateState.CompleteUpdate)
        {
            for(int i = 0; i < ProcessGraphIndexPanel.transform.childCount; i++)
            {
                CallPanelController.ContentSizeFitterReseter(ProcessGraphIndexPanel.transform.GetChild(i).gameObject);
            }

            CallPanelController.ContentSizeFitterReseter(ProcessGraphIndexPanel);
            CallPanelController.ContentSizeFitterReseter(ProcessGraphPanelCarrier);

            Canvas.ForceUpdateCanvases();

            UpdateState.ResetValue();
        }
        if(UpdateState.NeedUpdate)
        {
            UpdateProcessingGraphIndex();
        }
    }

    public void Initializing()
    {
        ElectricityStateList = new List<float>[3];
        ElectricityStateList[0] = new List<float>();
        ElectricityStateList[1] = new List<float>();
        ElectricityStateList[2] = new List<float>();

        LaborForceStateList = new List<float>[5];
        LaborForceStateList[0] = new List<float>();
        LaborForceStateList[1] = new List<float>();
        LaborForceStateList[2] = new List<float>();
        LaborForceStateList[3] = new List<float>();
        LaborForceStateList[4] = new List<float>();

        ProcessingStateList = new List<ProcessingState>();
        ProcessingState newState;
        newState = new ProcessingState("Warehouse", new Color(1f,1f,0.2f,1f));
        ProcessingStateList.Add(newState);
        newState = new ProcessingState("Total", new Color(1f,1f,1f,1f));
        ProcessingStateList.Add(newState);
        newState = new ProcessingState("Zero", new Color(0,0,0,0));
        ProcessingStateList.Add(newState);
        foreach(var FacilityInfo in CallFacilityValue.FacilityList)
        {
            if(FacilityInfo.ObjectActCall.Info.Type == "Processor")
            {
                ProcessorAct TargetProcessorValue = FacilityInfo.Object.GetComponent<ProcessorAct>();
                if(TargetProcessorValue.TargetGoodsRecipe == null) continue;

                bool isDuplicate = false;

                foreach(var State in ProcessingStateList)
                {
                    if(State.Title == TargetProcessorValue.TargetGoodsRecipe.OutputName)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if(!isDuplicate)
                {
                    newState = new ProcessingState(TargetProcessorValue.TargetGoodsRecipe.OutputName, new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f),1f));

                    ProcessingStateList.Add(newState);
                }
            }
        }

        GetElectricityStateValue();
        GetLaborForceStateValue();
        GetProcessingStateValue();

        CreateProcessingGraphIndex();

        DrawGraph();
    }

    void GetElectricityStateValue()
    {
        ElectricityStateList[0].Add(CallElectricityValue.TotalUsage);
        ElectricityStateList[1].Add(CallElectricityValue.AvailableElectricityAmount);
        ElectricityStateList[2].Add(0);
        
        for(int i = 0; i < 3; i++)
        {
            if(ElectricityStateList[i].Count > xValueMaximum) ElectricityStateList[i].RemoveAt(0);
        }
    }

    void GetLaborForceStateValue()
    {
        LaborForceStateList[0].Add(CallEmployeeValue.RequiredLabor);
        LaborForceStateList[1].Add(CallEmployeeValue.ActivatedRequiredLabor);
        LaborForceStateList[2].Add(CallEmployeeValue.TotalLabor);
        LaborForceStateList[3].Add(CallEmployeeValue.ActivatedLabor);
        LaborForceStateList[4].Add(0);
        
        for(int i = 0; i < 5; i++)
        {
            if(LaborForceStateList[i].Count > xValueMaximum) LaborForceStateList[i].RemoveAt(0);
        }
    }

    void GetProcessingStateValue()
    {
        foreach(var State in ProcessingStateList) State.Value.Add(0);

        int CurrentIndex = ProcessingStateList[0].Value.Count - 1;

        ProcessingStateList[0].Value[CurrentIndex] += CallGoodsValue.GetStoredGoods().Count;

        int TotalWorkLoad = 0;
        foreach(var State in ProcessingStateList)
        {
            foreach(var FacilityInfo in CallFacilityValue.FacilityList)
            {
                if(FacilityInfo.ObjectActCall.Info.Type != "Processor") continue;
                
                ProcessorAct TargetProcessorValue = FacilityInfo.Object.GetComponent<ProcessorAct>();
                if(TargetProcessorValue.TargetGoodsRecipe != null)
                {
                    if(TargetProcessorValue.TargetGoodsRecipe.OutputName == State.Title)
                    {
                        State.Value[CurrentIndex] += TargetProcessorValue.WorkLoadPerDay;
                        TotalWorkLoad += TargetProcessorValue.WorkLoadPerDay;
                    }
                }
            }
        }

        ProcessingStateList[1].Value[CurrentIndex] += TotalWorkLoad;
        
        for(int i = 0; i < ProcessingStateList.Count; i++)
        {
            if(ProcessingStateList[i].Value.Count > xValueMaximum) ProcessingStateList[i].Value.RemoveAt(0);
        }
    }

    void CreateProcessingGraphIndex()
    {
        for(int i = 3; i < ProcessingStateList.Count; i++)
        {
            GameObject newIndex = GameObject.Instantiate(ProcessGraphIndexCarrier.transform.GetChild(0).gameObject, ProcessGraphIndexCarrier.transform);
            newIndex.name = ProcessingStateList[i].Title;

            newIndex.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = ProcessingStateList[i].GraphColor;
            newIndex.transform.GetChild(1).gameObject.GetComponent<Text>().text = ProcessingStateList[i].Title;

            CallPanelController.ContentSizeFitterReseter(newIndex);
            CallPanelController.ContentSizeFitterReseter(newIndex.transform.GetChild(1).gameObject);
        }

        UpdateState.NeedUpdate = true;
    }

    void UpdateProcessingGraphIndex()
    {
        List<int> SplitIndexList = new List<int>();

        float IndexLength = 0;
        for(int i = 0; i < ProcessGraphIndexCarrier.transform.childCount; i++)
        {
            IndexLength += ProcessGraphIndexCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta.x;

            if(IndexLength > CallPanelController.CurrentUIsize * 11.2f)
            {
                SplitIndexList.Add(i);
                IndexLength = ProcessGraphIndexCarrier.transform.GetChild(i).gameObject.GetComponent<RectTransform>().sizeDelta.x;
            }
        }

        for(int i = 0; i < SplitIndexList.Count; i++)
        {
            GameObject newIndexCarrier = GameObject.Instantiate(ProcessGraphIndexCarrier, ProcessGraphIndexPanel.transform);
            newIndexCarrier.GetComponent<RectTransform>().sizeDelta = new Vector2(0, CallPanelController.CurrentUIsize * 0.5f);
            newIndexCarrier.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, - CallPanelController.CurrentUIsize * 0.5f * (i + 1), 0);

            for(int j = newIndexCarrier.transform.childCount - 1; j >= 0; j--) Destroy(newIndexCarrier.transform.GetChild(j).gameObject);

            int NextStartIndex = ProcessGraphIndexCarrier.transform.childCount;
            if(i + 1 < SplitIndexList.Count) NextStartIndex = SplitIndexList[i + 1];

            for(int j = SplitIndexList[i]; j < NextStartIndex; j++)
            {
                GameObject CopiedIndex = GameObject.Instantiate(ProcessGraphIndexCarrier.transform.GetChild(j).gameObject, newIndexCarrier.transform);

                CallPanelController.ContentSizeFitterReseter(CopiedIndex);
                CallPanelController.ContentSizeFitterReseter(CopiedIndex.transform.GetChild(1).gameObject);
            }

            ProcessGraphIndexPanel.GetComponent<RectTransform>().sizeDelta += new Vector2(0, CallPanelController.CurrentUIsize * 0.5f);
            ProcessGraphPanelCarrier.GetComponent<RectTransform>().sizeDelta += new Vector2(0, CallPanelController.CurrentUIsize * 0.5f);
        }

        if(SplitIndexList.Count > 0)
        {
            for(int i = ProcessGraphIndexCarrier.transform.childCount - 1; i >= SplitIndexList[0]; i--) Destroy(ProcessGraphIndexCarrier.transform.GetChild(i).gameObject);
        }

        UpdateState.CompleteUpdate = true;
    }

    void DrawGraph()
    {
        List<string> DummyXaxisList = new List<string>();
        Color[] ColorList;

        ColorList = new Color[3];
        ColorList[0] = new Color(1f,0.4f,0.4f,1f);
        ColorList[1] = new Color(1f,1f,0.2f,1f);
        ColorList[2] = new Color(0,0,0,0);

        ElectricityGraphCarrier.GetComponent<GraphDrawer>().DrawProgressGraph(null, DummyXaxisList, ElectricityStateList, ColorList);

        ColorList = new Color[5];
        ColorList[0] = new Color(1f,0.5f,0,1f);
        ColorList[1] = new Color(1f,0.4f,0.4f,1f);
        ColorList[2] = new Color(0.2f,0.2f,1f,1f);
        ColorList[3] = new Color(0.2f,1f,0.2f,1f);
        ColorList[4] = new Color(0,0,0,0);

        LaborForceGraphCarrier.GetComponent<GraphDrawer>().DrawProgressGraph(null, DummyXaxisList, LaborForceStateList, ColorList);

        int Count = 0;
        foreach(var State in ProcessingStateList) if(State.Activated) Count++;

        if(Count <= 0) return;
        List<float>[] ProcessingStateValueList = new List<float>[Count];

        ColorList = new Color[Count];
        int Index = 0;
        for(int i = 0; i < ProcessingStateList.Count; i++)
        {
            if(ProcessingStateList[i].Activated)
            {
                ProcessingStateValueList[Index] = new List<float>();
                foreach(var Value in ProcessingStateList[i].Value) ProcessingStateValueList[Index].Add(Value);

                ColorList[Index] = ProcessingStateList[i].GraphColor;
                Index++;
            }
        }
        
        ProcessGraphCarrier.GetComponent<GraphDrawer>().DrawProgressGraph(null, DummyXaxisList, ProcessingStateValueList, ColorList);
    }

    public void ToggleIndex(GameObject Target)
    {
        string TargetName = Target.transform.parent.parent.gameObject.name;

        ProcessingState TargetState = null;
        foreach(var State in ProcessingStateList) if(State.Title == TargetName) TargetState = State;

        TargetState.Activated = !TargetState.Activated;

        Image TargetStateImage = Target.transform.parent.GetChild(1).gameObject.GetComponent<Image>();
        if(TargetState.Activated) TargetStateImage.color = new Color(1f,1f,1f,0);
        else TargetStateImage.color = new Color(1f,1f,1f,1f);

        ClearAllGraph();
        DrawGraph();
    }

    void ClearAllGraph()
    {
        ElectricityGraphCarrier.GetComponent<GraphDrawer>().LineGraphClear();
        LaborForceGraphCarrier.GetComponent<GraphDrawer>().LineGraphClear();
        ProcessGraphCarrier.GetComponent<GraphDrawer>().LineGraphClear();
    }

    void ClearProcessGraphIndex()
    {
        for(int i = ProcessGraphIndexPanel.transform.childCount - 1; i >= 1; i--)
        {
            Destroy(ProcessGraphIndexPanel.transform.GetChild(i).gameObject);

            ProcessGraphIndexPanel.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, CallPanelController.CurrentUIsize * 0.5f);
            ProcessGraphPanelCarrier.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, CallPanelController.CurrentUIsize * 0.5f);
        }

        for(int i = ProcessGraphIndexCarrier.transform.childCount - 1; i >= 2; i--)
        {
            Destroy(ProcessGraphIndexCarrier.transform.GetChild(i).gameObject);
        }
    }

    void ClearPanel()
    {
        ClearAllGraph();
        ClearProcessGraphIndex();
    }

    public void ClosePanel()
    {
        ClearPanel();

        ElectricityStateList = null;
        LaborForceStateList = null;
        ProcessingStateList = null;
    }
}
