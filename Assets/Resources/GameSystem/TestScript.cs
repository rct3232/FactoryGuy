using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    CompanyValue CallCompanyValue;
    // Start is called before the first frame update
    void Start()
    {
        CallCompanyValue = GameObject.Find("CompanyMananger").GetComponent<CompanyManager>().GetPlayerCompanyValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void CompanyValueUP()
    // {
    //     CallCompanyValue.
    // }
}
