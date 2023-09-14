using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Dog_Identifier_Mobile.Helpers
{
    public static class ImageCreatorFromArray
    {
        public static ImageSource ToImage(byte[] array)
        {
            ImageSource src = ImageSource.FromStream(() =>
            {
                return new MemoryStream(array);
            });
            return src;
        }
    }
}
