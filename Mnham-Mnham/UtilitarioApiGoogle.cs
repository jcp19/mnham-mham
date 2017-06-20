using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.App;
using Android.Gms.Location;
using Android.Widget;
using Android.Locations;

namespace Mnham_Mnham
{
	public class UtilitarioApiGoogle : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks,
        GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener 
    {
		private static readonly string Tag = typeof(UtilitarioApiGoogle).Name;
		private Context contexto;
	    private GoogleApiClient clienteApiGoogle;
	    private LocationRequest pedidoLoc;

        private const long FastestInterval = 500;
	    private const long LocationUpdateInterval = 1000;

        public int ResConexao { get; private set; }

        public UtilitarioApiGoogle(Context contexto)
		{
			this.contexto = contexto;
			ConstruirClienteApiGoogle();
		}

        private void ConstruirClienteApiGoogle()
        {
            clienteApiGoogle = new GoogleApiClient.Builder(contexto)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(LocationServices.API)
                .Build();
        }

        public GoogleApiClient ObterClienteApiGoogle()
        {
            return clienteApiGoogle;
        }

        public bool ServicosGooglePlayEstaoDisponiveis()
        {
            GoogleApiAvailability disponibilidadeApiGoogle = GoogleApiAvailability.Instance;
            ResConexao = disponibilidadeApiGoogle.IsGooglePlayServicesAvailable(contexto);

            return (ResConexao == ConnectionResult.Success);
        }

		public void Connect()
		{
		    if (clienteApiGoogle != null)
		    {
                clienteApiGoogle.Connect();
		    }
		}

        public void Disconnect()
        {
            if (clienteApiGoogle != null && clienteApiGoogle.IsConnected)
            {
                clienteApiGoogle.Disconnect();
            }
        }

        public async void PedirAtualizacoesLocalizacao(int priority, long fastestInterval, long locationUpdateInterval)
        {
            if (clienteApiGoogle != null)
            {
                if (pedidoLoc == null)
                {
                    pedidoLoc = new LocationRequest();
                }
                pedidoLoc.SetPriority(priority);
                pedidoLoc.SetFastestInterval(fastestInterval);
                pedidoLoc.SetInterval(locationUpdateInterval);
                await LocationServices.FusedLocationApi.RequestLocationUpdates(clienteApiGoogle, pedidoLoc, this);
            }
        }

        // Usa os parâmetros por omissão (ver constantes FastestInterval e LocationUpdateInterval acima).
        public void PedirAtualizacoesLocalizacao()
        {
            PedirAtualizacoesLocalizacao(LocationRequest.PriorityHighAccuracy, FastestInterval, LocationUpdateInterval);
        }

        public async void PararAtualizacoesLocalizacao()
        {
            if (clienteApiGoogle != null)
            {
                await LocationServices.FusedLocationApi.RemoveLocationUpdates(clienteApiGoogle, this);
                pedidoLoc = null;
            }
        }

        public Location ObterLocalizacao()
        {
            return LocationServices.FusedLocationApi.GetLastLocation(clienteApiGoogle);
        }

        public bool IsConnected()
		{
			return (clienteApiGoogle == null) ? false : clienteApiGoogle.IsConnected;
		}

		public void OnConnected(Bundle bundle)
		{
			Toast.MakeText(contexto, "A ligação aos serviços da Google Play foi bem sucedida.", ToastLength.Short).Show();
        }

		public void OnConnectionSuspended(int causa)
		{
			string msg;

	        switch (causa)
	        {
	            case GoogleApiClient.ConnectionCallbacks.CauseNetworkLost:
	                msg = "A ligação ao serviço da Google Play foi abaixo por falta de rede.";
	                break;
	            case GoogleApiClient.ConnectionCallbacks.CauseServiceDisconnected:
	                msg = "O serviço da Google Play foi terminado.";
	                break;
	            default:
	                msg = "O serviço da Google Play foi suspenso por causa desconhecida.";
	                break;
	        }
	        Toast.MakeText(contexto, msg, ToastLength.Short);
            Disconnect();
            Connect();
		}

		public void OnConnectionFailed(ConnectionResult resultadoConexao)
		{
			AlertDialog.Builder alerta = new AlertDialog.Builder(contexto);

	        alerta.SetTitle("Não foi possível ligar aos serviços do Google Play. Código do erro: " + resultadoConexao.ErrorCode);
	        alerta.SetPositiveButton("OK", (sender, e) => { });
	        alerta.Create().Show();
		}

        public void OnLocationChanged(Location location)
        {
        }
    }
}
