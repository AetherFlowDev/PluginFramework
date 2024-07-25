namespace AetherFlow.Framework.Testing.Interfaces
{
    public interface ISpecification
    {
        void RunSpecification();
        void Arrange();
        void Act();
        void Cleanup();
    }
}
