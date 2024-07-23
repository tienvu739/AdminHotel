using AminHotel.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdminHotel
{
    /// <summary>
    /// Interaction logic for QLTK.xaml
    /// </summary>
    public partial class QLTK : Window
    {
        private readonly HttpClient _httpClient;
        public QLTK()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadAccounts();
        }
        private async Task<bool> CreateAccount(Admin account)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7226/api/Admin/create", account);
            return response.IsSuccessStatusCode;
        }
        private async void LoadAccounts()
        {
            try
            {
                var accounts = await _httpClient.GetFromJsonAsync<List<Admin>>("https://localhost:7226/api/Admin");
                AccountsDataGrid.ItemsSource = accounts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách tài khoản: {ex.Message}");
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var idAdmin = button?.Tag.ToString();

            if (idAdmin != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    bool isSuccess = await DeleteAccount(idAdmin);
                    if (isSuccess)
                    {
                        MessageBox.Show("Xóa tài khoản thành công!");
                        LoadAccounts();
                    }
                    else
                    {
                        MessageBox.Show("Xóa tài khoản thất bại. Vui lòng thử lại.");
                    }
                }
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string idAdmin = "String";
            string account = AccountTextBox.Text;
            string password = PasswordBox.Password;
            string name = NameTextBox.Text;
            string address = AddressTextBox.Text;
            string role = RoleTextBox.Text;

            var newAccount = new Admin
            {
                IdAdmin = idAdmin,
                Account = account,
                Password = password,
                Name = name,
                Address = address,
                Role = role
            };

            bool isSuccess = await CreateAccount(newAccount);
            if (isSuccess)
            {
                MessageBox.Show("Tạo tài khoản thành công!");
                LoadAccounts();
            }
            else
            {
                MessageBox.Show("Tạo tài khoản thất bại. Vui lòng thử lại.");
            }
        }
        private async Task<bool> DeleteAccount(string idAdmin)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7226/api/Admin/DeleteAccount?id={idAdmin}");
            return response.IsSuccessStatusCode;
        }
    }
}
