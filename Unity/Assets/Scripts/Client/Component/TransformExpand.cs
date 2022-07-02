using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public static class TransformExpand
    {
        public static Transform FindTargetChild(this Transform parent,string targetName)
        {
            Transform temp = parent.Find(targetName);
            if(temp != null)
            {
                return temp;
            }
            for(int i = 0; i < parent.childCount; i++)
            {
                temp = parent.GetChild(i).FindTargetChild(targetName);
                if(temp != null)
                {
                    return temp;
                }
            }
            return null;
        }
    }
}
