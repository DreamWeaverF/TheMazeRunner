using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMazeRunner
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SynchronizeMethodAttribute : BaseAttribute
    {
        //
        public string ClassName;
        //
        public string FieldName;
    }
}
