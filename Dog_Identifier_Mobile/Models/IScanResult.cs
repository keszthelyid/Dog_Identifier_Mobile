using System;

namespace Dog_Identifier_Mobile.Models
{
    internal interface IScanResult
    {
        string Id { get; set; }
        IDogViewModel Model { get; set; }
        string Text { get; set; }
        DateTime TimeOfScan { get; set; }

        void Delete();
    }
}