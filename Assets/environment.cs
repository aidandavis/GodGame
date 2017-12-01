using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class environment : MonoBehaviour
{

    List<EnvCreatureObject> creatureList = new List<EnvCreatureObject>();

    long startingEnergy = 500;
    float moveSpeed = 0.5f;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject creature in GameObject.FindGameObjectsWithTag("creature"))
        {
            creatureList.Add(new EnvCreatureObject(creature, startingEnergy));
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateDistances();

        foreach (EnvCreatureObject creature in creatureList) // interactions with environment (for now only life, death and tree)
        {
            if (creature.energy < 1)
            {
                Destroy(creature.thisCreature); // die
                continue;
            }
            if (CreatureAtTree(creature))
            {
                creature.energy += 1;
            }
            if (creature.investedReproductionEnergy == 50)
            {
                creature.investedReproductionEnergy = 0;
                Transform currentPos = creature.thisCreature.transform;
                Instantiate(creature.thisCreature, currentPos.position + currentPos.forward * 1, currentPos.rotation);
            }
        }

        List<EnvCreatureAction> creatureActions = new List<EnvCreatureAction>();
        foreach (EnvCreatureObject creature in creatureList)
        {
            // build a list of objects within 10m (get object distance, if less than 10 then pass it)
            List<DistanceToOtherCreatures> proximityList = new List<DistanceToOtherCreatures>();

            foreach (EnvDistanceToOtherCreaturesObject distanceObject in creature.distanceToOtherCreatures)
            {
                if (distanceObject.distance < 10)
                {
                    proximityList.Add(new DistanceToOtherCreatures(distanceObject.otherCreature.thisCreature.tag, distanceObject.distance));
                }
            }

            // call the environment method on this object which returns an action object indicating what the creature will do, add to list
            creatureActions.Add(new EnvCreatureAction(creature.thisCreature.GetComponent<Creature>().EnvironmentMethod(proximityList), creature));
        }

        // perform actions
        foreach (EnvCreatureAction actionByCreature in creatureActions)
        {
            foreach (EnvCreatureObject thisCreature in creatureList) { 
                if (!thisCreature.Equals(actionByCreature.GetCreaturePerforming()))
                {
                    continue; // get the creature performing the action
                }
                Transform thisCreatureTransform = thisCreature.thisCreature.transform;
                switch(actionByCreature.GetCreatureAction().GetAction()) {
                    case CreatureAction.Action.multiply:
                        thisCreature.energy -= 1;
                        thisCreature.investedReproductionEnergy += 1;
                        break;
                    case CreatureAction.Action.wait:
                        thisCreature.investedReproductionEnergy = 0;
                        break;
                    case CreatureAction.Action.move:
                        thisCreature.energy -= 1;
                        thisCreatureTransform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                        break;
                    case CreatureAction.Action.turn:
                        thisCreature.energy -= 1;
                        thisCreatureTransform.RotateAround(thisCreatureTransform.position, thisCreatureTransform.up, actionByCreature.GetCreatureAction().GetExtraInfo());
                        break;
                }

                // for fight, only works if within radius
            }
        }
    }

    private bool CreatureAtTree(EnvCreatureObject creature)
    {
        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("tree")) // could be many trees
        {
            if (Vector3.Distance(creature.thisCreature.transform.position, tree.transform.position) < 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    private void CalculateDistances()
    {
        foreach (EnvCreatureObject creature in creatureList)
        {
            foreach (EnvCreatureObject otherCreature in creatureList)
            {
                if (creature.thisCreature.Equals(otherCreature.thisCreature))
                {
                    continue;
                }
                creature.distanceToOtherCreatures.Add(new EnvDistanceToOtherCreaturesObject(otherCreature,
                    Vector3.Distance(creature.thisCreature.transform.position, otherCreature.thisCreature.transform.position)));
            }
        }
    }

    private class EnvCreatureAction
    {
        CreatureAction action;
        EnvCreatureObject creaturePerformingAction;

        public EnvCreatureAction(CreatureAction action, EnvCreatureObject thisCreature)
        {
            this.action = action;
            creaturePerformingAction = thisCreature;
        }

        public CreatureAction GetCreatureAction ()
        {
            return action;
        }

        public EnvCreatureObject GetCreaturePerforming()
        {
            return creaturePerformingAction;
        }
    }

    private class EnvCreatureObject
    {
        public long energy;
        public GameObject thisCreature;
        public List<EnvDistanceToOtherCreaturesObject> distanceToOtherCreatures;
        public long investedReproductionEnergy;

        public EnvCreatureObject(GameObject thisCreature, long energy)
        {
            this.thisCreature = thisCreature;
            this.energy = energy;
        }
    }

    private class EnvDistanceToOtherCreaturesObject
    {
        public EnvCreatureObject otherCreature;
        public float distance;

        public EnvDistanceToOtherCreaturesObject(EnvCreatureObject otherCreature, float distance)
        {
            this.otherCreature = otherCreature;
            this.distance = distance;
        }
    }
}
