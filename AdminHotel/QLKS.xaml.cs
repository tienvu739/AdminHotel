using AdminHotel.models;
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
    /// Interaction logic for QLKS.xaml
    /// </summary>
    public partial class QLKS : Window
    {
        private readonly HttpClient _httpClient;
        public QLKS()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadHotels();
        }
        private async void LoadHotels()
        {
            try
            {
                var hotels = await _httpClient.GetFromJsonAsync<List<Hotel>>("https://localhost:7226/api/Hotel/QLKS");
                HotelsDataGrid.ItemsSource = hotels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách sạn: {ex.Message}");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var idHotel = button?.Tag.ToString();

            if (idHotel != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn duyệt khách sạn này?", "Xác nhận", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    bool isSuccess = await ApproveHotel(idHotel);
                    if (isSuccess)
                    {
                        MessageBox.Show("Khách sạn đã được duyệt thành công!");
                        LoadHotels();
                    }
                    else
                    {
                        MessageBox.Show("Duyệt khách sạn thất bại. Vui lòng thử lại.");
                    }
                }
            }
        }
        private async Task<bool> ApproveHotel(string idHotel)
        {
            var response = await _httpClient.PostAsync($"https://localhost:7226/api/Hotel/ApproveHotel?idHotel={idHotel}", null);
            return response.IsSuccessStatusCode;
        }
    }
}
