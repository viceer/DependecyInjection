using System;

namespace DependencyInjector.Internals
{
    internal class Injection
    {
        public Type Type { get; set; }
        public bool SingleInstance { get; set; }
    }
}
