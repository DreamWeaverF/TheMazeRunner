using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    public class GateProxySession
    {
        private long UserId = 100000;
        private TChannel channel;
        private MemoryStream memoryStream = new MemoryStream();

        public async Task SendAsync<T1>(T1 _message)
        {
            byte[] _bytes = BitConverter.GetBytes(UserId);

            memoryStream.Position = 0;
            memoryStream.SetLength(sizeof(long));
            memoryStream.GetBuffer().WriteTo(0, UserId);
            memoryStream.Seek(sizeof(long), SeekOrigin.Begin);
            await MessagePackSerializer.SerializeAsync(memoryStream, _message);
            memoryStream.Seek(0, SeekOrigin.Begin);
            channel.Send(memoryStream);

            await memoryStream.ReadAsync(_bytes, 0, sizeof(long));
            long _userID = BitConverter.ToInt64(_bytes,0);
            T1 _outMessage = await MessagePackSerializer.DeserializeAsync<T1>(memoryStream);


        }
    }
}
