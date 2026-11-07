using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AGF6
{
    public class Individuo
    {
        public bool[] cromossomo {  get; }
        public double X { get; }
        public double Y { get; }
        public double aptidao { get; }

        public Individuo(bool[] cromossomo)
        {
            this.cromossomo = cromossomo;
            X = Decodificar(0);
            Y = Decodificar(Parametros.bitsPorVariavel);
            aptidao = FuncaoF6.F6(X, Y);
        }

        public static Individuo CriarAleatorio(Random rng)
        {
            var cromossomo = new bool[Parametros.tamanhoCromossomo];
            
            for (int i = 0; i < cromossomo.Length; i++)
            {
                cromossomo[i] = rng.Next(2) == 1;
            }

            return new Individuo(cromossomo);
        }
/*
 Converter 22 bits do cromossomo em um número real entre -100 e 100
*/
        private double Decodificar(int posicaoInicial)
        {
            bool[] bits = new bool[Parametros.bitsPorVariavel];
            Array.Copy(cromossomo,posicaoInicial,bits,0,bits.Length);

            bits = GrayParaBinario(bits);

            long numeroInteiro = BinarioParaInteiro(bits);

            return InteiroParaReal(numeroInteiro);

        }

        private static bool[] GrayParaBinario(bool[] gray)
        {
            bool[] binario = new bool[gray.Length];
            binario[0] = gray[0];

/*
Regra do código Gray: o primeiro bit é igual.
Para os outros: se o bit Gray for 1, o bit binário é o INVERSO do bit binário anterior;
se for 0, é IGUAL ao bit binário anterior.
*/
            for (int i = 1; i < gray.Length; i++)
            {
                if (gray[i])
                {
                    binario[i] = !binario[i - 1];
                } else
                {
                    binario[i] = binario[i - 1];
                }
            }
            return binario;
        }

        private static long BinarioParaInteiro(bool[] bits)
        {
            long valor = 0;

            foreach (bool bit in bits)
            {
                valor = valor * 2;

                if (bit)
                {
                    valor = valor + 1;
                }
            }

            return valor;
        }

        private static double InteiroParaReal(long numeroInteiro)
        {
            long maiorInteiro = (long)Math.Pow(2, Parametros.bitsPorVariavel) - 1;

            double proporcao = (double) numeroInteiro/maiorInteiro;
            
            double amplitude = Parametros.limiteSuperior - Parametros.limiteInferior;

            return Parametros.limiteInferior + proporcao * amplitude;
        }

        private string CromossomoComoTexto()
        {
            var bits = this.cromossomo.Select(b => b ? '1' : '0').ToArray();
            string parteX = new string(bits, 0, Parametros.bitsPorVariavel);
            string parteY = new string(bits, Parametros.bitsPorVariavel, Parametros.bitsPorVariavel);

            return $"{parteX} | {parteY}";
        }

        public override string ToString()
        {
            return $"x = {this.X,11:F6} y = {this.Y,11:F6} F6 = {this.aptidao:F8}"; //11 caract. na tela e 6 casas decimais
        }
    }
}
