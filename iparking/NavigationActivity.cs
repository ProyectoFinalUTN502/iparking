using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;

using iparking.Managment;
using iparking.Entities;
using Newtonsoft.Json;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Android.Net.Wifi;

namespace iparking
{
    [Activity(Label = "NavigationActivity", Theme = "@style/MyTheme.Base")]
    public class NavigationActivity : Activity
    {
        private const string errCode = "900";
        private const string errMsg = "No se ha podido iniciar la Navegacion Interna";
        private const int wifiInterval = 500;
        private const int controlInterval = 1000;

        private WebView mWebView;
        private ProgressBar mProgressBar;
        private Button mButtonCancel;
        private WebClient mWebClient;
        private List<int> mNavData;

        private FileManager fm;

        private FragmentTransaction trans;
        private DialogNoPosition mDialogNoPosition;
        private DialogNavigationControl mDialogNavigationControl;
        private DialogParkingOk mDialogParkingOk;

        private Timer mTimerControl;
        private Timer mTimerWifi;

        private int mPositionID;
        private string mMacAddress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Navigation);

            fm = new FileManager();

            mPositionID = 0;
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            mWebView = FindViewById<WebView>(Resource.Id.webView);
            mButtonCancel = FindViewById<Button>(Resource.Id.buttonCancel);
            mButtonCancel.Click += MButtonCancel_Click;

            mTimerControl = new Timer();
            mTimerControl.Interval = controlInterval;
            mTimerControl.Elapsed += MTimerControl_Elapsed;

            mTimerWifi = new Timer();
            mTimerWifi.Interval = wifiInterval;
            mTimerWifi.Elapsed += MTimerWifi_Elapsed;
            mTimerWifi.Enabled = true;

            // Cargo la Mac de mi Celu
            mMacAddress = DeviceManager.GetMacAdress(this);

            try
            {
                string data = Intent.GetStringExtra("data");
                mNavData = JsonConvert.DeserializeObject<List<int>>(data);
                GetPosition(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error ** : " + ex.ToString());
                Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }

        }

        private async void MTimerWifi_Elapsed(object sender, ElapsedEventArgs e)
        {
            var operation = await WifiScan();
            Console.WriteLine("** Escaneando Redes Wifi **");
        }

        private async void MTimerControl_Elapsed(object sender, ElapsedEventArgs e)
        {
            var result = await NavigationControl();
            Console.WriteLine("** Evento de Control: " + result + " **");
            if (result == "OK")
            {
                Console.WriteLine("** Evento de Control: OK **");
            } 
            else
            {
                mTimerControl.Enabled = false;
                FragmentTransaction trans = FragmentManager.BeginTransaction();
                mDialogNavigationControl = new DialogNavigationControl();
                mDialogNavigationControl.Show(trans, "Control de Navegacion");
                mDialogNavigationControl.mParkingEvent += MDialogNavigationControl_mParkingEvent;
                mDialogNavigationControl.mRoutingEvent += MDialogNavigationControl_mRoutingEvent;
                Console.WriteLine("** Evento de Control: ERROR");
            }
        }

        private void MDialogNavigationControl_mRoutingEvent(object sender, OnRoutingEvent e)
        {
            // Si recalcula tiene que levantar el timer que esta apagado
            mPositionID = 0;
            GetPosition(true);
            Console.WriteLine("** El usuario quiere recalcular **");
        }

        private void MDialogNavigationControl_mParkingEvent(object sender, OnParkingEvent e)
        {
            Park();
            Console.WriteLine("** El usuario quiere Estacionar **");
        }

        private void MButtonCancel_Click(object sender, EventArgs e)
        {
            // Levantar Dialog de Confirmacion para Cancelar
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogParkingCancel dialogCancel = new DialogParkingCancel();
            dialogCancel.Show(trans, "Dialog Cancel");
            dialogCancel.mCancelEvent += (object s, OnCancelEvent ev) => 
            {
                CancelReservation();
            };

        }

