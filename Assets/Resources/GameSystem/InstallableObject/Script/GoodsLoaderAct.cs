using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsLoaderAct : MonoBehaviour
{
    GameObject BaseSystem;
    public GameObject Mover;
    BeltAct BeltActCall;
    InstallableObjectAct ObjectActCall;
    CompanyManager CompanyManagerCall;
    SalesValue SalesValueCall;
    GoodsValue GoodsValueCall;
    GameObject MoverDetector;
    GameObject WarehouseDetector;
    bool isInitialized;
    int DoorDirection = -1;

    // Start is called before the first frame update
    void Start()
    {
        BaseSystem = GameObject.Find("BaseSystem");
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        CompanyManagerCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>();
        GoodsValueCall = CompanyManagerCall.GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        SalesValueCall = GameObject.Find("SalesManager").GetComponent<SalesValue>();

        MoverDetector = transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        WarehouseDetector = transform.GetChild(2).GetChild(1).GetChild(0).gameObject;

        isInitialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.GetComponent<InstallableObjectAct>().isInstall)
        {
            GetBelt();
            isInitialized = true;

            if(Mover == null)
            {
                isInitialized = false;
            }

            if(isInitialized)
            {
                LoadToWarehouse();
            }
        }
        else
        {
            ObjectActCall.CanInstall = CheckInstallCondition();
        }
    }

    void CheckDirection()
    {
        if (gameObject.transform.eulerAngles.y == 0)
        {
            DoorDirection = 0;
        }
        else if (gameObject.transform.eulerAngles.y == 90)
        {
            DoorDirection = 1;
        }
        else if (gameObject.transform.eulerAngles.y == 180)
        {
            DoorDirection = 2;
        }
        else if (gameObject.transform.eulerAngles.y == 270)
        {
            DoorDirection = 3;
        }
    }

    bool CheckInstallCondition()
    {
        bool result = true;

        CheckDirection();

        if(MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
        {
            BeltAct PrevBeltActCall = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
            if(PrevBeltActCall.BeltDirection - DoorDirection != 2 && PrevBeltActCall.BeltDirection - DoorDirection != -2)
            {
                result = false;
            }
        }

        if(WarehouseDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject == null)
        {
            result = false;
        }

        return result;
    }

    void GetBelt()
    {
        if (Mover != MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject)
        {
            // If mover has been attached (or changed) and mover's direction is same as Door's
            // Initialize the belt info and stop the belt
            if (MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                BeltAct BeltActCallTemp = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.GetComponent<BeltAct>();
                if(!BeltActCallTemp.ParentObject.GetComponent<InstallableObjectAct>().isInstall)
                {
                    // If detected belt is not installed
                    // Door will not work
                    Mover = null;
                    if (BeltActCallTemp.BeltDirection - DoorDirection == 2 || BeltActCallTemp.BeltDirection - DoorDirection == -2)
                    {
                        BeltActCallTemp.ModuleCondtion = true;   
                    }
                    else
                    {
                        // If detected mover's direction is not same as Door
                        // Cannot install mover
                        BeltActCallTemp.ModuleCondtion = false;
                    }
                }
                else
                {
                    if (BeltActCallTemp.BeltDirection - DoorDirection == 2 || BeltActCallTemp.BeltDirection - DoorDirection == -2)
                    {
                        Mover = MoverDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject;
                        BeltActCall = BeltActCallTemp;
                    }
                    else
                    {
                        // If detected mover's direction is not same as Door
                        // Door will not work
                        Mover = null;
                    }
                }
            }
            else
            {
                // If there is no mover
                // Door will not work
                Mover = null;
            }
        }
    }

    void LoadToWarehouse()
    {
        if(BeltActCall.GoodsOnBelt != null)
        {
            ObjectActCall.IsWorking = true;

            if (GoodsValueCall.CheckMovingState(int.Parse(BeltActCall.GoodsOnBelt.name)) == 0)
            {
                SalesValueCall.UpdateItemCount(CompanyManagerCall.PlayerCompanyName, GoodsValueCall.FindGoodsName(BeltActCall.GoodsOnBelt));

                GoodsValueCall.ChangeInMapState(int.Parse(BeltActCall.GoodsOnBelt.name), false, null);
                Destroy(BeltActCall.GoodsOnBelt);
                BeltActCall.GoodsOnBelt = null;
            }
        }
        else
        {
            ObjectActCall.IsWorking = false;
        }
    }

    public bool DeleteObject()
    {
        return true;
    }
}