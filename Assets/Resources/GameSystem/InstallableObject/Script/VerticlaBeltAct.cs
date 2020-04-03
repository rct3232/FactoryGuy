using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticlaBeltAct : MonoBehaviour
{
    public GameObject Mover;
    public GameObject PrevBelt;
    public GameObject PrevBeltDetector;
    GameObject TargetGoods;
    InstallableObjectAct ObjectActCall;
    BeltAct BeltActCall;
    BeltAct PrevBeltActCall;

    // Start is called before the first frame update
    void Start()
    {
        ObjectActCall = gameObject.GetComponent<InstallableObjectAct>();
        BeltActCall = Mover.GetComponent<BeltAct>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ObjectActCall.isInstall)
        {
            ObjectActCall.IsWorking = true;

            MovingGoods();
        }
        else
        {
            GetPrevBelt();
            if (PrevBelt == null)
            {
                ObjectActCall.CanInstall = false;
            }
            else
            {
                ObjectActCall.CanInstall = true;
            }
        }
    }

    void GetPrevBelt()
    {
        if (PrevBelt != PrevBeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject)
        {
            if (PrevBeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject != null)
            {
                if(PrevBeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject.transform.parent.parent.gameObject.GetComponent<InstallableObjectAct>().StructObject.layer == 23)
                {
                    if(PrevBelt != null)
                    {
                        PrevBeltActCall.NextBelt = null;
                        PrevBeltActCall.BeltDetector = PrevBelt.transform.GetChild(0).gameObject;
                        PrevBeltActCall.DetectingEnd();
                    }

                    PrevBelt = PrevBeltDetector.GetComponent<ObjectAttachmentDetector>().DetectedObject;

                    if(PrevBelt.GetComponent<BeltAct>().NextBelt == null)
                    {
                        PrevBeltActCall = PrevBelt.GetComponent<BeltAct>();
                        PrevBeltActCall.NextBelt = Mover;
                        PrevBeltActCall.BeltDetector = null;
                    }
                    else
                    {
                        PrevBelt = null;
                    }
                }
                else
                {
                    if(PrevBelt != null)
                    {
                        PrevBeltActCall.NextBelt = null;
                        PrevBeltActCall.BeltDetector = PrevBelt.transform.GetChild(0).gameObject;
                        PrevBeltActCall.DetectingEnd();
                    }

                    PrevBelt = null;
                }
                
            }
            else
            {
                if(PrevBelt != null)
                {
                    PrevBeltActCall.NextBelt = null;
                    PrevBeltActCall.BeltDetector = PrevBelt.transform.GetChild(0).gameObject;
                    PrevBeltActCall.DetectingEnd();
                }

                PrevBelt = null;
            }
        }
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
        Mover.GetComponent<BeltAct>().DeteleBelt();
        return true;
    }
}
