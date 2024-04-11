namespace Guard.Api.Contracts.Users;

public class RegisterRequest
{
	public string Name { get; set; }
	public string Password { get; set; }
	public int Validator { get; set; }
}
