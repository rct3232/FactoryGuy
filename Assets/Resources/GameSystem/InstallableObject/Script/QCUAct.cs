using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QCUAct : MonoBehaviour
{
    GoodsValue GoodsValueCall;
    AddonObjectAct AddonObjectActCall;
    InstallableObjectAct ObjectActCall;
    public GameObject Distributor;
    DistributorAct DistributorCall;
    public GameObject DistributorDetector;
    public bool isInitialized;
    int DistributeLength;
    public float[] TargetQuality;
    int LR;

    // Start is called before the first frame update
    void Start()
    {
        GoodsValueCall = GameObject.Find("CompanyManager").GetComponent<CompanyManager>().GetPlayerCompanyValue().GetGoodsValue().GetComponent<GoodsValue>();
        AddonObjectActCall = gameObject.GetComponent<AddonObjectAct>();
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();

        LR = 0;
        isInitialized = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!gameObject.GetComponent<InstallableObjectAct>().isInstall)
        {
            GetDistributor();

            if(Distributor == null)
            {
                gameObject.GetComponent<InstallableObjectAct>().CanInstall = false;
            }
            else
            {
                gameObject.GetComponent<InstallableObjectAct>().CanInstall = true;
            }
        }

        if (gameObject.GetComponent<InstallableObjectAct>().isInstall && isInitialized)
        {
            ObjectActCall.IsWorking = true;
            if(Distributor == null)
            {
                isInitialized = false;
                return;
            }
            if(AddonObjectActCall.Tic)
            {
                CheckQuality();
                AddonObjectActCall.Tic = false;
            }
        }
        else if(gameObject.GetComponent<InstallableObjectAct>().isInstall && !isInitialized)
        {
            ObjectActCall.IsWorking = false;
        }
    }

    void GetDistributor()
    {
        if(Distributor != DistributorDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject)
        {
            if (DistributorDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                Distributor = DistributorDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.transform.parent.parent.gameObject;
                DistributorCall = Distributor.GetComponent<DistributorAct>();
                DistributeLength = DistributorCall.Mover.Length;
                DistributorCall.AddonObject = gameObject;
            }
            else
            {
                if (Distributor != null)
                {
                    DistributorCall.AddonObject = null;
                }
                DistributorCall = null;
                Distributor = null;
                DistributeLength = -1;
                isInitialized = false;
            }
        }
    }

    public void Initializing(int LR, float[] Quality)
    {
        if(Distributor == null)
        {
            Debug.Log("No Attached Distributor!");
            return;
        }

        TargetQuality = new float[DistributeLength];

        this.LR = LR;
        for (int i = 0; i < DistributeLength; i++)
        {
            TargetQuality[i] = Quality[i];
        }

        isInitialized = true;
    }

    void CheckQuality()
    {        
        if(LR == 0)
        {
            for(int i = 0; i < DistributeLength; i++)
            {
                if(GoodsValueCall.CheckQuality(DistributorCall.PrevBelt.GetComponent<BeltAct>().GoodsOnBelt) > TargetQuality[i])
                {                    
                    DistributorCall.DistributeIndex = i;
                    DistributorCall.GetIndex = true;
                    return;
                }
            }
        }
        else
        {
            for(int i = DistributeLength - 1; i >= 0; i--)
            {
                if(GoodsValueCall.CheckQuality(DistributorCall.PrevBelt.GetComponent<BeltAct>().GoodsOnBelt) > TargetQuality[i])
                {
                    DistributorCall.DistributeIndex = i;
                    DistributorCall.GetIndex = true;
                    return;
                }
            }
        }
    }

    public bool DeleteObject()
    {
        DistributorCall.AddonObject = null;
        return true;
    }
}
