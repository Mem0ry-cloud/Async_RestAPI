using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Async_RestAPI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Sync_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            LoadDataSync();
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            textBlockInfo.Text += $"\n\nTotal time: {time}";
        }
        private async void Button_Async_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            //await LoadDataAsync();
            await LoadDataAsyncParallel();
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            textBlockInfo.Text += $"\n\nTotal time: {time}";
        }
        public async Task LoadDataAsync()
        {
            List<string> sites = PrepareLoadSites();
            foreach (string item in sites)
            {
                DataModel datamodel = await Task.Run(() => LoadSite(item));
                PrintInfo(datamodel);
            }
        }
        public async Task LoadDataAsyncParallel()
        {
            List<string> sites = PrepareLoadSites();
            List<Task<DataModel>> tasks = new List<Task<DataModel>>();
            foreach (string item in sites)
            {
                tasks.Add(Task.Run(() => LoadSite(item)));
            }
            DataModel[] dataModels = await Task.WhenAll(tasks);
            foreach(var item in dataModels)
            {
                PrintInfo(item);
            }
        }
        public void LoadDataSync()
        {
            List<string> sites = PrepareLoadSites();
            foreach (string item in sites)
            {
                DataModel datamodel = LoadSite(item);
                PrintInfo(datamodel);
            }
        }
        private void PrintInfo(DataModel datamodel)
        {
            textBlockInfo.Text += $"\nUrl: {datamodel.Url} Length: {datamodel.Data.Length}";
        }
        private List<string> PrepareLoadSites()
        {
            List<string> sites = new List<string>()
            {
                "https://google.com/",
                "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                //"";
            };
            return sites;
        }
        private DataModel LoadSite(string site)
        {
            DataModel datamodel = new DataModel();
            datamodel.Url = site;
            WebClient webClient = new WebClient();
            datamodel.Data = webClient.DownloadString(site);
            Dispatcher.BeginInvoke((Action)(() =>
            {
                textBlockInfo.Text = "info";
            }));
            return datamodel;
        }
    }
}
