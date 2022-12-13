using Pulumi.AwsNative.SES;
using Pulumi.AwsNative.SES.Inputs;

namespace Otp.Infra.Aws;

public class SesNativeComponent : ComponentResource
{
	public SesNativeComponent(Input<string> snsTopicArn,
		string name,
		ComponentResourceOptions? options = null) : base("ohtp:ses:SesNativeComponent",
		name,
		options)
	{
		var configSet = new ConfigurationSet($"{name}-configuration-set",
			new ConfigurationSetArgs { Name = $"{name}-configuration-set" });

		var eventDestination = new ConfigurationSetEventDestination($"{name}-events",
			new ConfigurationSetEventDestinationArgs
			{
				ConfigurationSetName = configSet.Name,
				EventDestination = new ConfigurationSetEventDestinationEventDestinationArgs
				{
					MatchingEventTypes = new List<string>
					{
						"send",
						"renderingFailure",
						"bounce",
						"delivery"
					},
					SnsDestination = new ConfigurationSetEventDestinationSnsDestinationArgs { TopicARN = snsTopicArn },
					Enabled = true
				}
			},
			new CustomResourceOptions { DependsOn = configSet });

		ConfigSetName = configSet.Name;
		EventDestinationName = eventDestination.EventDestination.Apply(e => e.Name);

		RegisterOutputs();
	}

	public Output<string> ConfigSetName { get; }
	public Output<string> EventDestinationName { get; }
}