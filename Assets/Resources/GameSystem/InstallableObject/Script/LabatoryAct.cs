using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabatoryAct : MonoBehaviour
{
    GameObject BaseSystem;
    InstallableObjectAct ObjectActCall;
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    TimeManager TimeManagerCall;
    PanelController PanelControllerCall;
    NotificationManager NotificationManagerCall;

    ///////////////////////////////////////////////////////////////////////////

    TechRecipe TechRecipeCall;
    TechValue TechValueCall;
    public string CurrentResearchingTech;

    ///////////////////////////////////////////////////////////////////////////

    GoodsRecipe GoodsRecipeCall;
    public class ResultObject
    {
        public ResultObject() {}
        public string Type;
        public List<string> Input = new List<string>();
        public string RequiredProcessor;
        public GoodsRecipe.Attractiveness Attractiveness;
        public float RequiredResearchPower;
        public int RequiredPoint;
    }
    public class DevelopingProduct
    {
        public DevelopingProduct() {}
        public string Name;
        public ResultObject ObjectInfo = new ResultObject();
        public float CompletedPoint;
        public int StartTime;

        public void CopyObjectInfo(ResultObject TargetInfo)
        {
            ObjectInfo.Type = TargetInfo.Type;
            ObjectInfo.Input = TargetInfo.Input;
            ObjectInfo.RequiredProcessor = TargetInfo.RequiredProcessor;
            ObjectInfo.Attractiveness = TargetInfo.Attractiveness;
            ObjectInfo.RequiredResearchPower = TargetInfo.RequiredResearchPower;
            ObjectInfo.RequiredPoint = TargetInfo.RequiredPoint;
        }
    }
    public DevelopingProduct CurrentDevelopingProduct;
    public ResultObject resultObject;

    ///////////////////////////////////////////////////////////////////////////

    public int Budget;
    public float ResearchPower;
    public float ResearchPowerLimit;
    
    // Start is called before the first frame update
    void Start()
    {
        BaseSystem = GameObject.Find("BaseSystem");
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CompanyValueCall = ObjectActCall.CompanyValueCall;
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        TechRecipeCall = BaseSystem.GetComponent<TechRecipe>();
        TechValueCall = CompanyManagerCall.GetCompanyValue(CompanyValueCall.CompanyName).GetTechValue().GetComponent<TechValue>();
        NotificationManagerCall = GameObject.Find("NotificationManager").GetComponent<NotificationManager>();
        GoodsRecipeCall = BaseSystem.GetComponent<GoodsRecipe>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();

        CurrentResearchingTech = null;

        resultObject = new ResultObject();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(CurrentResearchingTech != null)
        {
            ObjectActCall.IsWorking = true;

            if(TimeManagerCall.TimeValue % TimeManagerCall.Hour < TimeManagerCall.PlaySpeed)
            {
                if(CurrentResearchingTech != "ResearchPowerUpgrade")
                {
                    TechValueCall.ContributeResearchWork(CurrentResearchingTech, ResearchPower);
                }
                else
                {
                    if(ResearchPower > ResearchPowerLimit) ResearchPower = ResearchPowerLimit;
                    else if(ResearchPower < ResearchPowerLimit) ResearchPower += Budget * 0.001f;
                }
            }
        }
        else if(CurrentDevelopingProduct != null)
        {
            ObjectActCall.IsWorking = true;

            if(TimeManagerCall.TimeValue % TimeManagerCall.Hour < TimeManagerCall.PlaySpeed)
            {
                if(CurrentDevelopingProduct.ObjectInfo.RequiredPoint > CurrentDevelopingProduct.CompletedPoint)
                {
                    CurrentDevelopingProduct.CompletedPoint += ResearchPower / CurrentDevelopingProduct.ObjectInfo.RequiredResearchPower;
                    if(CurrentDevelopingProduct.ObjectInfo.RequiredPoint <= CurrentDevelopingProduct.CompletedPoint && CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName)
                        NotificationManagerCall.AddNews("Info", CurrentDevelopingProduct.Name + "'s development process has just been finished");
                }
                else
                {
                    CurrentDevelopingProduct.ObjectInfo.Attractiveness.PerfectionPoint += (ResearchPower / CurrentDevelopingProduct.ObjectInfo.RequiredResearchPower) * 0.01f;
                }

                if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName)
                {
                    if(PanelControllerCall.CurrentSidePanel != null)
                    {
                        if(PanelControllerCall.CurrentSidePanel.name == "LabatoryDevelopPanel")
                        {
                            if(PanelControllerCall.CurrentSidePanel.GetComponent<LabatoryDevelopPanelController>().TargetObject == gameObject)
                            {
                                PanelControllerCall.CurrentSidePanel.GetComponent<LabatoryDevelopPanelController>().UpdateProgressInfo();
                            }
                        }
                    }
                }
            }
        }
        else if(CurrentResearchingTech == null && CurrentDevelopingProduct == null)
        {
            ObjectActCall.IsWorking = false;
        }
    }

    public void StartResearch(string Name)
    {
        CurrentResearchingTech = Name;
        if(Name != "ResearchPowerUpgrade")
        {
            TechValueCall.StartResearch(CurrentResearchingTech, gameObject);
        }
    }

    public void StopResearch()
    {
        if(CurrentResearchingTech != "ResearchPowerUpgrade")
        {
            TechValueCall.RemoveResearchLabatory(CurrentResearchingTech, gameObject);
        }

        CurrentResearchingTech = null;
    }

    ///////////////////////////////////////////////////////////////////////////

    public bool setResultObject()
    {
        if(resultObject.RequiredProcessor == "None")
        {
            return false;
        }

        resultObject.Type = GoodsRecipeCall.getNewGoodsType(resultObject.Input);
        if(resultObject.Type == "None")
        {
            return false;
        }
        resultObject.Attractiveness = GoodsRecipeCall.CalculateAttractiveness(resultObject.Input.ToArray(), resultObject.RequiredProcessor);
        CalculateCost();
        
        return true;
    }

    void CalculateCost()
    {
        int ProcessorDevelopTime = 0;
        if(resultObject.RequiredProcessor != null)
        {
            string ProcessorType = resultObject.RequiredProcessor.Split('?')[0];
            string ProcessorName = resultObject.RequiredProcessor.Split('?')[1];
            switch(ProcessorName)
            {
                case "Dummy" :
                    ProcessorDevelopTime = 3;
                    break;
            }
            if(ProcessorType == "Assembler") ProcessorDevelopTime += 2;
        }

        resultObject.RequiredResearchPower = (resultObject.Attractiveness.TechPoint * 2f) + (float)ProcessorDevelopTime;
        resultObject.RequiredPoint = Mathf.CeilToInt(resultObject.Attractiveness.TechPoint * 1000f) + ProcessorDevelopTime;
    }

    public void StartDeveloping(string Name)
    {
        CurrentDevelopingProduct = new DevelopingProduct();
        CurrentDevelopingProduct.Name = Name;
        CurrentDevelopingProduct.CopyObjectInfo(resultObject);
        CurrentDevelopingProduct.CompletedPoint = 0;
        CurrentDevelopingProduct.StartTime = TimeManagerCall.TimeValue;

        resultObject = new ResultObject();
    }

    public void EndDeveloping()
    {
        if(CurrentDevelopingProduct.CompletedPoint > CurrentDevelopingProduct.ObjectInfo.RequiredPoint)
        {
            CurrentDevelopingProduct.ObjectInfo.Attractiveness.TotalPoint += CurrentDevelopingProduct.ObjectInfo.Attractiveness.PerfectionPoint;

            GoodsRecipeCall.MakeCustomRecipe(CurrentDevelopingProduct.ObjectInfo.Type, CurrentDevelopingProduct.Name, CurrentDevelopingProduct.ObjectInfo.Input.ToArray(),
                CurrentDevelopingProduct.ObjectInfo.Attractiveness, CompanyValueCall.CompanyName);
        }        

        CurrentDevelopingProduct = null;
    }

    public bool DeleteObject()
    {
        return true;
    }
}
