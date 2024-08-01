using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home(string role)
        {
            InitializeComponent();
            SetupMenu(role);
        }
        private void SetupMenu(string role)
        {
            if (role == "Admin")
            {
                CreateAccount.Visibility = Visibility.Visible;
                QLKS.Visibility = Visibility.Visible;
                QLP.Visibility = Visibility.Visible;
                QLKM.Visibility = Visibility.Visible;
                QLDV.Visibility = Visibility.Visible;
                QLDH.Visibility = Visibility.Visible;
                QLKH.Visibility = Visibility.Visible;
            }
            else if (role == "User")
            {
                CreateAccount.Visibility = Visibility.Collapsed;
                QLKS.Visibility = Visibility.Visible;
                QLP.Visibility = Visibility.Visible;
                QLKM.Visibility = Visibility.Visible;
                QLDV.Visibility = Visibility.Visible;
                QLDH.Visibility = Visibility.Visible;
                QLKH.Visibility = Visibility.Visible;
            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            QLTK qLTK = new QLTK();
            qLTK.Show();
        }

        private void QLKS_Click(object sender, RoutedEventArgs e)
        {
            QLKS qlks = new QLKS();
            qlks.Show();
        }

        private void QLDV_Click(object sender, RoutedEventArgs e)
        {
            QLDV qLDV = new QLDV();
            qLDV.Show();
        }

        private void QLKM_Click(object sender, RoutedEventArgs e)
        {
            QLKM qLKM = new QLKM();
            qLKM.Show();
        }

        private void QLDH_Click(object sender, RoutedEventArgs e)
        {
            QLDH qLDH = new QLDH();
            qLDH.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow f = new MainWindow();
            f.Show();
            this.Close();
        }

        private void QLKH_Click(object sender, RoutedEventArgs e)
        {
            QLKH qLKH = new QLKH();
            qLKH.Show();
        }

        private void QLP_Click(object sender, RoutedEventArgs e)
        {
            QLP qLP = new QLP();
            qLP.Show();
        }
    }
}
