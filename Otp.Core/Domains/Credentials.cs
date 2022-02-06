﻿using Otp.Core.Domains.Common.Models;
using Otp.Core.Domains.Entities;

namespace Otp.Core.Domains;

public class Credential : BaseEntity
{
	public string HashApiKey { get; set; } = default!;
	public string? CallbackUrl { get; set; }
	public string? CallbackUrlSecret { get; set; }
	public Principal Principal { get; set; }
}