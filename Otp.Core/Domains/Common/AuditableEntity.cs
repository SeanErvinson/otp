﻿namespace Otp.Core.Domains.Common;

public abstract class AuditableEntity : BaseEntity
{
	public DateTime CreatedAt { get; set; }
	public string CreatedBy { get; set; } = string.Empty;
	public DateTime? UpdatedAt { get; set; }
	public string? UpdatedBy { get; set; }
}