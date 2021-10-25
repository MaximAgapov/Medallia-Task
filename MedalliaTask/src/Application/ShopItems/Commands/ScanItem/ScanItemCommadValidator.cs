using FluentValidation;

namespace MedalliaTask.Application.Items.Commands.ScanItem
{
    public class ScanItemCommandValidator : AbstractValidator<ScanItemCommand>
    {
        public ScanItemCommandValidator()
        {
            RuleFor(v => v.Amount)
                .GreaterThan(0);
        }
        
    }
}