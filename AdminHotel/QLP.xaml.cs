using AdminHotel.models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdminHotel
{
    public partial class QLP : Window
    {
        private readonly HttpClient _httpClient;

        public QLP()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadRooms();
        }

        private async void LoadRooms()
        {
            try
            {
                var rooms = await _httpClient.GetFromJsonAsync<List<RoomViewModel>>("https://localhost:7226/api/Room/getAllRooms");
                RoomsDataGrid.ItemsSource = rooms;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phòng: {ex.Message}");
            }
        }

        private void RoomsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsDataGrid.SelectedItem is RoomViewModel selectedRoom)
            {
                IdRoomTextBox.Text = selectedRoom.IdRoom;
                NameRoomTextBox.Text = selectedRoom.NameRoom;
                AreaRoomTextBox.Text = selectedRoom.AreaRoom.ToString();
                PeopleTextBox.Text = selectedRoom.People.ToString();
                PolicyRoomTextBox.Text = selectedRoom.PolicyRoom;
                BedNumberTextBox.Text = selectedRoom.BedNumber.ToString();
                StatusRoomComboBox.SelectedIndex = selectedRoom.StatusRoom ? 0 : 1; // 0: Còn phòng, 1: Hết phòng
                foreach (ComboBoxItem item in TypeRoomComboBox.Items)
                {
                    if (item.Content.ToString() == selectedRoom.TypeRoom)
                    {
                        TypeRoomComboBox.SelectedItem = item;
                        break;
                    }
                }
                PriceTextBox.Text = selectedRoom.Price.ToString();
            }
        }

        private async void UpdateRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var updatedRoom = new RoomViewModel
            {
                IdRoom = IdRoomTextBox.Text,
                NameRoom = NameRoomTextBox.Text,
                AreaRoom = double.Parse(AreaRoomTextBox.Text),
                People = int.Parse(PeopleTextBox.Text),
                PolicyRoom = PolicyRoomTextBox.Text,
                BedNumber = int.Parse(BedNumberTextBox.Text),
                StatusRoom = StatusRoomComboBox.SelectedIndex == 0,
                TypeRoom = (TypeRoomComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Price = decimal.Parse(PriceTextBox.Text)
            };

            var response = await _httpClient.PutAsJsonAsync("https://localhost:7226/api/Room/updateRoom", updatedRoom);
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Cập nhật thông tin phòng thành công!");
                LoadRooms();
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin phòng thất bại. Vui lòng thử lại.");
            }
        }
    }

    public class RoomViewModel
    {
        public string IdRoom { get; set; }
        public string NameRoom { get; set; }
        public double AreaRoom { get; set; }
        public int People { get; set; }
        public string PolicyRoom { get; set; }
        public int BedNumber { get; set; }
        public bool StatusRoom { get; set; }
        public string TypeRoom { get; set; }
        public decimal Price { get; set; }
        public string IdHotel { get; set; }
        public string HotelName { get; set; }
        public List<ImageRoomViewModel> Images { get; set; }
    }

    public class ImageRoomViewModel
    {
        public string IdImageRoom { get; set; }
        public byte[] ImageData { get; set; }
    }
}
