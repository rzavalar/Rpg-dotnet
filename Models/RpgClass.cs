using System.Text.Json.Serialization;
// Importamos lo de arriva y agregamos la linea 5

namespace dotnet__rpg.Models
{   [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight = 1,
        Mage=2,

        Cleric = 3,

        Shaman = 4,
    }
}