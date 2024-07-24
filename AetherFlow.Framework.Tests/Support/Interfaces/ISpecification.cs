namespace AetherFlow.Framework.Tests.Support.Interfaces
{
    public interface ISpecification
    {
        void RunSpecification();
        void Arrange();
        void Act();
        void Cleanup();
    }
}
