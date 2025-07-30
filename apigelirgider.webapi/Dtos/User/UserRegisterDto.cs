public class UserRegisterDto
{
    public string UserName { get; set; }              // Kayıt için ad soyad
    public string UserEmail { get; set; }                 // Email
    public string Password { get; set; }              // Şifre (hashlenir)
}

public class UserLoginDto
{
    public string UserEmail { get; set; }                 // Giriş maili
    public string Password { get; set; }              // Şifre
}

