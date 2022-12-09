namespace Otp.Application.Common.Exceptions;

public sealed class NotFoundException : BusinessException
{
	public NotFoundException(string title, string detail) : base(title, detail)
	{
	}

	public NotFoundException(string detail) : base("EntityNotFound", detail)
	{
	}

	public NotFoundException(string name, object key) : this($"Entity {name} with key: {key} was not found: {name} ")
	{
	}

	public NotFoundException(string title, string detail, Exception inner)
		: base(title, detail, inner)
	{
	}
}