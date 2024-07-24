namespace Domain.Entities;

public class User
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string PasswordHash { get; set; }
	public long? TelegramId {  get; set; } 
}
