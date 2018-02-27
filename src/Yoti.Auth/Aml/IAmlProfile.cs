namespace Yoti.Auth.Aml
{
    public interface IAmlProfile
    {
        string GetGivenNames();

        string GetFamilyName();

        string GetSsn();

        IAmlAddress GetAmlAddress();
    }
}