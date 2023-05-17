using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Identifier_Mobile.Models
{
    class MixedScanResult : IScanResult
    {
        public string Id { get; set; }
        public DateTime TimeOfScan { get; set; }
        public IDogViewModel Model { get; set; }
        public string Text { get; set; }

        public static string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Scan History");

        public MixedScanResult(MixedDogPredictionViewModel model)
        {
            Id = "mix_" + Guid.NewGuid().ToString();
            TimeOfScan = DateTime.Now;
            Model = model;

            if ((Model as MixedDogPredictionViewModel).Predictions[0][0].Name == "None")
                Text = $"Didn't find any dogs at {TimeOfScan.ToShortDateString()} {TimeOfScan.ToShortTimeString()}";
            else if ((Model as MixedDogPredictionViewModel).Predictions.Length == 1)
                Text = $"Found 1 dog at {TimeOfScan.ToShortDateString()} {TimeOfScan.ToShortTimeString()}";
            else
                Text = $"Found {(Model as MixedDogPredictionViewModel).Predictions.Length} dogs at {TimeOfScan.ToShortDateString()} {TimeOfScan.ToShortTimeString()}";
        }


        public static void SerializeAsJson(MixedDogPredictionViewModel model)
        {
            MixedScanResult res = new MixedScanResult(model);
            string text = JsonConvert.SerializeObject(res, Formatting.Indented);
            File.WriteAllText(Path.Combine(filepath, res.Id + ".json"), text);
        }

        public void Delete()
        {
            File.Delete(Path.Combine(filepath, Id + ".json"));
        }
    }
}
