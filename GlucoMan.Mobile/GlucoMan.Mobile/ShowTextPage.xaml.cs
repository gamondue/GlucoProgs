using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowTextPage : ContentPage
    {
        private string fileContent;

        public ShowTextPage(string fileContent)
        {
            InitializeComponent();
            txtText.Text = fileContent;
        }
    }
}