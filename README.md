# GloboClima - API de Clima e Países Favoritos

**GloboClima API** é uma aplicação backend que permite aos usuários consultar informações climáticas de cidades e dados de países, além de salvar suas cidades e países favoritos para futuras consultas. A API foi construída com **.NET Core 8** e utiliza integrações com APIs públicas para obter dados dinâmicos de clima e informações sobre países.

## Funcionalidades

### Backend

1. **Consumo de APIs Públicas**:
   - A API consome dados da **OpenWeatherMap API** para informações climáticas e da **REST Countries API** para dados sobre países (como população, idiomas, moedas, etc.).

2. **Gerenciamento de Favoritos**:
   - Usuários autenticados podem salvar cidades e países como favoritos para fácil acesso no futuro. Esses dados são armazenados no **DynamoDB**.

3. **Autenticação e Segurança**:
   - A API implementa autenticação JWT para garantir que apenas usuários autenticados possam gerenciar seus favoritos (salvar, listar, deletar).

4. **API REST**:
   - Endpoints RESTful para:
     - Consultar o clima de uma cidade e informações de um país.
     - Salvar, listar e deletar cidades e países favoritos.

5. **Documentação com Swagger**:
   - A API é totalmente documentada com **Swagger**, incluindo exemplos de uso e descrições detalhadas de todas as rotas.

## Arquitetura

- **Backend**: API RESTful construída com .NET Core 8, hospedada no **AWS Lambda** (ou EC2/ECS, dependendo da escolha do candidato) e utilizando **DynamoDB** para armazenamento.
- **Autenticação**: **JWT** é utilizado para autenticar usuários e proteger rotas sensíveis da API.
- **Documentação**: A API é documentada usando **Swagger** para fácil entendimento e testes.

## Tecnologias Utilizadas

- **Backend**: 
  - .NET Core 8
  - AWS Lambda / EC2 / ECS
  - DynamoDB
  - JWT (para autenticação)
  - Swagger (para documentação)

- **CI/CD e Automação**:
  - AWS CodePipeline / GitHub Actions para automação do deploy contínuo
  - Infraestrutura como Código (IaC) com **CloudFormation** ou **Terraform**

## Como Rodar o Projeto Localmente

### 1. Clone o repositório:

```bash
git clone https://github.com/seu-usuario/globo-clima.git
cd globo-clima
