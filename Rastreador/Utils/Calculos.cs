using Microsoft.AspNetCore.Routing.Constraints;

namespace Rastreador.Utils
{
    public static class Calculos
    {
        public static double CalcularDistanciaPercorrida(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            // Converte as coordenadas de graus para radianos
            double lat1Rad = latitude1 * (Math.PI / 180);
            double lon1Rad = longitude1 * (Math.PI / 180);
            double lat2Rad = latitude2 * (Math.PI / 180);
            double lon2Rad = longitude2 * (Math.PI / 180);
            // Calcula a diferença entre as longitudes
            double deltaLon = lon2Rad - lon1Rad;
            // Calcula a distância usando a fórmula do Haversine
            double a = Math.Pow(Math.Sin((lat2Rad - lat1Rad) / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(deltaLon / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            // Raio da Terra em metros
            double raioTerra = 6371000;
            // Distância percorrida em metros
            double distanciaPercorrida = raioTerra * c;
            return distanciaPercorrida;
        }

        public static bool VerificarSeEstarForaDaAncoragem(double latitudeAtual, double longitudeAtual, double latitudeAncoragem, double longitudeAncoragem, double raioAncoragem)
        {
            // Calcula a distância entre a posição atual e a posição da ancoragem
            double distancia = CalcularDistanciaPercorrida(latitudeAtual, longitudeAtual, latitudeAncoragem, longitudeAncoragem);
            // Verifica se a distância é maior que o raio da ancoragem
            if (distancia > raioAncoragem)
            {
                return false; // Fora da ancoragem
            }
            else
            {
                return true; // Dentro da ancoragem
            }
        }

        public static double CalcularDistanciaEmMetros(double latitudeAnterior, double longitudeAnterior, double latitudeAtual, double longitudeAtual)
        {
            const double raioTerra = 6371000;

            double dLat = GrausParaRad(latitudeAtual - latitudeAnterior);
            double dLon = GrausParaRad(longitudeAtual - longitudeAnterior);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(GrausParaRad(latitudeAnterior)) *
                Math.Cos(GrausParaRad(latitudeAtual)) *
                Math.Sin(dLon / 2) *
                Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return raioTerra * c;
        }

        private static double GrausParaRad(double graus)
        {
            return graus * (Math.PI / 180);
        }
    }
}