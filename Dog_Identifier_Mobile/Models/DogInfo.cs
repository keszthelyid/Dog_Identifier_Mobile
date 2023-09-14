using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Shapes;

namespace Dog_Identifier_Mobile.Models
{
 
    public class DogInfo
    {
        public Data data { get; set; }
        public Links links { get; set; }
    }
    public class Attributes
    {
        public string name { get; set; }
        public string description { get; set; }
        public Life life { get; set; }
        public MaleWeight male_weight { get; set; }
        public FemaleWeight female_weight { get; set; }
        public bool hypoallergenic { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string type { get; set; }
        public Attributes attributes { get; set; }
        public Relationships relationships { get; set; }
    }

    public class FemaleWeight
    {
        public int max { get; set; }
        public int min { get; set; }
    }

    public class Group
    {
        public Data data { get; set; }
    }

    public class Life
    {
        public int max { get; set; }
        public int min { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }

    public class MaleWeight
    {
        public int max { get; set; }
        public int min { get; set; }
    }

    public class Relationships
    {
        public Group group { get; set; }
    }
}
