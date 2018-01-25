using System;

namespace LiteHomeManagement.App.Common
{
    public static class BasicFunctions
    {
        public static Out Then<In, Out>(this In input, Func<In, Out> convert)
        {
            return convert(input);
        }
    }
}
