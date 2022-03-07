using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheMazeRunner
{
    public enum EnumServiceType
    {
        None = 0,
        Client = 2,
        GateServer = 4,
        LoginServer = 8,
        CenterServer = 16,
        UserServer = 32,
        ChatServer = 64,
        MailServer = 128,
        FightServer = 256,
    }
}
