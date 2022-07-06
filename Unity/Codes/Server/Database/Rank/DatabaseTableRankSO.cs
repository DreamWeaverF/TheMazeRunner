using MessagePack;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace TheMazeRunner
{
    [AutoGenSO(GenPath = "Database/Table/Rank")]
    public class DatabaseTableRankSO : ADatabaseTable<int, byte[]> 
    {
        public string TestXXX { get; set; }

        protected override string tableName
        {
            get
            {
                return "t_ranks";
            }
        }

        protected override string keyName1
        {
            get
            {
                return "rankId";
            }
        }

        protected override string keyName2
        {
            get
            {
                return "rankData";
            }
        }

        public void Testxxx111()
        {

        }

        public async ETTask<RankData> SelectDatabase(int _rankId)
        {
            byte[] _bytes = await SelectCmd(_rankId);
            return MessagePackSerializer.Deserialize<RankData>(_bytes);
        }

        public async ETTask<bool> InsertIntoDatabase(RankData _rankData)
        {
            int _rankId = _rankData.RankID;
            byte[] _bytes = MessagePackSerializer.Serialize(_rankData);
            return await InsertIntoCmd(_rankId, _bytes);
        }

        public async ETTask<bool> UpdateDatabase(RankData _rankData)
        {
            int _rankId = _rankData.RankID;
            byte[] _bytes = MessagePackSerializer.Serialize(_rankData);
            return await UpdateCmd(_rankId, _bytes);
        }
    }
}
