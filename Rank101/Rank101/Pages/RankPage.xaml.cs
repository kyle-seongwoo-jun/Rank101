﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using Plugin.Settings.Abstractions;
using Plugin.Connectivity.Abstractions;

using Rank101.Models;

namespace Rank101.Pages
{
    public partial class RankPage : ContentPage
    {
        int week;
        bool isNeedToDownload;

        ISettings settings => Plugin.Settings.CrossSettings.Current;
        IConnectivity connectivity => Plugin.Connectivity.CrossConnectivity.Current;

        public RankPage(int week)
        {
            InitializeComponent();

            this.week = week;
            Title = week + "주차";

            isNeedToDownload = !settings.Contains($"week{week}");
            if (!isNeedToDownload)
            {
                string json = settings.GetValueOrDefault($"week{week}", "");
                TraineeListView.ItemsSource = parseJson(json);
            }
        }

        bool isFirst = true;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (isFirst && isNeedToDownload)
            {
                isFirst = false;

                if (connectivity.IsConnected)
                {
                    TraineeListView.ItemsSource = await DownloadTrainees(week);
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
                Plugin.Settings.CrossSettings.Current.AddOrUpdateValue($"week{week}", json);

                return parseJson(json);
            }
        }

        private Trainee[] parseJson(string json)
        {
            return JObject.Parse(json)["vl"].ToObject<Trainee[]>();
        }

        private void TraineeListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem != null)
            {
                var trainee = e.SelectedItem as Trainee;
                Navigation.PushAsync(new TraineePage(trainee));
            }
        }
    }
}