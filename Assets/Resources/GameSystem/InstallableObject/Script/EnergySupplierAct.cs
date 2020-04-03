using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySupplierAct : MonoBehaviour
{
    InstallableObjectAct ObjectActCall;
    CompanyValue CompanyValueCall;
    public float InputCapacity;
    bool isInitialized = false;
    // Start is called before the first frame update
    void Start()
    {
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        CompanyValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectActCall.isInstall && !isInitialized)
        {
            isInitialized = true;
            ObjectActCall.IsWorking = true;
            CompanyValueCall.GetElectricityValue().GetComponent<ElectricityValue>().AddElectricityInput(gameObject);
        }
    }

    public bool DeleteObject()
    {
        CompanyValueCall.GetGoodsValue().GetComponent<ElectricityValue>().DeleteElectricityInput(gameObject);

        return true;
    }
}