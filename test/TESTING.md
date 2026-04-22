# M2M Snippet Integration Testing

Live integration tests that run each language's `*-m2m.*` snippet against a real Auth0 tenant.

## Prerequisites

- Docker + docker-compose
- An Auth0 M2M application authorized for the Management API v2 with `read:clients` scope

## Setup

Create `test/.env` (gitignored):

```
AUTH0_DOMAIN=your-tenant.auth0.com
AUTH0_CLIENT_ID=xxx
AUTH0_CLIENT_SECRET=xxx
AUTH0_AUDIENCE=https://your-tenant.auth0.com/api/v2/
AUTH0_SCOPE=read:clients
API_ENDPOINT=https://your-tenant.auth0.com/api/v2/clients
```

## Approach

`docker-compose.yml` with one service per language. Each service:

1. Uses a minimal base image with the language runtime
2. Copies the `*-m2m.*` snippet for its language
3. Runs it with env vars from `.env`
4. Exits 0 if the snippet prints a response, non-zero on error

### Language runtimes

| Language | Base image | External deps needed |
|----------|-----------|---------------------|
| bash/curl | `alpine` | curl, jq (apk add) |
| node | `node:22-alpine` | none (uses fetch) |
| python | `python:3.12-alpine` | `requests` (pip) |
| go | `golang:1.22-alpine` | none (stdlib) |
| java | `eclipse-temurin:21` | unirest + org.json jars |
| csharp | `mcr.microsoft.com/dotnet/sdk:8.0` | RestSharp + Newtonsoft.Json (.csproj) |
| php | `php:8.3-cli` | none (curl built-in) |
| ruby | `ruby:3.3-alpine` | none (net/http is stdlib) |

### Running

```bash
cd test
docker compose up --build --abort-on-container-exit
```

All 8 services run in parallel. Compose exits non-zero if any service fails.

## TODO

- [ ] Write `docker-compose.yml`
- [ ] Write per-language Dockerfiles
- [ ] Add minimal `pom.xml` for Java (unirest + org.json)
- [ ] Add minimal `.csproj` for C# (RestSharp + Newtonsoft.Json)
- [ ] Add `.env` to `.gitignore`
