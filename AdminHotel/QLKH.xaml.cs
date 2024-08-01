using AdminHotel.models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace AdminHotel
{
    public partial class QLKH : Window
    {
        private readonly HttpClient _httpClient;

        public QLKH()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadUsers();
            LoadHoteliers();
        }

        private async void LoadUsers()
        {
            try
            {
                var users = await _httpClient.GetFromJsonAsync<List<UserViewModel>>("https://localhost:7226/api/User/dsUser");
                UsersDataGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách hàng: {ex.Message}");
            }
        }

        private async void LoadHoteliers()
        {
            try
            {
                var hoteliers = await _httpClient.GetFromJsonAsync<List<HotelierViewModel>>("https://localhost:7226/api/Hotelier/dsHotelier");
                HoteliersDataGrid.ItemsSource = hoteliers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách chủ khách sạn: {ex.Message}");
            }
        }
    }

    public class UserViewModel
    {
        public string IdUser { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string NameUser { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class HotelierViewModel
    {
        public string IdHotelier { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string NameHotelier { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
