# Account API - Bankdata Coding Challenge
 
Denne Web API giver dig mulighed for at oprette bankkonti, liste alle dine konti og foretage overførsler mellem dem. Dette projekt er en .NET Entity Framework Web API.

## Krav

Projektet kræver en installation af `.NET 8.0` som kan hentes her https://dotnet.microsoft.com/en-us/download.

Projektet gør sig også brug af en lokal SQLExpress database som kan hentes her https://www.microsoft.com/en-us/download/details.aspx?id=104781.

## Opsætning

For at køre de tilhørende migrations kræves `dotnet-ef` som kan installeres globalt via:

```
dotnet tool install --global dotnet-ef
```

Dernæst kan databasen opsættes fra root via kommandoen:

```
dotnet ef database update --project AccountApi
```

eller via følgende:

```
cd AccountApi
dotnet ef database update
```

Bemærk at projektet bruger en **DefaultConnection** string til at finde databasen, som enten kræver en server ved navn `SQLExpress` eller at `appsettings.json` filen ændres.

# API Specifikation

Projektet kan tilgås som et `Swagger UI` på https://localhost:7210/swagger/index.html.

APIet indeholder de følgende endpoints:

| Handling     | Path                           | Beskrivelse                                                                               |
| -------------|--------------------------------|-------------------------------------------------------------------------------------------|
| `GET`        | **/api/Accounts**              | henter alle konti fra databasen                                                           |
| `GET`        | **/api/Accounts/{id}**         | henter en specifik konto fra databasen ved hjælp af dens unikke id                        |
| `GET`        | **/api/Accounts/owner/{name}** | henter alle konti fra databasen, som tilhører den angivne ejer                            |
| `POST`       | **/api/Accounts**              | opretter en ny konto i databasen med de angivne oplysninger                               |
| `POST`       | **/api/Accounts/transfer**     | opretter en ny transaktion i databasen for at overføre et beløb fra en konto til en anden |