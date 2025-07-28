using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<TreatmentDetail> TreatmentDetails { get; set; } = new List<TreatmentDetail>();
}
