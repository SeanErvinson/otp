using System.Collections.Generic;
using Pulumi;
using Pulumi.Aws.Ses;
using Pulumi.Aws.Ses.Inputs;

namespace Otp.Infra.Aws;

public class SesComponent : ComponentResource
{
	public Output<string> ConfigSetName { get; }
	public Output<string> EventDestinationName { get; }
	public SesComponent(Input<string> snsTopicArn, string name, ComponentResourceOptions? options = null) : base(
		"ohtp:ses:SesComponent",
		name,
		options)
	{
		var configSet = new ConfigurationSet($"{name}-configuration-set", new ConfigurationSetArgs
		{
			Name = $"{name}-configuration-set"
		});

		var eventDestination = new EventDestination($"{name}-events",
			new EventDestinationArgs
			{
				ConfigurationSetName = configSet.Name,
				Enabled = true,
				SnsDestination = new EventDestinationSnsDestinationArgs { TopicArn = snsTopicArn },
				MatchingTypes = new List<string>() { "send", "renderingFailure", "bounce", "delivery" },
			},
			new CustomResourceOptions { DependsOn = configSet });

		ConfigSetName = configSet.Name;
		EventDestinationName = eventDestination.Name;
		
		RegisterOutputs();
	}
}