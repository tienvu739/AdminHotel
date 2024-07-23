using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdminHotel
{
    public partial class QLDV : Window
    {
        private readonly HttpClient _httpClient;
        private string _selectedServiceId;

        public QLDV()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadServiceList();
        }

        private async void LoadServiceList()
        {
            try
            {
                var services = await _httpClient.GetFromJsonAsync<List<CService>>("https://localhost:7226/api/Service/dsServicer");
                ServiceListView.ItemsSource = services;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách dịch vụ: {ex.Message}");
            }
        }

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newService = new CService
                {
                    IdService = Guid.NewGuid().ToString(),
                    NameService = ServiceNameTextBox.Text
                };
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7226/api/Service/addService", newService);
                if (response.IsSuccessStatusCode)
                {
                    LoadServiceList();
                    MessageBox.Show("Thêm dịch vụ thành công.");
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm dịch vụ.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dịch vụ: {ex.Message}");
            }
        }

        private async void EditServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedServiceId))
            {
                MessageBox.Show("Vui lòng chọn dịch vụ để sửa.");
                return;
            }

            try
            {
                var updatedService = new CService
                {
                    IdService = _selectedServiceId,
                    NameService = ServiceNameTextBox.Text
                };
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7226/api/Service/edit", updatedService);
                if (response.IsSuccessStatusCode)
                {
                    LoadServiceList();
                    MessageBox.Show("Sửa dịch vụ thành công.");
                }
                else
                {
                    MessageBox.Show("Lỗi khi sửa dịch vụ.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa dịch vụ: {ex.Message}");
            }
        }

        private async void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedServiceId))
            {
                MessageBox.Show("Vui lòng chọn dịch vụ để xóa.");
                return;
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7226/api/Service/deletService?id={_selectedServiceId}");
                if (response.IsSuccessStatusCode)
                {
                    LoadServiceList();
                    MessageBox.Show("Xóa dịch vụ thành công.");
                }
                else
                {
                    MessageBox.Show("Lỗi khi xóa dịch vụ.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dịch vụ: {ex.Message}");
            }
        }

        private void ServiceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceListView.SelectedItem is CService selectedService)
            {
                _selectedServiceId = selectedService.IdService;
                ServiceNameTextBox.Text = selectedService.NameService;
                ServiceNamePlaceholder.Visibility = string.IsNullOrEmpty(ServiceNameTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void ServiceNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ServiceNamePlaceholder.Visibility = string.IsNullOrEmpty(ServiceNameTextBox.Text) ? Visibility.Visible : Visibility.Hidden;
        }
    }

    public class CService
    {
        public string IdService { get; set; }
        public string NameService { get; set; }

        public static Service chuyendoi(CService cService)
        {
            return new Service
            {
                IdService = cService.IdService,
                NameService = cService.NameService
            };
        }
    }

    public class Service
    {
        public string IdService { get; set; }
        public string NameService { get; set; }
    }
}
