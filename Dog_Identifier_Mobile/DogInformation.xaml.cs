using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dog_Identifier_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DogInformation : ContentPage
    {
        public DogInformation(string link)
        {
            InitializeComponent();
            DogInformationPage.Source = new Uri(link);
        }
    }
}