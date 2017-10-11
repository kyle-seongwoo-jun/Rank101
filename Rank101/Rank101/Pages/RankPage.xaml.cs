using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;
using Plugin.Settings.Abstractions;
using Plugin.Connectivity.Abstractions;

using Rank101.Models;

namespace Rank101.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankPage : ContentPage
    {
        private BindableProperty WeekProperty = BindableProperty.Create("Week", typeof(int), typeof(RankPage), 0,
                                                                        propertyChanged: async (b, o, n) =>
                                                                        {
                                                                            var view = b as RankPage;
                                                                            var week = (int)n;

                                                                            await view.OnWeekChanged(week);
                                                                        });


        ISettings settings => Plugin.Settings.CrossSettings.Current;

        IConnectivity connectivity => Plugin.Connectivity.CrossConnectivity.Current;

        public int Week
        {
            get => (int)GetValue(WeekProperty);
            set => SetValue(WeekProperty, value);
        }

        public RankPage()
        {
            InitializeComponent();
        }

        private async Task OnWeekChanged(int week)
        {
            // set title
            Title = week >= 5 ? (week + 1) + "주차" : week + "주차";

            string json = settings.GetValueOrDefault($"week{week}", "");
            if (json != "")
            {
                TraineeListView.ItemsSource = JsonToTrainees(json);
            }
            else
            {
                if (connectivity.IsConnected)
                {
                    TraineeListView.ItemsSource = await DownloadTrainees(Week);
                }
                else
                {
                    await DisplayAlert("에러", "인터넷이 없네용", "확인");
                }
            }
        }

        private async Task<Trainee[]> DownloadTrainees(int week)
        {
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync($"http://onair.mnet.com/produce101/api/p101.profile.json?option=listCmd:s!-result{week}$,f!*@result{week}");
                if (Device.RuntimePlatform == Device.Android && Device.RuntimePlatform == Device.iOS)
                {
                    Plugin.Settings.CrossSettings.Current.AddOrUpdateValue($"week{week}", json);
                }

                return JsonToTrainees(json);
            }
        }

        private Trainee[] JsonToTrainees(string json)
        {
            try
            {
                return JObject.Parse(json)["vl"].ToObject<Trainee[]>();
            }
            catch
            {
                return null;
            }
        }

        private void TraineeListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem is Trainee trainee)
            {
                Navigation.PushAsync(new TraineePage(trainee));
            }
        }
    }
}
