# YouStore

Production-quality MVP for a multi-tenant SaaS e-commerce platform.

## Prereqs
- .NET 8 SDK
- Docker + Docker Compose

## Local run
1) Start dependencies:

   docker compose up -d

2) Run the API:

   dotnet run --project src/YouStore.Api

3) Open:
- Swagger: https://localhost:5001/swagger
- Health: https://localhost:5001/health

## Frontend workspace
1) Install dependencies:

   npm install

2) Start each Angular app with the dedicated script:
   - `npm run start:merchant`
   - `npm run start:admin`
   - `npm run start:storefront`

3) Build commands:
   - `npm run build:merchant`
   - `npm run build:admin`
   - `npm run build:storefront`

Each app is a lightweight Angular Material shell that targets the routes defined in the API (merchants, marketplace, storefront).

## Samples
- Use `youstore.http` to exercise the HTTP endpoints (register, login, create store, marketplace).

## Notes
- Copy `.env.example` to `.env` if you want to standardize local environment variables later.
