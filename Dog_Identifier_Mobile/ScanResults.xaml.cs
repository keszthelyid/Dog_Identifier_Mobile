using Dog_Identifier_Mobile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dog_Identifier_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanResults : ContentPage
    {
        DogViewModel ToDisplay;
        public ScanResults(DogViewModel model)
        {
            InitializeComponent();
            
            ToDisplay = model;

            Detected.ItemsSource = ToDisplay.Dogs.Distinct();

            if (ToDisplay.Dogs[0].Name == "None")
            {
                Detected.IsVisible = false;
                InfoLabel.IsVisible = false;
                DogNumberText.Text = "I did not find any dogs on your photo";          
            }
            else if(ToDisplay.Dogs.Length == 1)
            {
                DogNumberText.Text = "I found 1 dog on your photo";
            }
            else 
            {
                DogNumberText.Text = "I found " + ToDisplay.Dogs.Length + " dogs on your photo";
            }

            ResultImage.Source = ToImage(ToDisplay.PhotoData);        
        }

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            await Navigation.PopAsync();
        }

        private ImageSource ToImage(byte[] array)
        {
            ImageSource src = ImageSource.FromStream(() =>
            {
                return new MemoryStream(array);
            });
            return src;
        }

        private async void Detected_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((ListView)sender).SelectedItem == null)
                return;

            await Navigation.PushAsync(new DogDetailsPage(e.SelectedItem as Dog));

            ((ListView)sender).SelectedItem = null;
        }
    }
}