using MessagePack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    [MessagePackObject]
    public class RankData
    {
        [Key(1000)]
        public int RankID { get; set; }
        [Key(1001)]
        public List<long> RankList { get; set; }
        [Key(1002)]
        public long PreUpdateTime { get; set; }

    }
}
