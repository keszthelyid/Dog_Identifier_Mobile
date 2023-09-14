using System;
using System.Collections.Generic;
using System.Text;

namespace Dog_Identifier_Mobile.Models
{
    public class DogViewModel : IDogViewModel
    {
        public byte[] PhotoData { get; set; }
        public Dog[] Dogs { get; set; }
    }
}
