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
    }
    public class RecipeInfo
    {
        public GoodsRecipe.Recipe Recipe;
        public string Maker;
        public string Owner;
    }

    ObjInstantiater ObjInstantiaterCall;
    CompanyManager CompanyManagerCall;
    CompanyValue CompanyValueCall;
    TechRecipe TechRecipeCall;
    GoodsRecipe GoodsRecipeCall;
    public List<TechState> TechTreeList;
    public List<ResearchState> ResearchStateList;
    public List<ObjInstantiater.ObjectInfo> FacilityList;
    public List<TechRecipe.ProcessActorInfo> ActorList;
    public List<RecipeInfo> AvailableRecipe;

    void Start()
    {
        ObjInstantiaterCall = GameObject.Find("ObjectInstaller").GetComponent<ObjInstantiater>();
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        CompanyValueCall = transform.parent.gameObject.GetComponent<CompanyValue>();
        TechRecipeCall = GameObject.Find("BaseSystem").GetComponent<TechRecipe>();
        GoodsRecipeCall = GameObject.Find("BaseSystem").GetComponent<GoodsRecipe>();
        AvailableRecipe = new List<RecipeInfo>();

        Initializing();
    }
    
    void Initializing()
    {
        TechTreeList = new List<TechState>();
        
        TechState newState;
        foreach(var Info in TechRecipeCall.TechInfoList)
        {
            newState = new TechState();
            newState.Info = Info;
            newState.Completed = false;
            newState.Possible = false;
        }

        TechTreeList[0].Possible = true;
    }

    public void StartResearch(string Name, GameObject TargetLabatory)
    {
        ResearchState TargetResearch = null;
        foreach(var State in ResearchStateList)
        {
            if(State.TargetState.Info.Name == Name) TargetResearch = State;
        }

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
        }

        TargetResearch.LabatoryList.Add(TargetLabatory);
    }

    public float ContributeResearchWork(string Name, float Amount)
    {
        ResearchState TargetResearch = null;
        foreach(var State in ResearchStateList)
        {
            if(State.TargetState.Info.Name == Name) TargetResearch = State;
        }
        if(TargetResearch == null)
        {
            Debug.Log("Cannot Find " + Name + " on ResearchStateList");
            return -1.0f;
        }

        float Result = TargetResearch.GainedWorkLoad += Amount;

        return Result;
    }

    public bool CompleteResearch(string Name)
    {
        ResearchState TargetResearch = null;
        foreach(var State in ResearchStateList)
        {
            if(State.TargetState.Info.Name == Name) TargetResearch = State;
        }
        if(TargetResearch == null)
        {
            Debug.Log("Cannot Find " + Name + " on ResearchStateList");
            return false;
        }

        foreach(var Labatory in TargetResearch.LabatoryList)
        {
            LabatoryAct TargetLabatoryAct = Labatory.GetComponent<LabatoryAct>();

            TargetLabatoryAct.StopResearch();
        }

        TargetResearch.TargetState.Completed = true;

        foreach(var FacilityName in TargetResearch.TargetState.Info.UnlockFacility)
        {
            ObjInstantiater.ObjectInfo TargetFacility = ObjInstantiaterCall.GetInfoByName(FacilityName);
            FacilityList.Add(TargetFacility);
        }

        foreach(var ActorName in TargetResearch.TargetState.Info.UnlockActor)
        {
            TechRecipe.ProcessActorInfo TargetActor = TechRecipeCall.GetProcessActorInfo(ActorName);
            ActorList.Add(TargetActor);
        }

        switch(TargetResearch.TargetState.Info.UpgradeValueType)
        {
            case "WorkEfficiency" :
            CompanyValueCall.GetEmployeeValue().GetComponent<EmployeeValue>().WorkEifficiency += TargetResearch.TargetState.Info.UpgradeValueAmount;
            break;
            case "EnergyEfficiency" :
            CompanyValueCall.GetElectricityValue().GetComponent<ElectricityValue>().EnergyEfficiency += TargetResearch.TargetState.Info.UpgradeValueAmount;
            break;
            case "OrganizeEfficiency" :
            CompanyValueCall.GetGoodsValue().GetComponent<GoodsValue>().OrganizeEfficiency += TargetResearch.TargetState.Info.UpgradeValueAmount;
            break;
        }

        ResearchStateList.Remove(TargetResearch);
        TargetResearch = null;

        CheckPossible();

        return true;
    }

    void CheckPossible()
    {

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
