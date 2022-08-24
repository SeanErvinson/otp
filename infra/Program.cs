using Pulumi.Aws.Iam;
using Pulumi.Aws.Iam.Inputs;
using Pulumi.Aws.Ses;
using Pulumi.Aws.Ses.Inputs;
using Pulumi.Aws.Sns;
using Pulumi.Aws.Sqs;

return await Deployment.RunAsync(() =>
{
	const string appName = "ohtp";
	var current = Output.Create(GetCallerIdentity.InvokeAsync());
	var accountId = current.Apply(c => c.AccountId);
	var tags = new Dictionary<string, string> { ["app"] = appName, ["environment"] = "dev" };

	var configSet = new ConfigurationSet($"{appName}-configuration-set");

	var otpEventTopic = new Topic($"{appName}-topic", new TopicArgs { Tags = tags });

	var sesTopicPolicy = otpEventTopic.Arn.Apply(arn => GetPolicyDocument.Invoke(new GetPolicyDocumentInvokeArgs
	{
		Version = "2012-10-17",
		PolicyId = "ses-notification-policy",
		Statements = new GetPolicyDocumentStatementInputArgs
		{
			Effect = "Allow",
			Actions = "sns:Publish",
			Resources = arn,
			Principals =
				new GetPolicyDocumentStatementPrincipalInputArgs { Type = "Service", Identifiers = "ses.amazonaws.com", },
			Conditions = new List<GetPolicyDocumentStatementConditionInputArgs>
			{
				new() { Test = "StringEquals", Variable = "AWS:SourceAccount", Values = { accountId }, },
				new() { Test = "StringEquals", Variable = "AWS:SourceArn", Values = { configSet.Arn }, }
			},
		}
	}));

	_ = new TopicPolicy("ses-notification-policy",
		new TopicPolicyArgs { Arn = otpEventTopic.Arn, Policy = sesTopicPolicy.Apply(snsTopicPolicy => snsTopicPolicy.Json), });

	var otpEventQueue = new Queue($"{appName}-ses-events",
		new QueueArgs { DelaySeconds = 10, Tags = tags, Name = $"{appName}-ses-events" });

	var queuePolicy = otpEventQueue.Arn.Apply(arn => GetPolicyDocument.Invoke(new GetPolicyDocumentInvokeArgs
	{
		Version = "2012-10-17",
		PolicyId = "ses-notification-sns-pub-policy",
		Statements = new GetPolicyDocumentStatementInputArgs
		{
			Effect = "Allow",
			Actions = "sqs:SendMessage",
			Resources = arn,
			Principals =
				new GetPolicyDocumentStatementPrincipalInputArgs { Type = "Service", Identifiers = "ses.amazonaws.com", },
			Conditions = new GetPolicyDocumentStatementConditionInputArgs
			{
				Test = "ArnEquals", Variable = "AWS:SourceArn", Values = { otpEventTopic.Arn },
			}
		}
	}));

	_ = new QueuePolicy("ses-notification-sns-pub-policy",
		new QueuePolicyArgs { QueueUrl = otpEventQueue.Url, Policy = queuePolicy.Apply(policy => policy.Json), });

	var eventDestination = new EventDestination($"{appName}-events",
		new EventDestinationArgs
		{
			ConfigurationSetName = configSet.Name,
			Enabled = true,
			SnsDestination = new EventDestinationSnsDestinationArgs { TopicArn = otpEventTopic.Arn },
			MatchingTypes = new List<string> { "send", "renderingFailure", "bounce", "delivery" },
		},
		new CustomResourceOptions { DependsOn = configSet });

	_ = new TopicSubscription($"{appName}-topic-subscription",
		new TopicSubscriptionArgs
		{
			Endpoint = otpEventQueue.Arn, Protocol = "sqs", Topic = otpEventTopic.Arn, RawMessageDelivery = true,
		});

	return new Dictionary<string, object?>
	{
		["queueName"] = otpEventQueue.Name, ["configSet"] = configSet.Name, ["eventDestination"] = eventDestination.Name
	};
});