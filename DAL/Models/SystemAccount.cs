using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class SystemAccount
{
    public short AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }
    public string AccountType ()
    {
        if (AccountRole == 1) return "Staff";
        if (AccountRole == 2) return "Lecturer";
        return string.Empty;
    }
    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