        //==================================================================================
        public void GetPosition(bool reRoute)
        {
            try
            {
                mProgressBar.Visibility = ViewStates.Visible;

                Uri url = new Uri(ConfigManager.WebService + "/" + "positionProvider.php?pk_id=" + mNavData[0] + "&vt_id=" + mNavData[1]);
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadDataAsync(url);
                webClient.DownloadDataCompleted += (object s, System.Net.DownloadDataCompletedEventArgs e) =>
                {
                    RunOnUiThread(() => { mProgressBar.Visibility = ViewStates.Invisible; });
                    string json = Encoding.UTF8.GetString(e.Result);
                    OperationResult op = JsonConvert.DeserializeObject<OperationResult>(json);

                    if (op.error)
                    {
                        // No hay posicion para ese vehiculo, tengo que levantar el modal
                        trans = FragmentManager.BeginTransaction();
                        mDialogNoPosition = new DialogNoPosition();
                        mDialogNoPosition.Show(trans, "No Hay Posiciones Disponibles");
                        mDialogNoPosition.mCancelEvent += (object o, OnCancelEvent onCancelEvent) =>
                        {
                            // Elimino las posiciones de ese Usuario
                            ClearUserPositions();

                            // Le aviso a los Raspis que ya no tienen que monitorearme
                            Kill();
                            Managment.ActivityManager.TakeMeTo(this, typeof(MainActivity), true);
                        };
                    }
                    else
                    {
                        // Guardo la posicion
                        mPositionID = Convert.ToInt32(op.data);
                        if (reRoute)
                        {
                            // Tengo que recalcular
                            ReloadBorwser();
                        }
                        else
                        {
                            // Le aviso a los Raspi que me tienen que monitorear a mi
                            Bootstrap();
                            //Levanto el Browser por Primera vez
                            LoadBrowserView();
                        }
                        
                        Console.WriteLine("** Posicion Obtenida: " + mPositionID.ToString() + " **");
                    }
                };
            }
            catch (Exception ex)
            {
                Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }
        }

        public void LoadBrowserView()
        {
            string urlRef = ConfigManager.WebService + "/navigation.php?id=" + mPositionID.ToString() + "&" + "cl_id=" + mNavData[2];
            mWebClient = new WebClient();
            mWebClient.mOnProgressBarChanged += (int state) =>
            {
                if (state == 0)
                {
                    // termino, ocultar la progress
                    mProgressBar.Visibility = ViewStates.Invisible;
                    mTimerControl.Enabled = true;
                }
                else
                {
                    mProgressBar.Visibility = ViewStates.Visible;
                }
            };

            mWebView.Settings.JavaScriptEnabled = true;
            mWebView.LoadUrl(urlRef);
            mWebView.SetWebViewClient(mWebClient);
        }

        public void ReloadBorwser()
        {
            // Para cuando tengo que recalcular
            string urlRef = ConfigManager.WebService + "/navigation.php?id=" + mPositionID.ToString() + "&" + "cl_id=" + mNavData[2];
            mWebClient.ShouldOverrideUrlLoading(mWebView, urlRef);
        }

        public void CancelReservation()
        {
            if (mPositionID == 0)
            {
                return;
            }

            try
            {
                mProgressBar.Visibility = ViewStates.Visible;

                Uri url = new Uri(ConfigManager.WebService + "/" + "cancelNavigation.php?id=" + mPositionID.ToString());
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadDataAsync(url);
                webClient.DownloadDataCompleted += (object s, System.Net.DownloadDataCompletedEventArgs e) =>
                {
                    RunOnUiThread(() => { mProgressBar.Visibility = ViewStates.Invisible; });
                    string json = Encoding.UTF8.GetString(e.Result);
                    OperationResult op = JsonConvert.DeserializeObject<OperationResult>(json);

                    // Le aviso a los Raspis que ya no tienen que monitorearme
                    Kill();

                    // Elimino las posiciones de este Cliente 
                    ClearUserPositions();

                    // Si falla no me interesa mostrarselo al usuario, entonces redirecciono de una
                    Managment.ActivityManager.TakeMeTo(this, typeof(MainActivity), true);
                };
            }
            catch (Exception ex)
            {
                Managment.ActivityManager.ShowError(this, new Error(errCode, errMsg));
            }
        }

        public void Park()
        {
            try
            {
                mTimerControl.Enabled = false;
                mTimerWifi.Enabled = false;

                string vehicle = fm.GetValue("vehicle");
                if (vehicle == string.Empty) { return; }

                System.Net.WebClient wclient = new System.Net.WebClient();
                Uri uri = new Uri(ConfigManager.WebService + "/addHistoric.php");
                NameValueCollection param = new NameValueCollection();

                param.Add("id", mPositionID.ToString());
                param.Add("vehicle_id", vehicle);

                wclient.UploadValuesCompleted += Wclient_UploadValuesCompleted; ;
                wclient.UploadValuesAsync(uri, param);

            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error al Cargar en Historial **");
            }

        }

