namespace Core
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Utility
    {
        public static IDictionary<string, object> RemoveRange(this IDictionary<string, object> source, string value)
        {
            return source
                .Where(keys => !keys.Key.StartsWith(value))
                .ToDictionary(keys => keys.Key, keys => keys.Value);
        }
    }
}
