# Aplicación de Reseñas de Libros (BookReviewApp)

Esta es una aplicación web full-stack que permite a los usuarios buscar, explorar y reseñar libros. El backend está construido con .NET 8 y el frontend con Angular 18.

## Tecnologías Utilizadas

- **Backend:** ASP.NET Core 8 Web API, Entity Framework Core 8, ASP.NET Core Identity, JWT.
- **Frontend:** Angular 18, Angular Material, TypeScript, RxJS.
- **Base de Datos:** SQL Server.
- **Alojamiento y CI/CD:** Azure App Service, GitHub Actions.

## Características

-   Registro e inicio de sesión de usuarios.
-   Autenticación basada en tokens JWT.
-   Página de inicio con listado de libros.
-   Búsqueda de libros por título, autor y categoría.
-   Ver detalles de un libro, incluyendo resumen y reseñas de otros usuarios.
-   Los usuarios autenticados pueden añadir, editar y eliminar sus propias reseñas (calificación + comentario).
-   Perfil de usuario para ver y actualizar información personal.
-   Diseño responsivo y accesible gracias a Angular Material.

---

## Prerrequisitos

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [Node.js y npm](https://nodejs.org/) (LTS)
-   [Angular CLI v18](https://angular.io/cli) (`npm install -g @angular/cli`)
-   [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) (Express, Developer o cualquier otra edición)
-   Un editor de código como [Visual Studio Code](https://code.visualstudio.com/) o [Visual Studio 2022](https://visualstudio.microsoft.com/).

---

## Instalación y Ejecución Local

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-usuario/book-review-app.git
cd book-review-app
```

### 2. Configurar y Ejecutar el Backend (.NET API)

1.  **Navegar a la carpeta del backend:**
    ```bash
    cd backend
    ```

2.  **Configurar la base de datos:**
    -   Abre el archivo `appsettings.Development.json`.
    -   Modifica la cadena `DefaultConnection` en `ConnectionStrings` para que apunte a tu instancia local de SQL Server.
    -   Asegúrate de que la base de datos especificada exista o de que tu usuario tenga permisos para crearla.

3.  **Aplicar las migraciones de la base de datos:**
    -   Este comando creará la base de datos y todas las tablas necesarias a partir de los modelos de EF Core.
    ```bash
    dotnet ef database update
    ```
    *Si el script SQL ya se ejecutó manualmente, este paso puede dar un error si las tablas ya existen. En ese caso, puedes omitirlo.*

4.  **Ejecutar la API:**
    ```bash
    dotnet run
    ```
    La API estará disponible en `https://localhost:7001` (o un puerto similar que se indique en la consola).

### 3. Configurar y Ejecutar el Frontend (Angular)

1.  **Navegar a la carpeta del frontend:**
    ```bash
    cd ../frontend
    ```

2.  **Instalar dependencias:**
    ```bash
    npm install
    ```

3.  **Ejecutar la aplicación Angular:**
    ```bash
    ng serve --open
    ```
    La aplicación se abrirá automáticamente en tu navegador en `http://localhost:4200`.

---

## Despliegue (CI/CD con GitHub Actions y Azure)

Esta aplicación está diseñada para ser desplegada en **Azure App Service**, con el backend y el frontend como dos servicios de aplicaciones separados.

### Pasos para el Despliegue:

1.  **Crear Recursos en Azure:**
    -   Crea un **Grupo de Recursos**.
    -   Crea una **Base de Datos SQL de Azure** y un servidor. Copia la cadena de conexión.
    -   Crea dos **Planes de App Service** (o usa uno si lo prefieres).
    -   Crea dos **App Service**:
        -   `bookreview-api`: para el backend .NET (Stack: .NET 8).
        -   `bookreview-client`: para el frontend Angular (Stack: Node.js).

2.  **Configurar Secretos en GitHub:**
    -   En tu repositorio de GitHub, ve a `Settings > Secrets and variables > Actions`.
    -   Añade los siguientes secretos:
        -   `AZURE_WEBAPP_PUBLISH_PROFILE_API`: El perfil de publicación del App Service `bookreview-api`. Lo puedes descargar desde el portal de Azure.
        -   `AZURE_WEBAPP_PUBLISH_PROFILE_CLIENT`: El perfil de publicación del App Service `bookreview-client`.
        -   `AZURE_SQL_CONNECTION_STRING`: La cadena de conexión de tu base de datos SQL de Azure.

3.  **Configurar el Pipeline de GitHub Actions:**
    -   Crea un archivo `.github/workflows/deploy.yml` en tu repositorio con el siguiente contenido:

    ```yaml
    name: Build and Deploy .NET and Angular App

    on:
      push:
        branches:
          - main
      workflow_dispatch:

    jobs:
      build-and-deploy:
        runs-on: ubuntu-latest

        steps:
        - name: 'Checkout Github Action'
          uses: actions/checkout@v4

        # Build y Deploy Backend (.NET)
        - name: Setup .NET
          uses: actions/setup-dotnet@v4
          with:
            dotnet-version: '8.0.x'

        - name: Restore .NET dependencies
          run: dotnet restore ./backend

        - name: Build .NET
          run: dotnet build ./backend --configuration Release --no-restore

        - name: Publish .NET
          run: dotnet publish ./backend -c Release -o ${{ env.DOTNET_ROOT }}/myapp

        - name: 'Deploy to Azure Web App (API)'
          uses: azure/webapps-deploy@v2
          with:
            app-name: 'bookreview-api' # Nombre de tu App Service de API
            slot-name: 'production'
            publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE_API }}
            package: ${{ env.DOTNET_ROOT }}/myapp

        # Build y Deploy Frontend (Angular)
        - name: 'Setup Node.js'
          uses: actions/setup-node@v4
          with:
            node-version: '20.x'

        - name: 'Install Angular dependencies'
          run: |
            cd frontend
            npm install

        - name: 'Build Angular'
          run: |
            cd frontend
            npm run build -- --configuration production

        - name: 'Deploy to Azure Web App (Client)'
          uses: azure/webapps-deploy@v2
          with:
            app-name: 'bookreview-client' # Nombre de tu App Service de cliente
            slot-name: 'production'
            publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE_CLIENT }}
            package: ./frontend/dist/frontend/browser # Ruta a los artefactos de build de Angular

    ```

4.  **Configuración Final en Azure:**
    -   En el App Service de la API (`bookreview-api`), ve a `Configuración > Configuración de la aplicación` y añade la cadena de conexión `AZURE_SQL_CONNECTION_STRING` con el nombre `ConnectionStrings:DefaultConnection`.
    -   Asegúrate de que la configuración CORS de tu API en Azure permita solicitudes desde la URL de tu cliente (`https://bookreview-client.azurewebsites.net`).

Una vez que hagas un `push` a la rama `main`, la Action se ejecutará y desplegará automáticamente ambos proyectos.