using AGF6;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

/* 
   Uso:
   dotnet run                    → uma execução com semente aleatória
   dotnet run -- 42              → uma execução com semente fixa (resultado sempre igual)
   dotnet run -- experimento 30  → 30 execuções (sementes 1 a 30) e um resumo de quantas
                                   chegaram ao ótimo global
*/

if (args.Length > 0 && args[0].Equals("experimento", StringComparison.OrdinalIgnoreCase))
{
    int execucoes = args.Length > 1 && int.TryParse(args[1], out int n) ? n : 30;
    RodarExperimento(execucoes);

} else
{
    int semente = args.Length > 0 && int.TryParse(args[0], out int valor) ? valor : Environment.TickCount;
    RodarUmaVez(semente);
}

static void ImprimirParametros()
{
    Console.WriteLine($"População: {Parametros.tamanhoPopulacao} | Gerações: {Parametros.numeroGeracoes} | " +
                  $"Cruzamento: {Parametros.taxaCruzamento:P0} | Mutação: {Parametros.taxaMutacao:P1} por bit | " +
                  $"Elite: {Parametros.qtdeElite}");
    Console.WriteLine();
}

static void RodarUmaVez(int semente)
{
    Console.WriteLine("=== Algoritmo Genético para maximizar a função F6 ===");
    Console.WriteLine();
    Console.WriteLine($"Semente: {semente}");
    ImprimirParametros();

    var exemplo = Individuo.CriarAleatorio(new Random(semente));
    Console.WriteLine("Exemplo de indivíduo (genótipo → fenótipo → aptidão):");
    Console.WriteLine($"  Cromossomo (x | y): {exemplo.CromossomoComoTexto()}");
    Console.WriteLine($"  Decodificado:       {exemplo}");
    Console.WriteLine();

    var ag = new AlgoritmoGenetico(new Random(semente));
    Individuo melhor = ag.Executar();

    Console.WriteLine();
    Console.WriteLine("=== Melhor solução encontrada ===");
    Console.WriteLine($"  Cromossomo: {melhor.CromossomoComoTexto()}");
    Console.WriteLine($"  {melhor}");
    Console.WriteLine($"  Ótimo teórico: x = 0, y = 0, F6 = 1");
    Console.WriteLine($"  Diferença para o ótimo: {1.0 - melhor.aptidao:E3}");
}

static void RodarExperimento(int execucoes)
{
    const double limiarGlobal = 0.999;
    ImprimirParametros();

    int sucessos = 0;
    double somaMelhores = 0;

    for(int semente = 1; semente <= execucoes; semente++)
    {
        var ag = new AlgoritmoGenetico(new Random(semente));
        Individuo melhor = ag.Executar(imprimir:false);

        bool achouOtimo = melhor.aptidao > limiarGlobal;

        if (achouOtimo)
        {
            sucessos++;
        }

        somaMelhores += melhor.aptidao;

        string situacao = achouOtimo ? "Ótimo global" : "Preso em máximo local";
        Console.WriteLine($"Semente {semente,3}: {melhor} -> {situacao}");
    }

    Console.WriteLine();
    Console.WriteLine($"Chegou ao ótimo global em {sucessos} de {execucoes} execuções ({(double)sucessos / execucoes:P0}).");
    Console.WriteLine($"Média do melhor F6: {somaMelhores / execucoes:F6}");
}