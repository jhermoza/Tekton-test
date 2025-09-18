# ProductApi - Ejemplos de Uso / Usage Examples

Este documento proporciona ejemplos prácticos de cómo usar la ProductApi REST API para gestión de productos.

## Tabla de Contenidos
- [Configuración Inicial](#configuración-inicial)
- [Endpoints Disponibles](#endpoints-disponibles)
- [Ejemplos con cURL](#ejemplos-con-curl)
- [Ejemplos con Postman](#ejemplos-con-postman)
- [Ejemplos de Respuestas](#ejemplos-de-respuestas)
- [Códigos de Estado HTTP](#códigos-de-estado-http)
- [Validaciones](#validaciones)

## Configuración Inicial

### 1. Prerrequisitos
- .NET 8 SDK instalado
- SQL Server (local o remoto)
- Editor de texto o IDE (Visual Studio, VS Code, etc.)

### 2. Configurar Base de Datos
1. Ejecutar el script SQL ubicado en `ProductApi.Infrastructure/data/ProductDb.sql`
2. Modificar la cadena de conexión en `ProductApi.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductDb;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

### 3. Ejecutar la API
```bash
cd ProductChallenge
dotnet run --project ProductApi.API
```

La API estará disponible en: `https://localhost:7XXX` (el puerto se muestra en la consola)

## Endpoints Disponibles

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST   | `/api/products` | Crear un nuevo producto |
| GET    | `/api/products/{id}` | Obtener un producto por ID |
| PUT    | `/api/products/{id}` | Actualizar un producto existente |

## Ejemplos con cURL

### 1. Crear un Producto

```bash
curl -X POST "https://localhost:7042/api/products" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop Gaming ASUS",
    "status": 1,
    "stock": 15,
    "description": "Laptop gaming de alto rendimiento con RTX 4060",
    "price": 1299.99
  }'
```

**Respuesta esperada:**
```json
{
  "productId": 1,
  "name": "Laptop Gaming ASUS",
  "status": 1,
  "statusName": "Active",
  "stock": 15,
  "description": "Laptop gaming de alto rendimiento con RTX 4060",
  "price": 1299.99,
  "discount": 10,
  "finalPrice": 1169.99
}
```

### 2. Obtener un Producto por ID

```bash
curl -X GET "https://localhost:7042/api/products/1" \
  -H "Accept: application/json"
```

### 3. Actualizar un Producto

```bash
curl -X PUT "https://localhost:7042/api/products/1" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Laptop Gaming ASUS ROG",
    "status": 1,
    "stock": 12,
    "description": "Laptop gaming ROG con RTX 4060 y 16GB RAM",
    "price": 1399.99
  }'
```

## Ejemplos con Postman

### Configuración de Colección

1. **URL Base**: `https://localhost:7042`
2. **Headers comunes**:
   - `Content-Type: application/json`
   - `Accept: application/json`

### Request 1: Crear Producto de Prueba

- **Método**: POST
- **URL**: `{{baseUrl}}/api/products`
- **Body (JSON)**:
```json
{
  "name": "Smartphone Samsung Galaxy",
  "status": 1,
  "stock": 25,
  "description": "Smartphone con cámara de 108MP y 5G",
  "price": 799.99
}
```

### Request 2: Consultar Producto

- **Método**: GET
- **URL**: `{{baseUrl}}/api/products/1`

### Request 3: Actualizar Stock

- **Método**: PUT
- **URL**: `{{baseUrl}}/api/products/1`
- **Body (JSON)**:
```json
{
  "name": "Smartphone Samsung Galaxy",
  "status": 1,
  "stock": 20,
  "description": "Smartphone con cámara de 108MP y 5G",
  "price": 799.99
}
```

## Ejemplos de Respuestas

### Producto Creado Exitosamente (201)
```json
{
  "productId": 2,
  "name": "Smartphone Samsung Galaxy",
  "status": 1,
  "statusName": "Active",
  "stock": 25,
  "description": "Smartphone con cámara de 108MP y 5G",
  "price": 799.99,
  "discount": 15,
  "finalPrice": 679.99
}
```

### Producto No Encontrado (404)
```json
{
  "message": "Product with ID 999 was not found."
}
```

### Error de Validación (400)
```json
{
  "message": "Invalid product data.",
  "errors": [
    "Name is required.",
    "Price must be greater than zero."
  ]
}
```

## Códigos de Estado HTTP

| Código | Descripción | Cuándo se produce |
|--------|-------------|-------------------|
| 200 | OK | Producto obtenido exitosamente |
| 201 | Created | Producto creado exitosamente |
| 204 | No Content | Producto actualizado exitosamente |
| 400 | Bad Request | Datos de entrada inválidos |
| 404 | Not Found | Producto no encontrado |
| 500 | Internal Server Error | Error interno del servidor |

## Validaciones

### Reglas de Validación para ProductRequest

| Campo | Validación | Mensaje de Error |
|-------|------------|------------------|
| Name | Requerido, máximo 100 caracteres | "Name is required." / "Name must be at most 100 characters." |
| Status | Debe ser 0 (Inactivo) o 1 (Activo) | "Status must be 0 (Inactive) or 1 (Active)." |
| Stock | Debe ser >= 0 | "Stock must be zero or positive." |
| Description | Máximo 500 caracteres | "Description must be at most 500 characters." |
| Price | Debe ser > 0 | "Price must be greater than zero." |

### Ejemplos de Requests Inválidos

#### Nombre vacío:
```json
{
  "name": "",
  "status": 1,
  "stock": 10,
  "description": "Producto de prueba",
  "price": 100.00
}
```

#### Precio negativo:
```json
{
  "name": "Producto Test",
  "status": 1,
  "stock": 10,
  "description": "Producto de prueba",
  "price": -50.00
}
```

#### Status inválido:
```json
{
  "name": "Producto Test",
  "status": 5,
  "stock": 10,
  "description": "Producto de prueba",
  "price": 100.00
}
```

## Funcionalidades Avanzadas

### 1. Sistema de Descuentos
La API integra automáticamente descuentos desde un servicio externo:
- URL: `https://687993db63f24f1fdca2536a.mockapi.io/discount-api`
- El descuento se aplica automáticamente al crear/consultar productos
- El `finalPrice` se calcula como: `price - (price * discount / 100)`

### 2. Cache de Estados
Los nombres de estado (Active/Inactive) se almacenan en cache para mejorar el rendimiento.

### 3. Logging
Todos los tiempos de respuesta se registran automáticamente usando Serilog en la carpeta `logs/`.

### 4. Documentación Swagger
Acceder a `https://localhost:7042/swagger` para ver la documentación interactiva de la API.

## Consejos para Desarrollo

1. **Usar Swagger UI**: Es la forma más fácil de probar la API durante el desarrollo
2. **Revisar Logs**: Los archivos de log contienen información valiosa sobre el rendimiento
3. **Validar Datos**: Siempre verificar que los datos cumplan las reglas de validación
4. **Manejo de Errores**: Implementar manejo adecuado de todos los códigos de estado HTTP

## Arquitectura del Proyecto

Este proyecto sigue **Clean Architecture** con las siguientes capas:

- **Domain**: Entidades del negocio (`Product`)
- **Application**: Lógica de aplicación, DTOs e interfaces
- **Infrastructure**: Acceso a datos y servicios externos
- **API**: Controllers, validadores y configuración
- **Tests**: Pruebas unitarias con xUnit y Moq

Cada capa tiene responsabilidades específicas y mantiene bajo acoplamiento para facilitar el mantenimiento y las pruebas.