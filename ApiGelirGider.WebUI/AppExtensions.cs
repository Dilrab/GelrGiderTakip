using System.Text;
using System.Text.Json;

namespace ApiGelirGider.WebUI
{
    public static class AppExtensions
    {

        public static StringContent ToStringContent(this object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

    }


}
