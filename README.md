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
   Swagger: http://localhost:8080/swagger

## 📊 Modelagem de dados
<img width="800" alt="cardinalidade" src="https://github.com/user-attachments/assets/36d480f0-515a-4e55-8e9e-7bc4e9656b32" />

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
   <th>Funcionalidades</th>
   <th>Status</th>
   <tr>
      <td>Cadastro de moto levando em consideração dados obrigatório</td>
      <td>✔️ Realizado</td>
   </tr>
      <tr>
      <td>Evento de cadastro de moto com Mensageria</td>
      <td>Implementação futura</td>
   </tr>
      <tr>
      <td>Consulta das motos Cadastradas com possibilidade de filtrar por placa</td>
      <td>✔️ Realizado</td>
   </tr>
      <tr>
      <td>Modificar placa da moto</td>
      <td>✔️ Realizado</td>
   </tr>
   <tr>
      <td>Deletar moto cadastrada indevidamente</td>
      <td>✔️ Realizado</td>
   </tr>
   <tr>
      <td>Cadastrar entregador levando em consideração dados obrigatório</td>
      <td>✔️ Realizado</td>
   </tr>
   <tr>
      <td>Armazenamento da imagem do CNH utilizando serviço storage disco local</td>
      <td>✔️ Realizado</td>
   </tr>
   <tr>
      <td>Devolução do valor total com multa/adicionais</td>
      <td>✔️ Realizado</td>
   </tr>
      <tr>
      <td>Gerenciar usuario via token JWT</td>
      <td>Implementação futura</td>
   </tr>
   

   
</table>
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





