using System;
using System.Collections.Generic;
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

namespace Lopushok2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        public List<Product> Products { get; set; }
        public List<Product> FilteredProducts { get; set; }
        public List<ProductType> ProductTypes { get; set; }
        public Dictionary<string, Func<Product, object>> Sortings { get; set; }
        public ProductListPage()
        {
            InitializeComponent();
            Products  = new List<Product>(Connection.lopushokEntities.Product.ToList());
            lvProduct.ItemsSource = Products;
            ProductTypes = new List<ProductType>(Connection.lopushokEntities.ProductType.ToList());
            ProductTypes.Insert(0, new ProductType() { Name = "Все типы" });
            Sortings = new Dictionary<string, Func<Product, object>>
            {
                { "Без сортировки", x => x.Id },
                { "Минимальная стоимость по убыванию", x => x.MinPrice },   //reverse
                { "Минимальная стоимость по возрастанию", x => x.MinPrice },
                { "Номер цеха по убыванию", x => x.WorkshopId },            //reverse
                { "Номер цеха по возрастанию", x => x.WorkshopId },
                { "Наименование по убыванию", x => x.Name },                //reverse
                { "Наименование по возрастанию", x => x.Name }
            };
            this.DataContext = this;
        }

        public void Filter()
        {
            List<Product> filterProduct = Products;

            if (tbSearch.Text.Trim().Length != 0)
            {
                filterProduct = filterProduct.Where(x => x.Name.Contains(tbSearch.Text.Trim())).ToList();
            }

            if (cbFiltr.SelectedItem != null)
            {
                var selectFiltr = cbFiltr.SelectedItem as ProductType;
                if (selectFiltr.Id != 0)
                {
                    filterProduct = filterProduct.Where(x => x.ProductType.Id == selectFiltr.Id).ToList();
                }
            }

            if (cbSort.SelectedItem != null)
            {
                var selectSort = cbSort.SelectedItem as Sorting;

                if (selectSort.Id == 1)
                {
                    filterProduct = filterProduct.OrderByDescending(x => x.MinPrice).ToList();
                }
                else if (selectSort.Id == 2)
                {
                    filterProduct = filterProduct.OrderBy(x => x.MinPrice).ToList();
                }
                else if (selectSort.Id == 3)
                {
                    filterProduct = filterProduct.OrderByDescending(x => x.Name).ToList();
                }
                else if (selectSort.Id == 4)
                {
                    filterProduct = filterProduct.OrderBy(x => x.Name).ToList();
                }
                else if (selectSort.Id == 5)
                {
                    filterProduct = filterProduct.OrderBy(x => x.WorkshopId).ToList();
                }
                else if (selectSort.Id == 6)
                {
                    filterProduct = filterProduct.OrderByDescending(x => x.WorkshopId).ToList();
                }
            }

            lvProduct.ItemsSource = filterProduct;
            lvProduct.Items.Refresh();
        }

        public class Sorting
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void tbSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cbFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void btnAddProd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage(null));
        }

        private void lvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvProduct.SelectedItem != null)
            {
                var selectProd = lvProduct.SelectedItem as Product;
                NavigationService.Navigate(new ProductPage(selectProd));
            }
        }
    }
}
