using System;
using System.Collections.Generic;

namespace PROJECT_PRN221.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string? Name { get; set; }

    public string? Gmail { get; set; }

    public string? ContactContent { get; set; }

    public DateTime? ContactDate { get; set; }
}
