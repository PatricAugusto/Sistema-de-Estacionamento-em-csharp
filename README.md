# 🚗 Sistema de Gerenciamento de Estacionamento

Um sistema robusto desenvolvido em **C#** e **.NET** para gerenciar o fluxo de veículos em um estacionamento, com validação de placas no padrão Mercosul e persistência de dados.

## 📋 Funcionalidades

- **Configuração de Preços:** Definição de valor inicial e valor por hora na inicialização.
- **Cadastro de Veículos:** Entrada de veículos com validação rigorosa (Regex) para o padrão Mercosul (`AAA1A11`).
- **Remoção com Cálculo:** Saída de veículos com cálculo automático do valor total baseado nas horas de permanência.
- **Listagem de Veículos:** Visualização em tempo real de todos os carros no pátio.
- **Histórico de Pagamentos:** Registro persistente de todas as transações (data, hora, placa e valor).
- **Persistência de Dados:** Uso de arquivos `.txt` para que os dados não sejam perdidos ao fechar o programa.
- **Interface Colorida:** Feedback visual no console para facilitar a navegação.

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C#
* **Plataforma:** .NET 8.0 (ou superior)
* **Manipulação de Arquivos:** System.IO
* **Validação:** System.Text.RegularExpressions (Regex)

## 🚀 Como Executar o Projeto

1.  **Pré-requisitos:**
    * Ter o [SDK do .NET](https://dotnet.microsoft.com/download) instalado.
    * Git (opcional para clonagem).

2.  **Passo a passo:**
    ```bash
    # Clonar o repositório ou baixar os arquivos
    git clone [https://github.com/seu-usuario/sistema-estacionamento.git](https://github.com/seu-usuario/sistema-estacionamento.git)

    # Acessar a pasta do projeto
    cd SistemaEstacionamento

    # Executar a aplicação
    dotnet run
    ```

## 📂 Estrutura de Arquivos

* `Program.cs`: Contém o menu interativo e a lógica principal de execução.
* `Estacionamento.cs`: Classe com as regras de negócio e manipulação de listas.
* `veiculos.txt`: Banco de dados simples para os veículos atualmente estacionados.
* `historico.txt`: Registro permanente de todos os pagamentos realizados.

## 🛡️ Tratamento de Erros

O sistema foi desenvolvido utilizando blocos `try-catch` para garantir que entradas inválidas (como letras em campos de preço) não causem o fechamento inesperado do software.

---
Desenvolvido como exercício de aprendizado em C#.