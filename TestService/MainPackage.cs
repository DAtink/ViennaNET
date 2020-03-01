using SimpleInjector;
using SimpleInjector.Packaging;
using ViennaNET.WebApi.Configurators.Diagnosing;

namespace TestService
{
  class MainPackage : IPackage
  {
    public void RegisterServices(Container container)
    {
      var diagnosticPackage = new DiagnosticPackage();
      
      diagnosticPackage.RegisterServices(container);
    }
  }
}
