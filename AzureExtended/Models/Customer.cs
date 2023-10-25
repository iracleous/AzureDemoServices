using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureExtended.Models;

internal class Customer
{
    // always use id
    public Guid id { get; set; } = Guid.NewGuid();
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Country { get; set; }
    public int Income { get; set; }
    public int TaxUdf { get; set; }
    public List<string> Emails { get; set; } = new List<string>();
}
