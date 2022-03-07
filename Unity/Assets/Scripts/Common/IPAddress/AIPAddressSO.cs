using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace TheMazeRunner
{
    public abstract class AIPAddressSO : ScriptableObject
    {
        [SerializeField]
        private string DevOuterHost;
        [SerializeField]
        private string DevInnerHost;
        [SerializeField]
        private int DevPort;
        [SerializeField]
        private string QaOuterHost;
        [SerializeField]
        private string QaInnerHost;
        [SerializeField]
        private int QaPort;
        [SerializeField]
        private string ReleaseOuterHost;
        [SerializeField]
        private string ReleaseInnerHost;
        [SerializeField]
        private int ReleasePort;
    }
}
