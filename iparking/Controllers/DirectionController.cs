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
using Android.Gms.Maps.Model;
using iparking.Entities;

namespace iparking.Controllers
{
    class DirectionController
    {
        public static PolylineOptions ResolveRoute(GoogleDirection googleDirection)
        {
            PolylineOptions po = new PolylineOptions();

            if (googleDirection.routes.Count > 0)
            {
                string encodedPoints = googleDirection.routes[0].overview_polyline.points;
                List<LatLng> decodedPoints = DecodePolyLinePoints(encodedPoints);

                LatLng[] latLngPoints = new LatLng[decodedPoints.Count];
                int index = 0;
                foreach (LatLng loc in decodedPoints)
                {
                    latLngPoints[index++] = loc;
                }

                po.InvokeColor(Android.Graphics.Color.Red);
                po.Geodesic(true);
                po.Add(latLngPoints);
            }

            return po;
        }

        private static List<LatLng> DecodePolyLinePoints(string encodedPoints)
        {
            List<LatLng> poly = new List<LatLng>();

            if (string.IsNullOrEmpty(encodedPoints))
            {
                return poly;
            }

            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    double finalLat = Convert.ToDouble(currentLat) / 100000.0;
                    double finalLng = Convert.ToDouble(currentLng) / 100000.0;
                    LatLng p = new LatLng(finalLat, finalLng);
                    poly.Add(p);
                }
            }
            catch
            {
                poly.Clear();
            }

            return poly;
        }
    }
}