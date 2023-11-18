using Dog_Identifier_Mobile.Helpers;
using Dog_Identifier_Mobile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Dog_Identifier_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scan : ContentPage
    {
        private HttpClient client;
        public Scan()
        {
            InitializeComponent();
            client = new HttpClient();

            // http://192.168.0.200:81
            // http://dogidentifier.duckdns.org:81
            client.BaseAddress = new Uri("http://dogidentifier.duckdns.org:81");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
              new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void btn_camera_Clicked(object sender, EventArgs e)
        {
            activity.IsRunning = true;
            btn_camera.IsEnabled= false;
            btn_gallery.IsEnabled= false;

            string Path = await TakePhotoAsync();

            await ProcessPhotoAsync(Path);

            activity.IsRunning = false;
            btn_camera.IsEnabled = true;
            btn_gallery.IsEnabled = true;
        }

        private async void btn_gallery_Clicked(object sender, EventArgs e)
        {
            activity.IsRunning = true;
            btn_camera.IsEnabled = false;
            btn_gallery.IsEnabled = false;

            string Path = await PickPhotoAsync();

            await ProcessPhotoAsync(Path);      

            activity.IsRunning = false;
            btn_camera.IsEnabled = true;
            btn_gallery.IsEnabled = true;
        }

        private async Task<DogViewModel> GetDogType(DogViewModel model)
        {
            var response = await client.PostAsJsonAsync("/dog/getdogtype", model);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<DogViewModel>();
            }
            throw new Exception("Can't communicate with the server.");
        }

        private async Task<MixedDogPredictionViewModel> GetMixedDogType(DogViewModel model)
        {
            var response = await client.PostAsJsonAsync("/dog/getmixeddogtype", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<MixedDogPredictionViewModel>();
            }
            throw new Exception("Can't communicate with the server.");
        }

        private async Task<string> TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    string PhotoPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using (var stream = await photo.OpenReadAsync())
                    using (var newStream = File.OpenWrite(PhotoPath))
                        await stream.CopyToAsync(newStream);

                    return PhotoPath;
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error", "Your device does not support taking photos.", "OK");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error", "You did not gave permission to access your camera", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            return null;
        }

        private async Task<string> PickPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();

                if (photo != null)
                {
                    string PhotoPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                    using (var stream = await photo.OpenReadAsync())
                    using (var newStream = File.OpenWrite(PhotoPath))
                        await stream.CopyToAsync(newStream);

                    return PhotoPath;
                }
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error", "You did not gave permission to access your photos", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }

            return null;
        }

        private async Task ProcessPhotoAsync(string photoPath)
        {
            if (photoPath != null)
            {
                DogViewModel vm = new DogViewModel();
                vm.PhotoData = File.ReadAllBytes(photoPath);

                try
                {
                    if (Preferences.Get("MixedPred", false))
                    {
                        MixedDogPredictionViewModel result = await GetMixedDogType(vm);

                        if (Preferences.Get("SaveScans", true))
                        {
                            MixedScanResult.SerializeAsJson(result);
                            Preferences.Set("NewScan", true);
                        }

                        result.Predictions.ForEach(x => x.ForEach(y => y.ImgSrc = ImageCreatorFromArray.ToImage(y.PhotoData)));
                        img_display.Source = ImageCreatorFromArray.ToImage(result.PhotoData);
                        await Navigation.PushAsync(new MixedScanResults(result));
                    }
                    else
                    {
                        DogViewModel result = await GetDogType(vm);

                        if (Preferences.Get("SaveScans", true))
                        {
                            ScanResult.SerializeAsJson(result);
                            Preferences.Set("NewScan", true);
                        }

                        result.Dogs.ForEach(x => x.ImgSrc = ImageCreatorFromArray.ToImage(x.PhotoData));
                        img_display.Source = ImageCreatorFromArray.ToImage(result.PhotoData);

                        await Navigation.PushAsync(new ScanResults(result));
                    }


                }
                catch (Exception)
                {
                    await DisplayAlert("Can't connect to the server", "The server is not available right now or there is a problem with your internet connection. If your internet connection is fine please try again later.", "OK");
                }
            }
        }
    }
}