# Push сервиса как Docker Image на Docker Hub
  1.) Build <br>
`docker build -t smirnyy82/name-of-image:1.0`

  2.) Tag version (при желании) <br>
`docker tag smirnyy82/name-of-image:2.0`

  3.) Push <br>
`docker push smirnyy82/name-of-image:1.0`

#### Запуск сервиса через консоль 
`docker run -p 45330:8080 smirnyy82/name-of-image:1.0`

, где 45330 - внешний порт <br>
8080 - порт сервиса из файла Dockerfile (у всех экземплярах один порт внутри Докера) <br> <br>


<pre>FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080</pre>

#### Посмотреть все контейнеры
`docker ps`

#### Посмотреть логи контейнера non-stop
`docker logs docker-container-id -f`


# GraphQL - пример запроса с фильтрацией
<pre>{
  pets(where:  { 
    color:  {  
       eq: "Pink"  
    },
    name:  {  
       contains: "w" 
    },
    serialNumber:  {  
       eq: 30  
    }  
  }){  
    color  
    name  
    birthDate  
    serialNumber  
  }  
} </pre>

<br> 

<pre>{
  pets(where:  {
      or: [
      { color: { eq: "Pink" } },
      { serialNumber: { gt: 30 } }
    ],
      and: [ {name:  {contains: "b"}}
    ]
  }){
    color
    name
    birthDate
    serialNumber
  }
} </pre>


