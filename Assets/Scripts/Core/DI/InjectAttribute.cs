using System;

namespace Core.DI
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    { }
}