using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Xamarin.Forms;

using Produce101.Models;

namespace Produce101
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        
        public async Task DownloadTrainee()
        {
            using (var client = new HttpClient())
            {
                string json = await client.GetStringAsync("http://onair.mnet.com/produce101/api/p101.profile.json?option=listCmd:s!-result1$,f!*@result1");
                var trainees = JObject.Parse(json)["vl"].ToObject<Trainee[]>();
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await DownloadTrainee();
        }
    }
}
