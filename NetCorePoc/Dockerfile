FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 5001
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "NetCorePoc.Api.dll"]
