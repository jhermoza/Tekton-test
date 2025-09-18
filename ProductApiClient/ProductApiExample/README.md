# ProductApi Client Example

Este es un ejemplo completo de cómo crear un cliente C# para consumir la ProductApi REST API.

## ¿Qué hace este ejemplo?

Este programa de consola demuestra:

1. **Crear un producto** - Envía una solicitud POST con datos del producto
2. **Consultar un producto** - Obtiene un producto por su ID usando GET
3. **Actualizar un producto** - Modifica un producto existente con PUT
4. **Probar validaciones** - Intenta crear un producto con datos inválidos para mostrar cómo funciona la validación

## Requisitos

1. La ProductApi debe estar ejecutándose en `https://localhost:7042`
2. .NET 8 SDK instalado

## Cómo ejecutar el ejemplo

### 1. Primero, ejecutar la ProductApi

```bash
cd ProductChallenge
dotnet run --project ProductApi.API
```

La API debe estar disponible en `https://localhost:7042`

### 2. Ejecutar el cliente de ejemplo

En otra terminal:

```bash
cd ProductApiClient/ProductApiExample
dotnet run
```

## Salida esperada

```
=== ProductApi Client Example ===
Este ejemplo demuestra cómo usar la ProductApi

1. Creando un nuevo producto...
✅ Producto creado con ID: 1
   Nombre: Laptop Gaming Example
   Precio: $1499.99
   Descuento: 10%
   Precio Final: $1349.99

2. Consultando el producto creado...
✅ Producto encontrado: Laptop Gaming Example
   Estado: Active
   Stock: 10

3. Actualizando el producto...
✅ Producto actualizado exitosamente
   Nuevo stock: 8
   Nuevo precio: $1399.99

=== Ejemplo de Validaciones ===
4. Probando validaciones con datos inválidos...
✅ Validación funcionando correctamente (Status: BadRequest)
   Errores detectados: {"message":"Invalid product data.","errors":["Name is required.","Price must be greater than zero."]}

Presiona cualquier tecla para salir...
```

## Aspectos técnicos destacados

### 1. Manejo de errores
```csharp
try
{
    var response = await httpClient.PostAsync(baseUrl, content);
    if (response.IsSuccessStatusCode)
    {
        // Procesar respuesta exitosa
    }
    else
    {
        // Manejar errores HTTP
    }
}
catch (HttpRequestException ex)
{
    // Manejar errores de conexión
}
```

### 2. Serialización JSON
```csharp
var json = JsonSerializer.Serialize(productRequest, new JsonSerializerOptions 
{ 
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
});
```

### 3. DTOs (Data Transfer Objects)
Las clases `ProductRequest` y `ProductResponse` coinciden exactamente con los DTOs de la API.

### 4. Configuración de HttpClient
```csharp
var content = new StringContent(json, Encoding.UTF8, "application/json");
```

## Modificar para tu entorno

Si tu API está en un puerto diferente, cambia la variable `baseUrl`:

```csharp
private static readonly string baseUrl = "https://tu-servidor:puerto/api/products";
```

## Casos de uso adicionales

Este ejemplo básico se puede extender para:

- Implementar autenticación (Bearer tokens)
- Agregar logging más detallado
- Implementar retry policies
- Usar HttpClientFactory para aplicaciones más grandes
- Agregar configuración desde appsettings.json

## Estructura del código

- `Main()`: Orquesta todos los ejemplos
- `CreateProductExample()`: Demuestra POST
- `GetProductExample()`: Demuestra GET
- `UpdateProductExample()`: Demuestra PUT  
- `TestValidationExample()`: Muestra validaciones
- `ProductRequest/ProductResponse`: DTOs para la comunicación con la API