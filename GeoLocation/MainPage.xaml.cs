using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel.Communication;

namespace GeoLocation;

public partial class MainPage : ContentPage
{
    // Base URL for Bing Maps (from slide 12)
    private string _baseUrl = "https://bing.com/maps/default.aspx?cp=";

    public MainPage()
    {
        InitializeComponent();
    }

    // Button click event handler
    private async void OnShareClicked(object sender, EventArgs e)
    {
        // Step 1: Request location permission from user
        var permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        // Step 2: Check if user granted permission
        if (permissionStatus == PermissionStatus.Granted)
        {
            await ShareLocation();  // Get location and share it
        }
        else
        {

            // ✅ Current .NET MAUI syntax
            await DisplayAlertAsync("Permission Denied", "We need your location to share it.", "OK");
        }
    }

    private async Task ShareLocation()
    {
        try
        {
            // Step 1: Create a geolocation request with best accuracy
            var locationRequest = new GeolocationRequest(GeolocationAccuracy.Best);

            // Step 2: Get the user's current location
            var location = await Geolocation.GetLocationAsync(locationRequest);

            // Step 3: Check if we got a valid location
            if (location != null)
            {
                // Format coordinates (latitude, longitude)
                string coordinates = $"{location.Latitude},{location.Longitude}";

                // Update the label on screen
                LocationLabel.Text = $"📍 Your location:\nLatitude: {location.Latitude}\nLongitude: {location.Longitude}";

                // Create a Bing Maps link
                string mapUrl = $"{_baseUrl}{coordinates}";

                // Step 4: Share the location using the Share API
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = $"I'm currently at this location: {mapUrl}",
                    Title = "Share my location"
                });
            }
            else
            {

                // ✅ Current .NET MAUI syntax
                await DisplayAlertAsync("Location Failed", "Could not get your location. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {

            // ✅ Current .NET MAUI syntax
            await DisplayAlertAsync("Error", $"Failed to get location: {ex.Message}", "OK");
        }
    }
}