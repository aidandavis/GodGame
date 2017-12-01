using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToOtherCreatures
{
    public string otherCreatureTag;
    public float distance;

    public DistanceToOtherCreatures(string otherCreature, float distance)
    {
        this.otherCreatureTag = otherCreature;
        this.distance = distance;
    }

    public string GetOtherCreatureTag()
    {
        return otherCreatureTag;
    }

    public float GetDistance()
    {
        return distance;
    }
}
