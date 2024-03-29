﻿using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Entities;

public class Dish
{
    [Key] 
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int RestaurantId { get; set; }
    public virtual Restaurant Restaurant { get; set; }
}