        private void Wclient_UploadValuesCompleted(object sender, System.Net.UploadValuesCompletedEventArgs e)
        {
            try
            {
                string json = Encoding.UTF8.GetString(e.Result);
                OperationResult op = JsonConvert.DeserializeObject<OperationResult>(json);

                // Elimina las posiciones de ese Usuario
                ClearUserPositions();

                // Levantar Dialog de Agradecimiento y Cerrar la Aplicacion
                FragmentTransaction trans = FragmentManager.BeginTransaction();
                mDialogParkingOk = new DialogParkingOk();
                mDialogParkingOk.Show(trans, "iParking");
                mDialogParkingOk.mCancelEvent += (object o, OnCancelEvent cancelEvent) => 
                {
                    // Cierro la Aplicacion
                    Kill();
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine("** Error al tratar de Apagar la Aplicacion **");
            }

        }

        public void Bootstrap()
        {
            if (mMacAddress == string.Empty)
            {
                string var = "No fue posible iniciar el Monitoreo Wifi";
                Managment.ActivityManager.ShowError(this, new Error(errCode, var));
                return;
            }

            try
            {
                string urlRef = "bootstrap.php" + "?" + "mac=" + mMacAddress;
                Uri url = new Uri(ConfigManager.WebService + "/" + urlRef);
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadDataAsync(url);
                webClient.DownloadDataCompleted += (object s, System.Net.DownloadDataCompletedEventArgs e) =>
                {
                    try
                    {
                        string json = Encoding.UTF8.GetString(e.Result);
                        OperationResult op = JsonConvert.DeserializeObject<OperationResult>(json);

                        if (op.error) { Console.WriteLine("** No fue posible realizar Bootstrap **"); }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("** No fue posible realizar Bootstrap **");
                    }
                };
            }
            catch (Exception ex)
            {
                string var = "No fue posible iniciar el Monitoreo Wifi";
                Managment.ActivityManager.ShowError(this, new Error(errCode, var));
            }
        }

        public void Kill()
        {
            if (mMacAddress == string.Empty) { return; }

            try
            {
                string urlRef = "kill.php" + "?" + "mac=" + mMacAddress;
                Uri url = new Uri(ConfigManager.WebService + "/" + urlRef);
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadDataAsync(url);
                webClient.DownloadDataCompleted += (object s, System.Net.DownloadDataCompletedEventArgs e) =>
                {
                    try
                    {
                        string json = Encoding.UTF8.GetString(e.Result);
                        OperationResult op = JsonConvert.DeserializeObject<OperationResult>(json);
                        if (op.error) { Console.WriteLine("** No fue posible realizar Kill **"); }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("** No fue posible realizar Kill **");
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error al intentar cancelar el monitoreo **");
            }
        }

        public void ClearUserPositions()
        {
            try
            {
                string urlRef = "clearClientPosition.php" + "?" + "id=" + mNavData[2].ToString();
                Uri url = new Uri(ConfigManager.WebService + "/" + urlRef);
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadDataAsync(url);
                webClient.DownloadDataCompleted += (object s, System.Net.DownloadDataCompletedEventArgs e) =>
                {
                    try
                    {
                        string json = Encoding.UTF8.GetString(e.Result);
                        OperationResult op = JsonConvert.DeserializeObject<OperationResult>(json);
                        if (op.error) { Console.WriteLine("** Error al intentar limpiar el historial de posiciones **"); }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("** Error al intentar limpiar el historial de posiciones **");
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error al intentar limpiar el historial de posiciones **");
            }
        }

        public async Task<string> NavigationControl()
        {
            string result = string.Empty;
            byte[] bytes;
            System.Net.WebClient mClient = new System.Net.WebClient();
            string urlRef = "navigationControl.php" + "?" + "id=" + mPositionID.ToString();
            Uri url = new Uri(ConfigManager.WebService + "/" + urlRef);

            bytes = await mClient.DownloadDataTaskAsync(url);
            result = System.Text.Encoding.UTF8.GetString(bytes);
            return result;
        }     

        public async Task<int> WifiScan()
        {
            try
            {
                var WifiManager = (WifiManager)GetSystemService(WifiService);
                WifiManager.StartScan();
                await Task.Delay(10); // 10 ms
            }
            catch (Exception ex)
            {
                Console.WriteLine("** Error al Escanear Redes **");
            }
            
            return 1;
        }
       
    }

}