#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 62449
EXPOSE 44382

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Infrastructure.Api/Infrastructure.Api.csproj", "Infrastructure.Api/"]
RUN dotnet restore "Infrastructure.Api/Infrastructure.Api.csproj"
COPY . .
WORKDIR "/src/Infrastructure.Api"
RUN dotnet build "Infrastructure.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Infrastructure.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Infrastructure.Api.dll"]