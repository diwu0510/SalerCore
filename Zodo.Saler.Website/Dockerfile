#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

#FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709
#RUN mv /etc/apt/sources.list /etc/apt/sources.list.bak && \
#echo 'deb http://mirrors.163.com/debian/ jessie main non-free contrib' > /etc/apt/sources.list && \
#echo 'deb http://mirrors.163.com/debian/ jessie-updates main non-free contrib' >> /etc/apt/sources.list && \
#echo 'deb http://mirrors.163.com/debian-security/ jessie/updates main non-free contrib' >> /etc/apt/sources.list
#RUN apt-get update
#RUN apt-get install -y --no-install-recommends libgdiplus libc6-dev 

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

RUN ln -s /lib/x86_64-linux-gnu/libdl-2.24.so /lib/x86_64-linux-gnu/libdl.so
 
RUN echo "deb http://mirrors.aliyun.com/debian wheezy main contrib non-free \
deb-src http://mirrors.aliyun.com/debian wheezy main contrib non-free \
deb http://mirrors.aliyun.com/debian wheezy-updates main contrib non-free \
deb-src http://mirrors.aliyun.com/debian wheezy-updates main contrib non-free \
deb http://mirrors.aliyun.com/debian-security wheezy/updates main contrib non-free \
deb-src http://mirrors.aliyun.com/debian-security wheezy/updates main contrib non-free" > /etc/apt/sources.list
 
RUN apt-get update
RUN apt-get install libgdiplus -y && ln -s libgdiplus.so gdiplus.dll

RUN dotnet build "Zodo.Saler.Website.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Zodo.Saler.Website.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Zodo.Saler.Website.dll"]