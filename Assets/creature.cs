using System;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public CreatureAction EnvironmentMethod(List<DistanceToOtherCreatures> proximityList) {
        if (AtTree())
        {
            return new CreatureAction(CreatureAction.Action.wait, 0);
        }
        else if (CloseToTree())
        {
            _body.LookAt(tree.transform);
            _body.Translate(Vector3.forward * moveSpeedFast * Time.deltaTime);
        }
        else
        {
            _body.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    bool AtTree()
    {
        return (Vector3.Distance(tree.transform.position, transform.position) < 0.2f);
    }

    bool CloseToTree()
    {
        return (Vector3.Distance(tree.transform.position, transform.position) < TREE_PROXIMITY_THRESHOLD);
    }

    void OnCollisionEnter(Collision collision)
    {
        String otherObject = collision.gameObject.tag;
        if (otherObject == "wall")
        {
            _body.RotateAround(_body.position, _body.up, 150f);
        }
    }

    //returns true when pointing at tree
}
