﻿namespace Catering.Application.Aggregates.Items.Dtos;

public class CreateItemDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public IEnumerable<string> Ingredients { get; set; }
    public IEnumerable<string> Categories { get; set; }
}
