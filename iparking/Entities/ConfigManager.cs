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
        public const String SharedFile = "iparking";

        public const String WebService = "http://10.0.0.7/iparkweb";

        // Coordenadas por defecto para centrar el Mapa (Obelisco -34.6037345,-58.3837591)
        public const Double DefaultLatMap = -34.542496;
        public const Double DefaultLongMap = -58.5670585;
        // Un Zoom que me permite ver bien donde estoy parado
        public const Int32 DefaultZoomMap = 15;
    }
}