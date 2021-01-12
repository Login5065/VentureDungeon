using Dungeon.Variables;
using System.Collections;
using Dungeon.Creatures;

using System.Collections.Generic;
using UnityEngine;

public class FameEvent : EventTemplate
{
    public int Requirment = 75;
    public int addition = 75;
    public int importance = 2;
    public int time = 0;
    public override bool Check()
    {
        if (Dungeon.Variables.GameData.threat >= Requirment) {
            Requirment += addition;
            return true;
        }
        return false;

    }

    public override void Disable()
    {
        throw new System.NotImplementedException();
    }

    public override void Enable()
    {
        time++;
        for (int i = 0; i < time; i++) {
            Dungeon.Variables.GameData.gold += 1000*time;
        }
       
    }

    public override int getImportance()
    {
        return importance;
    }


}
