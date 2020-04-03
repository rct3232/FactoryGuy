using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttachmentDetector : MonoBehaviour
{
    public int TargetLayerNum;
    // Layer 0 : All Object EXCEPT itself and detector
    public GameObject DetectedObject;
    public bool DoNotCallStay = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider Col)
    {
        if (TargetLayerNum != 0)
        {
            if (Col.gameObject.layer == TargetLayerNum)
            {
                if (Col.GetComponent<InstallableObjectAct>() != null)
                {
                    if (Col.GetComponent<InstallableObjectAct>().isInstall)
                    {
                        DetectedObject = Col.gameObject;
                    }
                }
                else
                {
                    DetectedObject = Col.gameObject;
                }
            }
        }
        else
        {
            Transform[] This = transform.parent.gameObject.GetComponentsInChildren<Transform>();
            foreach(var tmp in This)
            {
                if (Col.gameObject == tmp.gameObject)
                    return;
            }
            if (Col.gameObject.layer == 14)
                return;
            if (Col.GetComponent<InstallableObjectAct>() != null)
            {
                if (Col.GetComponent<InstallableObjectAct>().isInstall)
                {
                    DetectedObject = Col.gameObject;
                }
            }
            else
            {
                DetectedObject = Col.gameObject;
            }
        }
    }

    void OnTriggerStay(Collider Col)
    {
        if(!DoNotCallStay)
        {
            if (TargetLayerNum != 0)
            {
                if (Col.gameObject != DetectedObject)
                {
                    if (Col.gameObject.layer == TargetLayerNum)
                    {
                        if (Col.GetComponent<InstallableObjectAct>() != null)
                        {
                            if (Col.GetComponent<InstallableObjectAct>().isInstall)
                            {
                                DetectedObject = Col.gameObject;
                            }
                        }
                        else
                        {
                            DetectedObject = Col.gameObject;
                        }
                    }
                }
            }
            else
            {
                Transform[] This = transform.parent.gameObject.GetComponentsInChildren<Transform>();
                foreach (var tmp in This)
                {
                    if (Col.gameObject == tmp.gameObject)
                        return;
                }
                if (Col.gameObject.layer == 14)
                    return;
                if (Col.gameObject != DetectedObject)
                {
                    if (Col.GetComponent<InstallableObjectAct>() != null)
                    {
                        if (Col.GetComponent<InstallableObjectAct>().isInstall)
                        {
                            DetectedObject = Col.gameObject;
                        }
                    }
                    else
                    {
                        DetectedObject = Col.gameObject;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider Col)
    {
        if(Col.gameObject == DetectedObject)
        {
            DetectedObject = null;
        }
    }
}
