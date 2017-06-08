using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Annotations;
using OxyPlot.Axes;

using Rank101.Models;

namespace Rank101.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TraineePage : ContentPage
    {
        Trainee trainee;

        public TraineePage(Trainee trainee)
        {
            InitializeComponent();

            this.trainee = trainee;
            BindingContext = trainee;

            Title = trainee.name;

            Device.BeginInvokeOnMainThread(async () =>
            {
                var ranks = await DownloadRanks();
                if (ranks != null)
                {
                    InitializeGraph(ranks);
                }
                else
                {
                    await DisplayAlert("에러", "인터넷이 없네용", "확인");
                }
            });
        }
        
        private void InitializeGraph(List<int> ranks)
        {
            var model = new PlotModel { Title = "순위" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 98, StartPosition = 1, EndPosition = 0, IsZoomEnabled = false, IsPanEnabled = false });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0.9, Maximum = ranks.Count + 0.1, IsZoomEnabled = false, IsPanEnabled = false, IntervalLength = 90 });

            var lineSeries = new LineSeries();
            for (int i = 0; i < ranks.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(i + 1, ranks[i]));
                model.Annotations.Add(new PointAnnotation { Text = "" + ranks[i], X = i + 1, Y = ranks[i] });

            }
            model.Series.Add(lineSeries);

            plotView.Model = model;
        }

        public async Task<List<int>> DownloadRanks()
        {
            if (Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
            {
                using (var client = new HttpClient())
                {
                    string json = await client.GetStringAsync($"http://onair.mnet.com/produce101/api/rank/{trainee.seq}.json");

                    var ranks = new List<int>();
                    foreach (var rank in JObject.Parse(json)["data"][0]["list"] as JArray)
                    {
                        var r = rank["value"].Value<string>();

                        if (r == null) break;
                        else ranks.Add(int.Parse(r));
                    }
                    return ranks;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
