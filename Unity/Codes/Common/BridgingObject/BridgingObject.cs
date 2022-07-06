using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public class BridgingObject 
    {
        private static BridgingObject instance;
        public BridgingObject()
        {
            if(instance == null)
            {
                instance = new BridgingObject();
            }
        }

    }
}
