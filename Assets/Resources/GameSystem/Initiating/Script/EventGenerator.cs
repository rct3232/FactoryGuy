using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    public class EventVariableInfo
    {
        EventVariableInfo(){}
        public string Name;
        public int Direction;
    }
    public class EventInfo
    {
        EventInfo(EventVariableInfo targetVar, int advantage, int changeMin, int changeMax, int durability)
        {
            this.TargetVar = targetVar;
            this.Advantage = advantage;
            this.ChangeValueRange = new int[2];
            this.ChangeValueRange[0] = changeMin;
            this.ChangeValueRange[1] = changeMax;
            this.Durability = durability;
        }
        private EventVariableInfo TargetVar;
        private int Advantage;
        private int[] ChangeValueRange;
        private int Durability;

        public EventVariableInfo GetTargetVar() {return this.TargetVar;}
        public int GetAdvantage() {return this.Advantage;}
        public int[] GetChangeValueRange() {return this.ChangeValueRange;}
        public int GetDurability() {return this.Durability;}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
