using UnityEngine;

namespace TheMazeRunner
{
    [CreateAssetMenu(fileName = "DatabaseTableTest", menuName = "ScriptableObjects/DatabaseTable/DatabaseTableTest")]
    public class DatabaseTableRank : ADatabaseTable<string,string>
    {
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
                return "xxx";
            }
        }

        protected override string keyName2
        {
            get
            {
                return "xxx";
            }
        }
    }
}
