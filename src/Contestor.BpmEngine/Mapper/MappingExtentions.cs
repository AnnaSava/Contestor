using Contestor.Proto.BpmEngine.JsonObjects;
using System.Collections.Generic;
using System.Linq;

namespace Contestor.Proto.BpmEngine.Mapper
{
    public static class MappingExtentions
    {
        public static Dictionary<int, string> ToStringDictionary(this ListObject listObject)
        {
            return listObject?.Value?.Select((val, index) => new { val, index })
                .ToDictionary(x => x.index, x => x.val) ?? new Dictionary<int, string>();
        }
    }
}
