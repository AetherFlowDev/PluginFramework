namespace AetherFlow.Framework.Tests.Interfaces
{
    public interface ISpecification
    {
        void RunSpecification();
        void Arrange();
        void Act();
        void Cleanup();
    }
}
