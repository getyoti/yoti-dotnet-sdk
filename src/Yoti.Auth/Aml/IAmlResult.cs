namespace Yoti.Auth.Aml
{
    public interface IAmlResult
    {
        bool IsOnFraudList();

        bool IsOnPepList();

        bool IsOnWatchList();
    }
}