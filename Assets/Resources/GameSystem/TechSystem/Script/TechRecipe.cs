using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechRecipe : MonoBehaviour
{
    TechValue TechValueCall;
    CompanyManager CompanyManagerCall;
    public class FacilityRecipe
    {
        public FacilityRecipe() {}
        public string Name;
        public int TechLevel;
        public string[] UnlockFacility;
        public int[] RequiredTech;
        public int RequiredWorkLoad;
        public int Cost;
    }
    public List<FacilityRecipe> FacilityRecipeArray = new List<FacilityRecipe>();
    public class ProcessActorInfo
    {
        public ProcessActorInfo() {}
        public string Name;
        public int Cost;
    }
    public class ProcessorRecipe
    {
        public ProcessorRecipe() {}
        public string Type;
        public string Name;
        public float PerformanceSpeed;
        public float PerformanceQuality;
        public List<ProcessActorInfo> ActorList;
    }
    public List<ProcessorRecipe> ProcessorArray = new List<ProcessorRecipe>();

    void Awake()
    {
        InitializingArray();
    }

    void Start()
    {
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        TechValueCall = CompanyManagerCall.GetPlayerCompanyValue().GetTechValue().GetComponent<TechValue>();
    }

    public bool CheckRequiredment(int techIndex)
    {
        foreach(var tmp in FacilityRecipeArray[techIndex].RequiredTech)
        {
            if(tmp == -1)
            {
                return true;
            }
            if(!TechValueCall.FacilityArray[tmp].isCompleted)
            {
                return false;
            }
        }

        return true;
    }

    public ProcessorRecipe GetProcessorRecipe(string ProcessorName)
    {
        ProcessorRecipe Result = null;

        foreach(var Processor in ProcessorArray)
        {
            if(Processor.Name == ProcessorName) Result = Processor;
        }

        return Result;
    }

    public ProcessActorInfo GetProcessActorInfo(string Name)
    {
        ProcessActorInfo Result = null;

        foreach(var Processor in ProcessorArray)
        {
            foreach(var Actor in Processor.ActorList)
            {
                if(Actor.Name == Name) Result = Actor;
            }
        }

        return Result;
    }

    void InitializingArray()
    {
        FacilityRecipeArray = new List<FacilityRecipe>();

        FacilityRecipe newRecipe = new FacilityRecipe();
        newRecipe.Name = "FirstTech";
        newRecipe.TechLevel = 1;
        newRecipe.UnlockFacility = new string[1];
        newRecipe.UnlockFacility[0] = "None";
        newRecipe.RequiredTech = new int[1];
        newRecipe.RequiredTech[0] = -1;
        newRecipe.RequiredWorkLoad = 75;
        newRecipe.Cost = 10;
        FacilityRecipeArray.Add(newRecipe);

        newRecipe = new FacilityRecipe();
        newRecipe.Name = "SecondTech";
        newRecipe.TechLevel = 2;
        newRecipe.UnlockFacility = new string[1];
        newRecipe.UnlockFacility[0] = "None";//"Assembler1";
        newRecipe.RequiredTech = new int[1];
        newRecipe.RequiredTech[0] = 0;
        newRecipe.RequiredWorkLoad = 210;
        newRecipe.Cost = 15;
        FacilityRecipeArray.Add(newRecipe);

        newRecipe = new FacilityRecipe();
        newRecipe.Name = "ThirdTech";
        newRecipe.TechLevel = 2;
        newRecipe.UnlockFacility = new string[1];
        newRecipe.UnlockFacility[0] = "None";//"Distributor";
        newRecipe.RequiredTech = new int[1];
        newRecipe.RequiredTech[0] = 0;
        newRecipe.RequiredWorkLoad = 210;
        newRecipe.Cost = 12;
        FacilityRecipeArray.Add(newRecipe);

        newRecipe = new FacilityRecipe();
        newRecipe.Name = "ForthTech";
        newRecipe.TechLevel = 3;
        newRecipe.UnlockFacility = new string[1];
        newRecipe.UnlockFacility[0] = "None";//"QualityControlUnit";
        // newTech.UnlockFacility[0] = "Destroyer";
        newRecipe.RequiredTech = new int[1];
        newRecipe.RequiredTech[0] = 1;
        newRecipe.RequiredWorkLoad = 300;
        newRecipe.Cost = 13;
        FacilityRecipeArray.Add(newRecipe);

        newRecipe = new FacilityRecipe();
        newRecipe.Name = "FifthTech";
        newRecipe.TechLevel = 4;
        newRecipe.UnlockFacility = new string[1];
        newRecipe.UnlockFacility[0] = "None";
        newRecipe.RequiredTech = new int[2];
        newRecipe.RequiredTech[0] = 2;
        newRecipe.RequiredTech[1] = 3;
        newRecipe.RequiredWorkLoad = 500;
        newRecipe.Cost = 14;
        FacilityRecipeArray.Add(newRecipe);

        //---------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------

        ProcessorArray = new List<ProcessorRecipe>();

        ProcessorRecipe newProcessor = new ProcessorRecipe();
        newProcessor.Type = "Processor";
        newProcessor.Name = "Processor1";
        newProcessor.PerformanceQuality = 0.5f;
        newProcessor.PerformanceSpeed = 1f;
        newProcessor.ActorList = new List<ProcessActorInfo>();
        ProcessorArray.Add(newProcessor);

        newProcessor = new ProcessorRecipe();
        newProcessor.Type = "Assembler";
        newProcessor.Name = "Assembler1";
        newProcessor.PerformanceQuality = 0.5f;
        newProcessor.PerformanceSpeed = 1f;
        newProcessor.ActorList = new List<ProcessActorInfo>();
        ProcessorArray.Add(newProcessor);

        //---------------------------------------------------------------------------------

        ProcessActorInfo newActor = new ProcessActorInfo();
        newActor.Name = "Dummy";
        newActor.Cost = 20;
        foreach(var Processor in ProcessorArray) if(Processor.Name == "Processor1") Processor.ActorList.Add(newActor);
        foreach(var Processor in ProcessorArray) if(Processor.Name == "Assembler1") Processor.ActorList.Add(newActor);
    }
}
