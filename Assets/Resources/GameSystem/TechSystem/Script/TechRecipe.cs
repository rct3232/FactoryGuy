using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechRecipe : MonoBehaviour
{
    TechValue TechValueCall;
    CompanyManager CompanyManagerCall;
    public class TechInfo
    {
        public TechInfo() {}
        public string Name;
        public string[] UnlockFacility;
        public string[] UnlockActor;
        public string UpgradeValueType;
        public float UpgradeValueAmount;
        public int[] RequiredTech;
        public int RequiredWorkLoad;
        public int Cost;
    }
    public List<TechInfo> TechInfoList = new List<TechInfo>();
    public class ProcessActorInfo
    {
        public ProcessActorInfo() {}
        public ProcessorRecipe ParentInfo;
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
        foreach(var tmp in TechInfoList[techIndex].RequiredTech)
        {
            if(tmp == -1)
            {
                return true;
            }
            if(!TechValueCall.TechTreeList[tmp].Completed)
            {
                return false;
            }
        }

        return true;
    }

    public TechInfo GetTechInfo(string Name)
    {
        TechInfo Result = null;
        foreach(var Info in TechInfoList)
        {
            if(Info.Name == Name)
            {
                Result = Info;
            }
        }

        return Result;
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
        TechInfoList = new List<TechInfo>();

        TechInfo newRecipe = new TechInfo();
        newRecipe.Name = "FirstTech";
        newRecipe.UnlockFacility = new string[1];
        newRecipe.UnlockFacility[0] = "None";
        newRecipe.UnlockActor = new string[1];
        newRecipe.UnlockActor[0] = "None";
        newRecipe.UpgradeValueType = "None";
        newRecipe.UpgradeValueAmount = 0;
        newRecipe.RequiredTech = new int[1];
        newRecipe.RequiredTech[0] = -1;
        newRecipe.RequiredWorkLoad = 75;
        newRecipe.Cost = 10;
        TechInfoList.Add(newRecipe);

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
