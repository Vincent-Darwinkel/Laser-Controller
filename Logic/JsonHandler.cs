using System;
using System.IO;
using System.Threading.Tasks;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Logic
{
    public class JsonHandler
    {
        public T Get<T>(string fileName)
        {
            using StreamReader file = File.OpenText(Directory.GetCurrentDirectory() + "/Json/" + fileName);
            using var reader = new JsonTextReader(file);
            string json = JToken.ReadFrom(reader).ToString(Formatting.None);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<Result> Save<T>(T type, string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(type, Formatting.None);

                await File.WriteAllTextAsync(Directory.GetCurrentDirectory() + "/Json/" + path, json);
                return new Result { Success = true };
            }

            catch (Exception e)
            {
                return new Result { Message = e.Message };
            }
        }
    }
}