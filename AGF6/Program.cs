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

}