using System.Collections.Generic;

namespace VkBotApi
{
    public class RequestCore
    {
    }

    public class RequestParams : List<string>
    {
        public object this[string paramName]
        {
            set
            {
                //string str = value == null ? string.Empty : (value is bool ? ((bool)value == true ? "1" : "0") : value.ToString());
                string str = value?.ToString() ?? string.Empty;
                this.Add($"{paramName}={str}");
            }
        }
    }
}