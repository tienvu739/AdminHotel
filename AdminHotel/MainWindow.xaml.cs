using AminHotel.models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminHotel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            bool isAuthenticated = await AuthenticateUser(username, password);
            if (isAuthenticated)
            {
                MessageBox.Show("Đăng nhập thành công!");
                var tokenResponse = await GetTokenResponse(username, password);
                var homeWindow = new Home(tokenResponse.Role);
                homeWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin đăng nhập.");
            }
        }
        private async Task<bool> AuthenticateUser(string username, string password)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["account"] = username;
            query["password"] = password;
            string queryString = query.ToString();

            var response = await _httpClient.GetAsync($"https://localhost:7226/api/Admin/login?{queryString}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task<TokenResponse> GetTokenResponse(string username, string password)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["account"] = username;
            query["password"] = password;
            string queryString = query.ToString();

            var response = await _httpClient.GetAsync($"https://localhost:7226/api/Admin/login?{queryString}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TokenResponse>();
            }
            return null;
        }
    }
}