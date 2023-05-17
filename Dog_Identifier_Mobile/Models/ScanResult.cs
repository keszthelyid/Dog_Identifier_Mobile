using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Dog_Identifier_Mobile.Models
{
    class ScanResult : IScanResult
    {
        public string Id { get; set; }
        public DateTime TimeOfScan { get; set; }
        public IDogViewModel Model { get; set; }
        public string Text { get; set; }

        public static string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Scan History");

        public ScanResult(DogViewModel model)
        {
            Id = "scn_" + Guid.NewGuid().ToString();
            TimeOfScan = DateTime.Now;
            Model = model;

            if ((Model as DogViewModel).Dogs[0].Name == "None")
                Text = $"Didn't find any dogs at {TimeOfScan.ToShortDateString()} {TimeOfScan.ToShortTimeString()}";
            else if ((Model as DogViewModel).Dogs.Length == 1)
                Text = $"Found 1 dog at {TimeOfScan.ToShortDateString()} {TimeOfScan.ToShortTimeString()}";
            else
                Text = $"Found {(Model as DogViewModel).Dogs.Length} dogs at {TimeOfScan.ToShortDateString()} {TimeOfScan.ToShortTimeString()}";
        }

        public static Task<IEnumerable<IScanResult>> ReadAll()
        {
            return Task<IEnumerable<IScanResult>>.Factory.StartNew(() =>
            {
                ConcurrentStack<IScanResult> Results = new ConcurrentStack<IScanResult>();


                Parallel.ForEach(Directory.GetFiles(filepath), file =>
                {
                    if (file.Contains("scn"))
                    {
                        ScanResult scn = JsonConvert.DeserializeObject<ScanResult>(File.ReadAllText(file));
                        Results.Push(scn);
                    }
                    else
                    {
                        MixedScanResult scn = JsonConvert.DeserializeObject<MixedScanResult>(File.ReadAllText(file));
                        Results.Push(scn);
                    }
                });

                return Results.OrderByDescending(x => x.TimeOfScan);
            });
        }

        public static void SerializeAsJson(DogViewModel model)
        {
            ScanResult res = new ScanResult(model);
            string text = JsonConvert.SerializeObject(res, Formatting.Indented);
            File.WriteAllText(Path.Combine(filepath, res.Id + ".json"), text);
        }

        public void Delete()
        {
            File.Delete(Path.Combine(filepath, Id + ".json"));
        }
    }
}
