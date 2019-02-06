FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Website.Containerpoc/Website.Containerpoc.csproj", "Website.Containerpoc/"]
RUN dotnet restore "Website.Containerpoc/Website.Containerpoc.csproj"
COPY . .
WORKDIR "/src/Website.Containerpoc"
RUN dotnet build "Website.Containerpoc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Website.Containerpoc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Website.Containerpoc.dll"]