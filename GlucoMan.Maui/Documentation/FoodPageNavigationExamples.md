# FoodPage Navigation Examples

## Overview
`FoodPage` can now be navigated in two ways:
1. **Modal Navigation** - Used when you need to get a result back (recommended for data selection)
2. **Regular Navigation** - Used for standard page flow (like other pages in the app)

Both methods preserve the ability to read the user's choice through the `PageClosedTask` and `FoodIsChosen` properties.

---

## Example 1: Modal Navigation (with result waiting)

This is the recommended approach when you need to get data back from FoodPage:

```csharp
private async void btnOpenFoodPage_Click(object sender, EventArgs e)
{
    // Create the FoodPage with the current food
    var foodPage = new FoodPage(currentFood);
    
    // Navigate as modal
    await Navigation.PushModalAsync(foodPage);
    
    // Wait for the page to be closed and get the result
    bool foodWasChosen = await foodPage.PageClosedTask;
    
    // Check if the user chose/confirmed the food
    if (foodWasChosen && foodPage.FoodIsChosen)
    {
        // User confirmed - use the updated food data
        Food updatedFood = foodPage.CurrentFood;
        // Do something with updatedFood...
        DisplayAlert("Success", $"Food updated: {updatedFood.Name}", "OK");
    }
    else
    {
        // User cancelled - no changes
        DisplayAlert("Info", "Operation cancelled", "OK");
    }
}
```

---

## Example 2: Regular Navigation (like other pages)

This approach works like standard navigation in the app:

```csharp
private async void btnOpenFoodPage_Click(object sender, EventArgs e)
{
    // Create the FoodPage with the current food
    var foodPage = new FoodPage(currentFood);
    
    // Navigate normally (pushed onto navigation stack)
    await Navigation.PushAsync(foodPage);
    
    // Optional: Wait for result if needed
    bool foodWasChosen = await foodPage.PageClosedTask;
    
    if (foodWasChosen && foodPage.FoodIsChosen)
    {
        // Handle the result
        Food updatedFood = foodPage.CurrentFood;
        // Process updatedFood...
    }
}
```

---

## Example 3: Fire-and-forget Navigation

When you don't need to wait for the result:

```csharp
private async void btnOpenFoodPage_Click(object sender, EventArgs e)
{
    var foodPage = new FoodPage(currentFood);
    
    // Just navigate - don't wait for result
    await Navigation.PushAsync(foodPage);
    
    // Code continues immediately
    // You can still check foodPage.FoodIsChosen later if the page reference is maintained
}
```

---

## Example 4: From FoodsPage (actual implementation)

This is how it's used in FoodsPage.xaml.cs:

```csharp
private async void btnFoodDetails_Click(object sender, EventArgs e)
{
    FromUiToClass();
    foodPage = new FoodPage(Food);
    
    // Can be navigated as modal or regular - both work now
    await Navigation.PushModalAsync(foodPage);
    
    // Wait for the page to be closed and get the result
    bool foodWasChosen = await foodPage.PageClosedTask;
    
    // Check if the user chose/confirmed the food
    if (foodWasChosen && foodPage.FoodIsChosen)
    {
        bl.FromFoodToFoodInMeal(foodPage.CurrentFood, bl.FoodInMeal);
        FromClassToUi();
    }
}
```

---

## Benefits of This Approach

1. **Flexibility**: Can use either modal or regular navigation
2. **Consistency**: Same pattern as FoodsPage and WeighFoodPage
3. **Clean Code**: TaskCompletionSource provides async/await support
4. **Back Button Support**: Properly handles hardware back button
5. **No Breaking Changes**: Existing code continues to work

---

## Technical Details

### How it works:
- `TaskCompletionSource<bool>` creates a task that completes when the page closes
- `PageClosedTask` property exposes this task for awaiting
- `FoodIsChosen` indicates if user confirmed (true) or cancelled (false)
- The page automatically detects if it was opened as modal or regular navigation and closes appropriately

### Thread Safety:
The TaskCompletionSource is thread-safe and can be safely called from UI thread.

### Memory Management:
The TaskCompletionSource is properly disposed when the page is closed, preventing memory leaks.
