using FluentValidation;

public class RegisterViewModel

{
    public User User { get; set; } = new User();
    public Login Login { get; set; } = new Login();
    public Address Address {get;set;} = new Address();  
    public bool ConfirmAccount {get;set;}

}

public class RegisterModelValidator : AbstractValidator<RegisterViewModel>
{
        public RegisterModelValidator()
        {
            RuleFor(x => x.User.Name)
                .NotEmpty().WithMessage("Ad alanı boş olamaz")
                .MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır");

            RuleFor(x => x.User.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş olamaz");

            RuleFor(x=> x.Login.Email)
                .NotEmpty().WithMessage("Email alanı boş olamaz")
                //.Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
                .EmailAddress()
                .WithMessage("Geçerli bir mail adresi giriniz");

            RuleFor(x=>x.Login.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakterli olmalıdır");

            RuleFor(x=>x.Login.ConfirmPassword)
                .NotEmpty().WithMessage("Şifre Doğrulama alanı boş olamaz")
                .Equal(x=>x.Login.Password).WithMessage("Şifre alanı aynı olmalıdır");


            RuleFor(x => x.Address.AddressOne)
                .NotEmpty().WithMessage("Adres alanı boş olamaz")
                .MinimumLength(8).WithMessage("Adres en az 10 karakter olmalıdır");

            RuleFor(x => x.ConfirmAccount)
            .Equal(true).WithMessage("Onayla alanını işaretlemelisiniz!");
        }
    
}