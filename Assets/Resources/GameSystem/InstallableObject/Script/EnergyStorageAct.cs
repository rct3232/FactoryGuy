using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStorageAct : MonoBehaviour
{
    public float StorableElectricity;
    public float StoredElectricity;
    public float CurrentChargingAmount;
    InstallableObjectAct ObjectActCall;
    CompanyValue CompanyValueCall;
    ElectricityValue ElectricityValueCall;

    // Start is called before the first frame update
    void Start()
    {
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        CompanyValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue();
        ElectricityValueCall = CompanyValueCall.GetElectricityValue().GetComponent<ElectricityValue>();

        CurrentChargingAmount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectActCall.isInstall)
        {
            if(ElectricityValueCall.Lack)
            {
                if(StoredElectricity < StorableElectricity)
                {
                    CurrentChargingAmount = ObjectActCall.Value.SuppliedElectricity * (ObjectActCall.Value.SuppliedLabor / ObjectActCall.Info.LaborRequirement);
                    StoredElectricity += CurrentChargingAmount;
                }
                else if(StoredElectricity > StorableElectricity)
                {
                    StoredElectricity = StorableElectricity;
                }
            }
            else
            {
                if(StoredElectricity > 0f)
                {
                    StoredElectricity -= (ElectricityValueCall.TotalUsage - ElectricityValueCall.AvailableElectricityAmount 
                        - ElectricityValueCall.TotalStoredAmount) * (1 / ElectricityValueCall.StoreObjectCount);
                }
                if(StoredElectricity < 0f)
                {
                    StoredElectricity = 0f;
                }
            }
        }
    }

    public bool DeleteObject()
    {
        return true;
    }
}
