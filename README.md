# CampusLove

## Descripción 📌
CampusLove es un programa que simula una aplicación de citas. Permite a los usuarios registrarse e iniciar sesión para acceder al sistema. Una vez dentro, pueden interactuar con otros perfiles mediante opciones de Like o Dislike. Si dos usuarios se dan Like mutuamente, se genera automáticamente un Match, indicando que ambos han mostrado interés.usuarios generan la interaccion de *Like* mutuamente automaticamente se generara un *MATCH* mostrando asi que las dos personas tuvieron una respuesta positiva

## Estructura del Proyecto 🏗️
El proyecto sigue la estructura de la arquitectura hexagonal:
```bash
InventoryManagementSystem/
├── Application/
│   ├── Config/
│   └── UI/
├── Diagrams/
│   ├── Modelo-Conceptual.png/
│   ├── Modelo-Der.png/
│   └── Modelo-Logico.png/
├── Domain/
│   ├── Entities/
│   ├── Factory/
│   └── Ports/
├── Infrastructure/
│   ├── Configuration/
│   ├── MySql/
│   └── Repositories/
├── db/
│   ├── ddl.sql
│   └── dml.sql
├── Program.cs
└── README.md
```

## Funcionalidades Principales ✅
- Registrar y Login usuarios.
- Visualizar perfil.
- Interactuar con otros perfiles.
- Gestionar un MATCH 
- Mostrar estadisticas del sistema.
- Interfaces intuitivas.
- Conexion con base de datos MySql.

## Historial de Commits 📈
<img src="https://github.com/user-attachments/assets/625ae3ed-e702-4240-ab40-546b9dbcb29c">

## Tecnologias Utilizadas 👾
- Lenguaje: C#
- Base de datos: MySql
- ORM: Conexión directa con MySqlConnector

## Instalación 📥
### 🔧 Requisitos Previos
- Tener instalado [Git](https://git-scm.com/)
- Tener instalado [MYSQL](https://dev.mysql.com/downloads/installer/)
- Un editor de código como Visual Studio Code, Visual Studio, o el de tu preferencia.
- SDK de .NET

### 🚀 Pasos de ejecución

1. Clonar Repositorio

```bash git clone https://github.com/Isa94d-lab/CampusLove.git ```

2. Ir al directorio del repositorio
```/cd CampusLove ```

3. Instalar la libreria necesaria para la conexión a la base de datos
```dotnet add package MySqlConnector ```

### Configuración Base de Datos:

- Abre tu editor de texto y asegúrate de tener instalada la extensión SQL.

- Agrega una nueva conexión usando tus credenciales de MySQL:

<img src="https://github.com/user-attachments/assets/fb01eec1-e270-49ac-b861-901a6a8b0230" height="70px">

- Ingresa el usuario y contraseña establecidos durante la instalación:

<img src="https://github.com/user-attachments/assets/73680b8d-5c34-4995-add3-7f235c13e2d9">

- Ejecuta los scripts para crear la base de datos y poblarla:
    - [DDL: Estructura](./db/ddl.sql)
    - [DML: Inserción de datos](./db/dml.sql)

<img src="https://github.com/user-attachments/assets/a5c9439f-3505-4e74-b1e9-310f52bb938a" height="100px">

### Ejecución:
```
dotnet run
```

## Colaboradores ✒️

- Andres Felipe Araque Pardo
- Isabella Stefphani Galvis Sandoval