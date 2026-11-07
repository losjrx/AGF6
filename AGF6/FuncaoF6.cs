using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGF6
{
    public static class FuncaoF6
    {
        public static double F6(double x, double y)
        {
            double somaQuadrados = x * x + y * y;
            double seno = Math.Sin(Math.Sqrt(somaQuadrados));

            double numerador = seno * seno - 0.5;
            double denominador = Math.Pow((1.0 + (0.001 * somaQuadrados)), 2);

            return 0.5 - (numerador / denominador);
        }
    }
}
