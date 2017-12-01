using System;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public CreatureAction EnvironmentMethod(List<DistanceToOtherCreatures> proximityList)
    {
        bool isTree = false;
        float distanceToTree = 50;
        float angleToTree = 0;

        foreach (DistanceToOtherCreatures distanceObject in proximityList)
        {
            if (distanceObject.otherCreatureTag.Equals("tree"))
            {
                isTree = true;
                distanceToTree = distanceObject.distance;
                angleToTree = distanceObject.angle;
            } else
            {
                isTree = false;
            }
        }

        if (isTree)
        {
            if (distanceToTree > 2)
            {
                if (angleToTree < 20)
                {
                    return new CreatureAction(CreatureAction.Action.move, 0);
                }
                else
                {
                    return new CreatureAction(CreatureAction.Action.turn, Convert.ToInt64(angleToTree));
                }
            }
            else if (distanceToTree > 0.9)
            {
                if (angleToTree < 50)
                {
                    return new CreatureAction(CreatureAction.Action.move, 0);
                } else
                {
                    return new CreatureAction(CreatureAction.Action.turn, Convert.ToInt64(angleToTree));
                }
            } else
            {
                return new CreatureAction(CreatureAction.Action.multiply, 0);
            }
        } else
        {
            return new CreatureAction(CreatureAction.Action.move, 0);
        }
    }
}
