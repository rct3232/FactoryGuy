using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjInstantiater : MonoBehaviour
{
    CompanyManager CompanyManagerCall;
    InGameValue ValueCall;
    GameObject newObject;
    public class ObjectInfo
    {
        public ObjectInfo() {}
        public GameObject Object;
        public string Name;
        public string Type;
        public int Price;
        public int UpkeepPrice;
        public int UpkeepMonthTerm;
        public float ElectricConsum;
        public float LaborRequirement;
        public bool isUnlock = true;
        public int InstallDate;
    }
    public List<ObjectInfo> InfoArr = new List<ObjectInfo>();

    // Start is called before the first frame update
    void Start()
    {
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        ValueCall = GameObject.Find("BaseSystem").GetComponent<InGameValue>();
        ObjectInitializing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUnlockedObject(string Name)
    {
        foreach(var infoArray in InfoArr)
        {
            if(infoArray.Name == Name)
            {
                if(infoArray.isUnlock)
                {
                    Debug.Log(Name + " is already unlocked");
                }
                else
                {
                    infoArray.isUnlock = true;
                    return;
                }
            }
        }
    }

    public void InstantiateNewObject(string name)
    {
        if (ValueCall.AttachedOnMouse != null)
        {
            Destroy(ValueCall.AttachedOnMouse);
        }
        foreach(var infoArray in InfoArr)
        {
            if (infoArray.Name == name)
            {
                newObject = GameObject.Instantiate(infoArray.Object, transform);
                ValueCall.AttachedOnMouse = newObject;
                newObject.transform.name = "#" + CompanyManagerCall.GetPlayerCompanyValue().GetFacilityValue().GetComponent<FacilityValue>().InstalledFacilityAmount + 1.ToString() 
                    + " " + infoArray.Type;
                newObject.transform.parent = this.transform;
                newObject.GetComponent<InstallableObjectAct>().Info = infoArray;
                break;
            }
        }
    }

    public ObjectInfo GetInfoByName(string Name)
    {
        ObjectInfo result = null;

        foreach(var Info in InfoArr)
        {
            if(Info.Name == Name)
            {
                result = Info;
                break;
            }
        }

        return null;
    }

    void ObjectInitializing()
    {
        ObjectInfo NewObject;

        NewObject = new ObjectInfo();
        NewObject.Name = "Warehouse";
        NewObject.Type = "Warehouse";
        NewObject.Price = 50;
        NewObject.UpkeepPrice = 5;
        NewObject.ElectricConsum = 0.5f;
        NewObject.LaborRequirement = 1f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "Labatory";
        NewObject.Type = "Labatory";
        NewObject.Price = 200;
        NewObject.UpkeepPrice = 10;
        NewObject.ElectricConsum = 3f;
        NewObject.LaborRequirement = 2f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "GoodsCreator";
        NewObject.Type = "GoodsCreator";
        NewObject.Price = 20;
        NewObject.UpkeepPrice = 2;
        NewObject.ElectricConsum = 1f;
        NewObject.LaborRequirement = 0f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "GoodsLoader";
        NewObject.Type = "GoodsLoader";
        NewObject.Price = 20;
        NewObject.UpkeepPrice = 2;
        NewObject.ElectricConsum = 1f;
        NewObject.LaborRequirement = 0f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "Belt1";
        NewObject.Type = "Belt";
        NewObject.Price = 10;
        NewObject.UpkeepPrice = 1;
        NewObject.ElectricConsum = 0.5f;
        NewObject.LaborRequirement = 0f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "Processor1";
        NewObject.Type = "Processor";
        NewObject.Price = 50;
        NewObject.UpkeepPrice = 3;
        NewObject.ElectricConsum = 1.5f;
        NewObject.LaborRequirement = 1f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "Assembler1";
        NewObject.Type = "Processor";
        NewObject.Price = 80;
        NewObject.UpkeepPrice = 4;
        NewObject.ElectricConsum = 1.7f;
        NewObject.LaborRequirement = 1f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "Distributor";
        NewObject.Type = "Distributor";
        NewObject.Price = 70;
        NewObject.UpkeepPrice = 3;
        NewObject.ElectricConsum = 1.5f;
        NewObject.LaborRequirement = 1f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);
        
        NewObject = new ObjectInfo();
        NewObject.Name = "Destroyer";
        NewObject.Type = "Destroyer";
        NewObject.Price = 30;
        NewObject.UpkeepPrice = 4;
        NewObject.ElectricConsum = 3f;
        NewObject.LaborRequirement = 1f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "QualityControlUnit";
        NewObject.Type = "QualityControlUnit";
        NewObject.Price = 100;
        NewObject.UpkeepPrice = 2;
        NewObject.ElectricConsum = 2f;
        NewObject.LaborRequirement = 0f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "VerticalBelt1_Up";
        NewObject.Type = "VerticalBelt";
        NewObject.Price = 20;
        NewObject.UpkeepPrice = 2;
        NewObject.ElectricConsum = 0.7f;
        NewObject.LaborRequirement = 0f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "VerticalBelt1_Down";
        NewObject.Type = "VerticalBelt";
        NewObject.Price = 20;
        NewObject.UpkeepPrice = 2;
        NewObject.ElectricConsum = 0.7f;
        NewObject.LaborRequirement = 0f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "SmallEnergyStorage";
        NewObject.Type = "EnergyStorage";
        NewObject.Price = 150;
        NewObject.UpkeepPrice = 10;
        NewObject.ElectricConsum = 0f;
        NewObject.LaborRequirement = 2f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "SmallEnergySupplier";
        NewObject.Type = "EnergySupplier";
        NewObject.Price = 50;
        NewObject.UpkeepPrice = 20;
        NewObject.ElectricConsum = 0f;
        NewObject.LaborRequirement = 1f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        NewObject = new ObjectInfo();
        NewObject.Name = "SmallDayRoom";
        NewObject.Type = "DayRoom";
        NewObject.Price = 150;
        NewObject.UpkeepPrice = 10;
        NewObject.ElectricConsum = 0.2f;
        NewObject.LaborRequirement = 0f;
        NewObject.UpkeepMonthTerm = 1;
        InfoArr.Add(NewObject);

        for(int i = 0; i < InfoArr.Count; i++)
        {
            InfoArr[i].Object = Resources.Load<GameObject>("GameSystem/InstallableObject/Object/" + InfoArr[i].Name);
        }
        
        List<GameObject> TempList = new List<GameObject>();
        foreach(var techArray in CompanyManagerCall.GetPlayerCompanyValue().GetTechValue().GetComponent<TechValue>().FacilityArray)
        {
            if(!techArray.isCompleted)
            {
                foreach(var unlockedOne in techArray.RecipeInfo.UnlockFacility)
                {
                    foreach(var infoArray in InfoArr)
                    {
                        if(unlockedOne == infoArray.Name)
                        {
                            infoArray.isUnlock = false;
                        }
                    }
                }
            }
        }
    }
}
