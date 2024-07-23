using AdminHotel.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for EditHotel.xaml
    /// </summary>
    public partial class EditHotel : Window
    {
        private readonly HttpClient _httpClient;
        private readonly string _idHotel;
        public EditHotel(string idHotel)
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _idHotel = idHotel;
            LoadHotelDetails();
        }
        private async void LoadHotelDetails()
        {
            try
            {
                var hotel = await _httpClient.GetFromJsonAsync<UpdateHotelDto>($"https://localhost:7226/api/Hotel/getDS?idHotel={_idHotel}");
                if (hotel != null)
                {
                    NameHotelTextBox.Text = hotel.NameHotel;
                    AddressHotelTextBox.Text = hotel.AddressHotel;
                    DescribeHotelTextBox.Text = hotel.DescribeHotel;
                    PolicyHotelTextBox.Text = hotel.PolicyHotel;
                    TypeHotelTextBox.Text = hotel.TypeHotel;
                    StatusHotelCheckBox.IsChecked = hotel.StatusHotel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin khách sạn: {ex.Message}");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var updateHotelDto = new UpdateHotelDto
            {
                NameHotel = NameHotelTextBox.Text,
                AddressHotel = AddressHotelTextBox.Text,
                DescribeHotel = DescribeHotelTextBox.Text,
                PolicyHotel = PolicyHotelTextBox.Text,
                TypeHotel = TypeHotelTextBox.Text,
                StatusHotel = StatusHotelCheckBox.IsChecked ?? false
            };

            var jsonContent = JsonSerializer.Serialize(updateHotelDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7226/api/Hotel/updateHotel?idHotel={_idHotel}", content);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Khách sạn đã được cập nhật thành công!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật khách sạn thất bại. Vui lòng thử lại.");
            }
        }
    }
}
