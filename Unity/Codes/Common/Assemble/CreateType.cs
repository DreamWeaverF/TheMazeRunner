using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMazeRunner
{
    public static class CreateType
    {
        public static T CreateInstance<T>() where T : class , IDisposable
        {
            using T t = Activator.CreateInstance<T>();
            return t;
        }
    }
}
