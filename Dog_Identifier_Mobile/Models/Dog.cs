using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Dog_Identifier_Mobile.Models
{
    public class Dog : IEquatable<Dog>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string ApiId { get; set; }
        public double? Percentage { get; set; }
        public byte[] PhotoData { get; set; }

        [JsonIgnore]
        public ImageSource ImgSrc { get; set; }

        public bool Equals(Dog other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
