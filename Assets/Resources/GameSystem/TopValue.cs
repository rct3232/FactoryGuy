using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopValue : MonoBehaviour
{
    public static TopValue TopValueSingleton;
    public int MapSize = 0;
    public float UIScale;

    // Start is called before the first frame update
    void Awake()
    {
        if(TopValueSingleton != null)
        {
            Destroy(this.gameObject);
            return;
        }
        TopValueSingleton = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

        if (MapSize == 0)
        {
            MapSize = 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
