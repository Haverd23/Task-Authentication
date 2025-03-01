# Task-Authentication

## Descrição do Projeto

O Task-Authentication é uma Web API construída em ASP.NET, com uma interface frontend desenvolvida em Angular. O sistema permite a criação e gerenciamento de tarefas, onde os usuários podem ter papéis de Admin ou Usuário Comum. Além disso, os usuários podem criar tarefas públicas ou privadas, com visibilidade controlada conforme a configuração escolhida.

## Problema Resolvido

Ele resolve a necessidade de gerenciar tarefas de forma simples, com controle de autenticação e autorização, além de permitir a visibilidade pública ou restrita das tarefas.

## Tecnologias Utilizadas

- **Backend:** ASP.NET 8.0 (API)
- **Frontend:** Angular
- **Banco de Dados:** SQL Server
- **Autenticação:** JWT
- **Docker:** Utilizado para a containerização do backend, frontend e banco de dados.

## Instalação e Execução Local

### Requisitos

- Docker (para rodar todos os containers)
- .NET SDK (se desejar rodar o backend localmente sem Docker)
- Node.js (se desejar rodar o frontend localmente sem Docker)

### Passos para Rodar o Projeto com Docker

Clone o repositório:

```bash
git clone <https://github.com/Haverd23/Task-Authentication.git>
```

Navegue até a pasta raiz do projeto:

```bash
cd <diretorio_do_projeto>
```

Execute o comando abaixo para iniciar todos os containers (backend, frontend, banco de dados):

```bash
docker-compose up --build
```

Ao executar o container, verifique se a imagem task-api está em execução. Se necessário, execute-a manualmente.

O backend estará acessível em `http://localhost:5125` e o frontend em `http://localhost:4200`.

O banco de dados SQL Server estará rodando na porta `1433`.

### Rodando Localmente sem Docker

#### Backend (ASP.NET)

Navegue até o diretório do backend (TaskAPI):

```bash
cd TaskAPI
```

Execute o projeto:

```bash
dotnet run
```

O backend estará acessível em `http://localhost:5125`.

#### Frontend (Angular)

Navegue até o diretório do frontend (TaskUI):

```bash
cd TaskUI
```

Instale as dependências:

```bash
npm install
```

Execute o servidor de desenvolvimento:

```bash
ng serve --port 4200
```

O frontend estará acessível em `http://localhost:4200`.

## Estrutura do Projeto

### Backend (TaskAPI)

- **Controllers:** Contém os controladores da API para autenticação, usuários, tarefas e estatísticas.
  - `AuthController`: Para login e geração de tokens JWT.
  - `UserController`: Para criar e gerenciar usuários.
  - `TaskController`: Para criar, atualizar, excluir e listar tarefas.
  - `StatisticsController`: Para obter estatísticas sobre o sistema.

- **Models:** Contém os modelos de dados e DTOs.
  - `User`: Modelo que representa o usuário.
  - `Task`: Modelo que representa as tarefas.
  - `LoginDTO`, `RegisterDTO`, `CreateTaskDTO`: Modelos para entrada e saída de dados.

- **Services:** Lógica de negócios e interações com o banco de dados.
  - `PasswordHasher`: Responsável pela criptografia da senha do usuário.
  - `TokenService`: Responsável pela criação e validação de Tokens JWT e Refresh-Token.
  

### Frontend (TaskUI)

- **src/app:** Contém o código-fonte do Angular, com os componentes, serviços e rotas.
  - **components:** Componentes de interface, como login, cadastro de usuário, listagem e criação de tarefas.
  - **services:** Serviços para interagir com os endpoints da API, como `AuthService`, `TaskService`, `UserService`.


## Docker

- **Dockerfile (backend):** Configuração para construir a aplicação ASP.NET no Docker.
- **Dockerfile (frontend):** Configuração para construir a aplicação Angular no Docker e serví-la com Nginx.
- **docker-compose.yml:** Orquestração para os containers do backend, frontend e banco de dados.

## EndPoints da API

### User

### `POST /api/User`

Faz o Registro de um usuário novo.

**Request Body:**

```json
{
  "name": "string",
  "email": "string",
  "password": "string"
}
```
"Ao registrar um novo usuário com o email "admin@gmail.com" você terá a role Admin no sistema"

### `GET /api/User/All-Users`

Retorna todos os users registrados no sistema.

### `DELETE /api/User/{id}`

Remove por meio do id determinado usuário do sistema.

### Autenticação (Auth)

#### `POST /api/Auth/login`

Realiza o login do usuário.

**Request Body:**

```json
{
  "email": "string",
  "password": "string"
}
```

Retorna um token JWT para o usuário.

#### `POST /api/Auth/refresh-token`

Atualiza o token JWT do usuário.

**Request Body:**

```json
{
  "refreshToken": "string",
  "userId": 0
}
```
Retorna um novo token JWT.

### Estatísticas

#### `GET /api/Statistics/GetStatistics`

Retorna as estatísticas do sistema.

**Exemplo de resposta:**

```json
{
  "userCount": 100,
  "totalTasks": 200,
  "publicTasks": 150,
  "privateTasks": 50
}
```

### Tarefas (Task)

#### `POST /api/Task`

Cria uma nova tarefa.

**Request Body:**

```json
{
  "name": "string",
  "description": "string",
  "isPublic": true
}
```
Retorna a tarefa criada.

#### `GET /api/Task/private`

Obtém as tarefas privadas do usuário.


Retorna a lista de tarefas privadas do usuário logado.

#### `GET /api/Task/public`

Obtém as tarefas públicas.


Retorna a lista de tarefas públicas.

#### `DELETE /api/Task/delete/{id}`

Exclui uma tarefa.

**Parâmetros:** `id` (ID da tarefa)


A tarefa será excluída.

#### `PUT /api/Task/update/{id}`

Atualiza uma tarefa existente.

**Parâmetros:** `id` (ID da tarefa)

**Request Body:**

```json
{
  "name": "string",
  "description": "string",
  "isPublic": true
}
```

A tarefa será atualizada com as novas informações.

