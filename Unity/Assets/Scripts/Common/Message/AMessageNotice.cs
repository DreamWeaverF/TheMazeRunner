using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMazeRunner
{
    [MessagePackObject]
    public abstract class AMessageNotice
    {
        [Key(1000)]
        public ushort Opcode { get; set; }
    }
}
