using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGF6
{
    public static class Parametros
    {
        public const double limiteInferior = -100.0;
        public const double limiteSuperior = 100.0;
        public const int bitsPorVariavel = 22;
        public const int tamanhoCromossomo = bitsPorVariavel * 2;
        public const int tamanhoPopulacao = 100;
        public const int numeroGeracoes = 400;
        public const double taxaCruzamento = 0.65;
        public const double taxaMutacao = 0.008;
        public const int qtdeElite = 1;
        public const int intervaloImpressao = 20;
    }
}
