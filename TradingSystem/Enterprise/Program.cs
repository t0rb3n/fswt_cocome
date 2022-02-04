using Enterprise.Application;

namespace Enterprise;

internal class Program
{
    private static void Main(string[] args)
    {
        EnterpriseApplication eApp = new EnterpriseApplication(1);
        //eApp.TestGrpc();
    }
}