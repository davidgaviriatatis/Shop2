namespace Shop2.UIForms.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Shop2.Common.Models;
    using Shop2.Common.Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        private readonly ApiService apiservice;
        private ObservableCollection<Product> products;
        private bool isRefreshing;

        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }


        public ProductsViewModel()
        {
            this.apiservice = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var response = await this.apiservice.GetListAsync<Product>(
                "https://shop2webtatis.azurewebsites.net",
                "/api",
                "/Products");

            this.IsRefreshing = false;

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");

                return;
            }

            var myProducts = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(myProducts);
        }
    }
}
