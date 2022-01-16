using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMazeRunner
{
    public abstract class AMessageNotice : IMessage
    {
        [Key(1000)]
        public ushort Opcode { get; set; }
    }
}
