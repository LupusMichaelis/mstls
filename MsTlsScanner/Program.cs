using System.Net;

// compatibility matrix:
// https://docs.microsoft.com/en-us/windows/win32/secauthn/protocols-in-tls-ssl--schannel-ssp-
// https://docs.microsoft.com/en-us/dotnet/framework/network-programming/tls#switchsystemnetdontenableschusestrongcrypto

// To check if a peticular protocol in activated, use the command `sslscan` against the URI

class ProgramClass
{
    static HttpClient client = new();

    static async Task Main(string[] args)
    {
        string[] uriList = {
            "https://no-soap.org/", // TLS1.3 activated along TLS1.2 and 1.1
            "https://lupusmic.org", // TLS1.3 deactivated
        };
        List<Task> taskList = new();
        foreach (string uri in uriList)
            taskList.Add(AttemptUri(uri));

        await Task.WhenAll(taskList);
    }

    static async Task AttemptUri(string uri)
    {
        Console.WriteLine("Attempt URI {0}", uri);
        try
        {
            string responseBody = await client.GetStringAsync(uri);
            Console.WriteLine("Received answer of {0} bytes", responseBody.Length);
        }
        catch (HttpRequestException e)
        {

            System.Exception se = e;
            Console.WriteLine("\nException Caught!");
            do
            {
                Console.WriteLine("\tMessage :{0} ", se.Message);
                se = se.InnerException;
            } while (se != null);
        }
    }
}
