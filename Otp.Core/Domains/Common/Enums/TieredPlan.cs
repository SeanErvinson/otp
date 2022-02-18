using Ardalis.SmartEnum;

namespace Otp.Core.Domains.Common.Enums;

public abstract class TieredPlan : SmartEnum<TieredPlan>
{
	public static readonly TieredPlan Basic = new BasicTieredPlan(nameof(Basic),1);
	public static readonly TieredPlan Regular = new RegularTieredPlan(nameof(Regular),2);
	public static readonly TieredPlan Pro = new ProTieredPlan(nameof(Pro),4);
	public abstract double EmailCredit { get; }
	public abstract double SmsCredit { get; }
	public abstract double SmsOverage { get; }
	public abstract double EmailOverage { get; }

	public decimal CalculateSmsOverageCost(int usedCredit)
	{
		if (usedCredit < SmsCredit)
		{
			return 0;
		}
		return (decimal) ((usedCredit - SmsCredit) * SmsOverage);
	}

	public decimal CalculateEmailOverageCost(int usedCredit)
	{
		if (usedCredit < SmsCredit)
		{
			return 0;
		}
		return (decimal) ((usedCredit - EmailCredit) * EmailOverage);
	}

	private TieredPlan(string name, int id) : base(name, id)
	{
	}

	private sealed class BasicTieredPlan : TieredPlan
	{
		public BasicTieredPlan(string name, int id) : base(name, id)
		{
			
		}

		public override double EmailCredit => 5000;
		public override double SmsCredit => 1000;
		public override double SmsOverage => .9;
		public override double EmailOverage => .075;
	}
	
	
	private sealed class RegularTieredPlan : TieredPlan
	{
		public RegularTieredPlan(string name, int id) : base(name, id)
		{
			
		}

		public override double EmailCredit => 10000;
		public override double SmsCredit => 2500;
		public override double SmsOverage => .85;
		public override double EmailOverage => .07;
	}
	
	
	private sealed class ProTieredPlan : TieredPlan
	{
		public ProTieredPlan(string name, int id) : base(name, id)
		{
			
		}

		public override double EmailCredit => 30000;
		public override double SmsCredit => 6000;
		public override double SmsOverage => .75;
		public override double EmailOverage => .06;
	}
}
