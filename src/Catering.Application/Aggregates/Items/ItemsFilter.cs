﻿using System.Text.Json.Serialization;
using Catering.Application.Filtering;

namespace Catering.Application.Aggregates.Items;

public class ItemsFilter : FilterBase
{
    [JsonIgnore]
    public Guid MenuId { get; set; }
    public IEnumerable<string> Categories { get; set; }
    public IEnumerable<string> Ingredients { get; set; }
    public decimal? TopPrice { get; set; }
    public decimal? BottomPrice { get; set; }
    public ItemsOrderBy? OrderBy { get; set; }
    public bool IsOrderByDescending { get; set; }
}
