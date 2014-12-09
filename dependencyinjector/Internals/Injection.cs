using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjector.Internals
{
    internal class Injection
    {
        public Type Type { get; set; }
        public bool SingleInstance { get; set; }
    }
}
