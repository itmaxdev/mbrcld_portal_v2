namespace Mbrcld.Application.Interfaces
{
    public interface IPreferredLanguageService
    {
        int GetPreferredLanguageLCID();
        void SetPreferredLanguage(string locale);
    }
}
