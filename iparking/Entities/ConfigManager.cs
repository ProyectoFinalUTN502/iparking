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

namespace iparking.Entities
{
    public class ConfigManager
    {
        private const String GoogleAPIKey = "AIzaSyBEUsgLqfDt_TmY-qeZVTVnytjeMyof2hI";
        public const String SharedFile = "iparking";

        public const String WebService = "http://192.168.0.133:5490/iparkservice";
        public const String GoogleService = "https://maps.googleapis.com/maps/api/directions/json?key=" + GoogleAPIKey + "&";

        // Coordenadas por defecto para centrar el Mapa (Obelisco -34.6037345,-58.3837591)
        public const Double DefaultLatMap = -34.5432143;//-34.542496;
        public const Double DefaultLongMap = -58.5675542; // -58.5670585;

        // Un Zoom que me permite ver bien donde estoy parado
        public const Int32 DefaultZoomMap = 15;
        // Un zoom mas amplio para que el Drageo se haga mas facil
        public const Int32 DefaultDraggZoomMap = 12;
    }
}