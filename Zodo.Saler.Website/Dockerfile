#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ["Zodo.Saler.Website/Zodo.Saler.Website.csproj", "Zodo.Saler.Website/"]
COPY ["HZC.Services.Common/HZC.Common.Services.csproj", "HZC.Services.Common/"]
COPY ["HZC.Database/HZC.Database.csproj", "HZC.Database/"]
COPY ["HZC.Core/HZC.Core.csproj", "HZC.Core/"]
COPY ["HZC.Utils/HZC.Utils.csproj", "HZC.Utils/"]
COPY ["Zodo.Saler.Services/Zodo.Saler.Services.csproj", "Zodo.Saler.Services/"]
RUN dotnet restore "Zodo.Saler.Website/Zodo.Saler.Website.csproj"
COPY . .
WORKDIR "/src/Zodo.Saler.Website"
RUN dotnet build "Zodo.Saler.Website.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Zodo.Saler.Website.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Zodo.Saler.Website.dll"]