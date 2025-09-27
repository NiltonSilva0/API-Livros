namespace WebAPI1.Dtos.Cliente
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Globalization;

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly string[] formatos = { "yyyy-MM-dd", "dd/MM/yyyy", "dd-MM-yyyy" };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var valor = reader.GetString();
            if (DateTime.TryParseExact(valor, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
            {
                return data;
            }
            throw new JsonException($"Formato de data inválido: {valor}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("dd/MM/yyyy"));
        }
    }
}