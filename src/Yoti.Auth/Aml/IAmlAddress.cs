namespace Yoti.Auth.Aml
{
    public interface IAmlAddress
    {
        string GetPostcode();

        string GetCountry();
    }
}