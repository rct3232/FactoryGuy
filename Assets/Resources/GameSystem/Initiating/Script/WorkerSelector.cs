using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSelector : MonoBehaviour
{
    public class Worker
    {
        public Worker() {}
        public int index;
        public string Name;
        public int LaborForce;
        public int Salary;
        public float LaborForceLossRate;
        public float SincerityRate;
        public int TrialCount;
        public float ExpectCompanyValue;
        public List<string> LeaveFactoryList;
    }
    public List<Worker> UnHiredList = new List<Worker>();
    int WorkerGenerateCounter;
    int CurrentWorkerIndex;
    TimeManager TimeManagerCall;
    CompanyManager CompanyManagerCall;
    // Start is called before the first frame update
    void Start()
    {
        TimeManagerCall = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();

        WorkerGenerateCounter = 0;
        CurrentWorkerIndex = 0;

        for(int i = 0; i < 5; i++) GenerateWorker();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(--WorkerGenerateCounter <= 0)
        {
            GenerateWorker();
        }

        if(TimeManagerCall.TimeValue % TimeManagerCall.Day < TimeManagerCall.PlaySpeed && UnHiredList.Count > 0)
        {
            SelectCompany();
        }
    }

    void GenerateWorker()
    {
        Worker newWorker = new Worker();
        float SelectionStandard;

        newWorker.index = CurrentWorkerIndex;
        newWorker.Name = GetRandomName();
        newWorker.LaborForce = Random.Range(1, Random.Range(1,Random.Range(1,Random.Range(1,Random.Range(1,Random.Range(1,200))))));
        newWorker.LaborForceLossRate = Random.Range(0.05f, 0.95f) / newWorker.LaborForce;
        newWorker.SincerityRate = Random.Range(Random.Range(newWorker.LaborForceLossRate - (0.05f / newWorker.LaborForce), 1), 1);
        SelectionStandard = newWorker.LaborForce * (1f - newWorker.LaborForceLossRate) * (1f + newWorker.SincerityRate);
        newWorker.Salary = Mathf.CeilToInt(Random.Range(SelectionStandard / Random.Range(1f, 2f), SelectionStandard * Random.Range(1f, 5f)));
        newWorker.TrialCount = Random.Range(10, Random.Range(10,100));
        newWorker.ExpectCompanyValue = Random.Range(SelectionStandard / Random.Range(1f, 2f),
             Random.Range(SelectionStandard, Random.Range(SelectionStandard, Random.Range(SelectionStandard, 1000f))));
        newWorker.LeaveFactoryList = new List<string>();

        UnHiredList.Add(newWorker);
        CurrentWorkerIndex++;

        WorkerGenerateCounter = Random.Range(1000, 3000);

        // Debug.Log("Name:"+ newWorker.Name + " LaborForce:" + newWorker.LaborForce + " LossRate:" + newWorker.LaborForceLossRate + " SincerityRate:" + newWorker.SincerityRate
        //     + " Salary:" + newWorker.Salary + " TrialCount:" + newWorker.TrialCount + " ExpectValue:" + newWorker.ExpectCompanyValue);
    }

    public Worker AddCustomWorker(string Name, int Force, int Salary, float LossRate, float GainRate, int TrialCount, float ExpectValue)
    {
        Worker newWorker = new Worker();

        if(Name == null)
        {
            Name = GetRandomName();
        }
        newWorker.index = CurrentWorkerIndex;
        newWorker.Name = Name;
        newWorker.LaborForce = Force;
        newWorker.Salary = Salary;
        newWorker.LaborForceLossRate = LossRate;
        newWorker.SincerityRate = GainRate;
        newWorker.TrialCount = TrialCount;
        newWorker.ExpectCompanyValue = ExpectValue;
        newWorker.LeaveFactoryList = new List<string>();

        UnHiredList.Add(newWorker);

        CurrentWorkerIndex++;

        return newWorker;
    }

    void SelectCompany()
    {
        List<Worker> TargetWorker = new List<Worker>();
        foreach(var worker in UnHiredList)
        {
            List<GameObject> SelectedCompany = new List<GameObject>();
            foreach(var company in CompanyManagerCall.GetAllCompanyObject())
            {
                CompanyValue TempCompanyValueCall = company.GetComponent<CompanyValue>();
                if(TempCompanyValueCall.TotalValue >= worker.ExpectCompanyValue && TempCompanyValueCall.TotalValue != 0)
                {
                    foreach(var CompanyName in worker.LeaveFactoryList)
                    {
                        if(TempCompanyValueCall.CompanyName == CompanyName)
                        {
                            continue;
                        }
                    }
                    SelectedCompany.Add(company);
                }
            }

            if(SelectedCompany.Count > 0)
            {
                CompanyValue CompanyValueCall = SelectedCompany[Random.Range(0, SelectedCompany.Count - 1)].GetComponent<CompanyValue>();
                EmployeeValue EmployeeValueCall = CompanyValueCall.GetEmployeeValue().GetComponent<EmployeeValue>();

                int HiringRate = 2;

                if(EmployeeValueCall.RequiredLabor <= EmployeeValueCall.TotalLabor)
                    HiringRate *= 10 * Mathf.CeilToInt(EmployeeValueCall.TotalLabor - EmployeeValueCall.RequiredLabor + 1);
                if(CompanyValueCall.TotalValue > worker.ExpectCompanyValue)
                    HiringRate *= 1 * Mathf.CeilToInt(CompanyValueCall.TotalValue / worker.ExpectCompanyValue);
                if(CompanyValueCall.TotalValue <= worker.ExpectCompanyValue)
                    HiringRate *= 4 * Mathf.CeilToInt(worker.ExpectCompanyValue / CompanyValueCall.TotalValue);

                if(Random.Range(0, HiringRate) == 0)
                {
                    EmployeeValueCall.HireEmployee(worker);
                    TargetWorker.Add(worker);
                    // Debug.Log(worker.Name + " is Hired on " + CompanyValueCall.CompanyName);
                }
                // else Debug.Log(worker.Name + " doesnt go anywhere");
            }
            
            if(--worker.TrialCount < 0)
            {
                TargetWorker.Add(worker);
            }
            else
            {
                worker.ExpectCompanyValue -= 0.5f;
            }
        }

        for(int i = 0; i < TargetWorker.Count; i++)
        {
            UnHiredList.Remove(TargetWorker[i]);
        }
    }

    string GetRandomName()
    {
        string result = null;
        List<string> FirstName = new List<string>();
        List<string> LastName = new List<string>();

        FirstName.Add("Alice"); FirstName.Add("Eva"); FirstName.Add("Hestia"); FirstName.Add("Betty"); FirstName.Add("Camilla"); FirstName.Add("Coy"); FirstName.Add("Della");
        FirstName.Add("Jessie"); FirstName.Add("Katherine"); FirstName.Add("Lily"); FirstName.Add("Maya"); FirstName.Add("Pandora"); FirstName.Add("Sarah"); FirstName.Add("Rudy");
        FirstName.Add("Aaron"); FirstName.Add("Beck"); FirstName.Add("Charles"); FirstName.Add("Daniel"); FirstName.Add("Dave"); FirstName.Add("Edward"); FirstName.Add("Gilbert"); 
        FirstName.Add("Hue"); FirstName.Add("Jacob"); FirstName.Add("Lonnie"); FirstName.Add("Pablo"); FirstName.Add("Paul"); FirstName.Add("Sparky"); FirstName.Add("Vincent");
        LastName.Add("James"); LastName.Add("Smith"); LastName.Add("Oliver"); LastName.Add("Williams"); LastName.Add("Taylor"); LastName.Add("Brown"); LastName.Add("Davies");
        LastName.Add("Allen"); LastName.Add("Powel"); LastName.Add("Green"); LastName.Add("Thomas"); LastName.Add("Hudson"); LastName.Add("Black"); LastName.Add("Harries");

        result = FirstName[Random.Range(0, FirstName.Count - 1)].ToString() + " " + LastName[Random.Range(0, LastName.Count - 1)].ToString();

        return result;
    }
}
