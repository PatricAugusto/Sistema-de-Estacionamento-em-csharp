using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SistemaEstacionamento
{
    // Classe principal que executa o programa
    class Program
    {
        static void Main(string[] args)
        {
            // Variáveis para configuração de preços
            decimal precoInicial = 0;
            decimal precoPorHora = 0;

            Console.Clear();
            ExibirCabecalho();

            // Bloco de configuração inicial com tratamento de erros (Try-Catch)
            bool configuracaoValida = false;
            while (!configuracaoValida)
            {
                try
                {
                    Console.Write("Digite o preço inicial (R$): ");
                    precoInicial = Convert.ToDecimal(Console.ReadLine());

                    Console.Write("Digite o preço por hora (R$): ");
                    precoPorHora = Convert.ToDecimal(Console.ReadLine());

                    configuracaoValida = true;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Erro: Use apenas números e vírgula para valores decimais.");
                    Console.ResetColor();
                }
            }

            // Instância da classe de negócio
            Estacionamento es = new Estacionamento(precoInicial, precoPorHora);

            bool exibirMenu = true;
            while (exibirMenu)
            {
                Console.Clear();
                ExibirCabecalho();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("  1 - Cadastrar veículo");
                Console.WriteLine("  2 - Remover veículo");
                Console.WriteLine("  3 - Listar veículos");
                Console.WriteLine("  4 - Exibir Historico");
                Console.WriteLine("  5 - Encerrar");
                Console.ResetColor();
                Console.WriteLine("  ──────────────────────────────");
                Console.Write("  Escolha uma opção: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        es.AdicionarVeiculo();
                        break;
                    case "2":
                        es.RemoverVeiculo();
                        break;
                    case "3":
                        es.ListarVeiculos();
                        break;
                    case "4":
                        es.ExibirHistorico();
                        break;
                    case "5":
                        exibirMenu = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }

                if (exibirMenu)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Sistema encerrado. Até logo!");
        }

        static void ExibirCabecalho()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
  ╔════════════════════════════════════════╗
  ║       SISTEMA DE ESTACIONAMENTO        ║
  ╚════════════════════════════════════════╝");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Classe responsável por gerenciar a lógica de veículos e persistência.
    /// </summary>
    public class Estacionamento
    {
        private decimal precoInicial;
        private decimal precoPorHora;
        private List<string> veiculos = new List<string>();
        private readonly string caminhoArquivo = "veiculos.txt";
        private readonly string caminhoHistorico = "historico.txt";

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
            CarregarDados();
        }

        // --- MÉTODOS DE PERSISTÊNCIA (Arquivos) ---

        private void SalvarDados()
        {
            try
            {
                File.WriteAllLines(caminhoArquivo, veiculos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar dados: {ex.Message}");
            }
        }

        private void CarregarDados()
        {
            if (File.Exists(caminhoArquivo))
            {
                veiculos = File.ReadAllLines(caminhoArquivo).ToList();
            }
        }

        // --- MÉTODOS DE NEGÓCIO ---

        public void AdicionarVeiculo()
        {
            Console.WriteLine("\n--- CADASTRAR VEÍCULO ---");
            Console.Write("Digite a placa (Padrão Mercosul AAA1A11): ");
            string placa = Console.ReadLine().ToUpper().Trim();

            if (ValidarPlacaMercosul(placa))
            {
                if (veiculos.Any(v => v == placa))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("⚠️ Este veículo já está estacionado.");
                }
                else
                {
                    veiculos.Add(placa);
                    SalvarDados();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("✅ Veículo cadastrado com sucesso!");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Placa inválida! Use o formato ABC1D23.");
            }
            Console.ResetColor();
        }

        public void RemoverVeiculo()
        {
            Console.WriteLine("\n--- REMOVER VEÍCULO ---");
            Console.Write("Digite a placa para remover: ");
            string placa = Console.ReadLine().ToUpper().Trim();

            if (veiculos.Contains(placa))
            {
                try
                {
                    Console.Write("Digite a quantidade de horas de permanência: ");
                    int horas = Convert.ToInt32(Console.ReadLine());

                    decimal valorTotal = precoInicial + (precoPorHora * horas);

                    veiculos.Remove(placa);
                    SalvarDados();

                    RegistrarPagamento(placa, valorTotal);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✅ Veículo {placa} removido. Total: R$ {valorTotal:N2}");
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Erro: Informe um número inteiro para as horas.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Veículo não encontrado no sistema.");
            }
            Console.ResetColor();
        }

        private void RegistrarPagamento(string placa, decimal valor)
        {
            string registro = $"{DateTime.Now:dd/MM/yyyy HH:mm:ss} | Placa: {placa} | Valor: R$ {valor:N2}";

            // File.AppendAllText adiciona ao final do arquivo em vez de apagar o anterior
            File.AppendAllText(caminhoHistorico, registro + Environment.NewLine);
        }

        public void ListarVeiculos()
        {
            Console.WriteLine("\n--- VEÍCULOS ESTACIONADOS ---");
            if (veiculos.Any())
            {
                int contador = 1;
                foreach (var v in veiculos)
                {
                    Console.WriteLine($"{contador} - {v}");
                    contador++;
                }
            }
            else
            {
                Console.WriteLine("Pátio vazio.");
            }
        }

        public void ExibirHistorico()
        {
            Console.WriteLine("\n--- HISTÓRICO DE PAGAMENTOS ---");
            if (File.Exists(caminhoHistorico))
            {
                string[] linhas = File.ReadAllLines(caminhoHistorico);
                decimal totalGeral = 0;

                foreach (var linha in linhas)
                {
                    Console.WriteLine(linha);
                    // Lógica simples para somar o total (opcional)
                }
                Console.WriteLine("  ──────────────────────────────");
                Console.WriteLine($"  Total de registros: {linhas.Length}");
            }
            else
            {
                Console.WriteLine("Nenhum histórico encontrado.");
            }
        }

        /// <summary>
        /// Valida se a placa segue o padrão Mercosul usando Expressão Regular (Regex).
        /// </summary>
        private bool ValidarPlacaMercosul(string placa)
        {
            string padrao = @"^[A-Z]{3}[0-9][A-Z][0-9]{2}$";
            return Regex.IsMatch(placa, padrao);
        }
    }
}