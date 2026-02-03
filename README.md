# MBRCLD

## Install global dependencies

Angular CLI version 10

```bash
npm install -g @angular/cli@10
```

## Install project dependencies

Restore nuget packages for dotnet sever

```bash
dotnet restore
```

Restore npm packages for client app

```bash
cd src/Web/ClientApp/
npm install
```

## Build

### Building the server

Debug build

```bash
cd src/Web/
dotnet build
```

Release build

```bash
cd src/Web/
dotnet build --configuration Release
```

### Building the client app

Debug build

```bash
cd src/Web/ClientApp/
npm install
npm run build
```

Production build

```bash
cd src/Web/ClientApp/
npm install
npm run build:prod
```

## Run

First run the Angular dev server

```bash
cd src/Web/ClientApp/
npm start
```

Then in a new terminal window run the dotnet server

```bash
cd src/Web/
dotnet run
```

## Deploy

Publish the server code with the client app for deployment

```bash
cd src/Web/
dotnet publish --configuration Release
```
