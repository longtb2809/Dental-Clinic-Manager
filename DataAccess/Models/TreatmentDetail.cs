using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace DataAccess.Models;

public partial class TreatmentDetail
{
    public int DetailId { get; set; }

    public int? TreatmentId { get; set; }

    public int? ServiceId { get; set; }

    public DateTime? TreatmentDate { get; set; }

    public string? ToothPosition { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public virtual Service? Service { get; set; }

    public virtual Treatment? Treatment { get; set; }


}
