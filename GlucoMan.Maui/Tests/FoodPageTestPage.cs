using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui.Tests;

/// <summary>
/// Esempio di test page per dimostrare l'uso di FoodPage con entrambi i tipi di navigazione
/// Questo file è solo a scopo dimostrativo e può essere rimosso in produzione
/// </summary>
public partial class FoodPageTestPage : ContentPage
{
    private BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    private Food testFood;
    
    public FoodPageTestPage()
    {
        InitializeComponent();
        
        // Crea un food di test
        testFood = new Food(new UnitOfFood("g", 1))
        {
            Name = "Test Food",
            Description = "Food per test navigazione",
            CarbohydratesPercent = new DoubleAndText { Double = 50.0 }
        };
    }
    
    private void InitializeComponent()
    {
        Title = "FoodPage Navigation Test";
        
        var stackLayout = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = 20
        };
        
        // Titolo
        var titleLabel = new Label
        {
            Text = "Test FoodPage Navigation Patterns",
            FontSize = 20,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 0, 0, 20)
        };
        stackLayout.Add(titleLabel);
        
        // Test 1: Modal Navigation con await
        var btnModalWithAwait = new Button
        {
            Text = "Test 1: Modal Navigation (with await)", 
            BackgroundColor = Colors.LightBlue
        };
        btnModalWithAwait.Clicked += BtnModalWithAwait_Clicked;
        stackLayout.Add(btnModalWithAwait);
        
        var lblModal = new Label
        {
            Text = "Apre FoodPage come modal e attende il risultato",
            FontSize = 12,
            TextColor = Colors.Gray,
            Margin = new Thickness(10, 0, 0, 0)
        };
        stackLayout.Add(lblModal);
        
        // Test 2: Regular Navigation con await
        var btnRegularWithAwait = new Button
        {
            Text = "Test 2: Regular Navigation (with await)",
            BackgroundColor = Colors.LightGreen
        };
        btnRegularWithAwait.Clicked += BtnRegularWithAwait_Clicked;
        stackLayout.Add(btnRegularWithAwait);
        
        var lblRegular = new Label
        {
            Text = "Apre FoodPage con navigazione normale e attende il risultato",
            FontSize = 12,
            TextColor = Colors.Gray,
            Margin = new Thickness(10, 0, 0, 0)
        };
        stackLayout.Add(lblRegular);
        
        // Test 3: Fire-and-forget
        var btnFireAndForget = new Button
        {
            Text = "Test 3: Fire-and-Forget Navigation",
            BackgroundColor = Colors.LightCoral
        };
        btnFireAndForget.Clicked += BtnFireAndForget_Clicked;
        stackLayout.Add(btnFireAndForget);
        
        var lblFireForget = new Label
        {
            Text = "Apre FoodPage senza attendere il risultato",
            FontSize = 12,
            TextColor = Colors.Gray,
            Margin = new Thickness(10, 0, 0, 0)
        };
        stackLayout.Add(lblFireForget);
        
        // Area risultati
        var resultsLabel = new Label
        {
            Text = "Risultati Test:",
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 20, 0, 10)
        };
        stackLayout.Add(resultsLabel);
        
        var resultsFrame = new Frame
        {
            BorderColor = Colors.Gray,
            CornerRadius = 5,
            Padding = 10,
            Content = new Label
            {
                Text = "I risultati dei test appariranno qui...",
                FontSize = 12
            }
        };
        stackLayout.Add(resultsFrame);
        
        // ScrollView per contenere tutto
        var scrollView = new ScrollView
        {
            Content = stackLayout
        };
        
        Content = scrollView;
    }
    
    /// <summary>
    /// Test 1: Modal Navigation con attesa del risultato
    /// Questo è il pattern raccomandato quando serve il risultato
    /// </summary>
    private async void BtnModalWithAwait_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Crea una copia del food per il test
            var foodCopy = new Food(new UnitOfFood("g", 1))
            {
                Name = testFood.Name,
                Description = testFood.Description,
                CarbohydratesPercent = new DoubleAndText { Double = testFood.CarbohydratesPercent.Double }
            };
            
            // Apri FoodPage come modal
            var foodPage = new FoodPage(foodCopy);
            await Navigation.PushModalAsync(foodPage);
            
            // Attendi il risultato
            bool foodWasChosen = await foodPage.PageClosedTask;
            
            // Mostra il risultato
            if (foodWasChosen && foodPage.FoodIsChosen)
            {
                await DisplayAlert(
                    "Test 1 - Success", 
                    $"Food confermato:\n" +
                    $"Nome: {foodPage.CurrentFood.Name}\n" +
                    $"CHO%: {foodPage.CurrentFood.CarbohydratesPercent?.Double ?? 0}",
                    "OK");
            }
            else
            {
                await DisplayAlert(
                    "Test 1 - Cancelled", 
                    "L'utente ha annullato l'operazione",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Test 1 - Error", ex.Message, "OK");
            General.LogOfProgram?.Error("FoodPageTestPage - BtnModalWithAwait_Clicked", ex);
        }
    }
    
    /// <summary>
    /// Test 2: Regular Navigation con attesa del risultato
    /// Funziona come la navigazione normale ma permette di attendere il risultato
    /// </summary>
    private async void BtnRegularWithAwait_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Crea una copia del food per il test
            var foodCopy = new Food(new UnitOfFood("g", 1))
            {
                Name = testFood.Name,
                Description = testFood.Description,
                CarbohydratesPercent = new DoubleAndText { Double = testFood.CarbohydratesPercent.Double }
            };
            
            // Apri FoodPage con navigazione normale
            var foodPage = new FoodPage(foodCopy);
            await Navigation.PushAsync(foodPage);
            
            // Attendi il risultato (opzionale)
            bool foodWasChosen = await foodPage.PageClosedTask;
            
            // Mostra il risultato
            if (foodWasChosen && foodPage.FoodIsChosen)
            {
                await DisplayAlert(
                    "Test 2 - Success", 
                    $"Food confermato:\n" +
                    $"Nome: {foodPage.CurrentFood.Name}\n" +
                    $"CHO%: {foodPage.CurrentFood.CarbohydratesPercent?.Double ?? 0}",
                    "OK");
            }
            else
            {
                await DisplayAlert(
                    "Test 2 - Cancelled", 
                    "L'utente ha annullato l'operazione",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Test 2 - Error", ex.Message, "OK");
            General.LogOfProgram?.Error("FoodPageTestPage - BtnRegularWithAwait_Clicked", ex);
        }
    }
    
    /// <summary>
    /// Test 3: Fire-and-forget
    /// Apre la pagina senza attendere il risultato
    /// Utile quando non serve sapere cosa ha fatto l'utente
    /// </summary>
    private async void BtnFireAndForget_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Crea una copia del food per il test
            var foodCopy = new Food(new UnitOfFood("g", 1))
            {
                Name = testFood.Name,
                Description = testFood.Description,
                CarbohydratesPercent = new DoubleAndText { Double = testFood.CarbohydratesPercent.Double }
            };
            
            // Apri FoodPage e non attendere
            var foodPage = new FoodPage(foodCopy);
            await Navigation.PushAsync(foodPage);
            
            // Mostra un messaggio che il codice continua immediatamente
            await DisplayAlert(
                "Test 3 - Info", 
                "Pagina aperta! Il codice continua senza attendere il risultato.\n" +
                "Puoi ancora interagire con questa pagina.",
                "OK");
            
            // Nota: foodPage.FoodIsChosen può ancora essere controllato 
            // in seguito se mantieni il riferimento alla pagina
        }
        catch (Exception ex)
        {
            await DisplayAlert("Test 3 - Error", ex.Message, "OK");
            General.LogOfProgram?.Error("FoodPageTestPage - BtnFireAndForget_Clicked", ex);
        }
    }
}
