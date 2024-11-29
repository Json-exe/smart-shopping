using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartShopping.Lib.Models;

public sealed record Product
{
    [JsonPropertyName("product_name")] public required string ProductName { get; init; } = string.Empty;
    [JsonPropertyName("selected_images")] public SelectedImages SelectedImages { get; init; }
    [JsonPropertyName("nutrient_levels")] public NutrientLevels NutrientLevels { get; init; }
    [JsonPropertyName("nutriscore_grade")] public Nutriscore NutriscoreGrade { get; init; }
    public Nutriments Nutriments { get; init; }
    [JsonPropertyName("ecoscore_data")] public EcoScoreData EcoScoreData { get; init; }
    public List<Ingredient> Ingredients { get; init; } = [];
}

public sealed record SelectedImages
{
    public Images Front { get; init; }
    public Images Ingredients { get; init; }
    public Images Nutrition { get; init; }
    public Images Packaging { get; init; }
}

public sealed record Images
{
    public Dictionary<string, string> Display { get; init; } = [];
    public Dictionary<string, string> Small { get; init; } = [];
    public Dictionary<string, string> Thumb { get; init; } = [];
}

public sealed record NutrientLevels
{
    public NutrientLevel Fat { get; init; }
    public NutrientLevel Salt { get; init; }
    public NutrientLevel Sugars { get; init; }
    [JsonPropertyName("saturated-fat")] public NutrientLevel SaturatedFat { get; init; }
}

public sealed record Nutriments
{
    [JsonPropertyName("energy-kj")] public float EnergyKj { get; init; } = 0;
    [JsonPropertyName("energy-kcal")] public float EnergyKcal { get; init; } = 0;
    public float Fat { get; init; } = 0;
    [JsonPropertyName("saturated-fat")] public float SaturatedFat { get; init; } = 0;
    public float Salt { get; init; } = 0;
    public float Proteins { get; init; } = 0;
    public float Sugars { get; init; } = 0;
    [JsonPropertyName("nova-group")] public int NovaGroup { get; init; } = 0;
}

public sealed record EcoScoreData
{
    public string Grade { get; init; }
}

public sealed record Ingredient
{
    public bool Vegan { get; init; }
    public bool Vegetarian { get; init; }
}

public enum NutrientLevel
{
    Low,
    Medium,
    High
}

public enum Nutriscore
{
    A = 1,
    B = 2,
    C = 3,
    D = 4,
    E = 5
}

internal sealed class IngredientJsonConverter : JsonConverter<Ingredient>
{
    // TODO: Need to improve this. An ingredient can have an ingredient as a sub type. Also is vegan and vegetarian correct to set to false?
    public override Ingredient Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartObject)
        {
            throw new NotSupportedException();
        }

        var vegan = false;
        var vegetarian = false;
        var originalDepth = reader.CurrentDepth;

        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject when reader.CurrentDepth == originalDepth:
                    return new Ingredient { Vegan = vegan, Vegetarian = vegetarian };
                case JsonTokenType.PropertyName:
                    var name = reader.GetString();
                    reader.Read();
                    switch (name)
                    {
                        case "vegan":
                            vegan = reader.GetString()?.ToLowerInvariant() == "yes";
                            break;
                        case "vegetarian":
                            vegetarian = reader.GetString()?.ToLowerInvariant() == "yes";
                            break;
                    }

                    break;
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Ingredient value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}