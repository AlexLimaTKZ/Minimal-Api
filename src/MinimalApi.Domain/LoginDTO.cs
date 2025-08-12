namespace MinimalApi.Domain.Entidades.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; } = default!;
        public string Senha { get; set; } = default!;
    }
}