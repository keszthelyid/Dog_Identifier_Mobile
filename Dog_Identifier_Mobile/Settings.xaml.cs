using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dog_Identifier_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            Sw_SaveHistory.IsToggled = Preferences.Get("SaveScans", true);
            Sw_MixedPrediction.IsToggled = Preferences.Get("MixedPred", false);
        }

        private void Sw_SaveHistory_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("SaveScans", Sw_SaveHistory.IsToggled);
        }

        private void Sw_MixedPrediction_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("MixedPred", Sw_MixedPrediction.IsToggled);
        }
    }
}