using Dog_Identifier_Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace Dog_Identifier_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DogDetailsPage : ContentPage
    {
        Dog dog;
        DogInfo currentDogInfo;
        HttpClient client;
        public DogDetailsPage(Dog d)
        {
            dog = d;
            
            client = new HttpClient();
            client.BaseAddress = new Uri("https://dogapi.dog/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
              new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

         
            InitializeComponent();

            MainStack.IsVisible = false;
        }

        private async Task<DogInfo> GetDogInformation(string dogId)
        {
            var response = await client.GetAsync("api/v2/breeds/" + dogId);
            if (response.IsSuccessStatusCode)
            {
                string text = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DogInfo>(text);
            }
            
            throw new Exception("Can't communicate with the server.");
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            activity.IsRunning = true;           

            currentDogInfo = await GetDogInformation(dog.ApiId);

            Description.Text = currentDogInfo.data.attributes.description;
            DogImage.Source = dog.ImgSrc;
            DogName.Text = currentDogInfo.data.attributes.name;
            MinLife.Text = currentDogInfo.data.attributes.life.min.ToString() + " years";
            MaxLife.Text = currentDogInfo.data.attributes.life.max.ToString() + " years";
            MinFemaleWeight.Text = currentDogInfo.data.attributes.female_weight.min + " kg";
            MaxFemaleWeight.Text = currentDogInfo.data.attributes.female_weight.max + " kg";
            MinMaleWeight.Text = currentDogInfo.data.attributes.male_weight.min + " kg";
            MaxMaleWeight.Text = currentDogInfo.data.attributes.male_weight.max + " kg";
            HypoAllergenic.Text = currentDogInfo.data.attributes.hypoallergenic == true ?  "This dog is hypoallergenic" : "This dog is not hypoallergenic";


            activity.IsRunning = false;
            activity.IsVisible = false;
            MainStack.IsVisible = true;       
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DogInformation(dog.Link));
        }

        private async void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}