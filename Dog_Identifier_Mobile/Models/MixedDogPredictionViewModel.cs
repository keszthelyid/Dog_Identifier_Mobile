﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dog_Identifier_Mobile.Models
{
    public class MixedDogPredictionViewModel : IDogViewModel
    {
        public byte[] PhotoData { get; set; }
        public Dog[][] Predictions { get; set; }
    }
}
