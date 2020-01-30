using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ULFG.Core.Data;

namespace ULFG.Core.Logic
{
    /// <summary>
    /// Clase para registrar un dispositivo cliente en el servicio de notificaciones mediante peticiones HTTP
    /// </summary>
    public class RegisterClient
    {
        readonly string POST_URL = Constants.ApplicationURL + "deviceregister";

        private class DeviceRegistration
        {
            public string Platform { get; set; }
            public string Handle { get; set; }
            public string UserTag { get; set; }
        }

        /// <summary>
        /// Registra el dispositivo cliente en el servicio de notificaciones
        /// </summary>
        /// <param name="platform">plataforma del dispositivo</param>
        /// <param name="handle">identificador del dispositivo</param>
        /// <param name="usertag">id del usuario</param>
        /// <returns></returns>
        public async Task RegisterAsync(string platform, string handle, string usertag)
        {
            var regId = await RetrieveRegistrationIdOrRequestNewOneAsync();
            var deviceRegistration = new DeviceRegistration
            {
                Platform = platform,
                Handle = handle,
                UserTag = usertag
            };

            var statusCode = await UpdateRegistrationAsync(regId, deviceRegistration);

            if (statusCode == HttpStatusCode.Gone)
            {
                // regId is expired, deleting from local storage & recreating
                Settings.NHRegistrationID = string.Empty;

                regId = await RetrieveRegistrationIdOrRequestNewOneAsync();
                statusCode = await UpdateRegistrationAsync(regId, deviceRegistration);
            }

            if (statusCode != HttpStatusCode.NoContent)
            {
                Debug.WriteLine("Registration error, estatus code: " + statusCode.ToString());
                Debugger.Break();
            }
        }

        private async Task<HttpStatusCode> UpdateRegistrationAsync(string regId, DeviceRegistration deviceRegistration)
        {
            using (var httpClient = new HttpClient())
            {
                var putUri = POST_URL + "/" + regId;
                var json = JsonConvert.SerializeObject(deviceRegistration);
                var response = await httpClient.PutAsync(putUri, new StringContent(json, Encoding.UTF8, "application/json"));
                return response.StatusCode;
            }
        }

        private async Task<string> RetrieveRegistrationIdOrRequestNewOneAsync()
        {
            var settings = Settings.NHRegistrationID;
            if (settings.Equals(string.Empty))
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(POST_URL, new StringContent(""));
                    if (response.IsSuccessStatusCode)
                    {
                        string regId = await response.Content.ReadAsStringAsync();
                        regId = regId.Substring(1, regId.Length - 2);
                        Settings.NHRegistrationID = regId;
                    }
                    else
                    {
                        throw new HttpRequestException("Bad Response from Server");
                    }
                }
            }
            return Settings.NHRegistrationID;
        }
        /// <summary>
        /// Elimina el registro del usuario actual en el servicio de notificaciones
        /// </summary>
        public async Task UnregisterUser()
        {
            var regId = Settings.NHRegistrationID;
            if (regId != string.Empty)
            {
                using(var httpClient = new HttpClient())
                {
                    var uri = POST_URL + "/" + regId;
                    var response = await httpClient.DeleteAsync(uri);
                    Settings.NHRegistrationID = string.Empty;
                    Settings.LoggedUser = string.Empty;
                    Debug.WriteLine(response.StatusCode);
                }
            }
            else
                Debug.WriteLine(HttpStatusCode.NotFound);
        }
    }
}

