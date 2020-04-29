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
        public int RequiredValue;
        public int RequiredWorkLoad;
    }
    public List<TechInfo> TechInfoList = new List<TechInfo>();
    public class FacilityInfo
    {
        public FacilityInfo() {}
        public GameObject Object;
        public string Name;
        public string Type;
        public int Price;
        public int UpkeepPrice;
        public int UpkeepMonthTerm;
        public float ElectricConsum;
        public float LaborRequirement;
    }
    public List<FacilityInfo> FacilityList = new List<FacilityInfo>();
    public class ProcessorInfo
    {
        public ProcessorInfo() {}
        public string Type;
        public string Name;
        public float PerformanceSpeed;
        public float PerformanceQuality;
        public string[] ActorList;
    }
    public List<ProcessorInfo> ProcessorList = new List<ProcessorInfo>();
    public class ProcessActorInfo
    {
        public ProcessActorInfo() {}
        public string Name;
        public int Cost;
    }
    public List<ProcessActorInfo> ActorList = new List<ProcessActorInfo>();

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

    public FacilityInfo GetFacilityInfo(string Name)
    {
        FacilityInfo Result = null;

        foreach(var Facility in FacilityList)
        {
            if(Facility.Name == Name) Result = Facility;
        }

        return Result;
    }

    public ProcessorInfo GetProcessorRecipe(string Name)
    {
        ProcessorInfo Result = null;

        foreach(var Processor in ProcessorList)
        {
            if(Processor.Name == Name) Result = Processor;
        }

        return Result;
    }

    public ProcessActorInfo GetProcessActorInfo(string Name)
    {
        ProcessActorInfo Result = null;

        foreach(var Actor in ActorList)
        {
            if(Actor.Name == Name) Result = Actor;
        }

        return Result;
    }

    void InitializingArray()
    {
        string[] FieldName;
        List<string[]> DataList;

        TechInfoList = new List<TechInfo>();
        FieldName = new string[] {"Name", "UnlockFacility", "UnlockActor", "UpgradeType", "UpgradeAmount", "RequiredIndex", "RequiredValue", "WorkLoad"};
        DataList = new List<string[]>();

        xmlReader.xmlReaderAccess.ReadXml("Data/Tech/XML/TechInfo", "TechInfo/Tech", FieldName, DataList);

        foreach(string[] Data in DataList)
        {
            TechInfo newRecipe = new TechInfo();
            newRecipe.Name = Data[0];
            newRecipe.UnlockFacility = Data[1].Split(',');
            newRecipe.UnlockActor = Data[2].Split(',');
            newRecipe.UpgradeValueType = Data[3];
            newRecipe.UpgradeValueAmount = System.Convert.ToSingle(Data[4]);
            string[] RequiredIndex = Data[5].Split(',');
            newRecipe.RequiredTech = new int[RequiredIndex.Length];
            for(int i = 0; i < RequiredIndex.Length; i++) newRecipe.RequiredTech[i] = System.Convert.ToInt32(RequiredIndex[i]);
            newRecipe.RequiredValue = System.Convert.ToInt32(Data[6]);
            newRecipe.RequiredWorkLoad = System.Convert.ToInt32(Data[7]);

            TechInfoList.Add(newRecipe);
        }

        FacilityList = new List<FacilityInfo>();
        FieldName = new string[] {"Type", "Name", "Price", "UpkeepPrice", "UpkeepMonthTerm", "ElectricConsum", "LaborRequirement"};
        DataList = new List<string[]>();

        xmlReader.xmlReaderAccess.ReadXml("Data/Tech/XML/FacilityInfo", "FacilityInfo/Facility", FieldName, DataList);

        foreach(string[] Data in DataList)
        {
            FacilityInfo newFacility =  new FacilityInfo();
            newFacility.Type = Data[0];
            newFacility.Name = Data[1];
            newFacility.Price = System.Convert.ToInt32(Data[2]);
            newFacility.UpkeepPrice = System.Convert.ToInt32(Data[3]);
            newFacility.UpkeepMonthTerm = System.Convert.ToInt32(Data[4]);
            newFacility.ElectricConsum = System.Convert.ToSingle(Data[5]);
            newFacility.LaborRequirement = System.Convert.ToSingle(Data[6]);
            newFacility.Object = Resources.Load<GameObject>("GameSystem/InstallableObject/Object/" + newFacility.Name);

            FacilityList.Add(newFacility);
        }

        ProcessorList = new List<ProcessorInfo>();
        FieldName = new string[] {"Type", "Name", "PerformanceQuality", "PerformanceSpeed", "ActorList"};
        DataList = new List<string[]>();

        xmlReader.xmlReaderAccess.ReadXml("Data/Tech/XML/ProcessorInfo", "ProcessorInfo/Processor", FieldName, DataList);

        foreach(string[] Data in DataList)
        {
            ProcessorInfo newProcessor = new ProcessorInfo();
            newProcessor.Type = Data[0];
            newProcessor.Name = Data[1];
            newProcessor.PerformanceQuality = System.Convert.ToSingle(Data[2]);
            newProcessor.PerformanceSpeed = System.Convert.ToSingle(Data[3]);
            newProcessor.ActorList = Data[4].Split(',');
            
            ProcessorList.Add(newProcessor);
        }

        ActorList = new List<ProcessActorInfo>();
        FieldName = new string[] {"Name", "Cost"};
        DataList = new List<string[]>();

        xmlReader.xmlReaderAccess.ReadXml("Data/Tech/XML/ActorInfo", "ActorInfo/Actor", FieldName, DataList);

        foreach(string[] Data in DataList)
        {
            ProcessActorInfo newActor = new ProcessActorInfo();
            newActor.Name = Data[0];
            newActor.Cost = System.Convert.ToInt32(Data[1]);

            ActorList.Add(newActor);
        }
    }
}
