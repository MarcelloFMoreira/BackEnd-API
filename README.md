# Sistema de Locação de Motos API

## Visão Geral
Esta é uma API RESTful desenvolvida em .NET , projetada para gerenciar o processo de locação de motos e o cadastro de entregadores. O sistema abrange funcionalidades essenciais, desde o registro de motos e entregadores até a gestão completa de locações, incluindo cálculos de custos com base em planos de aluguel e multas por atraso ou devolução antecipada.

## 🚀 Tecnologias Utilizadas
- .NET C#
- Entity Framework Core para ORM
- PostgreSQL como banco de dados
- Docker para containerização
- Docker Compose para orquestração de containers

## 📁 Estrutura do Projeto
- 
   ```bash
   BackEnd-API/ 
   ├── Slm.Domain/           # Camada de domínio e entidades         
   ├── slm.infraestrutura/   # Camada de infraestrutura e persistência         
   ├── sistema_locacao_motos/ # Camada de aplicação e API         
   ├── Dockerfile           # Configuração do container da aplicação         
   ├── docker-compose.yml   # Orquestração dos serviços      
   └── README.md           # Documentação do projeto         

## ⚙️ Pré-requisitos
- .NET 9.0 SDK ou superior
- Docker Desktop com Docker Compose
- Git para clonar o repositório

## Como Executar o Projeto
1. Clone o repositório:
   ```bash
   git clone https://github.com/MarcelloFMoreira/BackEnd-API.git
2. Entre no arquivo clonado:
   ```bash
   cd BackEnd-API
3. Execute os containers
   ```bash
   docker-compose up --build
4. A aplicação estará disponível em:    </br>  
   API: http://localhost:8080    
   Swagger: http://localhost:8080/swagger

## 📊 Modelagem de dados
<img width="800"  alt="Modelagem de dado" src="https://github.com/user-attachments/assets/43f99e83-0366-4bea-bfdb-0f3fa5b2735d" />

## 🎯 Endpoints 
 Gestão de Motos
- POST - /motos - Cadastrar nova moto
- GET - /motos - Consultar motos
- PUT - /motos/{id}/placa - Modificar placa de uma moto
- GET - /motos/{id} - Consultar motos existentes por id
- DELETE - /motos/{id} -Remover uma moto

 Gestão de entregadores
- POST - /entregadores - Cadastrar entregador
- POST - /entregadores/{id}/cnh - Enviar foto da CNH

 Gestão de locação
- POST - /locacao - alugar moto
- GET - /locacao/{id} - Consultar locacao por id
- PUT - /locacao/{id}/devolucao Informar data de devolução e calcular valor total </br></br>


<table>
      <th>Dev</th>
      <th>Icon</th>
      <th>Contato</th>
    <tr>
      <td>Marcello de Freitas Moreira</td>
      <td><a href="https://github.com/MarcelloFMoreira"><img src="https://avatars.githubusercontent.com/u/161846509?v=4" height="50" style="max-width: 100%;"></a></td>
       <td><a href="https://api.whatsapp.com/send/?phone=11981733002&text&type=phone_number&app_absent=0"><img width="50" height="50" alt="wpp" src="https://github.com/user-attachments/assets/7838408f-6089-4437-a0bb-19336456e2e7" /></a></td>
   </tr>
</table>



