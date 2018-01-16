
namespace LiteHomeManagement.App.Common
{
    public sealed class Command : IValidate
    {
        public string ActorId { get; set; }
        public string Name { get; set; }
        public string JsonPayload { get; set; }

        public ValidationResult Validate()
        {
            return new ReflectionValidation().Validate(this);
        }
    }
}
