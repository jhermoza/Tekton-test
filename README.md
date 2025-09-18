# ProductApi - Clean Architecture ASP.NET Core 8

Repositorio de código para REST API de gestión de productos, prueba técnica .NET para Tekton Labs.

## Repositorio
https://github.com/jhermoza/Tekton-test#

## 📋 Ejemplos de Uso

**¿Quieres ver ejemplos prácticos de cómo usar esta API?**

- 📖 **[Guía de ejemplos completa](USAGE_EXAMPLES.md)** - Ejemplos con cURL, Postman y más
- 💻 **[Cliente de ejemplo en C#](ProductApiClient/ProductApiExample/)** - Aplicación de consola completa
- 🔗 **[Colección de Postman](ProductApi-Postman-Collection.json)** - Importa y prueba directamente

## Pasos para ejecutar el proyecto
1. Tener instalado .NET 8
2. Verificar que el proyecto de inicio sea **ProductApi.API**
3. Modificar la cadena de conexión a SQL Server en el archivo **appsettings.json** del proyecto **ProductApi.API**
4. Crear la base de datos ejecutando el script **ProductApi.Infrastructure/data/ProductDb.sql** en SQL Server

## Arquitectura (Clean Architecture)

Se implementan los patrones **Repository** y **Options**, junto con buenas prácticas como **Dependency Injection**, **Separation of Concerns**, **Guard Clauses**, **Single Responsibility**, nomenclatura estándar, uso correcto de códigos HTTP, generics y principios **SOLID** para mantener un código limpio. La captura global de errores se realiza mediante middleware y validación avanzada con **FluentValidation**.

## Estructura de proyectos

* **ProductApi.Domain**: Entidades y lógica de negocio
* **ProductApi.Application**: Interfaces, DTOs y servicios de aplicación
* **ProductApi.Infrastructure**: Acceso a datos y servicios externos
* **ProductApi.API**: Web API, controladores, validadores, middleware y configuración
* **ProductApi.Tests**: Pruebas unitarias con xUnit y Moq

## Tecnologías y frameworks utilizados
* **.NET 8**
* **Entity Framework Core** para acceso a base de datos
* **Swagger** para documentación del API
* **IMemoryCache** para caché de estados de producto
* **Serilog** para guardado del archivo de logs (solo tiempos de respuesta de endpoints)
* **AutoMapper** para mapeo entre entidades y DTOs
* **FluentValidation** para validación de requests
* **SQL Server** (local)
* **xUnit** y **Moq** para pruebas unitarias

###
El API utilizado para obtener descuentos de productos es https://687993db63f24f1fdca2536a.mockapi.io/discount-api

---

Este proyecto sigue Clean Architecture y buenas prácticas profesionales para garantizar mantenibilidad, escalabilidad y claridad en el código.
