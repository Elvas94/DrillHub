namespace DrillHub.Infrastructure
{
    public interface IAggregateRoot<TKey>: IЕntity
    {
        TKey Id { get; set; }
    }
}
