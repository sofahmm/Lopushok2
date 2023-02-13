using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lopushok2.DB;
using Microsoft.Win32;

namespace Lopushok2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public static Product selectProduct { get; set; }
        public List<Workshop> workshops { get; set; }
        public List<Material> materials { get; set; }
        public List<ProductType> types { get; set; }
        public ProductPage(Product product)
        {
            InitializeComponent();
            selectProduct = product;

            workshops = Connection.lopushokEntities.Workshop.ToList();
            cbWork.ItemsSource = workshops;
            cbWork.DisplayMemberPath = "Name";

            types = Connection.lopushokEntities.ProductType.ToList();
            cbType.ItemsSource = types;
            cbType.DisplayMemberPath = "Name";

            if (product != null)
            {
                cbWork.SelectedItem = product.Workshop;
                cbType.SelectedItem = product.ProductType;
                btnDelete.Visibility = Visibility.Visible;
            }

            materials = Connection.lopushokEntities.Material.ToList();
            cbMaterial.ItemsSource = materials;
            cbMaterial.DisplayMemberPath = "Name";

            DataContext = this;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var messageDelete = MessageBox.Show("Вы хотите удалить продукт?", "Внимание", MessageBoxButton.YesNoCancel);
            if (messageDelete == MessageBoxResult.Yes)
            {
                Connection.lopushokEntities.Product.Remove(selectProduct);
                NavigationService.Navigate(new ProductListPage());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int newArticle = Convert.ToInt32(tbArticl.Text.Trim());
            var uniqAricle = Connection.lopushokEntities.Product.FirstOrDefault(x => x.Id == newArticle);
            if (uniqAricle == selectProduct || uniqAricle == null)
            {
                try
                {
                    Connection.lopushokEntities.Product.Add(selectProduct);
                }
                catch
                {
                    MessageBox.Show("Введены некорректные значения");
                    return;
                }
                Connection.lopushokEntities.SaveChanges();
                NavigationService.Navigate(new ProductListPage());
            }
            else if (uniqAricle != selectProduct)
            {
                MessageBox.Show("Артикул не уникальный, придумайте другой!");
            }
        }


        private void cbMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lvListMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };

            if (fileDialog.ShowDialog().Value)
            {
                var image = File.ReadAllBytes(fileDialog.FileName);
                selectProduct.Image = image;

                imgPhoto.Source = new BitmapImage(new Uri(fileDialog.FileName));
            }
        }

        private void btnDelPhoto_Click_1(object sender, RoutedEventArgs e)
        {
            var messageDelete = MessageBox.Show("Вы хотите удалить фото?", "Внимание", MessageBoxButton.YesNoCancel);
            if (messageDelete == MessageBoxResult.Yes)
            {
                selectProduct.Image = null;
                imgPhoto.Source = null;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
