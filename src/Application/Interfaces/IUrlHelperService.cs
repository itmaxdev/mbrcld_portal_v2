namespace Mbrcld.Application.Interfaces
{
    public interface IUrlHelperService
    {
        string GetAbsoluteUrlForAction(string actionName, object routeValues);
    }
}
