﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_Employees_DataTable.Models
{
    /// <summary>
    /// Класс для переопределения метода Read в классе JsonTextReader
    /// </summary>
    public class PreferInt32JsonTextReader : JsonTextReader
    {
        public PreferInt32JsonTextReader(TextReader reader) : base(reader) { }

        /// <summary>
        /// Переопределения метода Read в классе JsonTextReader
        /// </summary>
        /// <returns>True, если JsonTextReader должен использоваться для этого токена</returns>
        public override bool Read()
        {
            var ret = base.Read();

            // Read() is called for both an untyped read, and when reading a value typed as Int64
            // Thus if the value is larger than the maximum value of Int32 we can't just throw an 
            // exception, we have to do nothing.
            // 
            // Regardless of whether an Int32 or Int64 is returned, the serializer will always call
            // Convert.ChangeType() to convert to the final, desired primitive type (say, Uint16 or whatever).
            if (TokenType == JsonToken.Integer
                && ValueType == typeof(long)
                && Value is long)
            {
                var value = (long)Value;

                if (value <= int.MaxValue && value >= int.MinValue)
                {
                    var newValue = checked((int)value); // checked just in case
                    SetToken(TokenType, newValue, false);
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// Класс с расширением для JsonConvert
    /// </summary>
    public static class JsonExtensions
    {
        public static T PreferInt32DeserializeObject<T>(string jsonString, JsonSerializerSettings settings = null)
        {
            using (var sw = new StringReader(jsonString))
            using (var jsonReader = new PreferInt32JsonTextReader(sw))
            {
                return JsonSerializer.CreateDefault(settings).Deserialize<T>(jsonReader);
            }
        }
    }
}
