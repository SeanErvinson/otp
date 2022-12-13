using Pulumi.AwsNative.SNS;

namespace Otp.Infra.Aws;

public class SnsNativeComponent : ComponentResource
{
	public SnsNativeComponent(Input<string> snsTopicArn,
		string name,
		ComponentResourceOptions? options = null) : base("ohtp:ses:SnsNativeComponent",
		name,
		options)
	{
		var topic = new Topic("", new TopicArgs { }, new CustomResourceOptions());
	}
}