using System;
using System.Collections.Generic;

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

        public Estacionamento(decimal precoInicial, decimal precoPorHora)
        {
            this.precoInicial = precoInicial;
            this.precoPorHora = precoPorHora;
        }

        public void AdicionarVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para estacionar:");
            string placa = Console.ReadLine();
            veiculos.Add(placa.ToUpper());
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