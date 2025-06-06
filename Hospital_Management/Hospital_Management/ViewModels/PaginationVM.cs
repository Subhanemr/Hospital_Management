﻿namespace Hospital_Management.ViewModels;

public class PaginationVM<T>
{
    public int Take { get; set; }
    public int Order { get; set; }
    public string? Search { get; set; }
    public int CurrentPage { get; set; }
    public double TotalPage { get; set; }
    public ICollection<T>? Items { get; set; }
    public T? Item { get; set; }
}
