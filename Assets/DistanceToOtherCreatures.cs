using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToOtherCreatures
{
    public string otherCreatureTag;
    public float distance;
    public float angle;

    public DistanceToOtherCreatures(string otherCreature, float distance, float angle)
    {
        otherCreatureTag = otherCreature;
        this.distance = distance;
        this.angle = angle;
    }
}
