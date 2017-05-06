using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Xamarin.Forms;

using Produce101.Models;

namespace Produce101.Pages
{
    public partial class RankPage : ContentPage
    {
        int week;

        public RankPage(int week)
        {
            InitializeComponent();

            this.week = week;
            Title = week + "주차";
        }

        bool isFirst = true;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (isFirst)
            {
                isFirst = false;

                var trainees = await DownloadTrainee(week);
                TraineeListView.ItemsSource = trainees;
            }
        }

        public async Task<Trainee[]> DownloadTrainee(int week)
        {
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync($"http://onair.mnet.com/produce101/api/p101.profile.json?option=listCmd:s!-result{week}$,f!*@result{week}");
                return JObject.Parse(json)["vl"].ToObject<Trainee[]>();
            }
        }

        private void TraineeListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if(e.SelectedItem != null)
            {
                var trainee = e.SelectedItem as Trainee;
                DisplayAlert(trainee.agencyNm, trainee.name, "OK");
            }
        }
    }
}
