using Dog_Identifier_Mobile.Helpers;
using Dog_Identifier_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Dog_Identifier_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanHistory : ContentPage
    {
        IEnumerable<IScanResult> Results { get; set; }
        public ScanHistory()
        {
            InitializeComponent();
            Refresh();
        }

        private async void Scans_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((ListView)sender).SelectedItem == null)
                return;

            if (e.SelectedItem.GetType() == typeof(ScanResult))
            {
                ((e.SelectedItem as ScanResult).Model as DogViewModel).Dogs.ForEach(x => x.ImgSrc = ImageCreatorFromArray.ToImage(x.PhotoData));
                await Navigation.PushAsync(new ScanResults((e.SelectedItem as ScanResult).Model as DogViewModel));
            }
            else
            {
                ((e.SelectedItem as MixedScanResult).Model as MixedDogPredictionViewModel).Predictions.ForEach(x => x.ForEach(y => y.ImgSrc = ImageCreatorFromArray.ToImage(y.PhotoData)));
                await Navigation.PushAsync(new MixedScanResults(((e.SelectedItem as MixedScanResult).Model as MixedDogPredictionViewModel)));
            }
            

            ((ListView)sender).SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var res = btn.CommandParameter as IScanResult;
            res.Delete();
            Refresh();
        }

        private async void Refresh()
        {
            Results = await ScanResult.ReadAll();
            Scans.ItemsSource = Results;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (Preferences.Get("NewScan", false))
            {
                Preferences.Set("NewScan", false);
                Refresh();
            }      
        }
    }
}