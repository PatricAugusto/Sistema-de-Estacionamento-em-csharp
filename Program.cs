using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace SistemaEstacionamento
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configurações iniciais
            decimal precoInicial = 0;
            decimal precoPorHora = 0;

            Console.WriteLine("Bem-vindo ao sistema de estacionamento!");
            Console.WriteLine("Digite o preço inicial:");
            precoInicial = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Digite o preço por hora:");
            precoPorHora = Convert.ToDecimal(Console.ReadLine());

            // Instanciando nossa classe de lógica
            Estacionamento es = new Estacionamento(precoInicial, precoPorHora);

            string opcao = string.Empty;
            bool exibirMenu = true;

            // Loop do menu principal
            while (exibirMenu)
            {
                Console.Clear();
                Console.WriteLine("Digite a sua opção:");
                Console.WriteLine("1 - Cadastrar veículo");
                Console.WriteLine("2 - Remover veículo");
                Console.WriteLine("3 - Listar veículos");
                Console.WriteLine("4 - Encerrar");

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
                        exibirMenu = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }

                Console.WriteLine("Pressione uma tecla para continuar");
                Console.ReadLine();
            }

            Console.WriteLine("O programa se encerrou.");
        }
    }

    // Classe que contém a lógica do negócio
    public class Estacionamento
    {
        private decimal precoInicial = 0;
        private decimal precoPorHora = 0;
        private List<string> veiculos = new List<string>();
        private readonly string caminhoArquivo = "veiculos.txt";

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
            CarregarDados(); // Carrega os dados assim que a classe é criada
        }

        private void SalvarDados()
    {
        // File.WriteAllLines cria ou sobrescreve o arquivo com a lista
        File.WriteAllLines(caminhoArquivo, veiculos);
    }

    private void CarregarDados()
    {
        // Verifica se o arquivo existe antes de tentar ler
        if (File.Exists(caminhoArquivo))
        {
            veiculos = File.ReadAllLines(caminhoArquivo).ToList();
        }
    }


        public void AdicionarVeiculo()
{
    Console.WriteLine("Digite a placa do veículo (Padrão Mercosul: AAA1A11):");
    string placa = Console.ReadLine().ToUpper().Trim();

    if (ValidarPlacaMercosul(placa))
        {
            if (veiculos.Any(v => v == placa))
            {
                Console.WriteLine("Veículo já está no pátio.");
            }
            else
            {
                veiculos.Add(placa);
                SalvarDados(); // <--- Salvando no arquivo
                Console.WriteLine("Veículo cadastrado!");
            }
    }
    else
    {
        Console.WriteLine("Placa inválida! O padrão deve ser ABC1D23.");
    }
}

// Método auxiliar para validação
private bool ValidarPlacaMercosul(string placa)
{
    // O padrão Mercosul: 3 letras, 1 número, 1 letra, 2 números
    string padrao = @"^[A-Z]{3}[0-9][A-Z][0-9]{2}$";
    return Regex.IsMatch(placa, padrao);
}

        public void RemoverVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para remover:");
            string placa = Console.ReadLine().ToUpper();

            // Verifica se o veículo existe
            if (veiculos.Contains(placa))
            {
                Console.WriteLine("Digite a quantidade de horas que o veículo permaneceu estacionado:");
                int horas = Convert.ToInt32(Console.ReadLine());
                
                // Cálculo matemático: Preço Inicial + (Preço por Hora * Horas)
                decimal valorTotal = precoInicial + (precoPorHora * horas); 

                veiculos.Remove(placa);
                SalvarDados(); // <--- Atualizando o arquivo após remoção
                Console.WriteLine($"O veículo {placa} foi removido e o preço total foi de: R$ {valorTotal}");
            }
            else
            {
                Console.WriteLine("Desculpe, esse veículo não está estacionado aqui. Confira se digitou a placa corretamente.");
            }
        }

        public void ListarVeiculos()
        {
            if (veiculos.Count > 0)
            {
                Console.WriteLine("Os veículos estacionados são:");
                foreach (var v in veiculos)
                {
                    Console.WriteLine(v);
                }
            }
            else
            {
                Console.WriteLine("Não há veículos estacionados.");
            }
        }
    }
}