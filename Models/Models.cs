using System.ComponentModel.DataAnnotations;

public class Category
{
    public int CategoryID { get; set; }
    
    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
}

public class Product
{
    public int ProductID { get; set; }
    
    [Required]
    [StringLength(50)]
    public string SKU { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string ProductName { get; set; } = string.Empty;
    
    public int? CategoryID { get; set; }
    
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Stock { get; set; } = 0;
    
    public bool IsActive { get; set; } = true;
}

public class Sale
{
    public int SaleID { get; set; }
    
    [Required]
    public int ProductID { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    
    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
    
    public DateTime SaleDate { get; set; } = DateTime.Now;
}