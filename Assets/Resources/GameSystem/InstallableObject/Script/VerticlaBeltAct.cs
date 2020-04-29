using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticlaBeltAct : MonoBehaviour
{
    public GameObject Mover;
    public GameObject PrevBelt;
    public GameObject PrevBeltDetector;
    public bool isInitialized = false;
    GameObject TargetGoods;
    InstallableObjectAct ObjectActCall;
    BeltAct BeltActCall;
    BeltAct PrevBeltActCall;

    // Start is called before the first frame update
    void Start()
    {
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectActCall.isInstall && isInitialized)
        {
            ObjectActCall.IsWorking = true;

            MovingGoods();
        }
        else ObjectActCall.IsWorking = false;
    }

    public void Initializing()
    {
        if(Mover == null || PrevBelt == null) return;

        BeltActCall = Mover.GetComponent<BeltAct>();
        PrevBeltActCall = PrevBelt.GetComponent<BeltAct>();

        isInitialized = true;
    }

    void MovingGoods()
    {
        if(TargetGoods == null)
        {
            if(PrevBeltActCall.GoodsOnBelt != null)
            {
                TargetGoods = PrevBeltActCall.GoodsOnBelt;
            }
        }
        else
        {
            if(BeltActCall.GoodsOnBelt == null)
            {
                if(PrevBeltActCall.isStop)
                {
                    PrevBeltActCall.GoodsOnBelt.transform.position = 
                        new Vector3(Mover.transform.position.x, Mover.transform.position.y + 0.325f, Mover.transform.position.z);
                    PrevBeltActCall.GoodsOnBelt = null;

                    TargetGoods = null;
                }
                
            }
        }
    }

    public bool DeleteObject()
    {
        if(Mover != null && PrevBelt != null)
        {
            Mover.GetComponent<BeltAct>().ChangePrevBelt(null);
            PrevBelt.GetComponent<BeltAct>().ChangeNextBelt(null);   
        }
        return true;
    }
}
