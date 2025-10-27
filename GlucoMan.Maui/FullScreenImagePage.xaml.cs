using System;
using Microsoft.Maui.Controls;

namespace GlucoMan.Maui;

public partial class FullScreenImagePage : ContentPage
{
    public FullScreenImagePage(string imagePath)
    {
        InitializeComponent();
        if (!string.IsNullOrWhiteSpace(imagePath) && System.IO.File.Exists(imagePath))
        {
            fullImage.Source = ImageSource.FromFile(imagePath);
        }
    }

    private async void BtnCloseFullImage_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
