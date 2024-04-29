using System.Net.Http;
using System.Windows;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace WPF3API
{
    public partial class MainWindow : Window
    {
        static HttpClient httpClient = new HttpClient();
        const string genderApiUri = @"https://api.genderize.io/?name=";
        const string ageApiUri = @"https://api.agify.io/?name=";
        const string ipApiUri = @"https://api.ipify.org/";

        public MainWindow()
        {
            InitializeComponent();
            GetIP();
        }

        private async void GetIP()
        {
            string IP = await httpClient.GetStringAsync(ipApiUri);
            IpTb.Text = IP;
        }

        private async void GetGenderByNameAsync(string name)
        {
            GenderInfo? genderInfo = await httpClient.GetFromJsonAsync<GenderInfo>(genderApiUri + name);
            if(genderInfo != null)
            {
                GenderTb.Text = genderInfo.Gender;
                GenderProbabilityTb.Text = genderInfo.Probability.ToString();
            }
            else
            {
                MessageBox.Show("Ошибка доступа к " + genderApiUri);
            }
        }

        private async void GetAgeByNameAsync(string name)
        {
            AgeInfo? ageInfo = await httpClient.GetFromJsonAsync<AgeInfo>(ageApiUri + name);
            if (ageInfo != null)
            {
                AgeTb.Text = ageInfo.Age.ToString();
            }
            else
            {
                MessageBox.Show("Ошибка доступа к " + ageApiUri);
            }
        }

        private void GetInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NameTb.Text))
            {
                string userName = NameTb.Text.Trim();
                if(Regex.IsMatch(NameTb.Text, @"^([A-Z]|[a-z])*$"))
                {
                    GetGenderByNameAsync(userName);
                    GetAgeByNameAsync(userName);
                }
                else
                {
                    MessageBox.Show("Имя должно содержать только латинские символы!");
                }
            }
            else
            {
                MessageBox.Show("Заполните имя!");
            }
        }
    }
}