using System.Text;
using System.Text.Json;

namespace ProductApiExample;

/// <summary>
/// Example client application demonstrating how to consume the ProductApi
/// </summary>
class Program
{
    private static readonly HttpClient httpClient = new HttpClient();
    private static readonly string baseUrl = "https://localhost:7042/api/products";
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== ProductApi Client Example ===");
        Console.WriteLine("Este ejemplo demuestra cómo usar la ProductApi");
        Console.WriteLine();

        try
        {
            // Example 1: Create a product
            Console.WriteLine("1. Creando un nuevo producto...");
            var newProduct = await CreateProductExample();
            if (newProduct != null)
            {
                Console.WriteLine($"✅ Producto creado con ID: {newProduct.ProductId}");
                Console.WriteLine($"   Nombre: {newProduct.Name}");
                Console.WriteLine($"   Precio: ${newProduct.Price}");
                Console.WriteLine($"   Descuento: {newProduct.Discount}%");
                Console.WriteLine($"   Precio Final: ${newProduct.FinalPrice}");
                Console.WriteLine();

                // Example 2: Get the product
                Console.WriteLine("2. Consultando el producto creado...");
                var retrievedProduct = await GetProductExample(newProduct.ProductId);
                if (retrievedProduct != null)
                {
                    Console.WriteLine($"✅ Producto encontrado: {retrievedProduct.Name}");
                    Console.WriteLine($"   Estado: {retrievedProduct.StatusName}");
                    Console.WriteLine($"   Stock: {retrievedProduct.Stock}");
                    Console.WriteLine();

                    // Example 3: Update the product
                    Console.WriteLine("3. Actualizando el producto...");
                    var updated = await UpdateProductExample(newProduct.ProductId);
                    if (updated)
                    {
                        Console.WriteLine("✅ Producto actualizado exitosamente");
                        
                        // Get updated product
                        var updatedProduct = await GetProductExample(newProduct.ProductId);
                        if (updatedProduct != null)
                        {
                            Console.WriteLine($"   Nuevo stock: {updatedProduct.Stock}");
                            Console.WriteLine($"   Nuevo precio: ${updatedProduct.Price}");
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("=== Ejemplo de Validaciones ===");
            Console.WriteLine("4. Probando validaciones con datos inválidos...");
            await TestValidationExample();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
        finally
        {
            httpClient.Dispose();
        }

        Console.WriteLine();
        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }

    /// <summary>
    /// Example: Create a new product
    /// </summary>
    static async Task<ProductResponse?> CreateProductExample()
    {
        var productRequest = new ProductRequest
        {
            Name = "Laptop Gaming Example",
            Status = 1,
            Stock = 10,
            Description = "Laptop gaming creada desde ejemplo de cliente",
            Price = 1499.99m
        };

        var json = JsonSerializer.Serialize(productRequest, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(baseUrl, content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductResponse>(responseJson, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error al crear producto: {response.StatusCode}");
                Console.WriteLine($"   Detalles: {errorContent}");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Error de conexión: {ex.Message}");
            Console.WriteLine("   Asegúrate de que la API esté ejecutándose en https://localhost:7042");
            return null;
        }
    }

    /// <summary>
    /// Example: Get a product by ID
    /// </summary>
    static async Task<ProductResponse?> GetProductExample(int productId)
    {
        try
        {
            var response = await httpClient.GetAsync($"{baseUrl}/{productId}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductResponse>(responseJson, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"❌ Producto con ID {productId} no encontrado");
                return null;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error al consultar producto: {response.StatusCode}");
                Console.WriteLine($"   Detalles: {errorContent}");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Error de conexión: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Example: Update a product
    /// </summary>
    static async Task<bool> UpdateProductExample(int productId)
    {
        var productRequest = new ProductRequest
        {
            Name = "Laptop Gaming Example (Actualizada)",
            Status = 1,
            Stock = 8, // Reduced stock
            Description = "Laptop gaming actualizada desde ejemplo de cliente",
            Price = 1399.99m // Reduced price
        };

        var json = JsonSerializer.Serialize(productRequest, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PutAsync($"{baseUrl}/{productId}", content);
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"❌ Producto con ID {productId} no encontrado para actualizar");
                return false;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Error al actualizar producto: {response.StatusCode}");
                Console.WriteLine($"   Detalles: {errorContent}");
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Error de conexión: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Example: Test validation with invalid data
    /// </summary>
    static async Task TestValidationExample()
    {
        // Test with invalid data (empty name, negative price)
        var invalidProduct = new ProductRequest
        {
            Name = "", // Invalid: empty name
            Status = 1,
            Stock = 10,
            Description = "Producto con datos inválidos",
            Price = -100 // Invalid: negative price
        };

        var json = JsonSerializer.Serialize(invalidProduct, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(baseUrl, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"✅ Validación funcionando correctamente (Status: {response.StatusCode})");
                Console.WriteLine($"   Errores detectados: {errorContent}");
            }
            else
            {
                Console.WriteLine("❌ Las validaciones no funcionaron como esperado");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"❌ Error de conexión: {ex.Message}");
        }
    }
}

/// <summary>
/// DTO for product creation/update requests
/// </summary>
public class ProductRequest
{
    public string Name { get; set; } = string.Empty;
    public int Status { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

/// <summary>
/// DTO for product responses
/// </summary>
public class ProductResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public int Stock { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal FinalPrice { get; set; }
}
