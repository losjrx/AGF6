using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGF6
{
    internal class AlgoritmoGenetico
    {
        private readonly Random _geradorAleatorio;

        public AlgoritmoGenetico(Random geradorAleatorio)
        {
            _geradorAleatorio = geradorAleatorio;
        }

        public Individuo Executar()
        {
            List<Individuo> populacao = InicializarPopulacao();

            Individuo melhorDeTodos = MelhorDaPopulacao(populacao);

            ImprimirCabecalhoRelatorio();
            ImprimirGeracao(0, populacao);

            /*
             * Evolução
             */

            for (int geracao = 1; geracao <= Parametros.numeroGeracoes; geracao++)
            {
                populacao = GerarNovaPopulacao(populacao);

                Individuo melhorDaGeracao = MelhorDaPopulacao(populacao);

                if(melhorDaGeracao.aptidao > melhorDeTodos.aptidao)
                {
                    melhorDeTodos = melhorDaGeracao;
                }

                bool gerarRelatorio = geracao % Parametros.intervaloImpressao == 0 || geracao == Parametros.numeroGeracoes;

                if (gerarRelatorio)
                {
                    ImprimirGeracao(geracao, populacao);
                }
            }

            return melhorDeTodos;

        }

        /// Passo 1
        private List<Individuo> InicializarPopulacao()
        {
            var populacao = new List<Individuo>(Parametros.tamanhoPopulacao);

            for (int i = 0; i < Parametros.tamanhoPopulacao; i++)
            {
                populacao.Add(Individuo.CriarAleatorio(_geradorAleatorio));
            }

            return populacao;
        }

        /// Passo 2
        private List<Individuo> GerarNovaPopulacao(List<Individuo> populacaoAtual)
        {
            var novaPopulacao = new List<Individuo>(Parametros.tamanhoPopulacao);

            /// Elitismo
            novaPopulacao.AddRange(populacaoAtual.OrderByDescending(i => i.aptidao).Take(Parametros.qtdeElite));

            /// Completar a população com filhos: Seleção, Cruzamento e Mutação
            
            while( novaPopulacao.Count < Parametros.tamanhoPopulacao)
            {
                
                /// Seleção
                Individuo pai = Selecionar(populacaoAtual);
                Individuo mae = Selecionar(populacaoAtual);

                /// Cruzamento
                (bool[] filho1, bool[] filho2) = Cruzar(pai.cromossomo, mae.cromossomo);

                /// Mutação
                Mutar(filho1);
                Mutar(filho2);

                /// entrar na geração
                novaPopulacao.Add(new Individuo(filho1));
                if(novaPopulacao.Count < Parametros.tamanhoPopulacao)
                {
                    novaPopulacao.Add(new Individuo(filho2));
                }
            }
            
            return novaPopulacao;

        }

        // Seleção por ROLETA
        private Individuo Selecionar(List<Individuo> populacao)
        {
            double somaAptidoes = populacao.Sum(i => i.aptidao);
            double sorteio = _geradorAleatorio.NextDouble() * somaAptidoes;

            double acumulado = 0;
            foreach(Individuo individuo in populacao)
            {
                acumulado += individuo.aptidao;
                if(acumulado >= sorteio)
                {
                    return individuo;
                }
            }

            return populacao[^1];
        }

        // CRUZAMENTO
        private (bool[] filho1, bool[] filho2) Cruzar(bool[] pai, bool[] mae)
        {
            var filho1 = (bool[])pai.Clone();
            var filho2 = (bool[])mae.Clone(); 

            if(_geradorAleatorio.NextDouble() < Parametros.taxaCruzamento)
            {
                int pontoDeCorte = _geradorAleatorio.Next(1,pai.Length); // de 1 até tamanho - 1

                for(int i = pontoDeCorte; i < pai.Length; i++)
                {
                    filho1[i] = mae[i];
                    filho2[i] = pai[i];
                }
            }
            return (filho1, filho2);
        }

        //MUTACAO
        private void Mutar(bool[] cromossomo)
        {
            for(int i = 0; i < cromossomo.Length; i++)
            {
                if(_geradorAleatorio.NextDouble() < Parametros.taxaMutacao)
                {
                    cromossomo[i] = !cromossomo[i];
                }
            }
        }


        private static Individuo MelhorDaPopulacao(List<Individuo> populacao)
        {
            return populacao.MaxBy(i => i.aptidao)!;
        }

        public static void ImprimirCabecalhoRelatorio()
        {
            Console.WriteLine($"{"Geração",7} | {"Melhor F6",12} | {"Média F6",12} | {"x do melhor",12} | {"y do melhor",12}");
            Console.WriteLine(new string('-', 68));
        }

        private static void ImprimirGeracao(int geracao, List<Individuo> populacao)
        {
            Individuo melhor = MelhorDaPopulacao(populacao);
            double media = populacao.Average(i => i.aptidao);
            Console.WriteLine($"{geracao,7} | {melhor.aptidao,12:F8} | {media,12:F8} | {melhor.X,12:F6} | {melhor.Y,12:F6}");
        }
    }
}
