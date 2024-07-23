using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdminHotel
{
    public partial class QLKM : Window
    {
        private readonly HttpClient _httpClient;
        private string _selectedDiscountId;

        public QLKM()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadDiscountList();
        }

        private async void LoadDiscountList()
        {
            try
            {
                var discounts = await _httpClient.GetFromJsonAsync<List<CDiscount>>("https://localhost:7226/api/Discount/dsAllDiscount");
                DiscountListView.ItemsSource = discounts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khuyến mãi: {ex.Message}");
            }
        }

        private async void AddDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newDiscount = new CDiscount
                {
                    IdDiscount = IdDiscountTextBox.Text,
                    NameDiscount = NameDiscountTextBox.Text,
                    DescribeDiscount = DescribeDiscountTextBox.Text,
                    DiscountAmount = int.Parse(DiscountAmountTextBox.Text),
                    DiscountNumber = double.Parse(DiscountNumberTextBox.Text)
                };
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7226/api/Discount/adddiscount", newDiscount);
                if (response.IsSuccessStatusCode)
                {
                    LoadDiscountList();
                    MessageBox.Show("Thêm khuyến mãi thành công.");
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm khuyến mãi.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm khuyến mãi: {ex.Message}");
            }
        }

        private async void EditDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedDiscountId))
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi để sửa.");
                return;
            }

            try
            {
                var updatedDiscount = new CDiscount
                {
                    IdDiscount = _selectedDiscountId,
                    NameDiscount = NameDiscountTextBox.Text,
                    DescribeDiscount = DescribeDiscountTextBox.Text,
                    DiscountAmount = int.Parse(DiscountAmountTextBox.Text),
                    DiscountNumber = double.Parse(DiscountNumberTextBox.Text)
                };
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7226/api/Discount/edit", updatedDiscount);
                if (response.IsSuccessStatusCode)
                {
                    LoadDiscountList();
                    MessageBox.Show("Sửa khuyến mãi thành công.");
                }
                else
                {
                    MessageBox.Show("Lỗi khi sửa khuyến mãi.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa khuyến mãi: {ex.Message}");
            }
        }

        private async void DeleteDiscountButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedDiscountId))
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi để xóa.");
                return;
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"https://localhost:7226/api/Discount/deletDiscount?id={_selectedDiscountId}");
                if (response.IsSuccessStatusCode)
                {
                    LoadDiscountList();
                    MessageBox.Show("Xóa khuyến mãi thành công.");
                }
                else
                {
                    MessageBox.Show("Lỗi khi xóa khuyến mãi.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa khuyến mãi: {ex.Message}");
            }
        }

        private void DiscountListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DiscountListView.SelectedItem is CDiscount selectedDiscount)
            {
                _selectedDiscountId = selectedDiscount.IdDiscount;
                IdDiscountTextBox.Text = selectedDiscount.IdDiscount;
                NameDiscountTextBox.Text = selectedDiscount.NameDiscount;
                DescribeDiscountTextBox.Text = selectedDiscount.DescribeDiscount;
                DiscountAmountTextBox.Text = selectedDiscount.DiscountAmount.ToString();
                DiscountNumberTextBox.Text = selectedDiscount.DiscountNumber.ToString();
            }
        }
    }

    public class CDiscount
    {
        public string IdDiscount { get; set; }
        public string NameDiscount { get; set; }
        public string DescribeDiscount { get; set; }
        public int DiscountAmount { get; set; }
        public double DiscountNumber { get; set; }

        public static Discount chuyendoi(CDiscount cDiscount)
        {
            return new Discount
            {
                IdDiscount = cDiscount.IdDiscount,
                NameDiscount = cDiscount.NameDiscount,
                DescribeDiscount = cDiscount.DescribeDiscount,
                DiscountAmount = cDiscount.DiscountAmount,
                DiscountNumber = cDiscount.DiscountNumber
            };
        }
    }

    public class Discount
    {
        public string IdDiscount { get; set; }
        public string NameDiscount { get; set; }
        public string DescribeDiscount { get; set; }
        public int DiscountAmount { get; set; }
        public double DiscountNumber { get; set; }
    }
}
