using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechValue : MonoBehaviour
{
    public class TechState
    {
        public TechState() {}
        public TechRecipe.TechInfo Info;
        public bool Completed;
        public bool Possible;
    }
    public class ResearchState
    {
        public ResearchState() {}
        public TechState TargetState;
        public List<GameObject> LabatoryList;
        public float GainedWorkLoad;
        public int StartTime;
    }
    public class RecipeInfo
    {
        public GoodsRecipe.Recipe Recipe;
        public string Maker;
        public string Owner;
    }

    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    TimeManager TimeManangerCall;
    TechRecipe TechRecipeCall;
    GoodsRecipe GoodsRecipeCall;
    PanelController PanelControllerCall;
    public List<TechState> TechTreeList;
    public List<ResearchState> ResearchStateList;
    public List<TechRecipe.FacilityInfo> FacilityList;
    public List<TechRecipe.ProcessActorInfo> ActorList;
    public List<RecipeInfo> AvailableRecipe;

    void Start()
    {
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        TimeManangerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        TechRecipeCall = GameObject.Find("BaseSystem").GetComponent<TechRecipe>();
        GoodsRecipeCall = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        PanelControllerCall = GameObject.Find("Canvas").GetComponent<PanelController>();
        AvailableRecipe = new List<RecipeInfo>();

        Initializing();
    }
    
    void Initializing()
    {
        TechTreeList = new List<TechState>();
        FacilityList = new List<TechRecipe.FacilityInfo>();
        ActorList = new List<TechRecipe.ProcessActorInfo>();
        ResearchStateList = new List<ResearchState>();
        
        TechState newState;
        foreach(var Info in TechRecipeCall.TechInfoList)
        {
            newState = new TechState();
            newState.Info = Info;
            newState.Completed = false;
            newState.Possible = false;

            TechTreeList.Add(newState);
        }
        
        ResearchState FirstResearch = new ResearchState();
        FirstResearch.LabatoryList = new List<GameObject>();
        FirstResearch.TargetState = TechTreeList[0];
        ResearchStateList.Add(FirstResearch);
        CompleteResearch(FirstResearch);

        CheckTechPossible();
    }

    void ResearchPanelUpdate(ResearchState TargetResearch)
    {
        if(CompanyValueCall.CompanyName == CompanyManagerCall.PlayerCompanyName)
        {
            if(PanelControllerCall.CurrentSidePanel != null)
            {
                if(PanelControllerCall.CurrentSidePanel.name == "LabatoryResearchPanel")
                {
                    bool Exist = false;
                    GameObject PanelTargetObject = PanelControllerCall.CurrentSidePanel.GetComponent<LabatoryResearchPanelController>().TargetObject;
                    foreach(var Labatory in TargetResearch.LabatoryList)
                    {
                        if (Labatory == PanelTargetObject)
                        {
                            Exist = true;
                            break;
                        }
                    }
                    if(Exist)
                    {
                        PanelControllerCall.CurrentSidePanel.GetComponent<LabatoryResearchPanelController>().UpdateProgressInfo();
                    }
                }
            }
        }
    }

    public void StartResearch(string Name, GameObject TargetLabatory)
    {
        ResearchState TargetResearch = GetResearchState(Name);

        if(TargetResearch == null)
        {
            TargetResearch = new ResearchState();
            foreach(var TechState in TechTreeList)
            {
                if(TechState.Info.Name == Name)
                {
                    TargetResearch.TargetState = TechState;
                }
            }

            TargetResearch.GainedWorkLoad = 0;
            TargetResearch.LabatoryList = new List<GameObject>();

            TargetResearch.StartTime = TimeManangerCall.TimeValue;
        }

        TargetResearch.LabatoryList.Add(TargetLabatory);

        SetResearchState(TargetResearch);
    }

    public void ContributeResearchWork(string Name, float Amount)
    {
        ResearchState TargetResearch = GetResearchState(Name);
        if(TargetResearch == null)
        {
            Debug.Log("Cannot Find " + Name + " on ResearchStateList");
            return;
        }

        TargetResearch.GainedWorkLoad += Amount;

        if(TargetResearch.GainedWorkLoad >= TargetResearch.TargetState.Info.RequiredWorkLoad) CompleteResearch(TargetResearch);

        ResearchPanelUpdate(TargetResearch);
    }

    public bool CompleteResearch(ResearchState TargetResearch)
    {
        TargetResearch.TargetState.Completed = true;
        
        CheckTechPossible();

        foreach(var FacilityName in TargetResearch.TargetState.Info.UnlockFacility)
        {
            if(FacilityName == "None") break;
            TechRecipe.FacilityInfo TargetFacility = TechRecipeCall.GetFacilityInfo(FacilityName);
            FacilityList.Add(TargetFacility);
        }

        foreach(var ActorName in TargetResearch.TargetState.Info.UnlockActor)
        {
            if(ActorName == "None") break;
            TechRecipe.ProcessActorInfo TargetActor = TechRecipeCall.GetProcessActorInfo(ActorName);
            ActorList.Add(TargetActor);
        }

        switch(TargetResearch.TargetState.Info.UpgradeValueType)
        {
            case "Work" :
            CompanyValueCall.GetEmployeeValue().GetComponent<EmployeeValue>().WorkEifficiency = TargetResearch.TargetState.Info.UpgradeValueAmount;
            break;
            case "Energy" :
            CompanyValueCall.GetElectricityValue().GetComponent<ElectricityValue>().EnergyEfficiency = TargetResearch.TargetState.Info.UpgradeValueAmount;
            break;
            case "Organize" :
            CompanyValueCall.GetGoodsValue().GetComponent<GoodsValue>().OrganizeEfficiency = TargetResearch.TargetState.Info.UpgradeValueAmount;
            break;
        }

        ResearchPanelUpdate(TargetResearch);

        int limit = TargetResearch.LabatoryList.Count;
        for(int i = 0; i < limit; i++)
        {
            LabatoryAct TargetLabatoryAct = TargetResearch.LabatoryList[0].GetComponent<LabatoryAct>();

            TargetLabatoryAct.StopResearch();
        }

        ResearchStateList.Remove(TargetResearch);
        TargetResearch = null;

        return true;
    }

    public bool RemoveResearchLabatory(string Name, GameObject TargetLabatory)
    {
        ResearchState TargetResearch = GetResearchState(Name);

        TargetResearch.LabatoryList.Remove(TargetLabatory);

        if(TargetResearch.LabatoryList.Count <= 0)
        {
            ResearchStateList.Remove(TargetResearch);
            return true;
        }
        else
        {
            return false;
        }
    }

    void CheckTechPossible()
    {
        foreach(var State in TechTreeList)
        {
            if(State.Possible) continue;

            bool Possible = true;
            foreach(var TechIndex in State.Info.RequiredTech)
            {
                if(TechIndex == -1) break;
                if(!TechTreeList[TechIndex].Completed) Possible = false;
            }

            State.Possible = Possible;
        }
    }

    public bool GetTechPossible(string Name)
    {
        foreach(var State in TechTreeList)
        {
            if(State.Info.Name == Name && State.Possible && !State.Completed) return true;
        }

        return false;
    }

    public ResearchState GetResearchState(string Name)
    {
        foreach(var State in ResearchStateList)
        {
            if(State.TargetState.Info.Name == Name) return State;
        }

        return null;
    }

    public void SetResearchState(ResearchState targetResearch)
    {
        ResearchStateList.Add(targetResearch);
    }

    public bool GetActorPossible(string Name)
    {
        foreach(var Actor in ActorList)
        {
            if(Actor.Name == Name) return true;
        }

        return false;
    }

    public void AddRecipe(GoodsRecipe.Recipe Recipe, string CompanyName)
    {
        RecipeInfo newRecipe = new RecipeInfo();

        newRecipe.Recipe = Recipe;
        newRecipe.Maker = CompanyName;
        newRecipe.Owner = CompanyName;

        AvailableRecipe.Add(newRecipe);
    }

    public RecipeInfo GetRecipe(string Name)
    {
        RecipeInfo Result = null;

        foreach(var Recipe in AvailableRecipe)
        {
            if(Recipe.Recipe.OutputName == Name)
            {
                Result = Recipe;
            }
        }
        return Result;
    }
}
