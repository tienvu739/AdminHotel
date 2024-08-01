using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdminHotel
{
    public partial class QLDH : Window
    {
        private readonly HttpClient _httpClient;

        public QLDH()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadOrderList();
            LoadHotelList();
        }

        private async void LoadOrderList()
        {
            try
            {
                var orders = await _httpClient.GetFromJsonAsync<List<Order>>("https://localhost:7226/api/Order/dsOrder");
                OrderListView.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn hàng: {ex.Message}");
            }
        }

        private async void LoadHotelList()
        {
            try
            {
                var hotels = await _httpClient.GetFromJsonAsync<List<Hotel>>("https://localhost:7226/api/Order/getHotelStatistics");
                HotelListView.ItemsSource = hotels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách sạn: {ex.Message}");
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string roomId = button.Tag.ToString();
                if (string.IsNullOrEmpty(roomId))
                {
                    MessageBox.Show("Không thể xác định ID phòng.");
                    return;
                }

                try
                {
                    var response = await _httpClient.DeleteAsync($"https://localhost:7226/api/Order/delete?id={roomId}");
                    if (response.IsSuccessStatusCode)
                    {
                        LoadOrderList();
                        MessageBox.Show("Xóa đơn hàng thành công.");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi xóa đơn hàng.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa đơn hàng: {ex.Message}");
                }
            }
        }
    }

    public class Order
    {
        public string IdOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double Price { get; set; }
        public string IdUser { get; set; }
        public string IdDiscount { get; set; }
        public string IdRoom { get; set; }
    }

    public class Hotel
    {
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public double TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public List<MonthlyRevenue> MonthlyRevenue { get; set; }
    }

    public class MonthlyRevenue
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public double Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}
