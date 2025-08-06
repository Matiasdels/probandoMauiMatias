using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System.Text.Json;
using System.Threading.Tasks;

namespace probandoMaui
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            var nombre = Preferences.Get("Nombre", "Sin nombre");
            var edad = Preferences.Get("Edad", 99); //el 99 marca el valor por defecto de Edad si no hubiese ningun dato cargado
            var apellido = Preferences.Get("Apellido", "Gómez");
            var fecha = Preferences.Get("FechaNac", DateTime.Now.AddYears(25));
            Preferences.Set("Nombre", "Juan");
            Preferences.Set("FechaNac", DateTime.Now);
            Preferences.Remove("Edad");
            Preferences.Clear();

            List<string> list = new List<string>();
            list.Add("Esto es una lista");
            list.Add("Otro texto agregado");

            string filename = FileSystem.AppDataDirectory + "/ejemploArchivo.json";

            var serializedData = JsonSerializer.Serialize(list);
            File.WriteAllText(filename, serializedData); 

            var rawData = File.ReadAllText(filename);
            var listaDez = JsonSerializer.Deserialize<List<string>>(rawData);   



        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            //if(DeviceInfo.Current.Platform == DevicePlatform.Android)
            //{

            //}


            try
            {
                var foto = await MediaPicker.CapturePhotoAsync();
                if (foto != null) {
                    var stream = await foto.OpenReadAsync();
                    await DisplayAlert("GENIAL", "Ya sacaste la foto", "ok");
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("ERROR", "error al abrir la camara", "Cerrar");
            }
        }

        private async void btnHuella_Clicked(object sender, EventArgs e)
        {
            try
            {
                var request = new AuthenticationRequestConfiguration("obligatorio", "Para probar la huella");
                var result = await CrossFingerprint.Current.AuthenticateAsync(request);
                if (result.Authenticated)
                {
                    btnFoto.Background = new Color(200, 200, 200);

                }
                else
                {
                    await DisplayAlert("Error de autenticación", "tiene huella", "cerrar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error de autenticación", "tiene huella", "Cerrar");
            }
            
        }

        private async void btnGps_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    await DisplayAlert("Localizacion", "Estoy en latitud:" + location.Latitude + " y longitud:" + location.Longitude, "Cerrar");
                }
              
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo conectar con el gps", "Cerrar");
            }
        }
    }

}
