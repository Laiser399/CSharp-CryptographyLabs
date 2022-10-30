FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Util.RSA.WienerAttackTest/Util.RSA.WienerAttackTest.csproj", "Util.RSA.WienerAttackTest/"]
COPY ["Module.RSA/Module.RSA.csproj", "Module.RSA/"]
COPY ["Module.Core/Module.Core.csproj", "Module.Core/"]
RUN dotnet restore "Util.RSA.WienerAttackTest/Util.RSA.WienerAttackTest.csproj"
COPY . .
RUN dotnet tool restore
RUN dotnet paket restore
WORKDIR "/src/Util.RSA.WienerAttackTest"
RUN dotnet build "Util.RSA.WienerAttackTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Util.RSA.WienerAttackTest.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Util.RSA.WienerAttackTest.dll"]
