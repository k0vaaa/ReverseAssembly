using System;

namespace Core.Extensions
{
    public static class ObjectExtensions
    {
        public static String TypeName(this object obj)
        {
            return obj.GetType().Name;
        }
    }
}