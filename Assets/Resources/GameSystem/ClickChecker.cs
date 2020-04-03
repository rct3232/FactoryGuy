using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickChecker : MonoBehaviour
{
    InGameValue ValueCall;
    public GameObject target = null;
    public GameObject ui = null;
    public bool MouseOnUI;
    public LayerMask NormalMask;
    public LayerMask TileMask;
    public LayerMask GroupMask;
    GraphicRaycaster GraphicRaycasterCall;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(NormalMask.value);
        ValueCall = this.GetComponent<InGameValue>();
        GraphicRaycasterCall = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask ClickMask = NormalMask;
        if(ValueCall.AttachedOnMouse != null && ValueCall.ModeBit[0])
        {
            ClickMask = TileMask;
        }
        if(ValueCall.ModeBit[2])
        {
            ClickMask = GroupMask;
        }

        RaycastHit hit;
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (true == (Physics.Raycast(ray, out hit, 1000f, ClickMask)))
            {
                target = hit.collider.gameObject;
                // Debug.Log("Target is " + target);
            }
            else
            {
                target = null;
                //Debug.Log("Target has been Cleared");
            }
            ui = null;
            MouseOnUI = false;
        }
        else
        {
            var ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            GraphicRaycasterCall.Raycast(ped, results);

            if (results.Count > 0) ui = results[0].gameObject;
            target = null;
            MouseOnUI = true;
        }
        
        ////Pointer Debugging Section
        //if (target == null)
        //{
        //    Debug.Log("target is Empty");
        //}
        //else
        //{
        //    Debug.Log("target is " + target + " " + target.layer);
        //}
    }
}
