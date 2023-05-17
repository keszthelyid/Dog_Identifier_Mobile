﻿using Dog_Identifier_Mobile.Models;
using ImageCircle.Forms.Plugin.Abstractions;
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
    public partial class MixedScanResults : ContentPage
    {
        MixedDogPredictionViewModel ToDisplay;
        public MixedScanResults(MixedDogPredictionViewModel model)
        {
            InitializeComponent();

            ToDisplay = model;

            if (ToDisplay.Predictions.Length == 1)
            {
                if (ToDisplay.Predictions[0][0].Name != "None")
                {
                    ListView listView = new ListView();         
                    listView.ItemSelected += Detected_ItemSelected;
                    listView.ItemsSource = ToDisplay.Predictions[0];
                    listView.ItemTemplate = new DataTemplate(typeof(DogCell));
                    PageStack.Children.Add(listView);
                }
            }
            else
            {
                for (int i = 0; i < ToDisplay.Predictions.Length; i++)
                {
                    PageStack.Children.Add(new Label() { Text = $"{i + 1}. Dog ", FontSize = 15, FontAttributes = FontAttributes.Bold, TextColor = Color.White , Margin = 10});
                    ListView listView = new ListView();
                    listView.Margin = 0;
                    listView.ItemSelected += Detected_ItemSelected;
                    listView.ItemsSource = ToDisplay.Predictions[i];
                    listView.ItemTemplate = new DataTemplate(typeof(DogCell));
                    PageStack.Children.Add(listView);
                }
            }

            if (ToDisplay.Predictions[0][0].Name == "None")
            {
                InfoLabel.IsVisible = false;
                DogNumberText.Text = "I did not find any dogs on your photo";
            }
            else if (ToDisplay.Predictions.Length == 1)
            {
                DogNumberText.Text = "I found 1 dog on your photo";
            }
            else
            {
                DogNumberText.Text = "I found " + ToDisplay.Predictions.Length + " dogs on your photo";
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

            await Navigation.PushAsync(new DogInformation((e.SelectedItem as Dog).Link));

            ((ListView)sender).SelectedItem = null;
        }

        private class DogCell : ViewCell
        {
            public DogCell()
            {           
                CircleImage cI = new CircleImage();
                Label nameLabel = new Label();
                Label percentageLabel = new Label();
                StackLayout verticaLayout = new StackLayout();

                nameLabel.SetBinding(Label.TextProperty, new Binding("Name"));
                percentageLabel.SetBinding(Label.TextProperty, new Binding("Percentage") { StringFormat = "{0} %"});              
                cI.SetBinding(Image.SourceProperty, new Binding("ImgSrc"));

                verticaLayout.Orientation = StackOrientation.Horizontal;

                nameLabel.FontSize = 15;
                nameLabel.TextColor = Color.White;
                nameLabel.FontAttributes = FontAttributes.Bold;
                nameLabel.VerticalOptions = LayoutOptions.Center;

                percentageLabel.FontSize = 15;
                percentageLabel.TextColor = Color.White;
                percentageLabel.FontAttributes = FontAttributes.Bold;
                percentageLabel.VerticalOptions = LayoutOptions.Center;

                cI.HeightRequest = 70;
                cI.WidthRequest = 70;

                verticaLayout.Children.Add(cI);
                verticaLayout.Children.Add(nameLabel);
                verticaLayout.Children.Add(percentageLabel);

                View = verticaLayout;

                View.Margin = 0;             
            }
        }
    }
}