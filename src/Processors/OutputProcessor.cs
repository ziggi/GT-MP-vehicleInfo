using System.IO;
using GTA;
using GT_MP_vehicleInfo.Data;
using Newtonsoft.Json;

namespace GT_MP_vehicleInfo.Processors
{
    public class OutputProcessor
    {

        public static JsonSerializerSettings defaultSettings = NewSettings;

        public static JsonSerializerSettings NewSettings => new JsonSerializerSettings
        {
            ContractResolver = new GeneralResolver()
        };
        
        public static void OutputVehicleInfo()
        {
            JsonSerializerSettings settings = defaultSettings;

            settings.Formatting = Formatting.Indented;
            // Large, Indented (For research purposes)
            OutputToJson(settings, ".ind");
                
            settings.Formatting = Formatting.None;
                
            // Normal, with translation
            OutputToJson(settings, "-" + Main.languageCode + ".full");
                
            // Smaller, without translation
            settings.ContractResolver = new NoLocalizationResolver();
            OutputToJson(settings, ".noloc");
                
            // Smallest, without lists
            settings.ContractResolver = new NoListsResolver();
            OutputToJson(settings, ".nolist");

            defaultSettings = NewSettings;
            
            OutputSeperateVehicles();
        }

        private static void OutputSeperateVehicles()
        {
            JsonSerializerSettings settings = defaultSettings;
            settings.Formatting = Formatting.Indented;

            foreach (var entry in Main.Storage.vehicleStorage)
            {
                string path = Path.Combine(Main.BasePath, "output/vehicleInfo-" + Main.languageCode + "/") + entry.Value.name + ".json";
                Process(path, entry.Value, settings);
            }
            
            // COMPRESS FILES
            //var zipfile = Main.GetPath("output/vehicleInfo-" + Main.languageCode + ".zip");
            //if(File.Exists(zipfile)) File.Delete(zipfile);
            //System.IO.Compression.ZipFile.CreateFromDirectory(Main.GetPath("output/vehicleInfo-" + Main.languageCode + "/"), zipfile);
        }
        
        private static void OutputToJson(JsonSerializerSettings settings, string extension)
        {
            string path = Path.Combine(Main.BasePath, "output/") + "vehicleInfo" + extension + ".json";
            Process(path, Main.Storage.vehicleStorage, settings);
        }
        
        public static void Process(string path, object data, JsonSerializerSettings settings = null)
        {
            if (settings == null) settings = defaultSettings;
            string payload = JsonConvert.SerializeObject(data, settings);

            try
            {
                File.WriteAllText(path, payload);
            }
            catch (IOException e)
            {
                Console.Error("AN ERROR OCCURED: " + e.Message);
            }
            
        }
        
        
    }
}