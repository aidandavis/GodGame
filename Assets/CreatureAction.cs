using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAction {

	 public enum Action
        {
            wait, move, turn, fight, multiply
        }
        Action action;
        /* extraInfo is only relevant for turn and for fight.
         * turn: degrees to turn, between 0 and 359 clockwise
         * fight: energy to invest */
        long extraInfo;

        public CreatureAction(Action action, long extraInfo)
        {
            this.action = action;
            if (action == Action.turn) // turn is between 0 and 359
            {
                if (extraInfo < 0)
                {
                    extraInfo = 0;
                }
                else if (extraInfo > 359)
                {
                    extraInfo = 359;
                }
            }
            this.extraInfo = extraInfo;
        }

        public Action GetAction()
        {
            return action;
        }

        public long GetExtraInfo()
        {
            return extraInfo;
        }
}
