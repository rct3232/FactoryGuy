using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameValue : MonoBehaviour
{
    public string CompanyName;
    public bool[,] ActivatedGroup;
    public GameObject AttachedOnMouse;
    public bool[] ModeBit; // 0: NormalMode, 1: BolldozeMode, 2: GroupSelectMode
    GameObject CompanyManagerObject;

    // Start is called before the first frame update
    void Awake()
    {
        CompanyManagerObject = GameObject.Find("CompanyManager");

        CompanyName = "Player";
        ActivatedGroup = new bool[TopValue.TopValueSingleton.MapSize, TopValue.TopValueSingleton.MapSize];
        for(int i = 0; i < ActivatedGroup.GetLength(0); i++)
        {
            for(int j = 0; j < ActivatedGroup.GetLength(1); j++)
            {
                ActivatedGroup[i,j] = false;
            }
        }

        ModeBit = new bool[3];
        ModeBit[0] = true;
        for (int i = 1; i < ModeBit.Length; i++)
        {
            ModeBit[i] = false;
        }

        InitializePlayerCompany();
    }

    void Start()
    {
        InitializeSupplierAI();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void GetGameInfo()
    {
        
    }

    void InitializePlayerCompany()
    {
        GameObject newCompany = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Company/Object/Player"));
        newCompany.transform.SetParent(CompanyManagerObject.transform);

        CompanyManager CompanyManagerCall = CompanyManagerObject.GetComponent<CompanyManager>();
        WorkerSelector WorkerSelectorCall = GameObject.Find("WorkerManager").GetComponent<WorkerSelector>();

        CompanyManagerCall.AddCompany(CompanyName, newCompany);
        CompanyManagerCall.PlayerCompanyName = CompanyName;
        newCompany.name = CompanyName;

        EconomyValue EconomyValueCall = CompanyManagerCall.GetPlayerCompanyValue().GetEconomyValue().GetComponent<EconomyValue>();
        EconomyValueCall.Balance = 20000;
        WorkerSelector.Worker CEO = WorkerSelectorCall.AddCustomWorker(null, 3, 0, 0, 0, 0, 0);
        CompanyManagerCall.GetPlayerCompanyValue().GetEmployeeValue().GetComponent<EmployeeValue>().HireEmployee(CEO);
        WorkerSelectorCall.UnHiredList.Remove(CEO);
    }

    void InitializeSupplierAI()
    {
         GameObject newCompany = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Company/Object/Player"));
         newCompany.transform.SetParent(CompanyManagerObject.transform);
         newCompany.GetComponent<CompanyValue>().isAI = true;
         newCompany.GetComponent<CompanyValue>().Unevaluable = true;
         newCompany.GetComponent<CompanyValue>().TotalValue = 0f;

         GameObject SupplierAI = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/AI/Object/RawMaterialSupplyAI"));
         SupplierAI.transform.SetParent(newCompany.transform);
         SupplierAI.name = "RawMaterialSupplyAI";
         SupplierAI.GetComponent<RawMaterialSupplyAI>().Name = "General Industry Co.";


         CompanyManager CompanyManagerCall = CompanyManagerObject.GetComponent<CompanyManager>();
         CompanyManagerCall.AddCompany("General Industry Co.", newCompany);
         newCompany.name = "General Industry Co.";
         //

         newCompany = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Company/Object/Player"));
         newCompany.transform.SetParent(CompanyManagerObject.transform);
         newCompany.GetComponent<CompanyValue>().isAI = true;
         newCompany.GetComponent<CompanyValue>().Unevaluable = true;
         newCompany.GetComponent<CompanyValue>().TotalValue = 0f;

         SupplierAI = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/AI/Object/RawMaterialSupplyAI"));
         SupplierAI.transform.SetParent(newCompany.transform);
         SupplierAI.name = "RawMaterialSupplyAI";
         SupplierAI.GetComponent<RawMaterialSupplyAI>().Name = "Federal Agency of Industry";

         CompanyManagerCall = CompanyManagerObject.GetComponent<CompanyManager>();
         CompanyManagerCall.AddCompany("Federal Agency of Industry", newCompany);
         newCompany.name = "Federal Agency of Industry";
         //

         newCompany = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/Company/Object/Player"));
         newCompany.transform.SetParent(CompanyManagerObject.transform);
         newCompany.GetComponent<CompanyValue>().isAI = true;
         newCompany.GetComponent<CompanyValue>().Unevaluable = true;
         newCompany.GetComponent<CompanyValue>().TotalValue = 0f;

        //  SupplierAI = GameObject.Instantiate(Resources.Load<GameObject>("GameSystem/AI/Object/RawMaterialSupplyAI"));
        //  SupplierAI.transform.SetParent(newCompany.transform);
        //  SupplierAI.name = "RawMaterialSupplyAI";
        //  SupplierAI.GetComponent<RawMaterialSupplyAI>().Name = "Sasio";

        //  CompanyManagerCall = CompanyManagerObject.GetComponent<CompanyManager>();
        //  CompanyManagerCall.AddCompany("Sasio", newCompany);
        //  newCompany.name = "Sasio";
    }

    public void ModeSelector(int type)
    {
        Destroy(AttachedOnMouse);
        AttachedOnMouse = null;

        if(ModeBit[type])
        {
            if(type != 0)
            {
                ModeSelector(0);
            }
        }
        else
        {
            for(int i = 0; i < ModeBit.Length; i++)
            {
                if(type == i)
                {
                    ModeBit[i] = true;
                }
                else
                {
                    ModeBit[i] = false;
                }
            }
        }
    }

    public List<int> ShuffleIndex(int ListCount)
    {
        List<int> ResultList = new List<int>();

        for(int i = 0; i < ListCount; i++) ResultList.Add(i);

        for(int i = 0; i < ListCount - 1; i++)
        {
            int temp;
            int r = Random.Range(i, ListCount);
            temp = ResultList[i];
            ResultList[i] = ResultList[r];
            ResultList[r] = temp;
        }

        return ResultList;
    }
}
