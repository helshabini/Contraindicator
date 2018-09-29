FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 55884
EXPOSE 44382

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Contraindicator/Contraindicator.csproj", "Contraindicator/"]
RUN dotnet restore "Contraindicator/Contraindicator.csproj"
COPY . .
WORKDIR "/src/Contraindicator"
RUN dotnet build "Contraindicator.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Contraindicator.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Contraindicator.dll"]