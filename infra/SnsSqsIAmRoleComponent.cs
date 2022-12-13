using Pulumi.Aws.Iam;

namespace Otp.Infra.Aws;

public class SnsSqsIAmRoleComponent : ComponentResource
{
	public SnsSqsIAmRoleComponent(DeploymentContext deploymentContext,
		Input<string> sqsArn,
		Input<string> snsArn,
		Dictionary<string, string> tags,
		ComponentResourceOptions? options = null) : base("ohtp:iam:sns-sqs-IAmComponent",
		$"{deploymentContext}-SnsSqsIamRole",
		options)
	{
		var iamUser = new User($"{deploymentContext}-role", new UserArgs { Tags = tags, });

		var iamUserAccessKey = new AccessKey($"{deploymentContext}-access-key", new() { User = iamUser.Name, });

		_ = new UserPolicy($"{deploymentContext}-user-policy",
			new UserPolicyArgs
			{
				User = iamUser.Name,
				Policy = Output.Tuple(sqsArn, snsArn)
					.Apply(tuple =>
					{
						var (sqs, sns) = tuple;
						return $@"{{
    ""Version"": ""2012-10-17"",
    ""Statement"": [
        {{
            ""Sid"": ""SqsAccess"",
            ""Effect"": ""Allow"",
            ""Action"": [
                ""sqs:SetQueueAttributes"",
                ""sqs:ReceiveMessage"",
                ""sqs:CreateQueue"",
                ""sqs:DeleteMessage"",
                ""sqs:SendMessage"",
                ""sqs:GetQueueUrl"",
                ""sqs:GetQueueAttributes"",
                ""sqs:ChangeMessageVisibility"",
                ""sqs:PurgeQueue"",
                ""sqs:DeleteQueue"",
                ""sqs:TagQueue""
            ],
            ""Resource"": ""{sqs}""
        }},{{
            ""Sid"": ""SnsAccess"",
            ""Effect"": ""Allow"",
            ""Action"": [
                ""sns:GetTopicAttributes"",
                ""sns:CreateTopic"",
                ""sns:Publish"",
                ""sns:Subscribe""
            ],
            ""Resource"": ""{sns}""
        }},{{
            ""Sid"": ""SnsListAccess"",
            ""Effect"": ""Allow"",
            ""Action"": [
                ""sns:ListTopics""
            ],
            ""Resource"": ""*""
        }}
    ]
}}";
					})
			});

		AccessKey = iamUserAccessKey.Id;
		SecretKey = Output.CreateSecret(iamUserAccessKey.Secret);

		RegisterOutputs();
	}

	public Output<string> AccessKey { get; }
	public Output<string> SecretKey { get; }
}