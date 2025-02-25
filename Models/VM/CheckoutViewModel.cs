using FluentValidation;

public class CheckoutViewModel
{
    public ShopCartViewModel shopcartVM { get; set; }   = new ShopCartViewModel();
    public User user{ get; set; } = new User();
    public Address address{ get; set; } = new Address();
    public Login login{ get; set; } = new Login();
    public bool CheckoutConfirm { get; set; }  
}

public class CheckoutModelValidator : AbstractValidator<CheckoutViewModel>
{
        public CheckoutModelValidator()
        {
            RuleFor(x => x.CheckoutConfirm)
            .Equal(true).WithMessage("Devam edebilmek için onayla alanını işaretlemelisiniz!");
        }
}